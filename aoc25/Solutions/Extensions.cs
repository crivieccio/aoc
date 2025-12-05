
namespace Solutions;

public static class Extensions
{
  public static IEnumerable<long> Generate(long start, long end)
  {
    for (long i = start; i <= end; i++)
    {
      yield return i;
    }
  }
}