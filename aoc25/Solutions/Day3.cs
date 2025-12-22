
using System.Text;

namespace Solutions;

public class Day3 : BaseDay
{
    private static readonly int BatteryBankSize = 12;
    public override string Part1(List<string> input) => input.Sum(x => GetLargestJoltage(x, 2)).ToString();

    public override string Part2(List<string> input) => input.Sum(x => GetLargestJoltage(x, BatteryBankSize)).ToString();

    private static long GetLargestJoltage(string input, int bankSize)
    {
        var builder = new StringBuilder();
        builder.Clear();
        for (var count = bankSize; count > 0; count--)
        {
            char max = input[..^(count - 1)].Max();
            input = input[(input.IndexOf(max) + 1)..];
            builder.Append(max);
        }
        return long.Parse(builder.ToString());
    }
}