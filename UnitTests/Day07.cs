using FluentAssertions;
using Xunit.Abstractions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/7
/// </summary>
public class Day07
{
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly Dictionary<string, int> _handRanks = new[] { "HighCard", "1Pair", "2Pair", "3OfAKind", "FullHouse", "4OfAKind", "5OfAKind" }
        .Select((e, i) => new { Type = e, Rank = i })
        .ToDictionary(e => e.Type, e => e.Rank);

    public Day07(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("Input/07/example.txt", 6440)]
    [InlineData("Input/07/input.txt", 246424613)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var hands = inputs.Select(e => e.Split(' '))
            .Select(f => new Hand { Cards = f[0], Bid = int.Parse(f[1]) })
            .ToList();

        var scorer = new CardStrengthFinder();
        var identifier = new HandIdentifier();
        var handComparer = new HandComparer();

        var actual = hands.Select(h => identifier.IdentifyPart1(h))
            .Select(h => h with { Rank = _handRanks[h.Type] })
            .Order(handComparer)
            .Select((e, i) => e.Bid * (i + 1))
            .Sum();
        
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("Input/07/example.txt", 5905)]
    [InlineData("Input/07/input.txt", 34123437)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var hands = inputs.Select(e => e.Split(' '))
            .Select(f => new Hand2 { Cards = f[0], Bid = int.Parse(f[1]) })
            .ToList();

        var strengthFinder = new CardStrengthFinder();
        var identifier = new HandIdentifier();
        var handComparer = new HandComparer2();

        var preActual = hands.Select(h => h with { CardStrengths = strengthFinder.Score(h) })
            .Select(h=> h with {Cards = identifier.DoIt(h)})
            .Select(h => h with { Type = identifier.IdentifyPart2(h) })
            .Select(h => h with { Rank = _handRanks[h.Type] })
            .Order(handComparer);

        foreach (var e in preActual)
        {
            _testOutputHelper.WriteLine(e.Cards + "   " + e.Type + "   " + string.Join(',', e.CardStrengths) + "   " + e.Bid);
        }
        

        var actual = preActual
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
        var sut = new HandIdentifier();

        var hand = new Hand()
        {
            Cards = cards
        };

        sut.IdentifyPart1(hand).Type.Should().Be(expected);
    }

    [Fact]
    public void DoIt()
    {
        var sut = new HandIdentifier();

var actual =        sut.DoIt(new Hand2() { Cards = "JJ354" });

actual.Should().Be("55354");
    }
}

public class CardStrengthFinder
{
    private readonly Dictionary<char, int> _cardStrengths = new[] { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' }.Reverse()
        .Select((e, i) => new { Label = e, Strength = i })
        .ToDictionary(e => e.Label, e => e.Strength);

    public int[] Score(Hand2 hand) => hand.Cards.Select(c => _cardStrengths[c]).ToArray();
}

public record Hand
{
    public string Cards { get; set; }
    public string Type { get; set; }
    public int Bid { get; set; }
    public int Rank { get; set; }
    public int Index { get; set; }
}

public record Hand2 : Hand
{
    public int[] CardStrengths { get; set; }
}

public class HandComparer : IComparer<Hand>
{
    private readonly Dictionary<char, int> _cardStrengths = new[] { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' }.Reverse()
        .Select((e, i) => new { Label = e, Strength = i })
        .ToDictionary(e => e.Label, e => e.Strength);

    #region Implementation of IComparer<in Hand>

    public int Compare(Hand? x, Hand? y)
    {
        if (x.Rank > y.Rank)
            return 1;

        if (x.Rank == y.Rank)
        {
            for (var i = 0; i < x.Cards.Length; i++)
            {
                var a = _cardStrengths[ x.Cards[i]];
                var b = _cardStrengths[y.Cards[i]];

                if (a > b)
                    return 1;

                if (a == b)
                    continue;

                if (a < b)
                    return -1;
            }

            return 0;
        }

        return -1;
    }

    #endregion
}

public class HandComparer2 : IComparer<Hand2>
{
    #region Implementation of IComparer<in Hand>

    public int Compare(Hand2? x, Hand2? y)
    {
        if (x.Rank > y.Rank)
            return 1;

        if (x.Rank == y.Rank)
        {
            for (var i = 0; i < x.CardStrengths.Length; i++)
            {
                var temp = x.CardStrengths[i].CompareTo(y.CardStrengths[i]);
                if (temp != 0)
                    return temp;
            }

            return 0;
        }

        return -1;
    }

    #endregion
}

public class HandIdentifier
{
    public Hand IdentifyPart1(Hand hand)
    {
        var groupings = from e in hand.Cards
            group e by e
            into g
            select new
            {
                Character = g.Key,
                Count = g.Count()
            };

        hand.Type = groupings.Count() switch
        {
            5 => "HighCard",
            4 => "1Pair",
            3 => groupings.Any(e => e.Count == 3)
                ? "3OfAKind"
                : "2Pair",
            2 => groupings.Any(e => e.Count == 4)
                ? "4OfAKind"
                : "FullHouse",
            1 => "5OfAKind",
            _ => throw new ArgumentOutOfRangeException()
        };

        return hand;
    }
    private readonly Dictionary<char, int> _cardStrengths = new[] { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' }.Reverse()
        .Select((e, i) => new { Label = e, Strength = i })
        .ToDictionary(e => e.Label, e => e.Strength);
    public string DoIt(Hand2 hand)
    {
        if (!hand.Cards.Contains('J'))
            return hand.Cards;

        if (hand.Cards == "JJJJJ")
            return "AAAAA";

        var groupings = from e in hand.Cards
            group e by e
            into g
            select new
            {
                Character = g.Key,
                Count = g.Count(),

            };

        var mostCommonChar = groupings.Where(e => e.Character != 'J').MaxBy(e => e.Count)!.Character;
        return hand.Cards.Replace('J', mostCommonChar);
    }

    public string IdentifyPart2(Hand2 hand)
    {
        var groupings = from e in hand.Cards
            group e by e
            into g
            select new
            {
                Character = g.Key,
                Count = g.Count()
            };

        return groupings.Count() switch
        {
            5 => "HighCard",
            4 => "1Pair",
            3 => groupings.Any(e => e.Count == 3)
                ? "3OfAKind"
                : "2Pair",
            2 => groupings.Any(e => e.Count == 4)
                ? "4OfAKind"
                : "FullHouse",
            1 => "5OfAKind",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}