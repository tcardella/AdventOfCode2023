using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/13
/// </summary>
public class Day13
{
    [Theory]
    [InlineData("Input/13/example.txt", 405)]
    [InlineData("Input/13/input.txt", 9556713)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllTextAsync(inputFilePath);

        var patterns = inputs.Split("\r\n\r\n");
        if (patterns.Length == 1) patterns = inputs.Split("\n\n");

        var verticalSum = 0;
        var horizontalSum = 0;

        for (var i = 0; i < patterns.Length; i++)
        {
            var pattern = patterns[i].Split("\r\n");
            if (pattern.Length == 1)
                pattern = patterns[i].Split("\n");

            for (var j = 1; j < pattern[0].Length - 1; j++)
                if (CheckForVerticalReflectionAt(pattern, j))
                {
                    verticalSum += j;
                    break;
                }

            for (var j = 1; j < pattern.Length; j++)
                if (CheckForHorizontalReflectionAt(pattern, j))
                {
                    horizontalSum += j;
                    break;
                }
        }

        (verticalSum + 100 * horizontalSum).Should().Be(expected);
    }

    private bool CheckForHorizontalReflectionAt(string[] pattern, int rowIndex)
    {
        var top = rowIndex - 1;
        var bottom = rowIndex;

        while (top >= 0 && bottom < pattern.Count())
        {
            for (var i = 0; i < pattern[0].Length; i++)
                if (pattern[top][i] != pattern[bottom][i])
                    return false;

            top--;
            bottom++;
        }

        return true;
    }

    private bool CheckForVerticalReflectionAt(string[] pattern, int columnIndex)
    {
        var left = columnIndex - 1;
        var right = columnIndex;

        while (left >= 0 && right < pattern[0].Length)
        {
            for (var i = 0; i < pattern.Count(); i++)
            {
                Console.WriteLine($"{left} <=> {right}");
                if (pattern[i][left] != pattern[i][right])
                    return false;
            }

            left--;
            right++;
        }

        return true;
    }

    [Theory]
    [InlineData("Input/13/example.txt", 1030, 10)]
    [InlineData("Input/13/input.txt", 678626199476, 1_000_000)]
    public async Task Part2(string inputFilePath, long expected, long expansion)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);
    }
}