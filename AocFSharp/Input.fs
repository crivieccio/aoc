namespace AocFSharp

open System
open System.IO
open Types

/// Input file management with error handling
module Input =

    let getInputPath (dayNumber: int) (isExample: bool) =
        let dayStr = Common.formatDay dayNumber
        let exampleSuffix = if isExample then "_example" else ""
        $"../aoc25/Inputs/day{dayStr}{exampleSuffix}.txt"

    /// Read input file for a specific day
    let readInput (dayNumber: int) : AocResult<string list> =
        let filePath = getInputPath dayNumber false

        try
            if File.Exists(filePath) then
                File.ReadAllLines(filePath)
                |> Array.toList
                |> Success
            else
                Error(InputFileNotFound dayNumber)
        with
        | :? IOException as ex -> Error(InvalidInputFormat $"IO error reading input: {ex.Message}")
        | ex -> Error(InvalidInputFormat $"Unexpected error reading input: {ex.Message}")

    /// Read example input file for a specific day
    let readExampleInput (dayNumber: int) : AocResult<string list> =
        let filePath = getInputPath dayNumber true

        try
            if File.Exists filePath then
                File.ReadAllLines filePath
                |> Array.toList
                |> Success
            else
                Error(InputFileNotFound dayNumber)
        with
        | :? IOException as ex -> Error(InvalidInputFormat $"IO error reading example input: {ex.Message}")
        | ex -> Error(InvalidInputFormat $"Unexpected error reading example input: {ex.Message}")

    /// Read and validate input, returning clean lines
    let readAndCleanInput (dayNumber: int) : AocResult<string list> =
        match readInput dayNumber with
        | Success lines -> Success(List.filter (fun line -> not (String.IsNullOrWhiteSpace line)) lines)
        | Error e -> Error e

    /// Read and validate example input
    let readAndCleanExampleInput (dayNumber: int) : AocResult<string list> =
        match readExampleInput dayNumber with
        | Success lines -> Success(List.filter (fun line -> not (String.IsNullOrWhiteSpace line)) lines)
        | Error e -> Error e
