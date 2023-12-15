using System.Text.RegularExpressions;
using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/15
/// </summary>
public class Day15
{
    private readonly Dictionary<int, List<string>> _boxes =
        Enumerable.Range(0, 256).ToDictionary(e => e, _ => new List<string>());

    private readonly Regex _regex = new(@"^(?<label>\w+)(?<op>[-=])(?<focalLength>\d*)$");

    [Theory]
    [InlineData("Input/15/example0.txt", 52)]
    [InlineData("Input/15/example1.txt", 1320)]
    [InlineData("Input/15/input.txt", 511498)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var sum = 0;

        foreach (var input in inputs[0].Split(',')) sum += Hash(input);

        sum.Should().Be(expected);
    }

    private static int Hash(string input)
    {
        var currentValue = 0;

        foreach (var e in input)
        {
            currentValue += e;
            currentValue *= 17;
            currentValue %= 256;
        }

        return currentValue;
    }

    [Theory]
    [InlineData("Input/15/example1.txt", 145)]
    [InlineData("Input/15/input.txt", 284674)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var split = inputs[0].Split(',');
        for (var i = 0; i < split.Length; i++)
        {
            var input = split[i];

            var match = _regex.Match(input);
            var label = match.Groups["label"].Value;
            var targetBox = Hash(label);

            switch (match.Groups["op"].Value)
            {
                case "-":
                    Remove(targetBox, label);
                    break;
                case "=":
                    Add(targetBox, match, label);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        var sum = 0;
        for (var i = 0; i < _boxes.Count; i++)
        for (var j = 0; j < _boxes[i].Count; j++)
            sum += (i + 1) * (j + 1) * int.Parse(_boxes[i][j].Split(' ').Last());

        sum.Should().Be(expected);
    }

    private void Remove(int targetBox, string label)
    {
        for (var j = 0; j < _boxes[targetBox].Count; j++)
            if (_boxes[targetBox][j].StartsWith(label))
            {
                _boxes[targetBox].RemoveAt(j);
                return;
            }
    }

    private void Add(int targetBox, Match match, string label)
    {
        var lens = $"{label} {match.Groups["focalLength"].Value}";

        for (var j = 0; j < _boxes[targetBox].Count; j++)
            if (_boxes[targetBox][j].StartsWith(label))
            {
                _boxes[targetBox][j] = lens;
                return;
            }

        _boxes[targetBox].Add(lens);
    }
}