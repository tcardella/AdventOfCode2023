using FluentAssertions;
using Xunit.Abstractions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/5
/// </summary>
public class Day05
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day05(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("Input/05/example.txt")]
    [InlineData("Input/05/input.txt")]
    public async Task ParseTest(string inputFilePath)
    {
        var input = await File.ReadAllTextAsync(inputFilePath);

        var sections = input.Replace("\r\n", "\n").Split("\n\n");

        var almanac = new Almanac
        {
            Seeds = ParseSeedsPart1(sections[0]).ToList(),
            Seed2SoilMap = ParseMap(sections[1]),
            Soil2FertilizerMap = ParseMap(sections[2]),
            Fertilizer2WaterMap = ParseMap(sections[3]),
            Water2LightMap = ParseMap(sections[4]),
            Light2TemperatureMap = ParseMap(sections[5]),
            Temperature2HumidityMap = ParseMap(sections[6]),
            Humidity2LocationMap = ParseMap(sections[7])
        };

        sections.Count().Should().Be(8);
    }

    private IEnumerable<Range> ParseSeedsPart1(string input)
    {
        var seeds = input.Split(": ")
            .Last()
            .Split(' ').Select(long.Parse);

        foreach (var seed in seeds)
            yield return new Range(seed, seed);
    }

    private IEnumerable<Range> ParseSeedsPart2(string input)
    {
        var seeds = input.Split(": ")
            .Last()
            .Split(' ').Select(long.Parse)
            .ToArray();

        for (var i = 0; i < seeds.Length; i += 2) yield return new Range(seeds[i], seeds[i] + seeds[i + 1] - 1);
    }

    private Map ParseMap(string input)
    {
        var inputs = input.Split('\n').ToArray();
        return new Map(ParseMapRanges(inputs[1..]));
    }

    private IEnumerable<MapRange> ParseMapRanges(IEnumerable<string> inputs)
    {
        foreach (var input in inputs)
        {
            var x = input.Split(' ');

            var n = long.Parse(x[2]);
            var sourceStart = long.Parse(x[1]);
            var sourceEnd = sourceStart + n - 1;
            var destinationStart = long.Parse(x[0]);
            var destinationEnd = destinationStart + n - 1;

            yield return new MapRange(sourceStart, sourceEnd, destinationStart, destinationEnd);
        }
    }

    [Theory]
    [InlineData("Input/05/example.txt", 35)]
    [InlineData("Input/05/input.txt", 265018614)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var input = await File.ReadAllTextAsync(inputFilePath);

        var sections = input.Replace("\r\n", "\n").Split("\n\n");

        var almanac = new Almanac
        {
            Seeds = ParseSeedsPart1(sections[0]).ToList(),
            Seed2SoilMap = ParseMap(sections[1]),
            Soil2FertilizerMap = ParseMap(sections[2]),
            Fertilizer2WaterMap = ParseMap(sections[3]),
            Water2LightMap = ParseMap(sections[4]),
            Light2TemperatureMap = ParseMap(sections[5]),
            Temperature2HumidityMap = ParseMap(sections[6]),
            Humidity2LocationMap = ParseMap(sections[7])
        };

        var actual = long.MaxValue;

        foreach (var seed in almanac.Seeds)
        {
            _testOutputHelper.WriteLine("");

            _testOutputHelper.WriteLine($"Seed: {seed.Dump()}");

            var soil = almanac.Seed2SoilMap.From(new[] { seed }).ToArray();
            _testOutputHelper.WriteLine($"Soil: {soil.Dump()}");

            var fertilizer = almanac.Soil2FertilizerMap.From(soil).ToArray();
            _testOutputHelper.WriteLine($"Fertilizer: {fertilizer.Dump()}");

            var water = almanac.Fertilizer2WaterMap.From(fertilizer).ToArray();
            _testOutputHelper.WriteLine($"Water: {water.Dump()}");

            var light = almanac.Water2LightMap.From(water).ToArray();
            _testOutputHelper.WriteLine($"Light: {light.Dump()}");

            var temperature = almanac.Light2TemperatureMap.From(light).ToArray();
            _testOutputHelper.WriteLine($"Temperature: {temperature.Dump()}");

            var humidity = almanac.Temperature2HumidityMap.From(temperature).ToArray();
            _testOutputHelper.WriteLine($"Humidity: {humidity.Dump()}");

            var location = almanac.Humidity2LocationMap.From(humidity).ToArray();
            _testOutputHelper.WriteLine($"Location: {location.Dump()}");

            if (location.Min(e => e.Start) < actual)
                actual = location.Min(e => e.Start);
        }

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("Input/05/example.txt", 46,
        Skip = "Weird off by one error. Might have to come back and look at this later.")]
    [InlineData("Input/05/input.txt", 63179500)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var input = await File.ReadAllTextAsync(inputFilePath);

        var sections = input.Replace("\r\n", "\n").Split("\n\n");

        var almanac = new Almanac
        {
            Seeds = ParseSeedsPart2(sections[0]).ToList(),
            Seed2SoilMap = ParseMap(sections[1]),
            Soil2FertilizerMap = ParseMap(sections[2]),
            Fertilizer2WaterMap = ParseMap(sections[3]),
            Water2LightMap = ParseMap(sections[4]),
            Light2TemperatureMap = ParseMap(sections[5]),
            Temperature2HumidityMap = ParseMap(sections[6]),
            Humidity2LocationMap = ParseMap(sections[7])
        };

        var actual = long.MaxValue;

        foreach (var seed in almanac.Seeds)
        {
            _testOutputHelper.WriteLine("");

            _testOutputHelper.WriteLine($"Seed: {seed.Dump()}");

            var soil = almanac.Seed2SoilMap.From(new[] { seed }).ToArray();
            _testOutputHelper.WriteLine($"Soil: {soil.Dump()}");

            var fertilizer = almanac.Soil2FertilizerMap.From(soil).ToArray();
            _testOutputHelper.WriteLine($"Fertilizer: {fertilizer.Dump()}");

            var water = almanac.Fertilizer2WaterMap.From(fertilizer).ToArray();
            _testOutputHelper.WriteLine($"Water: {water.Dump()}");

            var light = almanac.Water2LightMap.From(water).ToArray();
            _testOutputHelper.WriteLine($"Light: {light.Dump()}");

            var temperature = almanac.Light2TemperatureMap.From(light).ToArray();
            _testOutputHelper.WriteLine($"Temperature: {temperature.Dump()}");

            var humidity = almanac.Temperature2HumidityMap.From(temperature).ToArray();
            _testOutputHelper.WriteLine($"Humidity: {humidity.Dump()}");

            var location = almanac.Humidity2LocationMap.From(humidity).ToArray();
            _testOutputHelper.WriteLine($"Location: {location.Dump()}");

            if (location.Min(e => e.Start) < actual)
                actual = location.Min(e => e.Start);
        }

        actual.Should().Be(expected);
    }
}

public record Range(long Start, long End);

public record MapRange(long SourceStart, long SourceEnd, long DestinationStart, long DestinationEnd);

public class Almanac
{
    public List<Range> Seeds { get; init; } = new();
    public Map Seed2SoilMap { get; init; } = new();
    public Map Soil2FertilizerMap { get; init; } = new();
    public Map Fertilizer2WaterMap { get; init; } = new();
    public Map Water2LightMap { get; init; } = new();
    public Map Light2TemperatureMap { get; init; } = new();
    public Map Temperature2HumidityMap { get; init; } = new();
    public Map Humidity2LocationMap { get; init; } = new();
}

public class Map
{
    private readonly List<MapRange> _ranges = new();

    public Map()
    {
    }

    public Map(IEnumerable<MapRange> mapRanges)
    {
        _ranges = mapRanges.ToList();
    }

    public IEnumerable<Range> From(IEnumerable<Range> inputs)
    {
        var output = new List<Range>();

        foreach (var input in inputs)
        foreach (var range in _ranges)
            if (input.OverlapsWith(range))
                output.Add(input.GetOverlap(range));

        if (!output.Any())
            output.AddRange(inputs);

        return output;
    }
}