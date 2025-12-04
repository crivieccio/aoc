using Solutions;
using Xunit.Abstractions;

namespace Tests;

public class Day5Test(ITestOutputHelper output) : BaseDayTest<Day5>(output)
{
  protected override int DayNumber => 5;

  protected override string Part1ExampleExpected => "-1";

  protected override string Part2ExampleExpected => "-1";

  protected override bool TestAgainstActualInput => false;
}