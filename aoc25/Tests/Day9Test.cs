using Solutions;

using Xunit.Abstractions;

namespace Tests;

public class Day9Test(ITestOutputHelper output) : BaseDayTest<Day9>(output)
{
    protected override int DayNumber => 9;

    protected override string Part1ExampleExpected => "50";

    protected override string Part2ExampleExpected => "0";
}