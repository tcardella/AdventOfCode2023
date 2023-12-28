using System.Drawing;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/16
/// </summary>
public class Day16
{
    [Theory]
    [InlineData("Input/16/example.txt", 46)]
    //[InlineData("Input/16/input.txt", 511498)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var start = new Point(1, 0);
    }

    //[Theory]
    //[InlineData("Input/16/example1.txt", 145)]
    //[InlineData("Input/16/input.txt", 284674)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);
    }
}