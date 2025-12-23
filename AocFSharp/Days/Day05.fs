namespace AocFSharp.Days

open AocFSharp.Types
open AocFSharp.BaseDay

module Day05 =
    open System
    type Range = { low: int64; high: int64 }

    let parse strings =
        strings
        |> List.takeWhile (fun s -> s |> String.exists (fun c -> c = '-'))
        |> List.map (fun s ->
            let split =
                s.Split('-', StringSplitOptions.RemoveEmptyEntries)

            { low = split.[0] |> int64
              high = split.[1] |> int64 }),
        strings
        |> List.skipWhile (fun s -> s |> String.exists (fun c -> c = '-'))
        |> List.map int64

    let part1 (input: string list) : PartResult =
        timeComputation (fun () ->
            let ranges, ids = input |> parse

            ids
            |> List.map (fun id ->
                ranges
                |> List.exists (fun range -> range.low <= id && id <= range.high))
            |> List.filter id
            |> List.length)

    let part2 (input: string list) : PartResult =
        timeComputation (fun () ->
            let ranges, _ = input |> parse

            ranges
            |> List.sortBy (fun range -> range.low)
            |> List.fold
                (fun (sum, max) range ->
                    let low, high = range.low, range.high

                    let low = if max >= low then max + 1L else low

                    let high = if max >= high then max else high
                    let sum = sum + high - low + 1L
                    sum, high)
                (0, Int64.MinValue)
            |> fst)

    let solution =
        { DayNumber = 5
          Part1 = part1
          Part2 = part2 }
