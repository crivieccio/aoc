using Solutions;

using Xunit.Abstractions;

namespace Tests;

public class Day8Test(ITestOutputHelper output) : BaseDayTest<Day8>(output)
{
    protected override int DayNumber => 8;

    protected override string Part1ExampleExpected => "40";

    protected override string Part2ExampleExpected => "25272";
}
