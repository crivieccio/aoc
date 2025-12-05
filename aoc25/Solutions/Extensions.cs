
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

  public static Range Merge(this Range range, Range other) => new(Math.Min(range.Low, other.Low), Math.Max(range.High, other.High));

  public static bool HasOverlap(this Range range, Range other) => LowInRange(range, other) || HighInRange(range, other);

  public static bool LowInRange(this Range range, Range other) => range.Low >= other.Low;

  public static bool HighInRange(this Range range, Range other) => range.High <= other.High;
}