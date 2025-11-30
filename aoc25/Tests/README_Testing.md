# Advent of Code Testing Framework

This document explains the testing framework designed for Advent of Code solutions using xUnit.

## Overview

The testing framework consists of:

1. **BaseDayTest.cs** - Abstract base class providing common testing functionality
2. **Day Test Classes** - Concrete implementations for each day (e.g., `Day1Test`, `Day2Test`)
3. **Example Tests** - Sample implementations showing how to use the framework

## BaseDayTest<T> Class Design

### Key Features

- **Generic Type Parameter**: `T` represents the solution class that inherits from `BaseDay`
- **Abstract Properties**: Day number, expected results, and configuration
- **Common Test Methods**: Example validation, performance testing, input validation
- **Helper Methods**: Utilities for subclass testing
- **Output Capture**: Captures console output for testing
- **Performance Monitoring**: Built-in timing and timeout functionality

### Abstract Properties to Override

```csharp
protected abstract int DayNumber { get; }              // Day number (1-25)
protected abstract string Part1ExampleExpected { get; } // Expected part 1 result
protected abstract string Part2ExampleExpected { get; } // Expected part 2 result
protected virtual bool TestAgainstActualInput => false; // Enable real input testing
protected virtual int MaxExecutionTimeMs => 5000;       // Performance threshold
```

### Core Methods

- **`GetInputPath(useExample)`** - Returns path to input files
- **`ReadInput(useExample)`** - Reads and returns input data
- **`ExecuteSolution(input)`** - Runs solution and captures output
- **`ProcessInput(instance, input)`** - Template method for solution execution

## Usage Example

### Creating a Day Test Class

```csharp
public class Day1Test : BaseDayTest<Day1> // Replace Day1 with your class
{
    public Day1Test(ITestOutputHelper output) : base(output) { }

    protected override int DayNumber => 1;
    protected override string Part1ExampleExpected => "142";
    protected override string Part2ExampleExpected => "281";
    
    // Enable only after solving to avoid spoilers
    // protected override bool TestAgainstActualInput => true;

    protected override void ProcessInput(Day1 instance, List<string> input)
    {
        // Your solution execution logic here
        instance.Run(); // Example: call the Run method
        
        // Or process input manually:
        // var result = ProcessMyInput(input);
        // Console.WriteLine(result);
    }
    
    // Add additional tests as needed
    [Fact]
    public void Day1_CustomTest()
    {
        var testInput = new List<string> { "test data" };
        AssertSolution(testInput, "expected", "Custom validation");
    }
}
```

### Input File Structure

The framework expects the following input file structure:

```
Inputs/
├── day01.txt              # Actual puzzle input
├── day01_example.txt      # Example input for testing
├── day02.txt              # Actual puzzle input
├── day02_example.txt      # Example input for testing
└── ...                    # Continue for each day
```

### Test Categories

The base class provides several test categories:

#### 1. Example Validation Tests

- `Part1_ExampleInput_ShouldMatchExpected()`
- `Part2_ExampleInput_ShouldMatchExpected()`

These test against example inputs with known expected results.

#### 2. Actual Input Tests (Optional)

- `Part1_ActualInput_ShouldComplete()`
- `Part2_ActualInput_ShouldComplete()`

Disabled by default to avoid spoilers. Enable with `TestAgainstActualInput = true`.

#### 3. Performance Tests

- `PerformanceTest_ShouldCompleteWithinTimeLimit()`
- Individual performance timing in example tests

#### 4. Validation Tests

- `InputValidation_EmptyInput_ShouldHandleGracefully()`
- `InstantiationTest_ShouldBeStateless()`

#### 5. Custom Tests

Subclasses can add their own test methods using xUnit attributes.

### Helper Methods for Subclasses

#### AssertSolution

```csharp
AssertSolution(input, expected, "description");
```

#### AssertSolutionWithTimeout

```csharp
AssertSolutionWithTimeout(input, expected, timeoutMs, "description");
```

## Testing Best Practices

### 1. Development Workflow

1. **Create solution class** inheriting from `BaseDay`
2. **Create test class** inheriting from `BaseDayTest<YourDayClass>`
3. **Implement ProcessInput** method to call your solution
4. **Add example data** to `Inputs/dayXX_example.txt`
5. **Run tests** to validate against examples
6. **Solve the puzzle** and verify with actual input
7. **Enable actual input testing** when ready

### 2. Test Organization

- **Keep example tests** to validate logic
- **Use custom tests** for edge cases and specific scenarios
- **Add theory tests** for parameterized testing
- **Monitor performance** with built-in timing

### 3. Input File Management

- **Example files** should contain the small example from the puzzle description
- **Actual input files** contain the real puzzle input (not included in version control)
- **Use descriptive comments** in test files explaining expected results

### 4. Performance Considerations

- Set appropriate `MaxExecutionTimeMs` values
- Use `AssertSolutionWithTimeout` for specific performance requirements
- Monitor timing output in test results

### 5. Error Handling

- Test against malformed input
- Verify graceful failure modes
- Use `Record.Exception()` for testing exception scenarios

## Integration with Existing Project Structure

### File Structure

```
aoc25/
├── Solutions/
│   ├── BaseDay.cs              # Input reading functionality
│   ├── Program.cs              # CLI for running solutions
│   ├── Day1.cs                 # Your solution classes
│   └── ...
├── Tests/
│   ├── BaseDayTest.cs          # This framework
│   ├── Day1Test.cs             # Example test class
│   └── ...
└── Inputs/
    ├── day01.txt               # Real input (not in git)
    ├── day01_example.txt       # Example input
    └── ...
```

### Dependencies

- **xUnit 2.9.3** - Testing framework
- **Microsoft.NET.Test.Sdk 17.12.0** - Test execution
- **xunit.runner.visualstudio 2.8.2** - Visual Studio test runner

## Running Tests

### Command Line

```bash
# Run all tests
dotnet test

# Run tests for specific day
dotnet test --filter "Day1Test"

# Run tests with detailed output
dotnet test --verbosity normal

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Visual Studio

- Use Test Explorer to run and debug tests
- Set breakpoints in test methods or solution code
- View test output in the Output window

## Advanced Usage

### Custom Test Data

```csharp
[Theory]
[InlineData("input1", "expected1")]
[InlineData("input2", "expected2")]
public void CustomTheoryTest(string input, string expected)
{
    var inputLines = new List<string> { input };
    AssertSolution(inputLines, expected, $"Theory test: {input}");
}
```

### Performance Profiling

```csharp
[Fact]
public void DetailedPerformanceTest()
{
    var input = ReadInput(useExample: true);
    var stopwatch = Stopwatch.StartNew();
    
    var result = ExecuteSolution(input);
    
    stopwatch.Stop();
    Output.WriteLine($"Detailed timing: {stopwatch.ElapsedMilliseconds}ms");
    
    Assert.True(stopwatch.ElapsedMilliseconds < 1000);
}
```

### Exception Testing

```csharp
[Fact]
public void ExceptionHandlingTest()
{
    var invalidInput = new List<string> { "invalid data" };
    
    var exception = Record.Exception(() => ExecuteSolution(invalidInput));
    
    Assert.NotNull(exception);
    Assert.Contains("expected error message", exception.Message);
}
```

## Benefits of This Framework

1. **Consistency**: Standardized testing approach across all days
2. **Reusability**: Common functionality inherited by all day tests
3. **Performance Monitoring**: Built-in timing and performance assertions
4. **Input Management**: Automatic handling of example vs actual inputs
5. **Output Capture**: Testing of console-based solutions
6. **Flexibility**: Easy to extend with custom test methods
7. **Documentation**: Clear structure and comprehensive examples

This framework provides a robust foundation for testing Advent of Code solutions while maintaining flexibility for day-specific requirements.
