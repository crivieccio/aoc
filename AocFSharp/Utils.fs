namespace AocFSharp

open System
open System.Collections.Generic
open System.Linq

/// Utility functions for common operations
module Utils =

    /// Safe list indexing
    let inline safeIndex (list: 'T list) (index: int) : 'T option =
        if index >= 0 && index < List.length list then
            Some(List.item index list)
        else
            None

    /// Create a list of numbers in range
    let range (start: int) (count: int) : int list =
        [ for i in start .. start + count - 1 -> i ]

    /// Split list into chunks of specified size
    let chunk (size: int) (list: 'T list) : 'T list list = list |> List.chunkBySize size

    /// Remove duplicates from list while preserving order
    let distinct (list: 'T list) : 'T list when 'T: equality = list |> List.distinct

    /// Count occurrences of each element
    let frequencies (list: 'T list) : Map<'T, int> when 'T: equality = list |> List.countBy id |> Map.ofList

    /// Find all indices where predicate is true
    let findIndices (predicate: 'T -> bool) (list: 'T list) : int list =
        list
        |> List.mapi (fun i item -> if predicate item then Some i else None)
        |> List.choose id

    /// Rotate a list left by n positions
    let rotateLeft (n: int) (list: 'T list) : 'T list =
        let len = List.length list

        if len = 0 then
            list
        else
            let n' = ((n % len) + len) % len // Handle negative rotations
            let splitPoint = len - n'
            List.append (List.skip splitPoint list) (List.take splitPoint list)

    /// Rotate a list right by n positions
    let rotateRight (n: int) (list: 'T list) : 'T list = rotateLeft (-n) list

    /// Get all combinations of k elements from list
    let combinations (k: int) (list: 'T list) : 'T list list when 'T: equality =
        let rec combinations acc n l =
            match n, l with
            | 0, _ -> [ acc ]
            | _, [] -> []
            | _, x :: xs ->
                (combinations (x :: acc) (n - 1) xs)
                @ (combinations acc n xs)

        combinations [] k list

    /// Calculate Manhattan distance between two points
    let manhattanDistance (x1: int, y1: int) (x2: int, y2: int) = abs (x2 - x1) + abs (y2 - y1)

    /// Convert string to char array and process
    let chars (str: string) = str.ToCharArray() |> Array.toList

    /// Check if string is palindrome
    let isPalindrome (str: string) =
        let chars = str.ToCharArray()
        let reversed = Array.rev chars |> Array.toList
        let original = Array.toList chars
        reversed = original

    /// Convert int to binary string
    let toBinary (n: int) = Convert.ToString(n, 2)

    /// Convert binary string to int
    let fromBinary (binary: string) = Convert.ToInt32(binary, 2)

    /// Calculate GCD of two numbers
    let gcd (a: int) (b: int) : int =
        let rec gcd' a b = if b = 0 then abs a else gcd' b (a % b)
        gcd' a b

    /// Calculate LCM of two numbers
    let lcm (a: int) (b: int) =
        if a = 0 || b = 0 then
            0
        else
            abs (a * b) / (gcd a b)

    /// Create a 2D array filled with default value
    let create2DArray (rows: int) (cols: int) (defaultValue: 'T) : 'T [,] = Array2D.create rows cols defaultValue

    /// Get neighbors in 4 directions (up, down, left, right)
    let get4Neighbors (x: int) (y: int) (maxX: int) (maxY: int) =
        [ (x - 1, y)
          (x + 1, y)
          (x, y - 1)
          (x, y + 1) ]
        |> List.filter (fun (nx, ny) -> nx >= 0 && nx < maxX && ny >= 0 && ny < maxY)

    /// Get neighbors in 8 directions (including diagonals)
    let get8Neighbors (x: int) (y: int) (maxX: int) (maxY: int) =
        [ for dx in -1 .. 1 do
              for dy in -1 .. 1 do
                  if dx <> 0 || dy <> 0 then
                      yield (x + dx, y + dy) ]
        |> List.filter (fun (nx, ny) -> nx >= 0 && nx < maxX && ny >= 0 && ny < maxY)

    /// Convert coordinates to linear index in 2D array
    let toLinearIndex (x: int) (y: int) (width: int) = y * width + x

    /// Convert linear index to coordinates in 2D array
    let fromLinearIndex (index: int) (width: int) =
        let x = index % width
        let y = index / width
        (x, y)
