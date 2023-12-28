using System.Drawing;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/17
/// </summary>
public class Day17
{
    [Theory]
    [InlineData("Input/17/example.txt", 102)]
    //[InlineData("Input/17/input.txt", 511498)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        //char[][] map = inputs.Select(input => input.ToCharArray()).ToArray();

        //      var start = new Point(1, 0);

        //// TODO: Implement djikstra's algorithm
        
    }

    //[Theory]
    //[InlineData("Input/17/example1.txt", 145)]
    //[InlineData("Input/17/input.txt", 284674)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

    }
}