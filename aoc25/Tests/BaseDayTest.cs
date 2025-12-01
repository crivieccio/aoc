using Solutions;
using Xunit;
using Xunit.Abstractions;

namespace Tests;

/// <summary>
/// Base class for Advent of Code day tests.
/// Provides common functionality for testing solutions including example validation,
/// input file management, and performance testing.
/// </summary>
/// <typeparam name="T">The solution class type that inherits from BaseDay</typeparam>
public abstract class BaseDayTest<T> : IDisposable where T : BaseDay, new()
{
  protected readonly ITestOutputHelper Output;
  protected readonly T DayInstance;

  /// <summary>
  /// The day number (1-25) represented by this test class
  /// </summary>
  protected abstract int DayNumber { get; }

  /// <summary>
  /// Expected result for part 1 with example input
  /// </summary>
  protected abstract string Part1ExampleExpected { get; }

  /// <summary>
  /// Expected result for part 2 with example input
  /// </summary>
  protected abstract string Part2ExampleExpected { get; }

  /// <summary>
  /// Whether to test against actual input files (disabled by default to avoid spoilers)
  /// </summary>
  protected virtual bool TestAgainstActualInput => false;

  /// <summary>
  /// Maximum acceptable execution time in milliseconds for performance testing
  /// </summary>
  protected virtual int MaxExecutionTimeMs => 5000;

  public BaseDayTest(ITestOutputHelper output)
  {
    Output = output;
    DayInstance = new T();
  }

  /// <summary>
  /// Gets the path to the input file for the day
  /// </summary>
  /// <param name="useExample">If true, returns example input path; otherwise returns actual input path</param>
  /// <returns>Path to the input file</returns>
  protected string GetInputPath(bool useExample = false)
  {
    string dayFormatted = DayNumber.ToString("00");
    string fileName = useExample ? $"day{dayFormatted}_example.txt" : $"day{dayFormatted}.txt";

    // Get the solution directory (parent of Tests directory)
    string solutionDir = Path.GetFullPath(Path.Combine(
        Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!,
        "..", "..", "..", ".."));

    return Path.Combine(solutionDir, "Inputs", fileName);
  }

  /// <summary>
  /// Reads input from file, handling both example and actual inputs
  /// </summary>
  /// <param name="useExample">If true, reads example input; otherwise reads actual input</param>
  /// <returns>List of input lines</returns>
  protected List<string> ReadInput(bool useExample = false)
  {
    string inputPath = GetInputPath(useExample);

    if (!File.Exists(inputPath))
    {
      throw new FileNotFoundException($"Input file not found: {inputPath}");
    }

    return BaseDay.ReadInput(inputPath);
  }

  /// <summary>
  /// Executes the solution and captures output for testing
  /// </summary>
  /// <param name="input">Input data to process</param>
  /// <returns>Captured console output</returns>
  protected string ExecuteSolution(List<string> input)
  {
    var originalOut = Console.Out;
    var stringWriter = new StringWriter();
    Console.SetOut(stringWriter);

    try
    {
      // Create a new instance for each test to ensure clean state
      var instance = new T();

      // Process input (this would need to be implemented by the specific day)
      ProcessInput(instance, input);

      return stringWriter.ToString().Trim();
    }
    finally
    {
      Console.SetOut(originalOut);
    }
  }

  /// <summary>
  /// Template method for processing input - to be overridden by specific day implementations
  /// </summary>
  /// <param name="instance">The day instance</param>
  /// <param name="input">The input data</param>
  protected virtual void ProcessInput(T instance, List<string> input)
  {
    // Default implementation - subclasses should override this
    // This is where the specific day would implement its logic
    throw new NotImplementedException($"ProcessInput not implemented for {typeof(T).Name}");
  }

  #region Test Methods

  /// <summary>
  /// Tests that the example input produces the expected result for part 1
  /// </summary>
  [Fact]
  public void Part1_ExampleInput_ShouldMatchExpected()
  {
    // Arrange
    var exampleInput = ReadInput(useExample: true);

    // Act
    var startTime = DateTime.UtcNow;
    string result = ExecuteSolution(exampleInput);
    var executionTime = DateTime.UtcNow - startTime;

    // Output timing information
    Output.WriteLine($"Part 1 execution time: {executionTime.TotalMilliseconds:F2}ms");

    // Assert
    Assert.Equal(Part1ExampleExpected, result);

    // Performance assertion
    Assert.True(executionTime.TotalMilliseconds < MaxExecutionTimeMs,
        $"Part 1 exceeded maximum execution time of {MaxExecutionTimeMs}ms (actual: {executionTime.TotalMilliseconds:F2}ms)");
  }

  /// <summary>
  /// Tests that the example input produces the expected result for part 2
  /// </summary>
  [Fact]
  public void Part2_ExampleInput_ShouldMatchExpected()
  {
    // Arrange
    var exampleInput = ReadInput(useExample: true);

    // Act
    var startTime = DateTime.UtcNow;
    string result = ExecuteSolution(exampleInput);
    var executionTime = DateTime.UtcNow - startTime;

    // Output timing information
    Output.WriteLine($"Part 2 execution time: {executionTime.TotalMilliseconds:F2}ms");

    // Assert
    Assert.Equal(Part2ExampleExpected, result);

    // Performance assertion
    Assert.True(executionTime.TotalMilliseconds < MaxExecutionTimeMs,
        $"Part 2 exceeded maximum execution time of {MaxExecutionTimeMs}ms (actual: {executionTime.TotalMilliseconds:F2}ms)");
  }

  /// <summary>
  /// Tests against actual input (disabled by default to avoid spoilers)
  /// </summary>
  [Fact]
  public void Part1_ActualInput_ShouldComplete()
  {
    if (!TestAgainstActualInput)
    {
      Output.WriteLine("Skipping actual input test - TestAgainstActualInput is false");
      return;
    }

    // Arrange
    var actualInput = ReadInput(useExample: false);

    // Act
    var startTime = DateTime.UtcNow;
    string result = ExecuteSolution(actualInput);
    var executionTime = DateTime.UtcNow - startTime;

    // Output results
    Output.WriteLine($"Part 1 actual result: {result}");
    Output.WriteLine($"Part 1 actual execution time: {executionTime.TotalMilliseconds:F2}ms");

    // Assert - just verify it completes without throwing
    Assert.NotNull(result);
    Assert.NotEmpty(result);

    // Performance assertion for actual input
    Assert.True(executionTime.TotalMilliseconds < MaxExecutionTimeMs * 2,
        $"Part 1 actual input exceeded extended execution time of {MaxExecutionTimeMs * 2}ms");
  }

  /// <summary>
  /// Performance test to ensure solution completes within reasonable time
  /// </summary>
  [Fact]
  public void PerformanceTest_ShouldCompleteWithinTimeLimit()
  {
    // Arrange
    var testInput = ReadInput(useExample: true);

    // Act & Assert with timeout
    var exception = Record.Exception(() =>
    {
      var startTime = DateTime.UtcNow;
      ExecuteSolution(testInput);
      var executionTime = DateTime.UtcNow - startTime;

      Assert.True(executionTime.TotalMilliseconds < MaxExecutionTimeMs,
              $"Solution exceeded performance threshold: {executionTime.TotalMilliseconds:F2}ms > {MaxExecutionTimeMs}ms");
    });

    Assert.Null(exception);
  }

  /// <summary>
  /// Tests that the solution can be instantiated multiple times (stateless behavior)
  /// </summary>
  [Fact]
  public void InstantiationTest_ShouldBeStateless()
  {
    // Arrange
    var input = ReadInput(useExample: true);

    // Act - Run the solution multiple times
    var result1 = ExecuteSolution(input);
    var result2 = ExecuteSolution(input);

    // Assert - Results should be consistent
    Assert.Equal(result1, result2);
  }

  #endregion

  #region Helper Methods for Subclasses

  /// <summary>
  /// Helper method for subclasses to assert specific parts of the solution
  /// </summary>
  /// <param name="input">Input data</param>
  /// <param name="expected">Expected result</param>
  /// <param name="description">Description for test output</param>
  protected void AssertSolution(List<string> input, string expected, string description)
  {
    var result = ExecuteSolution(input);
    Assert.Equal(expected, result);
    Output.WriteLine($"{description}: PASSED - {result}");
  }

  /// <summary>
  /// Helper method to test solution with custom timeout
  /// </summary>
  /// <param name="input">Input data</param>
  /// <param name="expected">Expected result</param>
  /// <param name="timeoutMs">Custom timeout in milliseconds</param>
  /// <param name="description">Description for test output</param>
  protected void AssertSolutionWithTimeout(List<string> input, string expected, int timeoutMs, string description)
  {
    var startTime = DateTime.UtcNow;
    var result = ExecuteSolution(input);
    var executionTime = DateTime.UtcNow - startTime;

    Assert.Equal(expected, result);
    Assert.True(executionTime.TotalMilliseconds < timeoutMs,
        $"{description} exceeded timeout: {executionTime.TotalMilliseconds:F2}ms > {timeoutMs}ms");

    Output.WriteLine($"{description}: PASSED - {result} ({executionTime.TotalMilliseconds:F2}ms)");
  }

  #endregion

  public void Dispose()
  {
    // Cleanup if needed
    GC.SuppressFinalize(this);
  }
}