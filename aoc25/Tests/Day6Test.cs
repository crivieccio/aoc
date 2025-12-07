using Solutions;

using Xunit.Abstractions;

namespace Tests;

public class Day6Test(ITestOutputHelper output) : BaseDayTest<Day6>(output)
{
    protected override int DayNumber => 6;

    protected override string Part1ExampleExpected => "4277556";

    protected override string Part2ExampleExpected => "3263827";

    protected override bool TestAgainstActualInput => false;
}