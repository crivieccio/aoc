using Solutions;
using Xunit;
using Xunit.Abstractions;

namespace Tests;

/// <summary>
/// Example test class demonstrating usage of BaseDayTest for Day 1
/// This shows how to implement tests for a specific Advent of Code day
/// Replace 'Day1' with your actual solution class name
/// </summary>
public class Day1Test : BaseDayTest<Day1> // Replace 'Day1' with your actual class
{
  public Day1Test(ITestOutputHelper output) : base(output)
  {
  }

  protected override int DayNumber => 1;

  protected override string Part1ExampleExpected => "142"; // Expected result from example
  protected override string Part2ExampleExpected => "281"; // Expected result from second example

  // Enable this only after you've solved the day and want to test against actual input
  // protected override bool TestAgainstActualInput => true;

  protected override void ProcessInput(Day1 instance, List<string> input)
  {
    // This is where the specific day implementation would run
    // For Day1, this might call instance.Run() or specific methods
    // The exact implementation depends on how the Day1 class is structured

    // Example implementation (adjust based on actual Day1 implementation):
    // Option 1: Call the Run method directly
    instance.Part1(input);

    // Option 2: Process input manually if your solution doesn't use Console output
    // var result = ProcessInputData(input);
    // Console.WriteLine(result);
  }

  // You can add additional test methods specific to this day
  // For example, testing specific algorithms or edge cases

  [Fact]
  public void Day1_AdditionalValidation_ShouldPass()
  {
    // Arrange
    var testInput = new List<string> { "1 2 3" };

    // Act & Assert using the helper method
    AssertSolution(testInput, "expected_output", "Additional validation");
  }

  [Theory]
  [InlineData("1 2 3", "6")]
  [InlineData("10 20 30", "60")]
  public void Day1_TheoryTests_ShouldPass(string input, string expected)
  {
    // Arrange
    var inputLines = new List<string> { input };

    // Act & Assert
    AssertSolution(inputLines, expected, $"Theory test: {input}");
  }

  // Example of a test that uses custom timeout
  [Fact]
  public void Day1_PerformanceTest_WithCustomTimeout()
  {
    // Arrange
    var testInput = ReadInput(useExample: true);

    // Act & Assert with custom timeout
    AssertSolutionWithTimeout(testInput, "142", 1000, "Day 1 with 1 second timeout");
  }
}