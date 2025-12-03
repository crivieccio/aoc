
using System.Net.Sockets;
using System.Text;

namespace Solutions;

public class Day3 : BaseDay
{
  private static readonly int _batteryBankSize = 12;
  public override string Part1(List<string> input) => GetJoltage(input).Sum().ToString();

  public override string Part2(List<string> input) => GetLargestJoltage(input).Sum().ToString();

  private static IEnumerable<int> GetJoltage(List<string> input)
  {
    foreach (var line in input)
    {
      var start = 0;
      var highest = -1;
      for (var i = start; i < line.Length; i++)
      {
        var next = i + 1;
        while (next < line.Length)
        {
          var current = int.Parse($"{line[i]}{line[next]}");
          if (current > highest) highest = current;
          next++;
        }
      }
      yield return highest;
    }
  }

  private static IEnumerable<long> GetLargestJoltage(List<string> input)
  {
    var builder = new StringBuilder();
    foreach (var line in input)
    {
      var startIdx = FindStart(line);
      builder.Append(line[startIdx]);
      var next = startIdx + 1;
      while (builder.Length < _batteryBankSize && next < line.Length - 1)
      {
        //Take the larger of next or next + 1
        var current = long.Parse($"{line[next]}");
        var nextVal = long.Parse($"{line[next + 1]}");
        if (nextVal > current) current = nextVal;
        builder.Append(current);
        next += 2;

        //if builder.length + line[next..] == 12 append line and yield return
        if (builder.Length + line[next..].Length == _batteryBankSize)
        {
          builder.Append(line[next..]);
          var testInner = builder.ToString();
          yield return long.Parse(builder.ToString());
        }
      }
      var test = builder.ToString();
      yield return long.Parse(builder.ToString());
      builder.Clear();
    }
  }

  private static int FindStart(string line)
  {
    var rest = line[..^_batteryBankSize];
    var highest = int.Parse(rest[0].ToString());
    var highestIdx = 0;
    for (var i = 1; i < rest.Length; i++)
    {
      var current = int.Parse(rest[i].ToString());
      if (current > highest) highest = i;
    }
    return highestIdx;
  }
}