using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/8
/// </summary>
public class Day08
{
    [Theory]
    [InlineData("Input/08/example0.txt", 2)]
    [InlineData("Input/08/example1.txt", 6)]
    [InlineData("Input/08/input.txt", 12737)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var instructions = inputs[0];

        var map = new Dictionary<string, string[]>();

        foreach (var line in inputs[2..])
        {
            var x = line.Split('=');

            var y = x[1].Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Split(',').Select(e => e.Trim())
                .ToArray();

            map.Add(x[0].Trim(), y);
        }

        var steps = 0;

        var token = "AAA";

        var i = 0;

        while (token != "ZZZ")
        {
            var a = map[token];

            if (i >= instructions.Length)
                i = 0;

            token = instructions[i] == 'L' ? a[0] : a[1];
            steps++;
            i++;
        }

        steps.Should().Be(expected);
    }

    [Theory]
    [InlineData("Input/08/example2.txt", 6)]
    [InlineData("Input/08/input.txt", 9064949303801)]
    public async Task Part2(string inputFilePath, long expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var instructions = inputs[0];

        var map = new Dictionary<string, string[]>();

        foreach (var line in inputs[2..])
        {
            var x = line.Split('=');

            var y = x[1].Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Split(',').Select(e => e.Trim())
                .ToArray();

            map.Add(x[0].Trim(), y);
        }

        var startNodes = map.Keys.Where(e => e.EndsWith('A')).ToArray();
        var endNodes = map.Keys.Where(e => e.EndsWith('Z'));
        startNodes.Should().HaveSameCount(endNodes);

        var outputs = new List<int>();

        foreach (var node in startNodes)
        {
            var steps = 0;
            var i = 0;

            var token = node;

            while (!token.EndsWith("Z"))
            {
                var a = map[token];

                if (i >= instructions.Length)
                    i = 0;

                token = instructions[i] == 'L' ? a[0] : a[1];
                steps++;
                i++;
            }

            outputs.Add(steps);
        }

        var lcm = LCM(outputs[0], outputs[1]);

        for (var i = 2; i < outputs.Count; i++) lcm = LCM(lcm, outputs[i]);

        lcm.Should().Be(expected);
    }

    public long LCM(long a, long b) => Math.Abs(a * b) / GCD(a, b);

    private long GCD(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }
}