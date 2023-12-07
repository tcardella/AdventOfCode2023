using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/6
/// </summary>
public class Day06
{
    [Theory]
    [InlineData("Input/06/example.txt", 288)]
    [InlineData("Input/06/input.txt", 131376)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var raceInputs = inputs.Select(e => e.Split(':')
                .Last()
                .Split(' ')
                .Where(f => !string.IsNullOrWhiteSpace(f))
                .Select(int.Parse))
            .ToArray();

        var races = raceInputs[0].Zip(raceInputs[1]).Select(e => new { Time = e.First, Distance = e.Second }).ToArray();

        var counts = new List<int>();

        for (var i = 0; i < races.Count(); i++)
        {
            var count = 0;

            for (var j = 0; j < races[i].Time; j++)
            {
                var distance = (races[i].Time - j) * j;

                if (distance > races[i].Distance)
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

        var raceInputs = inputs.Select(e => e.Split(':')
                .Last()
                .Replace(" ", string.Empty))
            .Select(long.Parse)
            .ToArray();

        var race = new { Time = raceInputs.First(), Distance = raceInputs.Last() };

        var count = 0;

        for (var i = 0; i < race.Time; i++)
        {
            var distance = (race.Time - i) * i;

            if (distance > race.Distance)
                count++;
        }

        count.Should().Be(expected);
    }
}