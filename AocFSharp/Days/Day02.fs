namespace AocFSharp.Days

open AocFSharp.Types
open AocFSharp.BaseDay

module Day02 =
    open FSharp.Text.RegexProvider
    open System

    type Parser = Regex<pattern= @"(?<Start>\d+)(?:-)(?<Ends>\d+)+(?:,|$)+">
    type Part2 = Regex<pattern= @"^(\d+)(\1)+$">
    type Range = { Start: int64; End: int64 }

    let parse (strings: string list) =
        strings
        |> List.collect (fun s ->
            s
            |> Parser().TypedMatches
            |> Seq.map (fun m ->
                { Start = m.Start.Value |> int64
                  End = m.Ends.Value |> int64 })
            |> Seq.toList)

    let testId id =
        let asString = id |> string
        let len = asString |> String.length
        let half = len / 2
        let firstHalf = asString.Substring(0, half)
        let secondHalf = asString.Substring half
        String.Equals(firstHalf, secondHalf)

    let invalidIds filterF ranges =
        ranges
        |> List.collect (fun r -> [ r.Start .. r.End ])
        |> List.filter filterF
        |> List.sum

    let part1 (input: string list) : PartResult =
        timeComputation (fun () -> input |> parse |> invalidIds testId)

    let part2 (input: string list) : PartResult =
        timeComputation (fun () ->
            input
            |> parse
            |> invalidIds (string >> Part2().IsMatch))

    let solution =
        { DayNumber = 2
          Part1 = part1
          Part2 = part2 }
