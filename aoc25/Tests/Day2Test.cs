
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
  protected override int DayNumber => 1;

  protected override string Part1ExampleExpected => "3"; // Expected result from example
  protected override string Part2ExampleExpected => "6"; // Expected result from second example

  // Enable this only after you've solved the day and want to test against actual input
  // protected override bool TestAgainstActualInput => true;

  protected override void ProcessInput(Day2 instance, List<string> input)
  {
    // This is where the specific day implementation would run
    // For Day1, this might call instance.Run() or specific methods
    // The exact implementation depends on how the Day1 class is structured

    // Example implementation (adjust based on actual Day1 implementation):
    // Option 1: Call the Run method directly
    var result = instance.Part1(input);
    Console.WriteLine(result);

    // Option 2: Process input manually if your solution doesn't use Console output
    // var result = ProcessInputData(input);
    // Console.WriteLine(result);
  }
}