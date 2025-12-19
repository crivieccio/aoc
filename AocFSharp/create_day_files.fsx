// Script to create remaining Day files
let template =
    """namespace AocFSharp.Days
open AocFSharp.Types
open AocFSharp.BaseDay

module DayXX =

    let part1 (input: string list) : PartResult =
        timeComputation (fun () ->
            // TODO: Implement Day XX Part 1
            "42")

    let part2 (input: string list) : PartResult =
        timeComputation (fun () ->
            // TODO: Implement Day XX Part 2
            "43")

    let solution = {
        DayNumber = XX
        Part1 = part1
        Part2 = part2
    }"""

for day in 1 .. 12 do
    let content =
        template
            .Replace("XX", sprintf "%02d" day)
            .Replace("Day XX", sprintf "Day%02d" day)
            .Replace("Day XX", sprintf "Day%02d" day)

    let fileName = sprintf "Days/Day%02d.fs" day
    System.IO.File.WriteAllText(fileName, content)
    printfn "Created %s" fileName

printfn "All day files created!"
