using System.Security.Cryptography;

namespace Solutions;


public class Day11 : BaseDay
{
    public override string Part1(List<string> input)
    {
        var map = input.Parse();
        var startingPoint = map["you"];
        var queue = new Queue<string>();
        foreach (var s in startingPoint) queue.Enqueue(s);
        var total = 0;
        while (queue.TryDequeue(out var s))
        {
            if (s == "out")
            {
                total++;
                continue;
            }
            var connected = map[s];
            foreach (var connection in connected) queue.Enqueue(connection);
        }
        return total.ToString();
    }
    public override string Part2(List<string> input)
    {
        var map = input.ParsePart2(false);
        return FindPath("svr", map, [], false, false).ToString();
    }

    private static long FindPath(string device, Dictionary<string, HashSet<string>> devices, Dictionary<(string, bool, bool), long> cache, bool dacVisited, bool fftVisited)
    {
        var result = 0L;

        if (device == "out")
        {
            return dacVisited && fftVisited ? 1L : 0L;
        }

        if (cache.TryGetValue((device, dacVisited, fftVisited), out var value))
        {
            return value;
        }

        dacVisited |= device == "dac";
        fftVisited |= device == "fft";

        foreach (var connectedDevice in devices[device])
        {
            var intermediateResult = FindPath(connectedDevice, devices, cache, dacVisited, fftVisited);

            result += cache[(connectedDevice, dacVisited, fftVisited)] = intermediateResult;
        }

        return result;
    }
}

public static class Day11Extensions
{
    extension(List<string> source)
    {
        public Dictionary<string, HashSet<string>> Parse() =>
            source
            .Select(x => x.Split(':', StringSplitOptions.RemoveEmptyEntries))
            .ToDictionary(array => array[0], array => array[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet());

        public Dictionary<string, HashSet<string>> ParsePart2(bool example) =>
            example ? source.Skip(11).Select(x => x.Split(':', StringSplitOptions.RemoveEmptyEntries))
            .ToDictionary(array => array[0], array => array[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet()) :
            source.Parse();
    }
}