using AdventOfCode.Helpers;
using AdventOfCode.Helpers.Parsing;

namespace AdventOfCode.Year2023;

public class Day04 : Day
{
    private readonly List<Card> cards;

    public Day04()
    {
        cards = Input.Select(line => new Card(line)).ToList();
    }

    protected override object PartOne()
    {
        return cards.Sum(card => card.Points);
    }

    protected override object? PartTwo()
    {
        var processedCount = 0;
        var unprocessed = new Queue<Card>(cards);

        while (unprocessed.Count > 0)
        {
            var card = unprocessed.Dequeue();
            processedCount++;
            for (var id = card.Id + 1; id <= card.Id + card.Matches; id++)
            {
                unprocessed.Enqueue(Copy(id));
            }
        }

        return processedCount;
    }

    private Card Copy(int id)
    {
        return cards[id - 1];
    }

    private class Card : LineWithId
    {
        public Card(string line) : base("Card", line)
        {
            var dataSplit = Data.Split(" | ");
            var winningNumbers = ParseNumbers(dataSplit[0]);
            var numbersYouHave = ParseNumbers(dataSplit[1]);

            Matches = numbersYouHave.Intersect(winningNumbers).Count();
            Points = Matches > 0 ? (int)Math.Pow(2, Matches - 1) : 0;
        }

        public int Matches { get; }
        public int Points { get; }

        private static IEnumerable<int> ParseNumbers(string data)
        {
            return data.Split(" ")
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => int.Parse(value.Trim()));
        }
    }
}