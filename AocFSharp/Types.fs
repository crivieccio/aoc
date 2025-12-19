namespace AocFSharp

module Types =
    open System


    /// Result of running a part solution
    type PartResult =
        { Value: string
          ExecutionTime: TimeSpan }

    /// Complete day solution with both parts
    type DaySolution =
        { DayNumber: int
          Part1: string list -> PartResult
          Part2: string list -> PartResult }

    /// Common direction type
    type Direction =
        | Left
        | Right

    /// Turn instruction
    type Turn = { Direction: Direction; Amount: int }

    /// Processing result for a single line
    type LineResult = { Value: int; Index: int }

    /// Error types for better error handling
    type AocError =
        | InputFileNotFound of int
        | InvalidDayNumber of string
        | InvalidPartNumber of string
        | SolutionNotFound of int
        | InvalidInputFormat of string

    /// Result type for operations
    type AocResult<'T> =
        | Success of 'T
        | Error of AocError

    /// Utility module for common functions
    module Common =
        let formatDay (dayNumber: int) = sprintf "%02d" dayNumber

        let validateDay (dayStr: string) : AocResult<int> =
            match Int32.TryParse dayStr with
            | true, day when day >= 1 && day <= 12 -> Success day
            | _ -> Error(InvalidDayNumber dayStr)

        let validatePart (partStr: string) : AocResult<int> =
            match Int32.TryParse partStr with
            | true, part when part = 1 || part = 2 -> Success part
            | _ -> Error(InvalidPartNumber partStr)
