using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Solutions;

public partial class Day1 : BaseDay
{
  public override string Part1(List<string> input)
  {
    var startingNumber = 50;
    var regex = MyRegex();
    return input.Select(l =>
    {
      var match = regex.Match(l);
      var dir = match.Groups[1].Value == "L" ? Direction.Left : Direction.Right;
      var amount = int.Parse(match.Groups[2].Value);
      return new Turns(dir, amount);
    })
    .Select(t =>
    {
      startingNumber = CalculateNewPosition(startingNumber, t);
      return startingNumber;
    })
    .Count(x => x == 0)
    .ToString();
  }

  public override string Part2(List<string> input)
  {
    var startingNumber = 50;
    var fullRotations = 0;
    var regex = MyRegex();
    var turns = input.Select(l =>
    {
      var match = regex.Match(l);
      var dir = match.Groups[1].Value == "L" ? Direction.Left : Direction.Right;
      var amount = int.Parse(match.Groups[2].Value);
      return new Turns(dir, amount);
    });

    foreach (var turn in turns)
    {
      var (rotations, position) = FullRotations(startingNumber, turn);
      fullRotations += rotations;
      startingNumber = position;
    }

    return fullRotations.ToString();
  }

  public record Turns(Direction Direction, int Amount);
  public enum Direction { Left, Right }

  [GeneratedRegex("([LR])(\\d+)")]
  private static partial Regex MyRegex();

  private static int CalculateNewPosition(int startingNumber, Turns turn) => turn.Direction switch
  {
    Direction.Left => (100 + (startingNumber - turn.Amount)) % 100,
    Direction.Right => (100 + startingNumber + turn.Amount) % 100,
    _ => throw new InvalidOperationException("Invalid direction")
  };

  private static (int, int) FullRotations(int startingNumber, Turns turn)
  {
    var steps = Enumerable.Range(0, turn.Amount);
    var zeroes = 0;
    var position = startingNumber;
    foreach (var step in steps)
    {
      var newPosition = CalculateNewPosition(position, turn with { Amount = 1 });
      if (newPosition == 0) zeroes++;
      position = newPosition;
    }
    return (zeroes, CalculateNewPosition(startingNumber, turn));
  }
}
