namespace Solutions;

public partial class Day5 : BaseDay
{
    public override string Part1(List<string> input)
    {
        var ranges = input.TakeWhile(x => !string.IsNullOrEmpty(x)).Select(Range.Create).ToList();
        var ids = input.SkipWhile(x => !string.IsNullOrEmpty(x)).Where(x => !string.IsNullOrEmpty(x)).Select(long.Parse);
        return ids.Count(i => ranges.Exists(r => r.Low <= i && r.High >= i)).ToString();
    }

    public override string Part2(List<string> input) =>
      input
        .TakeWhile(x => !string.IsNullOrEmpty(x))
        .Select(Range.Create)
        .OrderBy(r => r.Low)
        .Aggregate((sum: 0L, max: long.MinValue), (x, y) =>
        {
            var ((sum, max), (lo, hi)) = (x, y);

            if (max >= lo)
                lo = max + 1;
            if (max >= hi) hi = max;

            sum += hi - lo + 1;
            return (sum, hi);
        })
        .sum
        .ToString();
}

public record Range(long Low, long High)
{
    public static Range Create(string range)
    {
        var splitLine = range.Split('-', StringSplitOptions.RemoveEmptyEntries);
        return new Range(long.Parse(splitLine[0]), long.Parse(splitLine[1]));
    }
};