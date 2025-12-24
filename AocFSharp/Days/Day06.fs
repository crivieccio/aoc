namespace AocFSharp.Days

open AocFSharp.Types
open AocFSharp.BaseDay
open AocFSharp.Utils

module Day06 =
    open System

    type Operation =
        | Addition
        | Multiplication
        | Invalid

    type Operands = int64 list

    type Problem =
        { Operands: Operands
          Operation: Operation }

    let parseOperations (operations: string) =
        operations.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (fun s ->
            match s with
            | "+" -> Addition
            | "*" -> Multiplication
            | _ -> Invalid)
        |> List.ofArray

    let topToBottom operations (numbers: int64 list list) =
        operations
        |> List.mapi (fun index op ->
            let ops =
                numbers
                |> List.mapi (fun row _ -> numbers.[index].[row])

            { Operands = ops; Operation = op })

    let parse (problems: string list) =
        problems
        |> List.takeWhile (fun s -> (s, problems |> List.last) |> String.Equals |> not)
        |> List.map (fun s ->
            s.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            |> Array.map int64
            |> List.ofArray)


    let part1 (input: string list) : PartResult =
        timeComputation (fun () -> input |> parse)

    let part2 (input: string list) : PartResult =
        timeComputation (fun () ->
            // TODO: Implement Day 06 Part 2
            "43")

    let solution =
        { DayNumber = 6
          Part1 = part1
          Part2 = part2 }
