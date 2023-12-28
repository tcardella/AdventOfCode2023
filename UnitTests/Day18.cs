using System.Drawing;
using System.Numerics;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/18
/// </summary>
public class Day18
{
    [Theory]
    [InlineData("Input/18/example.txt", 62)]
    [InlineData("Input/18/input.txt", 511498)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var regex = new Regex(@"^(?<Direction>[UDLR]{1}) (?<Spaces>\d+)");

        IEnumerable<(char Value, int)> z = inputs.Select(e => regex.Match(e))
            .Select(e => (e.Groups["Direction"].Value[0], int.Parse(e.Groups["Spaces"].Value)));

        var u = z.Where(e => e.Value is 'U').Select(e => e.Item2).Sum();
        var d = z.Where(e => e.Value is 'D').Select(e => e.Item2).Sum();

        var v1 = d - u;

        char[][] map = Enumerable.Range(0, 250).Select(_ => Enumerable.Range(0, 250).Select(_=> '.').ToArray()).ToArray();

        var cp = new Point(0, 0);
        map[cp.Y][cp.X] = '#';
        
        foreach (var e in z)
        {
            var xIncrement = e.Value switch
            {
                'R' => 1,
                'L' => -1,
                _ => 0
            };

            var yIncrement = e.Value switch
            {
                'U' => 1,
                'D' => -1,
                _=> 0
            };

            var v = new Vector2(xIncrement, yIncrement);

            for (int i = 0; i < e.Item2; i++)
            {
                cp = cp.Add(v);
                try
                {
                    map[cp.Y][cp.X] = '#';
                }
                catch
                {
                    var t = 0;
                }
            }

        }

        FillIn(map);

        var count = 0;
        
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == '#')
                    count++;
            }
        }

        count.Should().Be(expected);
    }

    private void FillIn(char[][] map)
    {
        for (int i = 0; i < map.Length; i++)
        {
            var fill = new List<int>();
            
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == '#')
                {
                    fill.Add(j);
                }
            }

            if (fill.Any())
            {
                var start = fill.Min();
                var end = fill.Max();

                for (int j = start; j < end; j++)
                {
                    map[i][j] = '#';
                }
            }
            Dump(map);

            Console.WriteLine();
        }
    }

    private void Dump(char[][] map)
    {
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[0].Length; j++)
            {
                Console.Write(map[i][j]);
            }
            
            Console.WriteLine();
        }
    }

    [Theory]
    //[InlineData("Input/18/example1.txt", 145)]
    //[InlineData("Input/18/input.txt", 284674)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

    }
}

public record LineItem(char Direction, int Spaces, string Color)
{
}