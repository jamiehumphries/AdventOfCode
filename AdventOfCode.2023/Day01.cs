using AdventOfCode.Helpers;
using static System.StringComparison;

namespace AdventOfCode.Year2023;

public class Day01 : Day
{
    private static readonly Dictionary<string, int> CalibrationValues = new()
    {
        { "0", 0 },
        { "1", 1 },
        { "2", 2 },
        { "3", 3 },
        { "4", 4 },
        { "5", 5 },
        { "6", 6 },
        { "7", 7 },
        { "8", 8 },
        { "9", 9 },
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 }
    };

    protected override object PartOne()
    {
        var calibrationValues = Input.Select(GetCalibrationValueFromDigitsOnly);
        return calibrationValues.Sum();
    }

    protected override object PartTwo()
    {
        var calibrationValues = Input.Select(GetCalibrationValue);
        return calibrationValues.Sum();
    }

    private static int GetCalibrationValueFromDigitsOnly(string line)
    {
        var firstDigit = line.First(char.IsDigit).ToString();
        var lastDigit = line.Last(char.IsDigit).ToString();
        return int.Parse(firstDigit + lastDigit);
    }

    private static int GetCalibrationValue(string line)
    {
        var firstMatch = GetFirstCalibrationKey(line);
        var firstDigit = CalibrationValues[firstMatch].ToString();
        var lastMatch = GetLastCalibrationKey(line);
        var lastDigit = CalibrationValues[lastMatch].ToString();
        return int.Parse(firstDigit + lastDigit);
    }

    private static string GetFirstCalibrationKey(string line)
    {
        return CalibrationValues.Keys.MinBy(key =>
        {
            var index = line.IndexOf(key, Ordinal);
            return index == -1 ? int.MaxValue : index;
        }) ?? string.Empty;
    }

    private static string GetLastCalibrationKey(string line)
    {
        return CalibrationValues.Keys.MaxBy(key =>
        {
            var index = line.LastIndexOf(key, Ordinal);
            return index == -1 ? int.MinValue : index + key.Length - 1;
        }) ?? string.Empty;
    }
}