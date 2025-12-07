namespace Solutions;

public class Day7 : BaseDay
{
    public override string Part1(List<string> input)
    {
        List<Beam> start = [input.GetStartingBeam()];
        var splits = 0;
        var maxRow = input.Count - 1;
        while (true)
        {
            splits += CountSplits(input, start);
            var nextBeams = start
                .Select(b => b.MoveDown())
                .Where(beam => beam.Row < maxRow)
                .SelectMany(beam => input[beam.Row][beam.Col] == '^' ? beam.Split() : [beam])
                .GroupBy(beam => (beam.Row, beam.Col), (pos, beams) => new Beam(pos.Row, pos.Col, beams.Aggregate(0L, (a, b) => a + b.Count)))
                .ToList();

            if (nextBeams.Count == 0) break;
            start = nextBeams;
        }
        return splits.ToString();
    }

    public override string Part2(List<string> input)
    {
        List<Beam> start = [input.GetStartingBeam()];
        var totalBeams = start.Aggregate(0L, (a, b) => a + b.Count);

        var maxRow = input.Count - 1;

        while (true)
        {
            var nextBeams = start
                .Select(b => b.MoveDown())
                .Where(beam => beam.Row < maxRow)
                .SelectMany(beam => input[beam.Row][beam.Col] == '^' ? beam.Split() : [beam])
                .GroupBy(beam => (beam.Row, beam.Col), (pos, beams) => new Beam(pos.Row, pos.Col, beams.Aggregate(0L, (a, b) => a + b.Count)))
                .ToList();

            if (nextBeams.Count == 0) break;
            start = nextBeams;
            totalBeams = start.Aggregate(0L, (a, b) => a + b.Count);
        }

        return totalBeams.ToString();
    }

    private static int CountSplits(List<string> manifold, List<Beam> beams) =>
        beams.Select(b => b.MoveDown()).Count(beam => manifold[beam.Row][beam.Col] == '^');

}

public record Beam(int Row, int Col, long Count);

public static class BeamExtensions
{
    extension(Beam source)
    {
        public Beam MoveDown() => source with { Row = source.Row + 1 };

        public List<Beam> Split() => [source with { Col = source.Col - 1 }, source with { Col = source.Col + 1 }];
    }
}