using System.Reflection;
using System.Text.RegularExpressions;
using static System.Console;

namespace AdventOfCode.Helpers;

public abstract partial class Puzzle
{
    private readonly string day;
    private readonly string year;

    protected Puzzle()
    {
        var assemblyName = Assembly.GetAssembly(GetType())!.GetName().Name!;
        var className = GetType().Name;

        year = AssemblyNamePattern().Match(assemblyName).Groups["year"].Value;
        day = ClassNamePattern().Match(className).Groups["day"].Value;

        Input = File.ReadAllLines($"Inputs/{year}-{day}.txt");
    }

    protected string[] Input { get; init; }

    public void Run()
    {
        WriteLine($"Advent of Code {year} - Day {day}");
        WriteLine();

        var partOneAnswer = PartOne();
        WriteLine("Part One");
        WriteLine(partOneAnswer);

        var partTwoAnswer = PartTwo();
        if (partTwoAnswer == null)
        {
            return;
        }

        WriteLine();
        WriteLine("Part Two");
        WriteLine(partTwoAnswer);
    }

    protected abstract object PartOne();

    protected abstract object? PartTwo();

    [GeneratedRegex(@"^AdventOfCode\.Year(?<year>\d{4})$")]
    private static partial Regex AssemblyNamePattern();

    [GeneratedRegex(@"^Day(?<day>\d{2})$")]
    private static partial Regex ClassNamePattern();
}