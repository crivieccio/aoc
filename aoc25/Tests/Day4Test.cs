using Solutions;
using Xunit.Abstractions;

namespace Tests;

public class Day4Test(ITestOutputHelper output) : BaseDayTest<Day4>(output)
{
  protected override int DayNumber => 4;

  protected override string Part1ExampleExpected => "13";

  protected override string Part2ExampleExpected => "43";

  protected override bool TestAgainstActualInput => false;

  protected override void ProcessInput(Day4 instance, List<string> input)
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
      var result1 = instance.Part1(input);
      var result2 = instance.Part2(input);
      Console.WriteLine($"Part1: {result1}");
      Console.WriteLine($"Part2: {result2}");
    }
  }
}