
using System.Numerics;

namespace Solutions;

public record struct Point(long X, long Y);

public record struct Rectangle(long Top, long Left, long Bottom, long Right);
public class Day9 : BaseDay
{
    public override string Part1(List<string> input) =>
        input
        .ParsePoints()
        .GetAllPairs()
        .Select(p => CalculateArea(p.Item1, p.Item2))
        .Max()
        .ToString();

    public override string Part2(List<string> input)
    {
        var points = input.ParseComplex();
        var segments = points.Boundary().ToArray();
        return points
        .GetAllPairs()
        .Select(p => Rectangle.FromPoints(p.Item1, p.Item2))
        .Where(r => segments.All(s => !Intersect(r, s)))
        .Select(r => r.Area())
        .Max()
        .ToString();
    }

    private static long CalculateArea(Point a, Point b)
    {
        return a.X == b.X ? Math.Abs(a.X - b.X) + 1
            : a.Y == b.Y ? Math.Abs(a.Y - b.Y) + 1
            : (Math.Abs(a.Y - b.Y) + 1) * (Math.Abs(a.X - b.X) + 1);
    }

    private static bool Intersect(Rectangle a, Rectangle b)
    {
        var aIsToTheLeft = a.Right <= b.Left;
        var aIsToTheRight = a.Left >= b.Right;
        var aIsAbove = a.Bottom <= b.Top;
        var aIsBelow = a.Top >= b.Bottom;
        return !(aIsToTheRight || aIsToTheLeft || aIsAbove || aIsBelow);
    }
}

public static class Day9Extensions
{
    extension(List<string> source)
    {
        public IEnumerable<Point> ParsePoints() => source.Select(s => s.Split(',').Select(long.Parse).ToArray()).Select(s => new Point(s[0], s[1]));

        public IEnumerable<Complex> ParseComplex() => source.Select(s => s.Split(',').Select(int.Parse).ToArray()).Select(array => array[0] + Complex.ImaginaryOne * array[1]);
    }

    extension<T>(IEnumerable<T> source)
    {
        public IEnumerable<(T, T)> GetAllPairs() => source.SelectMany((_, i) => source.Where((_, j) => i < j), (a, b) => (a, b));
    }

    extension(IEnumerable<Complex> source)
    {
        public IEnumerable<Rectangle> Boundary() =>
            source.Zip(source.Prepend(source.Last()))
            .Select(pair => Rectangle.FromPoints(pair.First, pair.Second));
    }

    extension(Rectangle source)
    {
        public static Rectangle FromPoints(Complex first, Complex second)
        {
            var top = Math.Min(first.Imaginary, second.Imaginary);
            var bottom = Math.Max(first.Imaginary, second.Imaginary);
            var left = Math.Min(first.Real, second.Real);
            var right = Math.Max(first.Real, second.Real);
            return new Rectangle((long)top, (long)left, (long)bottom, (long)right);
        }

        public long Area() => (source.Bottom - source.Top + 1) * (source.Right - source.Left + 1);
    }
}