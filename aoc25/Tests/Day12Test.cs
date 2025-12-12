using Solutions;

using Xunit.Abstractions;


namespace Tests;


public class Day12Test(ITestOutputHelper output) : BaseDayTest<Day12>(output)
{
    protected override int DayNumber => 12;
    protected override string Part1ExampleExpected => "2";
    protected override string Part2ExampleExpected => "0";
}