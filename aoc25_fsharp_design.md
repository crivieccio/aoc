# F# Advent of Code Console Application Design

## Overview

Design for a console application in F# that can run Advent of Code problems given a day number (01-12), inspired by the existing C# implementation but leveraging F# idioms and functional programming patterns.

## Core Architecture

### 1. Base Day Solution Pattern

```fsharp
type PartResult = {
    Value: string
    ExecutionTime: System.TimeSpan
}

type DaySolution = {
    Part1: string list -> PartResult
    Part2: string list -> PartResult
    DayNumber: int
}
```

### 2. Input Management

```fsharp
module Input =
    let readInput (dayNumber: int) : string list option =
        let filePath = sprintf "../Inputs/day%02d.txt" dayNumber
        if System.IO.File.Exists(filePath) then
            System.IO.File.ReadAllLines(filePath) |> Array.toList |> Some
        else
            None
    
    let readExampleInput (dayNumber: int) : string list option =
        let filePath = sprintf "../Inputs/day%02d_example.txt" dayNumber
        if System.IO.File.Exists(filePath) then
            System.IO.File.ReadAllLines(filePath) |> Array.toList |> Some
        else
            None
```

## Project Structure

```
AocFSharp/
├── AocFSharp.fsproj
├── Program.fs                 # Main entry point and CLI
├── BaseDay.fs                 # Base types and utilities
├── Input.fs                   # Input file management
├── Days/
│   ├── Day01.fs
│   ├── Day02.fs
│   └── ...
├── Types.fs                   # Shared types and domain models
└── Utils.fs                   # Common utilities
```

## Day Implementation Pattern

Each day follows a functional pattern:

```fsharp
module Day01 =
    let part1 (input: string list) : PartResult =
        let startTime = System.Diagnostics.Stopwatch.StartNew()
        // Solution logic using functional composition
        let result = input 
                     |> List.map parseLine
                     |> List.fold calculatePosition (50, 0)
                     |> fst
                     |> sprintf "%d"
        { Value = result; ExecutionTime = startTime.Elapsed }
    
    let part2 (input: string list) : PartResult =
        // Similar pattern for part 2
    
    let solution = {
        DayNumber = 1
        Part1 = part1
        Part2 = part2
    }
```

## CLI and Runner Design

### Command Line Interface

```fsharp
[<EntryPoint>]
let main argv =
    match argv with
    | [| "run"; dayStr; partStr |] ->
        match Int32.TryParse dayStr, Int32.TryParse partStr with
        | (true, day), (true, part) when day >= 1 && day <= 12 && (part = 1 || part = 2) ->
            runSolution day part
        | _ -> printInvalidArguments()
    | [| "list" |] -> listAvailableSolutions()
    | _ -> printUsage()
    0
```

### Solution Discovery

```fsharp
module SolutionRunner =
    let private dayModules = [
        Day01.solution
        Day02.solution
        // ... other days
    ]
    
    let getSolution (dayNumber: int) : DaySolution option =
        dayModules 
        |> List.tryFind (fun s -> s.DayNumber = dayNumber)
    
    let runSolution (dayNumber: int) (part: int) =
        match getSolution dayNumber with
        | Some solution ->
            match Input.readInput dayNumber with
            | Some input ->
                let result = 
                    match part with
                    | 1 -> solution.Part1 input
                    | 2 -> solution.Part2 input
                    | _ -> failwith "Invalid part"
                
                printfn "Day %02d Part %d: %s" dayNumber part result.Value
                printfn "Execution time: %A" result.ExecutionTime
            | None -> printfn "Error: Input file not found for day %02d" dayNumber
        | None -> printfn "Error: Solution not found for day %02d" dayNumber
```

## Key F# Idioms and Patterns

### 1. Functional Error Handling

```fsharp
// Using Option types for safe input reading
let safeReadInput dayNumber =
    Input.readInput dayNumber
    |> Option.map (fun lines -> 
        // Process input safely
        lines |> List.filter (System.String.IsNullOrWhiteSpace >> not))
```

### 2. Pipeline Composition

```fsharp
let solvePart1 input =
    input
    |> List.map parseInput
    |> List.fold processStep initialState
    |> extractAnswer
    |> sprintf "%d"
```

### 3. Pattern Matching

```fsharp
match input with
| [] -> "0"
| [single] -> processSingleLine single
| multiple -> multiple |> List.map processLine |> combineResults
```

### 4. Computation Expressions (if needed)

```fsharp
type OptionBuilder() =
    member _.Bind(x, f) = Option.bind f x
    member _.Return(x) = Some x
    member _.Zero() = None

let option = OptionBuilder()

let complexCalculation input =
    option {
        let! parsed = parseInput input
        let! processed = processData parsed
        let! result = calculateAnswer processed
        return result
    }
```

## Input File Structure

```
../Inputs/
├── day01.txt
├── day01_example.txt
├── day02.txt
├── day02_example.txt
└── ...
```

## Example Day Implementation

```fsharp
module Day01 =
    open System.Text.RegularExpressions
    
    type Direction = Left | Right
    type Turn = { Direction: Direction; Amount: int }
    
    let parseLine (line: string) : Turn =
        let regex = Regex(@"([LR])(\d+)")
        let match_ = regex.Match(line)
        { 
            Direction = if match_.Groups.[1].Value = "L" then Left else Right
            Amount = int match_.Groups.[2].Value 
        }
    
    let calculateNewPosition (currentPos: int) (turn: Turn) : int =
        match turn.Direction with
        | Left -> (100 + (currentPos - turn.Amount)) % 100
        | Right -> (100 + currentPos + turn.Amount) % 100
    
    let part1 (input: string list) : PartResult =
        let startTime = System.Diagnostics.Stopwatch.StartNew()
        let result = input
                     |> List.map parseLine
                     |> List.fold (fun pos turn -> calculateNewPosition pos turn) 50
                     |> sprintf "%d"
        { Value = result; ExecutionTime = startTime.Elapsed }
    
    let part2 (input: string list) : PartResult =
        // Implementation for part 2
        let startTime = System.Diagnostics.Stopwatch.StartNew()
        let result = "42" // Placeholder
        { Value = result; ExecutionTime = startTime.Elapsed }
    
    let solution = {
        DayNumber = 1
        Part1 = part1
        Part2 = part2
    }
```

## Benefits of F# Approach

1. **Type Safety**: F#'s strong type system prevents runtime errors
2. **Immutability**: Safer concurrent/parallel execution
3. **Functional Composition**: More concise and readable code
4. **Pattern Matching**: Elegant control flow
5. **Less Boilerplate**: Cleaner syntax compared to C#
6. **Performance**: Efficient functional data structures
7. **Correctness**: Algebraic data types and option handling

## Command Examples

```bash
# Run day 1 part 1
dotnet run --project AocFSharp.fsproj run 01 1

# Run day 5 part 2  
dotnet run --project AocFSharp.fsproj run 05 2

# List available solutions
dotnet run --project AocFSharp.fsproj list
```

## Build and Test Commands

```bash
# Build
dotnet build AocFSharp.fsproj

# Run specific solution
dotnet run --project AocFSharp.fsproj run 01 1

# Run all solutions (future enhancement)
dotnet run --project AocFSharp.fsproj run-all
```

This design leverages F#'s functional strengths while maintaining the same user experience and workflow as the C# version.
