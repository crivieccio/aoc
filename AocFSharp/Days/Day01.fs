namespace AocFSharp.Days

open AocFSharp.Types
open AocFSharp.BaseDay
open FSharp.Text.RegexProvider

module Day01 =
    open System

    type Direction =
        | Left
        | Right

    type Turn = { Direction: Direction; Amount: int64 }
    type RotationRegex = Regex<pattern= @"(?<Direction>[LR])(?<Amount>\d+)">

    let parse lines =
        lines
        |> List.map (fun l ->
            let test = l |> RotationRegex().TypedMatch

            { Direction =
                test.Direction.Value
                |> function
                    | "L" -> Left
                    | _ -> Right
              Amount = test.Amount.Value |> int64 })

    let calculateNewPosition start turn =
        match turn.Direction with
        | Left -> (100L + (start - turn.Amount)) % 100L
        | Right -> (100L + start + turn.Amount) % 100L

    let zeroes total start turn =
        let max = turn.Amount - 1L

        [ 0L .. max ]
        |> List.fold
            (fun (acc, position) _ ->
                let newPos =
                    calculateNewPosition position { turn with Amount = 1L }

                match newPos with
                | 0L -> acc + 1L, newPos
                | _ -> acc, newPos)
            (total, start)
        |> fst,
        calculateNewPosition start turn

    let fullRotations start (turns: Turn list) =
        turns
        |> List.fold (fun (acc, nextPos) t -> zeroes acc nextPos t) (0L, start)
        |> fst

    let part1 (input: string list) : PartResult =
        timeComputation (fun () ->
            input
            |> parse
            |> List.scan calculateNewPosition 50
            |> List.filter (fun x -> x = 0)
            |> List.length)

    let part2 (input: string list) : PartResult =
        timeComputation (fun () -> input |> parse |> fullRotations 50)

    let solution =
        { DayNumber = 1
          Part1 = part1
          Part2 = part2 }
