using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/9
/// </summary>
public class Day09
{
    [Theory]
    [InlineData("Input/09/example.txt", 114)]
    [InlineData("Input/09/input.txt", 1938731307)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var sum = 0;

        foreach (var input in inputs)
        {
            var rows = BuildRows(input);

            var below = 0;

            for (var i = rows.Count - 1; i >= 0; i--)
            {
                if (i == rows.Count - 1)
                {
                    rows[i].Add(0);
                    below = 0;
                    continue;
                }

                below = rows[i].Last() + below;
                rows[i].Add(below);
            }

            sum += rows[0].Last();
        }

        sum.Should().Be(expected);
    }

    [Theory]
    [InlineData("Input/09/example.txt", 2)]
    [InlineData("Input/09/input.txt", 948)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var sum = 0;

        foreach (var input in inputs)
        {
            var rows = BuildRows(input);

            var below = 0;

            for (var i = rows.Count - 1; i >= 0; i--)
            {
                if (i == rows.Count - 1)
                {
                    rows[i].Insert(0, 0);
                    below = 0;
                    continue;
                }

                below = rows[i].First() - below;
                rows[i].Insert(0, below);
            }

            sum += rows[0].First();
        }

        sum.Should().Be(expected);
    }

    private List<List<int>> BuildRows(string input)
    {
        var output = new List<List<int>>();
        var row = input.Split(' ').Select(int.Parse).ToList();

        output.Add(row);

        while (row.Any(e => e != 0))
        {
            row = CalcNextRow(row);
            output.Add(row);
        }

        return output;
    }

    private List<int> CalcNextRow(List<int> input)
    {
        var output = new List<int>();

        for (var i = 1; i < input.Count; i++) output.Add(input[i] - input[i - 1]);

        return output;
    }
}