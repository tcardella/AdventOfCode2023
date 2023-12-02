using FluentAssertions;

namespace UnitTests;

public class Day02
{
    [Theory]
    [InlineData("Input/02/example0.txt", 8)]
    [InlineData("Input/02/input.txt", 2156)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var availableCubes = new Dictionary<string, int>() { { "red", 12 }, { "green", 13 }, { "blue", 14 } };

        var sum = 0;

        foreach (var input in inputs)
        {
            var line = input.Split(':');
            var gameNumber = line.First().Split(' ').Last();
            var game = line.TakeLast(1).First();
            var sets = game.Split(';');

            var isValid = true;
            var d = new Dictionary<string, int>();

            foreach (var set in sets)
            {
                var cubes = set.Split(',')
                    .Select(e => e.Trim())
                    .Select(e => e.Split(' '))
                    .ToDictionary(e => e[1], e => int.Parse(e[0]));

                foreach (var cube in cubes)
                {
                    if (!d.ContainsKey(cube.Key))
                        d.Add(cube.Key, cube.Value);
                    else
                    {
                        if (d[cube.Key] < cube.Value)
                            d[cube.Key] = cube.Value;
                    }

                    if (availableCubes[cube.Key] < cube.Value)
                    {
                        isValid = false;
                        break;
                    }
                }
            }

            if (isValid)
                sum += int.Parse(gameNumber);
        }

        sum.Should().Be(expected);
    }

    [Theory]
    [InlineData("Input/02/example0.txt", 2286)]
    [InlineData("Input/02/input.txt", 66909)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var sum = 0;

        foreach (var input in inputs)
        {
            var line = input.Split(':');
            var game = line.TakeLast(1).First();
            var sets = game.Split(';');

            var d = new Dictionary<string, int>();

            foreach (var set in sets)
            {
                var cubes = set.Split(',')
                    .Select(e => e.Trim())
                    .Select(e => e.Split(' '))
                    .ToDictionary(e => e[1], e => int.Parse(e[0]));

                foreach (var cube in cubes)
                {
                    if (!d.ContainsKey(cube.Key))
                        d.Add(cube.Key, cube.Value);
                    else
                    {
                        if (d[cube.Key] < cube.Value)
                            d[cube.Key] = cube.Value;
                    }
                }
            }

            var aggregate = d.Values.Aggregate((a, b) => a * b);
            sum += aggregate;
        }

        sum.Should().Be(expected);
    }
}