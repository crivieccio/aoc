using System.Text.RegularExpressions;

namespace Solutions;

public record struct Region(int Rows, int Columns, int[] Shapes);

public class Day12 : BaseDay
{
    public override string Part1(List<string> input)
    {
        var (shapes, regions) = input.ParseDay12();
        var total = 0;
        foreach (var region in regions)
        {
            var areaNeeded = Enumerable.Zip(shapes, region.Shapes).Sum(x => x.First * x.Second);
            if (areaNeeded <= region.Area()) total++;
        }
        return total.ToString();
    }
    public override string Part2(List<string> input) => "0";
}

public static partial class Day12Extensions
{

    [GeneratedRegex(@"(?<regions>\d+x\d+:( \d+)+)")]
    public static partial Regex Regions { get; }

    extension(List<string> source)
    {
        public (IEnumerable<int> shapes, IEnumerable<Region> regions) ParseDay12()
        {
            var regions = source.Where(x => Regions.IsMatch(x)).Select(s => s.Split(':', StringSplitOptions.RemoveEmptyEntries).ToArray());
            return (source.ParseShapes(), source.ParseRegions());
        }

        private IEnumerable<int> ParseShapes() => string.Join(',', source).Split(",,", StringSplitOptions.RemoveEmptyEntries)[..^1].Select(x => x.Count(c => c == '#'));

        private IEnumerable<Region> ParseRegions() =>
            source
            .Where(x => Regions.IsMatch(x))
            .Select(s => s.Split(':', StringSplitOptions.RemoveEmptyEntries))
            .Select(array =>
                {
                    var (rows, cols) = ParseRegionDimensions(array);

                    return new Region(rows, cols, [.. array[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)]);
                });

        private static (int rows, int columns) ParseRegionDimensions(string[] array)
        {
            var dims = array[0].Split('x', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            return (dims[0], dims[1]);
        }
    }

    extension(Region source)
    {
        public long Area() => source.Rows * source.Columns;
    }
}