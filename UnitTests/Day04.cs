using System.Text.RegularExpressions;
using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/4
/// </summary>
public class Day04
{
    [Theory]
    [InlineData("Input/04/example.txt", 13)]
    [InlineData("Input/04/input.txt", 22674)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var sum = 0;

        foreach (var input in inputs)
        {
            var card = input.Split(':');
            var numbers = card[1].Split("|");
            var winningNumbers = numbers[0].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e));
            var myNumbers = numbers[1].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e));

            var matchingNumbers = (from e in winningNumbers
                join mn in myNumbers on e equals mn
                select e).ToArray();

            var cardSum = 0;

            for (var i = 0; i < matchingNumbers.Length; i++)
                if (i == 0)
                    cardSum = 1;
                else
                    cardSum *= 2;

            sum += cardSum;
        }

        sum.Should().Be(expected);
    }

    [Theory]
    [InlineData("Input/04/example.txt", 30)]
    [InlineData("Input/04/input.txt", 5747443)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var winningCardCounts = Enumerable.Range(0, inputs.Length).Select(e => new Item(e, 0, 1)).ToArray();

        for (var i = 0; i < inputs.Length; i++)
        {
            var card = inputs[i].Split(':');
            var numbers = card[1].Split("|");
            var winningNumbers = numbers[0].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e));
            var myNumbers = numbers[1].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e));

            var matchingNumbersCount = (from e in winningNumbers
                join mn in myNumbers on e equals mn
                select e).Count();

            winningCardCounts[i].MatchingNumbersCount = matchingNumbersCount;
        }

        for (var i = 0; i < winningCardCounts.Length; i++)
        for (var j = 0; j < winningCardCounts[i].Copies; j++)
        for (var k = 0; k < winningCardCounts[i].MatchingNumbersCount; k++)
            winningCardCounts[i + k + 1].Copies += 1;

        winningCardCounts.Sum(e => e.Copies).Should().Be(expected);
    }
}

public class Item
{
    public Item(int gameNumber, int matchingNumbersCount, int copies)
    {
        GameNumber = gameNumber;
        MatchingNumbersCount = matchingNumbersCount;
        Copies = copies;
    }

    public int GameNumber { get; }
    public int MatchingNumbersCount { get; set; }
    public int Copies { get; set; }

    #region Overrides of Object

    public override string ToString() => $"{GameNumber}: {MatchingNumbersCount} {Copies}";

    #endregion
}