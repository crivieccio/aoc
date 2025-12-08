
using System.Text.RegularExpressions;

namespace Solutions;


public record struct JunctionBox(decimal X, decimal Y, decimal Z);

public partial class Day8 : BaseDay
{
    private const int TestReps = 10;
    private const int FullReps = 1_000;
    public override string Part1(List<string> input)
    {
        var boxes = ParseJunctionBoxes(input).ToArray();
        Dictionary<JunctionBox, HashSet<JunctionBox>> adjacentBoxes = boxes.ToDictionary(b => b, b => new HashSet<JunctionBox> { b });
        foreach (var (a, b) in GetOrderedPairs(boxes).Take(FullReps))
        {
            if (adjacentBoxes[a] != adjacentBoxes[b])
            {
                Connect(a, b, adjacentBoxes);
            }
        }

        return adjacentBoxes
            .Values
            .Distinct()
            .OrderByDescending(set => set.Count)
            .Take(3)
            .Aggregate(1L, (a, b) => a * b.Count)
            .ToString();
    }

    public override string Part2(List<string> input)
    {
        var boxes = ParseJunctionBoxes(input).ToArray();
        var count = boxes.Length;
        var adjacentBoxes = boxes.ToDictionary(b => b, b => new HashSet<JunctionBox> { b });
        var result = 0m;
        foreach (var (a, b) in GetOrderedPairs(boxes).TakeWhile(_ => count > 1))
        {
            if (adjacentBoxes[a] != adjacentBoxes[b])
            {
                Connect(a, b, adjacentBoxes);
                result = a.X * b.X;
                count--;
            }
        }
        return result.ToString();
    }

    static void Connect(JunctionBox a, JunctionBox b, Dictionary<JunctionBox, HashSet<JunctionBox>> setOf)
    {
        setOf[a].UnionWith(setOf[b]);
        foreach (var p in setOf[b])
        {
            setOf[p] = setOf[a];
        }
    }

    static IEnumerable<(JunctionBox a, JunctionBox b)> GetOrderedPairs(JunctionBox[] points) =>
        from a in points
        from b in points
        where (a.X, a.Y, a.Z).CompareTo((b.X, b.Y, b.Z)) < 0
        orderby Metric(a, b)
        select (a, b);

    static decimal Metric(JunctionBox a, JunctionBox b) =>
        (a.X - b.X) * (a.X - b.X) +
        (a.Y - b.Y) * (a.Y - b.Y) +
        (a.Z - b.Z) * (a.Z - b.Z);

    private static IEnumerable<JunctionBox> ParseJunctionBoxes(List<string> input) =>
        input
        .Select(x => Regex.Matches(x))
        .Select(m => new JunctionBox(decimal.Parse(m[0].Value), decimal.Parse(m[1].Value), decimal.Parse(m[2].Value)));

    [GeneratedRegex(@"\d+")]
    private static partial Regex Regex { get; }
}