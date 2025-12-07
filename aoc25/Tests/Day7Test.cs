using Solutions;

using Xunit.Abstractions;

namespace Tests;

public class Day7Test(ITestOutputHelper output) : BaseDayTest<Day7>(output)
{
    protected override int DayNumber => 6;

    protected override string Part1ExampleExpected => "-1";

    protected override string Part2ExampleExpected => "-1";

    protected override bool TestAgainstActualInput => false;
}