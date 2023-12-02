using FluentAssertions;

namespace UnitTests;

public class Day01
{
    private readonly List<string> _digitTokens;
    private readonly Dictionary<string, string> _textTokens;

    public Day01()
    {
        _digitTokens = Enumerable.Range(1, 9).Select(e => e.ToString()).ToList();
        _textTokens = "one|two|three|four|five|six|seven|eight|nine".Split('|')
            .Zip(_digitTokens).ToDictionary(r => r.First, r => r.Second);
    }

    [Theory]
    [InlineData("Input/01/example0.txt", 142)]
    [InlineData("Input/01/input.txt", 56465)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var sum = 0;

        foreach (var input in inputs)
        {
            var outputTokens = input.Where(e => _digitTokens.Contains(e.ToString())).Select(e => e.ToString()).ToList();

            var value = int.Parse($"{outputTokens.First()}{outputTokens.Last()}");

            sum += value;
        }

        sum.Should().Be(expected);
    }

    [Theory]
    [InlineData("Input/01/example1.txt", 281)]
    [InlineData("Input/01/input.txt", 55902)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var sum = 0;

        foreach (var input in inputs)
        {
            var outputTokens = new List<string>();

            for (var i = 0; i < input.Length; i++)
            {
                if (_digitTokens.Contains(input[i].ToString()))
                {
                    outputTokens.Add(input[i].ToString());
                    continue;
                }

                outputTokens.AddRange(_textTokens.Keys.Where(e => input[i..].StartsWith(e))
                    .Select(e => _textTokens[e]));
            }

            var value = int.Parse($"{outputTokens.First()}{outputTokens.Last()}");

            sum += value;
        }

        sum.Should().Be(expected);
    }
}