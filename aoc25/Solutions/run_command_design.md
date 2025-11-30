# Run Command Design

## Current Issues

1. **Variable Name Conflict**: Line 10 creates a variable `day` that shadows the parameter `day`
2. **Hardcoded Class**: Always creates `Day1` regardless of the parameter
3. **No Validation**: No input file validation or parameter checking
4. **No Error Handling**: No graceful handling of missing classes or files

## Improved Design

```csharp
// See https://aka.ms/new-console-template for more information
using Cocona;
using System.Reflection;

var app = CoconaApp.Create();

app.AddCommand("hello", () => Console.WriteLine("Hello, World!"));

app.AddCommand("run", (string day) =>
{
    try
    {
        // 1. Validate day parameter format (should be "01", "02", etc.)
        if (string.IsNullOrWhiteSpace(day) || day.Length != 2 || !int.TryParse(day, out int dayNumber))
        {
            Console.WriteLine("Error: Day parameter must be in format '01', '02', etc.");
            return;
        }

        // 2. Validate day range (1-25 for Advent of Code)
        if (dayNumber < 1 || dayNumber > 25)
        {
            Console.WriteLine("Error: Day must be between 01 and 25.");
            return;
        }

        // 3. Check if input file exists
        string inputFilePath = $"./Inputs/day{day}.txt";
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"Error: Input file not found at {inputFilePath}");
            Console.WriteLine("Please ensure the input file exists in the Inputs directory.");
            return;
        }

        // 4. Create day class name (Day1, Day2, etc.)
        string className = $"Day{dayNumber}";
        Type? dayType = Assembly.GetExecutingAssembly().GetType($"Solutions.{className}");
        
        if (dayType == null)
        {
            Console.WriteLine($"Error: {className} class not found. Please implement the solution for day {dayNumber}.");
            return;
        }

        // 5. Check if class has a Run method
        MethodInfo? runMethod = dayType.GetMethod("Run");
        if (runMethod == null)
        {
            Console.WriteLine($"Error: {className} class does not have a Run method.");
            return;
        }

        // 6. Create instance and run
        object? dayInstance = Activator.CreateInstance(dayType);
        if (dayInstance == null)
        {
            Console.WriteLine($"Error: Failed to create instance of {className}.");
            return;
        }

        Console.WriteLine($"Running solution for day {dayNumber}...");
        runMethod.Invoke(dayInstance, null);
        Console.WriteLine($"Solution for day {dayNumber} completed.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error running solution: {ex.Message}");
    }
});

app.Run();
```

## Key Improvements

### 1. Parameter Validation

- Checks for proper "01", "02" format
- Validates day range (1-25)
- Provides clear error messages

### 2. Input File Validation

- Checks for existence of `Inputs/day{day}.txt`
- Provides helpful error message if file missing

### 3. Dynamic Class Instantiation

- Uses reflection to find the appropriate Day class
- Creates instance dynamically based on day parameter
- Validates class has required Run method

### 4. Error Handling

- Comprehensive try-catch block
- Specific error messages for different failure scenarios
- Graceful degradation

### 5. User Experience

- Clear status messages during execution
- Helpful error messages with actionable guidance
- Confirmation when solution completes

## Usage Examples

```bash
# Run day 1 solution
dotnet run -- run "01"

# Run day 15 solution  
dotnet run -- run "15"

# Invalid format
dotnet run -- run "1"  # Error: Day parameter must be in format '01', '02', etc.

# Out of range
dotnet run -- run "00"  # Error: Day must be between 01 and 25.

# Missing input file
dotnet run -- run "26"  # Error: Input file not found at ./Inputs/day26.txt

# Missing class implementation
dotnet run -- run "03"  # Error: Day3 class not found. Please implement the solution for day 3.
```

## Required Project Structure

```
Solutions/
├── Program.cs
├── BaseDay.cs
├── Day1.cs
├── Day2.cs
└── Inputs/
    ├── day01.txt
    ├── day02.txt
    └── ...
```

## Example Day Class Structure

```csharp
namespace Solutions;

public class Day1 : BaseDay
{
    public void Run()
    {
        var input = ReadInput("./Inputs/day01.txt");
        
        // Solution implementation here
        Console.WriteLine("Day 1 Solution:");
        // ... solution code ...
    }
}


## Example Day Class Structure

```csharp
namespace Solutions;

public class Day1 : BaseDay
{
    public void Run()
    {
        var input = ReadInput("./Inputs/day01.txt");
        
        Console.WriteLine("Day 1 Solution:");
        Console.WriteLine($"Read {input.Count} lines of input");
        
        // Example solution logic - replace with actual Advent of Code solution
        foreach (var line in input)
        {
            Console.WriteLine($"Processing: {line}");
        }
        
        Console.WriteLine("Day 1 solution completed!");
    }
}
```

## Example Input File

Create `Solutions/Inputs/day01.txt` with sample content:

```
sample input line 1
sample input line 2
sample input line 3
```

## Implementation Steps

1. **Replace the current Program.cs** with the improved design
2. **Create Day classes** following the example structure (Day1.cs, Day2.cs, etc.)
3. **Create Inputs directory** and add input files (day01.txt, day02.txt, etc.)
4. **Test the run command** with various scenarios to ensure proper validation and error handling

## Benefits of This Design

- **Scalable**: Easy to add new days by creating new DayX.cs classes
- **Robust**: Comprehensive validation and error handling
- **User-friendly**: Clear error messages and status updates
- **Flexible**: Uses reflection for dynamic class loading
- **Maintainable**: Clean separation between command logic and solution implementation
