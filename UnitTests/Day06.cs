using FluentAssertions;
using Xunit.Abstractions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/6
/// </summary>
public class Day06
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day06(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("Input/06/example.txt", 288)]
    [InlineData("Input/06/input.txt", 131376)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var x = inputs.Select(e => e.Split(':')
                .Last()
                .Split(' ')
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Select(int.Parse))
            .ToArray();

        var a = x[0].Zip(x[1]).Select(e => new { Time = e.First, Distance = e.Second }).ToArray();

        var counts = new List<int>();

        for (var i = 0; i < a.Count(); i++)
        {
            var count = 0;

            for (var j = 0; j < a[i].Time; j++)
            {
                var time2Move = a[i].Time - j;
                var speed = 1 * j;

                var d = time2Move * speed;

                if (d > a[i].Distance)
                    count++;
            }

            counts.Add(count);
        }

        counts.Aggregate((a, b) => a * b).Should().Be(expected);
    }

    [Theory]
    [InlineData("Input/06/example.txt", 71503)]
    [InlineData("Input/06/input.txt", 34123437)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var x = inputs.Select(e => e.Split(':')
                .Last()
                .Replace(" ", string.Empty))
            .Select(long.Parse);

        var a = new { Time = x.First(), Distance = x.Last() };

        var counts = new List<int>();

        var count = 0;

        for (var i = 0; i < a.Time; i++)
        {
            var time2Move = a.Time - i;
            var speed = 1 * i;

            var d = time2Move * speed;

            if (d > a.Distance)
                count++;
        }

        counts.Add(count);

        counts.Aggregate((a, b) => a * b).Should().Be(expected);
    }
}