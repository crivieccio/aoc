

using Solutions;
using Xunit.Abstractions;

namespace Tests;

/// <summary>
/// Example test class demonstrating usage of BaseDayTest for Day 1
/// This shows how to implement tests for a specific Advent of Code day
/// Replace 'Day1' with your actual solution class name
/// </summary>
public class Day3Test(ITestOutputHelper output) : BaseDayTest<Day3>(output) // Replace 'Day1' with your actual class
{
  protected override int DayNumber => 3;

  protected override string Part1ExampleExpected => "357"; // Expected result from example
  protected override string Part2ExampleExpected => "3121910778619"; // Expected result from second example

  // Enable this only after you've solved the day and want to test against actual input
  protected override bool TestAgainstActualInput => false;

  protected override void ProcessInput(Day3 instance, List<string> input)
  {
    // This test framework calls ProcessInput for both Part1 and Part2 tests
    // We need to determine which part to run based on the context

    // Check if we're being called from Part1 or Part2 test by examining the stack
    var stackTrace = new System.Diagnostics.StackTrace();
    var testName = stackTrace.GetFrames()
        .Select(f => f.GetMethod()?.Name)
        .FirstOrDefault(m => m?.Contains("Part1_ExampleInput_ShouldMatchExpected") == true ||
                            m?.Contains("Part2_ExampleInput_ShouldMatchExpected") == true);

    if (testName?.Contains("Part1_") == true)
    {
      var result = instance.Part1(input);
      Console.WriteLine(result);
    }
    else if (testName?.Contains("Part2_") == true)
    {
      var result = instance.Part2(input);
      Console.WriteLine(result);
    }
    else
    {
      // Fallback: run both parts
      var result1 = instance.Part1(input);
      var result2 = instance.Part2(input);
      Console.WriteLine($"Part1: {result1}");
      Console.WriteLine($"Part2: {result2}");
    }
  }
}