**Purpose**: Guide AI coding agents to be productive in this Advent of Code C# repo.

This repo implements Advent of Code solutions in C# (.NET 9). Key folders are `Solutions/` (solution classes and CLI), `Tests/` (xUnit test harness), and `Inputs/` (example and real puzzle inputs).

**Big Picture**

- **Architecture**: Each day's solution is a class `DayN` in `Solutions/` that inherits `BaseDay` and implements `Part1(List<string>)` and `Part2(List<string>)` returning `string`.
- **Runner**: `Solutions/Program.cs` uses the Cocona CLI and reflection to locate `Solutions.Day{N}` and invoke `Part1` or `Part2`. It expects input files under `../Inputs/` relative to `Solutions/`.
- **Testing**: `Tests/BaseDayTest<T>` is a generic test harness. Each concrete test class supplies `DayNumber` and expected example outputs. Tests read `Inputs/day{DD}_example.txt` and `Inputs/day{DD}.txt`.

**Developer workflows & concrete commands**

- **Build whole solution**: `dotnet build aoc25.sln`
- **Run a day from the CLI** (from repo root):
  - `dotnet run --project Solutions/Solutions.csproj -- run 01 1` # run day 01 part 1
- **Run tests** (from repo root):
  - `dotnet test Tests/Tests.csproj`
- **Run a single test class**: use `dotnet test --filter` or run via IDE test runner. Tests assume `Inputs/` files exist; example tests use `dayNN_example.txt`.

**Project-specific conventions and patterns**

- **File layout**: Implementations go under `Solutions/` alongside `BaseDay.cs` and `Program.cs`. Tests under `Tests/` mirror day numbers and inherit `BaseDayTest<T>`.
- **Class naming**: Day classes must be `public class Day{N} : BaseDay` (no extra namespace changes). `Program.cs` locates `Solutions.Day{N}` by exact name.
- **Method signatures**: `Part1(List<string> input)` and `Part2(List<string> input)` must be `public override string` to be discoverable and printable.
- **Input access**: Use `BaseDay.ReadInput(path)` to read input lines. Tests and `Program.cs` expect input files in `Inputs/` sibling to `Solutions/`.
- **Console output**: Tests capture `Console.Out` and compare the printed output; solutions that rely on `Console.WriteLine` should ensure the output matches expected test strings.

**Common pitfalls discovered in code**

- `Program.cs` uses reflection and exact method names; avoid renaming `Part1`/`Part2` or the `DayN` class name.
- Input file paths are relative to the running project's working directory (the `Solutions` project expects `../Inputs/`). When running tests or the CLI, run from repo root using the commands above to keep paths correct.
- Tests default `TestAgainstActualInput` to `false` to avoid spoilers; enable deliberately when needed.

**Key files to inspect when modifying behavior**

- `Solutions/BaseDay.cs` — input helper and contract for days
- `Solutions/Program.cs` — CLI wiring and reflection rules (day name format, input path)
- `Solutions/Day1.cs` — example day implementation
- `Tests/BaseDayTest.cs` — test patterns, expected file names, timing checks
- `Inputs/` — example and actual input files (e.g. `day01_example.txt`, `day01.txt`)

**If you change project or runtime settings**

- Keep `Directory.Packages.props` and project references in sync. Use `dotnet restore` after edits to package references.

**Instructions for AI coding agents**

- In all interactions, use the most concise and precise language possible. Sacrifice clarity for concision.

**Planning**

- Before writing code, ask for clarification on the problem statement. Be concise in your questions. Sacrifice clarity for concision.
