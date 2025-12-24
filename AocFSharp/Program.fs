open AocFSharp.Types.Common
/// Solution runner module
module SolutionRunner =
    open AocFSharp.Input
    open AocFSharp.Types
    open AocFSharp.Days

    let private daySolutions =
        [ Day01.solution
          Day02.solution
          Day03.solution
          Day04.solution
          Day05.solution
          Day06.solution
          Day07.solution
          Day08.solution
          Day09.solution
          Day10.solution
          Day11.solution
          Day12.solution ]

    let getAllSolutions () : DaySolution list = daySolutions

    let getSolution (dayNumber: int) : DaySolution option =
        daySolutions
        |> List.tryFind (fun s -> s.DayNumber = dayNumber)

    let runSolution (dayNumber: int) (part: int) : AocResult<PartResult> =
        match getSolution dayNumber with
        | Some solution ->
            match readAndCleanExampleInput dayNumber with
            | Success input ->
                try
                    let result =
                        match part with
                        | 1 -> solution.Part1 input
                        | 2 -> solution.Part2 input
                        | _ -> failwith "Invalid part number"

                    Success result
                with
                | ex -> Error(InvalidInputFormat $"Error executing solution: {ex.Message}")
            | Error error -> Error error
        | None -> Error(SolutionNotFound dayNumber)

let printUsage () =
    printfn "Advent of Code F# Solutions"
    printfn ""
    printfn "Usage:"
    printfn "  AocFSharp run <day> <part>    Run a specific day and part"
    printfn "  AocFSharp list                List all available solutions"
    printfn "  AocFSharp help               Show this help message"
    printfn ""
    printfn "Examples:"
    printfn "  AocFSharp run 01 1           Run day 1 part 1"
    printfn "  AocFSharp run 05 2           Run day 5 part 2"
    printfn ""
    printfn "Day numbers: 01-12"
    printfn "Part numbers: 1-2"

let listAvailableSolutions () =
    printfn "Available Advent of Code Solutions:"
    printfn ""

    let solutions = SolutionRunner.getAllSolutions ()

    solutions
    |> List.sortBy (fun s -> s.DayNumber)
    |> List.iter (fun solution -> printfn "Day %02d: ✓" solution.DayNumber)

    printfn ""
    printfn "Total solutions: %d" (List.length solutions)

let runSolution (dayStr: string) (partStr: string) =
    match validateDay dayStr, validatePart partStr with
    | AocFSharp.Types.AocResult.Success dayNumber, AocFSharp.Types.AocResult.Success partNumber ->
        match SolutionRunner.runSolution dayNumber partNumber with
        | AocFSharp.Types.AocResult.Success result ->
            printfn "Day %02d Part %d: %s" dayNumber partNumber result.Value
            printfn "Execution time: %A" result.ExecutionTime
        | AocFSharp.Types.AocResult.Error error ->
            match error with
            | AocFSharp.Types.AocError.InputFileNotFound day ->
                printfn "Error: Input file not found for day %02d. Message: %s" day (error.ToString())
            | AocFSharp.Types.AocError.SolutionNotFound day -> printfn "Error: Solution not found for day %02d" day
            | AocFSharp.Types.AocError.InvalidInputFormat msg -> printfn "Error: Invalid input format: %s" msg
            | AocFSharp.Types.AocError.InvalidDayNumber day -> printfn "Error: Invalid day number: %s" day
            | AocFSharp.Types.AocError.InvalidPartNumber part -> printfn "Error: Invalid part number: %s" part
    | AocFSharp.Types.AocResult.Error error, _ ->
        match error with
        | AocFSharp.Types.AocError.InvalidDayNumber day -> printfn "Error: Invalid day number: %s" day
        | _ -> printfn "Error: Invalid day number"
    | _, AocFSharp.Types.AocResult.Error error ->
        match error with
        | AocFSharp.Types.AocError.InvalidPartNumber part -> printfn "Error: Invalid part number: %s" part
        | _ -> printfn "Error: Invalid part number"


[<EntryPoint>]
let main argv =
    match argv with
    | [| "run"; dayStr; partStr |] -> runSolution dayStr partStr
    | [| "list" |] -> listAvailableSolutions ()
    | [| "help" |]
    | [| "--help" |]
    | [| "-h" |] -> printUsage ()
    | _ -> printUsage ()

    0
