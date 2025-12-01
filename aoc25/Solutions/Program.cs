// See https://aka.ms/new-console-template for more information
using System.Reflection;
using Cocona;
using Solutions;

var app = CoconaApp.Create();

app.AddCommand("hello", () => Console.WriteLine("Hello, World!"));
app.AddCommand("run", (string day, string part) =>
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
    string inputFilePath = $"../Inputs/day{day}.txt";
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

    // 5. Check if class has a Part1 and Part2 method
    MethodInfo? part1 = dayType.GetMethod("Part1");
    MethodInfo? part2 = dayType.GetMethod("Part2");
    if (part1 == null || part2 == null)
    {
      Console.WriteLine($"Error: {className} class does not have correct methods.");
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

    // Read input data for the solution
    List<string> inputData = BaseDay.ReadInput(inputFilePath);

    if (part == "1")
    {
      Console.WriteLine($"Part 1: {part1.Invoke(dayInstance, [inputData])}");
    }
    else
    {
      Console.WriteLine($"Part 2: {part2.Invoke(dayInstance, [inputData])}");
    }

    Console.WriteLine($"Solution for day {dayNumber} completed.");
  }
  catch (Exception ex)
  {
    Console.WriteLine($"Error running solution: {ex.Message}");
  }
});
app.Run();
