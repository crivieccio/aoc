
using System.Text.RegularExpressions;

namespace Solutions;

public partial class Day2 : BaseDay
{
    public override string Part1(List<string> input) => input
        .Select(s => s.Split(',', StringSplitOptions.RemoveEmptyEntries))
        .SelectMany(s => s.Select(s => s.Split('-', StringSplitOptions.RemoveEmptyEntries))
        .Select(s => new Range(s[0], s[1]))
        .ToList())
        .SelectMany(InvalidIdsInRange)
        .Sum()
        .ToString();

    public override string Part2(List<string> input) =>
      input
        .Select(s => s.Split(',', StringSplitOptions.RemoveEmptyEntries))
        .SelectMany(s => s.Select(s => s.Split('-', StringSplitOptions.RemoveEmptyEntries))
        .Select(s => new Range(s[0], s[1]))
        .ToList())
        .SelectMany(r => IEnumerable<long>.Generate(long.Parse(r.Start), long.Parse(r.End)).TakeWhile(y => y <= long.Parse(r.End)))
        .Where(x => InvalidPart2IdRegex.IsMatch(x.ToString()))
        .Sum()
        .ToString();

    private record Range(string Start, string End);

    private static IEnumerable<long> InvalidIdsInRange(Range range)
    {
        //Find all the ids that are made up of a repeated sequence of digits
        var start = long.Parse(range.Start);
        var end = long.Parse(range.End);

        for (long i = start; i <= end; i++)
        {
            var id = i.ToString();
            if (id.Length % 2 != 0) continue;

            var half = id.Length / 2;
            var firstHalf = id[..half];
            var secondHalf = id[half..];

            if (firstHalf == secondHalf)
            {
                yield return i;
            }
        }
    }

    [GeneratedRegex(@"^(\d+)(\1)+$")]
    private static partial Regex InvalidPart2IdRegex { get; }
}
