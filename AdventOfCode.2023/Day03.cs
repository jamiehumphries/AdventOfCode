using System.Text.RegularExpressions;
using AdventOfCode.Helpers;
using AdventOfCode.Helpers.Grid;

namespace AdventOfCode.Year2023;

public partial class Day03 : Day
{
    private readonly IEnumerable<int> partNumbers;
    private readonly Dictionary<Cell, List<int>> potentialGears = [];

    public Day03()
    {
        partNumbers = Input.SelectMany((row, i) =>
        {
            var lineMatches = NumberPattern().Matches(row);
            return lineMatches
                .Where(match =>
                {
                    var j = match.Index;
                    var cells = CellRange(i, i, j, j + match.Length - 1).ToList();
                    var neighbours = cells.SelectMany(cell => cell.Neighbours()).Except(cells).ToHashSet();

                    var gears = neighbours.Where(cell => cell.Value == '*');
                    foreach (var gear in gears)
                    {
                        if (!potentialGears.TryGetValue(gear, out _))
                        {
                            potentialGears.Add(gear, []);
                        }

                        potentialGears[gear].Add(PartNumber(match));
                    }

                    return neighbours.Any(IsSymbol);
                })
                .Select(PartNumber);
        });
    }

    protected override object PartOne()
    {
        return partNumbers.Sum();
    }

    protected override object PartTwo()
    {
        var gears = potentialGears.Values.Where(parts => parts.Count == 2);
        return gears.Sum(gear => gear[0] * gear[1]);
    }

    private static bool IsSymbol(Cell cell)
    {
        var value = cell.Value;
        return !char.IsLetterOrDigit(value) && value != '.';
    }

    private static int PartNumber(Capture match)
    {
        return int.Parse(match.Value);
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumberPattern();
}