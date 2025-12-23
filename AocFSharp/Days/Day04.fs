namespace AocFSharp.Days

open AocFSharp.Types
open AocFSharp.BaseDay
open AocFSharp.Utils

module Day04 =

    let buildGrid (strings: string list) =
        match strings with
        | [] -> Array2D.create 0 0 ' '
        | _ ->
            let rows = strings |> List.length
            let cols = strings |> List.head |> String.length
            Array2D.init rows cols (fun row col -> strings.[row].[col])

    let getNeighborCounts grid =
        let maxX = grid |> Array2D.length1
        let maxY = grid |> Array2D.length2

        grid
        |> Array2D.mapi (fun row col value ->
            get8Neighbors row col maxX maxY
            |> List.fold
                (fun acc (nX, nY) ->
                    match grid.[nX, nY], value with
                    | '@', '@' -> acc + 1
                    | '.', '@' -> acc
                    | '@', '.' -> acc
                    | '.', '.' -> acc
                    | _, _ -> 0)
                0)
        |> Array2D.mapi (fun row col value ->
            match grid.[row, col] with
            | '.' -> false, value
            | '@' -> true, value
            | _ -> false, value)

    let canAccess grid =
        grid
        |> getNeighborCounts
        |> flatten2DArray
        |> Seq.filter (fun (isRoll, count) -> isRoll && count < 4)

    let removableRolls grid =
        let rec loop removable removed acc =
            match removed with
            | 0 -> acc
            | _ ->
                let newGrid =
                    removable
                    |> Array2D.map (fun (isRoll, count) ->
                        match isRoll, count with
                        | false, _ -> '.'
                        | _, count when count < 4 -> '.'
                        | _, _ -> '@')

                loop (newGrid |> getNeighborCounts) (newGrid |> canAccess |> Seq.length) (acc + removed)

        let removable = grid |> canAccess |> Seq.length
        loop (grid |> getNeighborCounts) removable 0

    let part1 (input: string list) : PartResult =
        timeComputation (fun () -> input |> buildGrid |> canAccess |> Seq.length)

    let part2 (input: string list) : PartResult =
        timeComputation (fun () -> input |> buildGrid |> removableRolls)

    let solution =
        { DayNumber = 4
          Part1 = part1
          Part2 = part2 }
