namespace Solutions;

public class Day7 : BaseDay
{
    public override string Part1(List<string> input)
    {
        var start = input.GetStartingBeam();
        var activeBeams = new Queue<List<Beam>>();
        activeBeams.Enqueue([start]);
        var splits = 0;
        var maxRow = input.Count - 1;
        while (activeBeams.Count > 0)
        {
            var current = activeBeams.Dequeue();
            if (current.All(beam => beam.Row == input.Count - 1)) break;

            List<Beam> beams = [];
            foreach (var beam in current)
            {
                var newBeam = beam.MoveDown();
                if (newBeam.Row == maxRow) continue;
                if (input[newBeam.Row][newBeam.Col] == '^')
                {
                    var split = beam.Split();
                    if (!beams.Contains(split[0])) beams.Add(split[0]);
                    if (!beams.Contains(split[1])) beams.Add(split[1]);
                    splits++;
                    continue;
                }
                beams.Add(newBeam);
            }
            activeBeams.Enqueue([.. beams.Distinct()]);
        }
        return splits.ToString();
    }

    public override string Part2(List<string> input) => "0";

}

public record Beam(int Row, int Col);

public static class BeamExtensions
{
    extension(Beam source)
    {
        public Beam MoveDown() => source with { Row = source.Row + 1 };

        public List<Beam> Split() => [source with { Col = source.Col - 1, Row = source.Row + 1 }, source with { Col = source.Col + 1, Row = source.Row + 1 }];
    }
}