using Solutions;
using Xunit.Abstractions;

namespace Tests;

public class Day5Test(ITestOutputHelper output) : BaseDayTest<Day5>(output)
{
  protected override int DayNumber => 5;

  protected override string Part1ExampleExpected => "-1";

  protected override string Part2ExampleExpected => "-1";

  protected override bool TestAgainstActualInput => false;

  protected override void ProcessInput(Day5 instance, List<string> input, TestPartContext context)
  {
    switch (context)
    {
      case TestPartContext.Part1:
        Console.WriteLine(instance.Part1(input));
        break;
      case TestPartContext.Part2:
        Console.WriteLine(instance.Part2(input));
        break;
      case TestPartContext.Both:
        Console.WriteLine($"Part1: {instance.Part1(input)}");
        Console.WriteLine($"Part2: {instance.Part2(input)}");
        break;
    }
  }
}