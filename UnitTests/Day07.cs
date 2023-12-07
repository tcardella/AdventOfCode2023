using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/7
/// </summary>
public class Day07
{
    private readonly Dictionary<string, int> _handRanks =
        new[] { "HighCard", "1Pair", "2Pair", "3OfAKind", "FullHouse", "4OfAKind", "5OfAKind" }
            .Select((e, i) => new { Type = e, Rank = i })
            .ToDictionary(e => e.Type, e => e.Rank);

    [Theory]
    [InlineData("Input/07/example.txt", 6440)]
    [InlineData("Input/07/input.txt", 246424613)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var hands = inputs.Select(e => e.Split(' '))
            .Select(f => new Hand { Cards = f[0], Bid = int.Parse(f[1]) })
            .ToList();

        var cardStrengths = new[] { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' }
            .Reverse()
            .Select((e, i) => new { Label = e, Strength = i })
            .ToDictionary(e => e.Label, e => e.Strength);
        var utilities = new HandUtilities(cardStrengths);
        var comparer = new HandComparer();

        var actual = hands
            .Select(h => h with { Strengths = utilities.Score(h) })
            .Select(h => h with { Type = utilities.Identify(h) })
            .Select(h => h with { Rank = _handRanks[h.Type] })
            .Order(comparer)
            .Select((e, i) => e.Bid * (i + 1))
            .Sum();

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("Input/07/example.txt", 5905)]
    [InlineData("Input/07/input.txt", 248256639)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var hands = inputs.Select(e => e.Split(' '))
            .Select(f => new Hand { Cards = f[0], Bid = int.Parse(f[1]) })
            .ToList();

        var cardStrengths = new[] { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' }
            .Reverse()
            .Select((e, i) => new { Label = e, Strength = i })
            .ToDictionary(e => e.Label, e => e.Strength);
        var utilities = new HandUtilities(cardStrengths);
        var comparer = new HandComparer();

        var actual = hands
            .Select(h => h with { Strengths = utilities.Score(h) })
            .Select(h => h with { Cards = utilities.ApplyJRule(h) })
            .Select(h => h with { Type = utilities.Identify(h) })
            .Select(h => h with { Rank = _handRanks[h.Type] })
            .Order(comparer)
            .Select((e, i) => e.Bid * (i + 1))
            .Sum();

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("AAAAA", "5OfAKind")]
    [InlineData("AA8AA", "4OfAKind")]
    [InlineData("23332", "FullHouse")]
    [InlineData("TTT98", "3OfAKind")]
    [InlineData("23432", "2Pair")]
    [InlineData("A23A4", "1Pair")]
    [InlineData("23456", "HighCard")]
    public void HandIdentifier_HappyPathTest(string cards, string expected)
    {
        var sut = new HandUtilities(new Dictionary<char, int>());

        var hand = new Hand
        {
            Cards = cards
        };

        sut.Identify(hand).Should().Be(expected);
    }

    [Fact]
    public void ApplyJRule_HappyPathTest()
    {
        var sut = new HandUtilities(new[] { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' }
            .Reverse()
            .Select((e, i) => new { Label = e, Strength = i })
            .ToDictionary(e => e.Label, e => e.Strength));

        var actual = sut.ApplyJRule(new Hand { Cards = "JJ354" });

        actual.Should().Be("55354");
    }
}

public record Hand
{
    public string Cards { get; set; }
    public string Type { get; set; }
    public int Bid { get; set; }
    public int Rank { get; set; }
    public int[] Strengths { get; set; }
}

public class HandComparer() : IComparer<Hand>
{
    #region Implementation of IComparer<in Hand>

    public int Compare(Hand? x, Hand? y)
    {
        if (x.Rank > y.Rank)
            return 1;

        if (x.Rank == y.Rank)
        {
            for (var i = 0; i < x.Cards.Length; i++)
            {
                var temp = x.Strengths[i].CompareTo(y.Strengths[i]);
                if (temp == 0)
                    continue;
                return temp;
            }

            return 0;
        }

        return -1;
    }

    #endregion
}

public class HandUtilities(Dictionary<char, int> cardStrengths)
{
    public int[] Score(Hand hand) => hand.Cards.Select(c => cardStrengths[c]).ToArray();

    public string Identify(Hand hand)
    {
        var characters = (from e in hand.Cards
            group e by e
            into g
            select new
            {
                Character = g.Key,
                Count = g.Count()
            }).ToArray();

        return characters.Length switch
        {
            5 => "HighCard",
            4 => "1Pair",
            3 => characters.Any(e => e.Count == 3)
                ? "3OfAKind"
                : "2Pair",
            2 => characters.Any(e => e.Count == 4)
                ? "4OfAKind"
                : "FullHouse",
            1 => "5OfAKind",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public string ApplyJRule(Hand hand)
    {
        if (!hand.Cards.Contains('J'))
            return hand.Cards;

        if (hand.Cards == "JJJJJ")
            return "AAAAA";

        var mostCommonChar = from e in from e in hand.Cards
                group e by e
                into g
                select new
                {
                    Character = g.Key,
                    Count = g.Count(),
                    Score = cardStrengths[g.Key]
                }
            where e.Character != 'J'
            orderby e.Count, e.Score
            select e.Character;

        return hand.Cards.Replace('J', mostCommonChar.Last());
    }
}