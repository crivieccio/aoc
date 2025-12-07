
using System.Text;

namespace Solutions;

public class Day6 : BaseDay
{
    public override string Part1(List<string> input)
    {
        var numbers = ParseNumbers(input);
        var operations = ParseOperations(input);

        var total = 0L;
        for (var i = 0; i < operations.Count; i++)
        {
            var operands = CollectOperands(i, numbers);
            total += operations[i] switch
            {
                Operation.Addition => operands.Sum(),
                Operation.Multiplication => operands.Aggregate(1L, (a, b) => a * b),
                _ => throw new InvalidOperationException("How did we get here?")
            };
        }
        return total.ToString();
    }

    public override string Part2(List<string> input) => Aggregate(ParseVertically(input)).ToString();

    private static List<long> CollectOperands(int index, long[][] inputs)
    {
        List<long> operands = [];
        for (var i = 0; i < inputs.Length; i++)
        {
            operands.Add(inputs[i][index]);
        }

        return operands;
    }

    private static ulong Aggregate(IEnumerable<string> fields)
    {
        ulong sum = 0UL;
        List<ulong> pending = [];

        foreach (var field in fields)
        {
            if (field == "*")
            {
                sum += pending.Aggregate(1UL, (acc, val) => acc * val);
                pending.Clear();
            }
            else if (field == "+")
            {
                sum += pending.Aggregate(0UL, (acc, val) => acc + val);
                pending.Clear();
            }
            else if (ulong.TryParse(field, out var number))
            {
                pending.Add(number);
            }
            else
            {
                throw new InvalidDataException($"Invalid field: {field}");
            }
        }

        return sum;
    }

    private static IEnumerable<string> ParseVertically(List<string> rawInput)
    {
        for (int col = rawInput.Max(line => line.Length) - 1; col >= 0; col--)
        {
            int row = 0;
            while (row < rawInput.Count)
            {
                while (row < rawInput.Count && (col >= rawInput[row].Length || rawInput[row][col] == ' ')) row++;

                var segment = new StringBuilder();
                while (row < rawInput.Count && col < rawInput[row].Length && rawInput[row][col] != ' ')
                {
                    if ("*+".Contains(rawInput[row][col]) && segment.Length > 0)
                    {
                        yield return segment.ToString();
                        segment.Clear();
                    }
                    segment.Append(rawInput[row++][col]);
                }

                if (segment.Length > 0) yield return segment.ToString();
            }
        }
    }

    private static long[][] ParseNumbers(List<string> input) =>
        [.. input[.. ^1]
        .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        .Select(x =>
            x
            .TakeWhile(x => long.TryParse(x, out var number))
            .Select(long.Parse)
            .ToArray())];

    private static List<Operation> ParseOperations(List<string> input) =>
        [.. input[^1]
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(c => c == "+" ? Operation.Addition : Operation.Multiplication)];

    private enum Operation
    {
        Addition,
        Multiplication,
    }
}