using System.Reflection;
using System.Text.RegularExpressions;
using AdventOfCode.Helpers.Grid;
using static System.Console;

namespace AdventOfCode.Helpers;

public abstract partial class Day
{
    private readonly string day;
    private readonly string year;

    protected Day()
    {
        var assemblyName = Assembly.GetAssembly(GetType())!.GetName().Name!;
        var className = GetType().Name;

        year = AssemblyNamePattern().Match(assemblyName).Groups["year"].Value;
        day = ClassNamePattern().Match(className).Groups["day"].Value;

        Input = File.ReadAllLines($"Inputs/{year}-{day}.txt");
        Grid = Input.Select(line => line.ToCharArray()).ToArray();
    }

    protected string[] Input { get; }

    protected char[][] Grid { get; }

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

    protected Cell Cell(int i, int j)
    {
        return new Cell(Grid, i, j);
    }

    protected IEnumerable<Cell> Range(int minI, int maxI, int minJ, int maxJ)
    {
        for (var i = minI; i <= maxI; i++)
        {
            for (var j = minJ; j <= maxJ; j++)
            {
                yield return Cell(i, j);
            }
        }
    }

    [GeneratedRegex(@"^AdventOfCode\.Year(?<year>\d{4})$")]
    private static partial Regex AssemblyNamePattern();

    [GeneratedRegex(@"^Day(?<day>\d{2})$")]
    private static partial Regex ClassNamePattern();
}