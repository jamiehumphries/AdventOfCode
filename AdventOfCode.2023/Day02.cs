using AdventOfCode.Helpers;

namespace AdventOfCode.Year2023;

public class Day02 : Day
{
    private readonly IEnumerable<Game> games;

    public Day02()
    {
        games = Input.Select(line => new Game(line));
    }

    protected override object PartOne()
    {
        return games.Where(game => game.Possible(12, 13, 14)).Sum(game => game.Id);
    }

    protected override object PartTwo()
    {
        return games.Sum(game => game.Power);
    }

    private class Game
    {
        private readonly IEnumerable<Round> rounds;

        public Game(string line)
        {
            var split = line.Split(": ");

            Id = int.Parse(split[0]["Game ".Length..]);

            var game = split[1];
            rounds = game.Split("; ").Select(round => new Round(round));
        }

        public int Id { get; }
        public int Power => MaxRed * MaxGreen * MaxBlue;

        private int MaxRed => MaxCubes("red");
        private int MaxGreen => MaxCubes("green");
        private int MaxBlue => MaxCubes("blue");

        public bool Possible(int red, int green, int blue)
        {
            return MaxRed <= red &&
                   MaxGreen <= green &&
                   MaxBlue <= blue;
        }

        private int MaxCubes(string color)
        {
            return rounds.Max(round => round.Cubes(color));
        }
    }

    private class Round(string round)
    {
        private readonly Dictionary<string, int> cubes = round
            .Split(", ")
            .Select(reveal =>
            {
                var split = reveal.Split(" ");
                var count = int.Parse(split[0]);
                var color = split[1];
                return new KeyValuePair<string, int>(color, count);
            })
            .ToDictionary();


        public int Cubes(string color)
        {
            return cubes.GetValueOrDefault(color, 0);
        }
    }
}