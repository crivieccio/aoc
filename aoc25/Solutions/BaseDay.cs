namespace Solutions;

public abstract class BaseDay
{
    public static List<string> ReadInput(string fileName) => [.. File.ReadAllLines(fileName)];

    public abstract string Part1(List<string> input);
    public abstract string Part2(List<string> input);
}