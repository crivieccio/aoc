using Solutions;
using Xunit.Abstractions;

namespace Tests;

public class Day4Test(ITestOutputHelper output) : BaseDayTest<Day4>(output)
{
  protected override int DayNumber => 4;

  protected override string Part1ExampleExpected => "13";

  protected override string Part2ExampleExpected => "43";

  protected override bool TestAgainstActualInput => false;
}