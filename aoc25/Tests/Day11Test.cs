using Solutions;

using Xunit.Abstractions;

namespace Tests;


public class Day11Test(ITestOutputHelper output) : BaseDayTest<Day11>(output)
{
    protected override int DayNumber => 11;
    protected override string Part1ExampleExpected => "5";
    protected override string Part2ExampleExpected => "2";
}