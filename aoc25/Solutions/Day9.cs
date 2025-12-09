
namespace Solutions;

public record struct Point(long X, long Y);
public class Day9 : BaseDay
{
    public override string Part1(List<string> input) =>
        input
        .ParsePoints()
        .GetAllPairs()
        .Select(p => CalculateArea(p.Item1, p.Item2))
        .Max()
        .ToString();

    public override string Part2(List<string> input) => throw new NotImplementedException();

    private static long CalculateArea(Point a, Point b)
    {
        return a.X == b.X ? Math.Abs(a.X - b.X) + 1
            : a.Y == b.Y ? Math.Abs(a.Y - b.Y) + 1
            : (Math.Abs(a.Y - b.Y) + 1) * (Math.Abs(a.X - b.X) + 1);
    }
}

public static class Day9Extensions
{
    extension(List<string> source)
    {
        public IEnumerable<Point> ParsePoints() => source.Select(s => s.Split(',')).Select(s => new Point(long.Parse(s[0]), long.Parse(s[1])));
    }

    extension(IEnumerable<Point> source)
    {
        public IEnumerable<(Point, Point)> GetAllPairs() => source.SelectMany((_, i) => source.Where((_, j) => i < j), (a, b) => (a, b));
    }
}