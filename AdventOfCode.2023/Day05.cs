using System.Text.RegularExpressions;
using AdventOfCode.Helpers;

namespace AdventOfCode.Year2023;

public partial class Day05 : Day
{
    private readonly List<Map> maps = [];
    private readonly List<long> seedData;

    public Day05()
    {
        seedData = Input[0]["seeds: ".Length..].Split(" ").Select(long.Parse).ToList();

        Map? map = null;
        foreach (var line in Input.Skip(2))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var mapMatch = MapLinePattern().Match(line);
            if (mapMatch.Success)
            {
                var groups = mapMatch.Groups;
                map = new Map(groups["source"].Value, groups["destination"].Value);
                maps.Add(map);
                continue;
            }

            map!.AddRange(line);
        }
    }

    protected override object PartOne()
    {
        return seedData.Min(GetLocation);
    }

    protected override object PartTwo()
    {
        var seedRanges = new List<Range>();
        for (var i = 0; i < seedData.Count; i += 2)
        {
            var start = seedData[i];
            var length = seedData[i + 1];
            var end = start + length - 1;
            seedRanges.Add(new Range(start, end));
        }

        var locationRanges = GetLocationRanges(seedRanges);
        return locationRanges.Min(r => r.Start);
    }

    private long GetLocation(long seed)
    {
        var category = "seed";
        var value = seed;

        while (category != "location")
        {
            var map = maps.Single(m => m.SourceCategory == category);
            value = map.GetDestination(value);
            category = map.DestinationCategory;
        }

        return value;
    }

    private IEnumerable<Range> GetLocationRanges(IEnumerable<Range> seedRanges)
    {
        var category = "seed";
        var ranges = seedRanges;

        while (category != "location")
        {
            var map = maps.Single(m => m.SourceCategory == category);
            ranges = ranges.SelectMany(map.GetDestinationRanges);
            category = map.DestinationCategory;
        }

        return ranges;
    }

    [GeneratedRegex("(?<source>[a-z]+)-to-(?<destination>[a-z]+) map:")]
    private static partial Regex MapLinePattern();

    private class Map(string sourceCategory, string destinationCategory)
    {
        private IEnumerable<MapRange> mapRanges = [];

        public string SourceCategory { get; } = sourceCategory;
        public string DestinationCategory { get; } = destinationCategory;

        public void AddRange(string line)
        {
            var data = line.Split(' ').Select(long.Parse).ToList();
            var range = new MapRange(data[0], data[1], data[2]);
            mapRanges = mapRanges.Append(range).OrderBy(r => r.SourceRangeStart);
        }

        public long GetDestination(long source)
        {
            var range = GetMapRange(source);
            return range?.GetDestination(source) ?? source;
        }

        public IEnumerable<Range> GetDestinationRanges(Range range)
        {
            var sourceStart = range.Start;
            while (sourceStart <= range.End)
            {
                var destinationStart = GetDestination(sourceStart);

                var mapRange = GetMapRange(sourceStart);
                var nextSourceStart = mapRange != null
                    ? mapRange.SourceRangeEnd + 1
                    : mapRanges.FirstOrDefault(r => r.SourceRangeStart > sourceStart)?.SourceRangeStart ??
                      long.MaxValue;

                var sourceEnd = Math.Min(nextSourceStart - 1, range.End);
                var destinationEnd = GetDestination(sourceEnd);
                yield return new Range(destinationStart, destinationEnd);

                sourceStart = nextSourceStart;
            }
        }

        private MapRange? GetMapRange(long source)
        {
            return mapRanges.FirstOrDefault(r =>
                source >= r.SourceRangeStart &&
                source <= r.SourceRangeEnd);
        }
    }

    private class MapRange(long destinationRangeStart, long sourceRangeStart, long rangeLength)
    {
        public long SourceRangeStart { get; } = sourceRangeStart;
        public long SourceRangeEnd { get; } = sourceRangeStart + rangeLength - 1;

        public long GetDestination(long source)
        {
            if (source < SourceRangeStart || source > SourceRangeEnd)
            {
                throw new ArgumentOutOfRangeException(nameof(source));
            }

            return destinationRangeStart + source - SourceRangeStart;
        }
    }

    private class Range(long start, long end)
    {
        public long Start { get; } = start;
        public long End { get; } = end;
    }
}