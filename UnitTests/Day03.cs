using System.Text.RegularExpressions;
using FluentAssertions;

namespace UnitTests;

/// <summary>
/// Prompt: https://adventofcode.com/2023/day/3
/// </summary>
public class Day03
{
    private readonly Regex _symbolsRegex = new(@"[^.0-9]");
    private readonly Regex _numbersRegex = new(@"\d+");
    private readonly Regex _gearsRegex = new(@"\*");

    [Theory]
    [InlineData("Input/03/example.txt", 4361)]
    [InlineData("Input/03/input.txt", 532428)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var symbols = (
            from r in Enumerable.Range(0, inputs.Length)
            from m in _symbolsRegex.Matches(inputs[r])
            select new Part(m.Value, r, m.Index)
        );

        var numbers = (
            from r in Enumerable.Range(0, inputs.Length)
            from m in _numbersRegex.Matches(inputs[r])
            select new Part(m.Value, r, m.Index)
        );

        numbers.Where(n => symbols.Any(s => NextTo(s, n)))
            .Select(n => n.Number)
            .Sum()
            .Should().Be(expected);
    }

    bool NextTo(Part p1, Part p2) =>
        Math.Abs(p2.RowIndex - p1.RowIndex) <= 1 &&
        p1.ColumnIndex <= p2.ColumnIndex + p2.Value.Length &&
        p2.ColumnIndex <= p1.ColumnIndex + p1.Value.Length;

    record Part(string Value, int RowIndex, int ColumnIndex)
    {
        public int Number => int.Parse(Value);
    }

    

    [Theory]
    [InlineData("Input/03/example.txt", 467835)]
    [InlineData("Input/03/input.txt", 84051670)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var gears = (
            from r in Enumerable.Range(0, inputs.Length)
            from m in _gearsRegex.Matches(inputs[r])
            select new Part(m.Value, r, m.Index)
        );

        var numbers = (
            from r in Enumerable.Range(0, inputs.Length)
            from m in _numbersRegex.Matches(inputs[r])
            select new Part(m.Value, r, m.Index)
        );

        (from g in gears
                let gp = numbers.Where(n => NextTo(n, g)).Select(n => n.Number)
                where gp.Count() == 2
                select gp.First() * gp.Last()).Sum()
            .Should().Be(expected);
    }
}