using AdventOfCode.Helpers;

namespace AdventOfCode.Year2023;

public class Day06 : Day
{
    protected override object PartOne()
    {
        var records = new List<Record>
        {
            new(40, 215),
            new(92, 1064),
            new(97, 1505),
            new(90, 1100)
        };
        return records.Product(r => r.WaysToWin());
    }

    protected override object PartTwo()
    {
        var record = new Record(40_92_97_90, 215_1064_1505_1100);
        return record.WaysToWin();
    }

    private record Record(int Time, long Distance)
    {
        public int WaysToWin()
        {
            var t = Enumerable.Range(0, Time).First(ms => ms * (long)(Time - ms) > Distance);
            return Time + 1 - 2 * t;
        }
    }
}