
namespace Solutions;

public class Day4 : BaseDay
{
  public override string Part1(List<string> input)
  {
    var grid = BuildGrid(input);
    var positions = new List<Position>();
    for (var y = 0; y < grid.Length; y++)
    {
      for (var x = 0; x < grid[y].Length; x++)
      {
        positions.Add(new Position(y, x));
      }
    }

    var count = 0;
    foreach (var position in positions)
    {
      if (CanAccess(position, grid) && grid[position.X][position.Y] == '@')
      {
        count++;
      }
    }

    return count.ToString();
  }

  public override string Part2(List<string> input)
  {

    var grid = BuildGrid(input);
    var positions = new List<Position>();
    for (var x = 0; x < grid.Length; x++)
    {
      for (var y = 0; y < grid[0].Length; y++)
      {
        positions.Add(new Position(x, y));
      }
    }

    return Calculate(positions, grid).ToString();
  }

  private static int Calculate(List<Position> positions, char[][] grid)
  {
    var (count, removable) = RemovableRolls(positions, grid);
    if (count == 0) return count;

    for (var x = 0; x < grid.Length; x++)
    {
      for (var y = 0; y < grid[0].Length; y++)
      {
        if (removable.Contains(new(x, y))) grid[x][y] = '.';
      }
    }

    return count + Calculate(positions, grid);
  }
  private static (int, List<Position>) RemovableRolls(List<Position> positions, char[][] grid)
  {
    var removable = new List<Position>();
    foreach (var position in positions)
    {
      if (CanAccess(position, grid) && grid[position.X][position.Y] == '@') removable.Add(position);
    }
    return (removable.Count, removable);
  }
  private static bool CanAccess(Position position, char[][] grid, int maxNeighbors = 4)
  {
    var neighbors = GetNeighbors(position, grid);
    var count = neighbors.Count(n => grid[n.X][n.Y] == '@');
    return count < maxNeighbors;
  }
  private static List<Position> GetNeighbors(Position position, char[][] grid)
  {
    var neighbors = new List<Position>();
    var directions = new List<(int dx, int dy)> { (0, 1), (1, 0), (0, -1), (-1, 0), (-1, -1), (1, -1), (-1, 1), (1, 1) };
    foreach (var (dx, dy) in directions)
    {
      var newX = position.X + dx;
      var newY = position.Y + dy;
      if (newX >= 0 && newX < grid[0].Length && newY >= 0 && newY < grid.Length)
      {
        neighbors.Add(new Position(newX, newY));
      }
    }
    return neighbors;
  }

  private static char[][] BuildGrid(List<string> input) => [.. input.Select(line => line.ToCharArray())];

  private record Position(int X, int Y);
}