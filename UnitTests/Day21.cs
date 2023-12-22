using System.Drawing;
using System.Numerics;
using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2123/day/21
/// </summary>
public class Day21
{
    [Theory]
    [InlineData("Input/21/example.txt", 6, 16)]
    [InlineData("Input/21/input.txt", 64, 3770)]
    public async Task Part1(string inputFilePath, int steps, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var map = inputs.Select(e => e.ToCharArray()).ToArray();

        var start = GetStart(map);

        var directions = new[] { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };

        var lastBatch = new List<Point>() {start };
        var currentBatch = new List<Point>() { };

        foreach (var step in Enumerable.Range(0, steps))
        {
            currentBatch = lastBatch.SelectMany(e => directions, (e, d) => e.Add(d))
                .Where(e => map[e.Y][e.X] != '#')
                .Distinct()
                .ToList();

            lastBatch = currentBatch;
        }

        lastBatch.Count().Should().Be(expected);
    }

    private void Dump(List<Point> lastBatch)
    {
        foreach (var e in lastBatch)
        {
            Console.WriteLine(e.ToString());
        }
        
        Console.WriteLine();
    }

    private static Point GetStart(char[][] map)
    {
        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == 'S')
                {
                    return new Point(j, i);
                }
            }
        }
        
        return Point.Empty;
    }

    [Theory]
    [InlineData("Input/21/example.txt", 0)]
    [InlineData("Input/21/input.txt", 0)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);
    }
}