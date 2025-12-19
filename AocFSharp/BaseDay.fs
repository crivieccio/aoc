namespace AocFSharp

open System
open System.Diagnostics
open System.Text.RegularExpressions
open Types

/// Base functionality for day solutions
module BaseDay =

    /// Create a PartResult with execution timing
    let createResult (computation: string) : PartResult =
        { Value = computation
          ExecutionTime = TimeSpan.Zero }

    /// Time a computation and return result with duration
    let timeComputation (computation: unit -> 'T) : PartResult =
        let stopwatch = Stopwatch.StartNew()

        try
            let result = computation ()
            stopwatch.Stop()

            { Value = sprintf "%A" result
              ExecutionTime = stopwatch.Elapsed }
        with
        | ex ->
            stopwatch.Stop()

            { Value = $"Error: {ex.Message}"
              ExecutionTime = stopwatch.Elapsed }

    /// Safe integer parsing with error handling
    let safeParseInt (str: string) : int option =
        match Int32.TryParse str with
        | (true, value) -> Some value
        | _ -> None

    /// Parse integer from string, defaulting to 0 if parsing fails
    let parseInt (str: string) : int =
        safeParseInt str |> Option.defaultValue 0

    /// Create a regex pattern matcher
    let createMatcher (pattern: string) = Regex(pattern, RegexOptions.Compiled)

    /// Split string by whitespace
    let splitWhitespace (str: string) =
        str.Split([| ' '; '\t'; '\n'; '\r' |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.toList

    /// Convert list to array safely
    let listToArray (lst: 'T list) = lst |> List.toArray

    /// Calculate sum of list of integers
    let sum (numbers: int list) = List.sum numbers

    /// Calculate product of list of integers
    let product (numbers: int list) = List.reduce (*) numbers

    /// Create a matrix from input lines
    let createMatrix (lines: string list) : char [,] =
        let rows = lines.Length
        let cols = if rows > 0 then lines.[0].Length else 0
        let matrix = Array2D.create rows cols ' '

        lines
        |> List.iteri (fun row line ->
            line.ToCharArray()
            |> Array.iteri (fun col ch -> matrix.[row, col] <- ch))

        matrix

    /// Find all matches in a string using regex
    let findAllMatches (pattern: string) (input: string) : Match list =
        let regex = Regex(pattern)
        [ for m in regex.Matches(input) -> m ]

    /// Group consecutive elements by a key function
    let groupConsecutive (keyFn: 'T -> 'K) (items: 'T list) : ('K * 'T list) list =
        match items with
        | [] -> []
        | first :: rest ->
            let groups =
                rest
                |> List.fold
                    (fun (currentGroup, groups) item ->
                        let currentKey = keyFn (List.head currentGroup)

                        if keyFn item = currentKey then
                            (item :: currentGroup, groups)
                        else
                            ([ item ], (currentKey, List.rev currentGroup) :: groups))
                    ([ first ], [])

            let finalGroups =
                match snd groups with
                | [] -> [ (keyFn (List.head (fst groups)), List.rev (fst groups)) ]
                | gs ->
                    (keyFn (List.head (fst groups)), List.rev (fst groups))
                    :: gs

            List.rev finalGroups

    /// Generic timeout function for computations
    let withTimeout (timeout: TimeSpan) (computation: unit -> 'T) : AocResult<'T> =
        let task =
            System.Threading.Tasks.Task.Run(computation)

        try
            if task.Wait(timeout) then
                Success(task.Result)
            else
                Error(InvalidInputFormat "Computation timed out")
        with
        | :? System.Threading.Tasks.TaskCanceledException -> Error(InvalidInputFormat "Computation was canceled")
        | ex -> Error(InvalidInputFormat $"Computation failed: {ex.Message}")
