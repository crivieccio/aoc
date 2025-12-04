
using Solutions;
using Xunit.Abstractions;

namespace Tests;

/// <summary>
/// Example test class demonstrating usage of BaseDayTest for Day 1
/// This shows how to implement tests for a specific Advent of Code day
/// Replace 'Day1' with your actual solution class name
/// </summary>
public class Day2Test(ITestOutputHelper output) : BaseDayTest<Day2>(output) // Replace 'Day1' with your actual class
{
  protected override int DayNumber => 2;

  protected override string Part1ExampleExpected => "1227775554"; // Expected result from example
  protected override string Part2ExampleExpected => "4174379265"; // Expected result from second example

  // Enable this only after you've solved the day and want to test against actual input
  protected override bool TestAgainstActualInput => false;

  protected override void ProcessInput(Day2 instance, List<string> input, TestPartContext context)
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