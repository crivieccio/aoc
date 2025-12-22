namespace AocFSharp.Days

open AocFSharp.Types
open AocFSharp.BaseDay

module Day03 =
    open System.Text

    let getLargest convert bankSize (joltage: string) =
        let rec loop count (list: string) (acc: StringBuilder) =
            match count with
            | 0 -> acc.ToString() |> convert
            | _ ->
                let max = list.[0..^(count - 1)] |> Seq.max

                loop (count - 1) list.[list.IndexOf max + 1..] (acc.Append max)

        loop bankSize joltage (new StringBuilder())

    let part1 (input: string list) : PartResult =
        timeComputation (fun () -> input |> List.map (getLargest int 2) |> List.sum)

    let part2 (input: string list) : PartResult =
        timeComputation (fun () ->
            input
            |> List.map (getLargest int64 12)
            |> List.sum)

    let solution =
        { DayNumber = 3
          Part1 = part1
          Part2 = part2 }
