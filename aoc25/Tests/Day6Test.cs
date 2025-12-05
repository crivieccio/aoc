using Solutions;

using Xunit.Abstractions;

namespace Tests;

public class Day6Test(ITestOutputHelper output) : BaseDayTest<Day6>(output)
{
    protected override int DayNumber => 6;

    protected override string Part1ExampleExpected => "3";

    protected override string Part2ExampleExpected => "14";

    protected override bool TestAgainstActualInput => false;
}