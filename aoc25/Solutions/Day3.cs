
using System.Net.Sockets;
using System.Text;

namespace Solutions;

public class Day3 : BaseDay
{
    private static readonly int _batteryBankSize = 12;
    public override string Part1(List<string> input) => GetJoltage(input).Sum().ToString();

    public override string Part2(List<string> input) => input.Select(GetLargestJoltage).Sum().ToString();

    private static IEnumerable<int> GetJoltage(List<string> input)
    {
        foreach (var line in input)
        {
            var start = 0;
            var highest = -1;
            for (var i = start; i < line.Length; i++)
            {
                var next = i + 1;
                while (next < line.Length)
                {
                    var current = int.Parse($"{line[i]}{line[next]}");
                    if (current > highest) highest = current;
                    next++;
                }
            }
            yield return highest;
        }
    }

    private static long GetLargestJoltage(string input)
    {
        var builder = new StringBuilder();
        builder.Clear();
        for (var count = _batteryBankSize; count > 0; count--)
        {
            char max = input[..^(count - 1)].Max();
            input = input[(input.IndexOf(max) + 1)..];
            builder.Append(max);
        }
        return long.Parse(builder.ToString());
    }
}