
namespace Solutions;

public static class IEnumerableExtensions
{
    extension(IEnumerable<long>)
    {
        public static IEnumerable<long> Generate(long start, long end)
        {
            for (var i = start; i < end; i++)
            {
                yield return i;
            }
        }
    }
}

public static class DayExtensions
{
    extension(List<string> input)
    {
        public Beam GetStartingBeam() => new(1, input[0].IndexOf('S'), 1);
    }
}