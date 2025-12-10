
using System.Numerics;
using System.Text.RegularExpressions;

using Microsoft.Z3;

namespace Solutions;

public partial class Day10 : BaseDay
{
    public override string Part1(List<string> input) =>
        ParsePart1(input)
        .Aggregate(0L, (acc, b) =>
        {
            var targetPattern = 0;
            var (indicatorLights, buttons, _) = b;
            var bitWidth = indicatorLights.Length;

            foreach (var indicatorChar in indicatorLights)
            {
                targetPattern = (targetPattern << 1) | (indicatorChar == '#' ? 1 : 0);
            }

            var buttonBitMasks = new int[buttons.Count];

            for (var buttonIndex = 0; buttonIndex < buttons.Count; buttonIndex++)
            {
                var mask = 0;

                foreach (var lightIndex in buttons[buttonIndex])
                {
                    mask |= 1 << (bitWidth - lightIndex - 1);
                }

                buttonBitMasks[buttonIndex] = mask;
            }

            var buttonCount = buttonBitMasks.Length;
            var bestPressCount = int.MaxValue;

            for (var subsetMask = 0; subsetMask < (1 << buttonCount); subsetMask++)
            {
                var currentPattern = 0;

                for (var buttonIndex = 0; buttonIndex < buttonCount; buttonIndex++)
                {
                    if (((subsetMask >> buttonIndex) & 1) != 0)
                    {
                        currentPattern ^= buttonBitMasks[buttonIndex];
                    }
                }

                if (currentPattern == targetPattern)
                {
                    var pressCount = BitOperations.PopCount((uint)subsetMask);

                    if (pressCount < bestPressCount)
                    {
                        bestPressCount = pressCount;
                    }
                }
            }

            return bestPressCount + acc;
        })
        .ToString();

    public override string Part2(List<string> input) =>
        ParsePart1(input)
        .Aggregate(0L, (acc, b) =>
        {
            //cspell:disable
            var (_, buttons, joltageRequirements) = b;
            var counterCount = joltageRequirements.Count();
            var buttonCount = buttons.Count;

            if (counterCount == 0 || buttonCount == 0)
            {
                return 0;
            }

            using var context = new Context();
            using var optimize = context.MkOptimize();

            var buttonPressVariables = new IntExpr[buttonCount];

            for (var i = 0; i < buttonCount; ++i)
            {
                buttonPressVariables[i] = context.MkIntConst($"button_{i}");
                optimize.Add(context.MkGe(buttonPressVariables[i], context.MkInt(0)));
            }

            for (var i = 0; i < counterCount; ++i)
            {
                var affectingButtonTerms = new List<ArithExpr>();

                for (var buttonIndex = 0; buttonIndex < buttonCount; buttonIndex++)
                {
                    if (buttons[buttonIndex].Contains(i))
                    {
                        affectingButtonTerms.Add(buttonPressVariables[buttonIndex]);
                    }
                }

                var sumForCounter = affectingButtonTerms.Count == 1
                    ? affectingButtonTerms[0]
                    : context.MkAdd([.. affectingButtonTerms]);

                optimize.Add(context.MkEq(sumForCounter, context.MkInt(joltageRequirements[i])));
            }

            var totalPressCount = context.MkAdd(buttonPressVariables);

            optimize.MkMinimize(totalPressCount);

            var status = optimize.Check();
            var evaluatedTotalPressCount = optimize.Model.Evaluate(totalPressCount).Simplify();

            return ((IntNum)evaluatedTotalPressCount).Int + acc;
        })
        .ToString();
    //cspell:enable

    [GeneratedRegex(@"\[(?<indicators>\D+)\]|\{(?'joltage'\d+(?:,\s*\d+)*)\}")]
    public static partial Regex Regex { get; }

    [GeneratedRegex(@"\((?:\d+(?:,\d+)*)\)")]
    public static partial Regex Buttons { get; }

    //cspell:disable
    private static IEnumerable<(string indicators, List<int[]> buttons, List<int> joltages)> ParsePart1(List<string> input)
    {
        foreach (var line in input)
        {
            var groups = Regex.Matches(line).Select(m => m.Groups);
            var indicators = groups.Select(g => g["indicators"].Value).First();
            var joltages = groups.SelectMany(g => g["joltage"].Value.Split(',', StringSplitOptions.RemoveEmptyEntries)).Select(c => int.Parse(c.ToString())).ToList();
            var buttons = Buttons.Matches(line).Select(m => m.Value).Select(s => s.Where(c => char.IsDigit(c)).Select(x => int.Parse(x.ToString())).ToArray()).ToList();
            yield return (indicators, buttons, joltages);
        }
    }
    //cspell:enable

}

