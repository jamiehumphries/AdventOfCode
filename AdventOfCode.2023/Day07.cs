using AdventOfCode.Helpers;

namespace AdventOfCode.Year2023;

public class Day07 : Day
{
    protected override object PartOne()
    {
        var hands = Input.Select(line => new Hand(line)).ToList();
        return TotalWinnings(hands);
    }

    protected override object PartTwo()
    {
        var hands = Input.Select(line => new Hand(line, true)).ToList();
        return TotalWinnings(hands);
    }

    private static int TotalWinnings(IEnumerable<Hand> hands)
    {
        var orderedHands = hands.OrderBy(h => h.Strength()).ToList();
        return orderedHands
            .Select((h, i) => h.Bid * (i + 1))
            .Sum();
    }

    private class Hand
    {
        private const string AllJokers = "JJJJJ";

        private readonly string cards;
        private readonly bool useJokers;

        public Hand(string line, bool useJokers = false)
        {
            this.useJokers = useJokers;

            var data = line.Split(' ');
            cards = data[0];
            Bid = int.Parse(data[1]);
        }

        public int Bid { get; }

        public string Strength()
        {
            var groupedCards = cards
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());

            if (useJokers && cards != AllJokers)
            {
                var mostCommonCard = groupedCards
                    .Where(kv => kv.Key != 'J')
                    .MaxBy(kv => kv.Value)
                    .Key;

                groupedCards[mostCommonCard] += groupedCards.GetValueOrDefault('J', 0);
                groupedCards['J'] = 0;
            }

            var strengths = groupedCards.Select(kv => kv.Value)
                .OrderDescending()
                .ToList();

            while (strengths.Count < 6)
            {
                strengths.Add(0);
            }

            strengths.AddRange(cards.Select(CardStrength));

            return string.Join("", strengths.Select(s => ZeroPad(s)));
        }

        private int CardStrength(char label)
        {
            return label switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => useJokers ? 1 : 11,
                'T' => 10,
                _ => int.Parse(label.ToString())
            };
        }

        private static string ZeroPad(int n, int length = 2)
        {
            var nString = n.ToString();
            while (nString.Length < length)
            {
                nString = "0" + nString;
            }

            return nString;
        }
    }
}