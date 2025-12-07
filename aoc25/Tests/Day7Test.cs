using Solutions;

using Xunit.Abstractions;

namespace Tests;

public class Day7Test(ITestOutputHelper output) : BaseDayTest<Day7>(output)
{
    protected override int DayNumber => 7;

    protected override string Part1ExampleExpected => "21";

    protected override string Part2ExampleExpected => "40";

    protected override bool TestAgainstActualInput => false;
}