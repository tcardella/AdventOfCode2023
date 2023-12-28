using System.Drawing;
using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/14
/// </summary>
public class Day14
{
    [Theory]
    [InlineData("Input/14/example.txt", 136)]
    //[InlineData("Input/14/input.txt", 9556714)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllTextAsync(inputFilePath);

        // build platform
        var platform = inputs.Split("\n");

        var roundedRocks = new List<Point>();
        var cubeShapedRocks = new List<Point>();
        
        // parse roundedRocks
        for (int i = 0; i < platform.Length; i++)
        {
            for (int j = 0; j < platform[0].Length; j++)
            {
                if (platform[i][j] == 'O')
                    roundedRocks.Add(new Point(j, i));

                if (platform[i][j] == '#')
                    cubeShapedRocks.Add(new Point(j,i));
            }
        }

        // parse cubeShapedRocks


    }

    //[Theory]
    //[InlineData("Input/14/example.txt", 1030, 10)]
    //[InlineData("Input/14/input.txt", 678626199476, 1_000_000)]
    public async Task Part2(string inputFilePath, long expected, long expansion)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);
    }
}