using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/5
/// </summary>
public class Day05
{
    [Theory]
    [InlineData("Input/05/example.txt", 35)]
    [InlineData("Input/05/input.txt", 265018614)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var almanac = new Almanac();

        for (var i = 0; i < inputs.Length; i++)
        {
            if (inputs[i].StartsWith("seeds:"))
            {
                var rawSeeds = inputs[i].Split(':').Last().Split(' ').Where(e => !string.IsNullOrWhiteSpace(e))
                    .Select(long.Parse).ToList();

                for (int j = 0; j < rawSeeds.Count(); j += 2)
                {
                    almanac.Seeds.Add(new(rawSeeds[j], 1));
                }
            }

            if (inputs[i].StartsWith("seed-to-soil map:"))
            {
                i += 1;
                while (i < inputs.Length && !string.IsNullOrWhiteSpace(inputs[i]))
                {
                    var x = inputs[i].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(long.Parse).ToArray();
                    var y = Parse(x);
                    almanac.Seed2SoilMap.Add(y.source, y.destination, y.n);

                    i++;
                }
            }

            if (inputs[i].StartsWith("soil-to-fertilizer map:"))
            {
                i += 1;
                while (i < inputs.Length && !string.IsNullOrWhiteSpace(inputs[i]))
                {
                    var x = inputs[i].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(long.Parse).ToArray();
                    var y = Parse(x);
                    almanac.Soil2FertilizerMap.Add(y.source, y.destination, y.n);

                    i++;
                }
            }

            if (inputs[i].StartsWith("fertilizer-to-water map:"))
            {
                i += 1;
                while (i < inputs.Length && !string.IsNullOrWhiteSpace(inputs[i]))
                {
                    var x = inputs[i].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(long.Parse).ToArray();
                    var y = Parse(x);
                    almanac.Fertilizer2WaterMap.Add(y.source, y.destination, y.n);

                    i++;
                }
            }

            if (inputs[i].StartsWith("water-to-light map:"))
            {
                i += 1;
                while (i < inputs.Length && !string.IsNullOrWhiteSpace(inputs[i]))
                {
                    var x = inputs[i].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(long.Parse).ToArray();
                    var y = Parse(x);
                    almanac.Water2LightMap.Add(y.source, y.destination, y.n);

                    i++;
                }
            }

            if (inputs[i].StartsWith("light-to-temperature map:"))
            {
                i += 1;
                while (i < inputs.Length && !string.IsNullOrWhiteSpace(inputs[i]))
                {
                    var x = inputs[i].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(long.Parse).ToArray();
                    var y = Parse(x);
                    almanac.Light2TemperatureMap.Add(y.source, y.destination, y.n);

                    i++;
                }
            }

            if (inputs[i].StartsWith("temperature-to-humidity map:"))
            {
                i += 1;
                while (i < inputs.Length && !string.IsNullOrWhiteSpace(inputs[i]))
                {
                    var x = inputs[i].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(long.Parse).ToArray();
                    var y = Parse(x);
                    almanac.Temperature2HumidityMap.Add(y.source, y.destination, y.n);

                    i++;
                }
            }

            if (inputs[i].StartsWith("humidity-to-location map:"))
            {
                i += 1;
                while (i < inputs.Length && !string.IsNullOrWhiteSpace(inputs[i]))
                {
                    var x = inputs[i].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(long.Parse).ToArray();
                    var y = Parse(x);
                    almanac.Humidity2LocationMap.Add(y.source, y.destination, y.n);

                    i++;
                }
            }
        }

        var actual = long.MaxValue;

        foreach (var seed in almanac.Seeds)
        {
            for (long i = seed.Item1; i < seed.Item1 + seed.Item2; i++)
            {
                var soil = almanac.Seed2SoilMap.From(i);
            var fertilizer = almanac.Soil2FertilizerMap.From(soil);
            var water = almanac.Fertilizer2WaterMap.From(fertilizer);
            var light = almanac.Water2LightMap.From(water);
            var temperature = almanac.Light2TemperatureMap.From(light);
            var humidity = almanac.Temperature2HumidityMap.From(temperature);
            var location = almanac.Humidity2LocationMap.From(humidity);

            if (location < actual)
                actual = location;
            }
        }

        actual.Should().Be(expected);
    }

    private (long source, long destination, long n) Parse(long[] input)
    {
        return new(input[1], input[0], input[2]);
    }

    [Theory]
    [InlineData("Input/05/example.txt", 46)]
    [InlineData("Input/05/input.txt", 5747443)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var almanac = new Almanac();

        for (var i = 0; i < inputs.Length; i++)
        {
            if (inputs[i].StartsWith("seeds:"))
            {
                var rawSeeds = inputs[i].Split(':').Last().Split(' ').Where(e => !string.IsNullOrWhiteSpace(e))
                    .Select(long.Parse).ToList();

                for (int j = 0; j < rawSeeds.Count(); j+=2)
                {
                    almanac.Seeds.Add(new(rawSeeds[j], rawSeeds[j + 1]));
                }
            }

            if (inputs[i].StartsWith("seed-to-soil map:"))
            {
                i += 1;
                while (i < inputs.Length && !string.IsNullOrWhiteSpace(inputs[i]))
                {
                    var x = inputs[i].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(long.Parse).ToArray();
                    var y = Parse(x);
                    almanac.Seed2SoilMap.Add(y.source, y.destination, y.n);

                    i++;
                }
            }

            if (inputs[i].StartsWith("soil-to-fertilizer map:"))
            {
                i += 1;
                while (i < inputs.Length && !string.IsNullOrWhiteSpace(inputs[i]))
                {
                    var x = inputs[i].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(long.Parse).ToArray();
                    var y = Parse(x);
                    almanac.Soil2FertilizerMap.Add(y.source, y.destination, y.n);

                    i++;
                }
            }

            if (inputs[i].StartsWith("fertilizer-to-water map:"))
            {
                i += 1;
                while (i < inputs.Length && !string.IsNullOrWhiteSpace(inputs[i]))
                {
                    var x = inputs[i].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(long.Parse).ToArray();
                    var y = Parse(x);
                    almanac.Fertilizer2WaterMap.Add(y.source, y.destination, y.n);

                    i++;
                }
            }

            if (inputs[i].StartsWith("water-to-light map:"))
            {
                i += 1;
                while (i < inputs.Length && !string.IsNullOrWhiteSpace(inputs[i]))
                {
                    var x = inputs[i].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(long.Parse).ToArray();
                    var y = Parse(x);
                    almanac.Water2LightMap.Add(y.source, y.destination, y.n);

                    i++;
                }
            }

            if (inputs[i].StartsWith("light-to-temperature map:"))
            {
                i += 1;
                while (i < inputs.Length && !string.IsNullOrWhiteSpace(inputs[i]))
                {
                    var x = inputs[i].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(long.Parse).ToArray();
                    var y = Parse(x);
                    almanac.Light2TemperatureMap.Add(y.source, y.destination, y.n);

                    i++;
                }
            }

            if (inputs[i].StartsWith("temperature-to-humidity map:"))
            {
                i += 1;
                while (i < inputs.Length && !string.IsNullOrWhiteSpace(inputs[i]))
                {
                    var x = inputs[i].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(long.Parse).ToArray();
                    var y = Parse(x);
                    almanac.Temperature2HumidityMap.Add(y.source, y.destination, y.n);

                    i++;
                }
            }

            if (inputs[i].StartsWith("humidity-to-location map:"))
            {
                i += 1;
                while (i < inputs.Length && !string.IsNullOrWhiteSpace(inputs[i]))
                {
                    var x = inputs[i].Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(long.Parse).ToArray();
                    var y = Parse(x);
                    almanac.Humidity2LocationMap.Add(y.source, y.destination, y.n);

                    i++;
                }
            }
        }

        var actual = long.MaxValue;

        foreach (var seed in almanac.Seeds)
        {
            for (var i = seed.Item1; i < seed.Item1 + seed.Item2; i++)
            {
                var soil = almanac.Seed2SoilMap.From(i);
                var fertilizer = almanac.Soil2FertilizerMap.From(soil);
                var water = almanac.Fertilizer2WaterMap.From(fertilizer);
                var light = almanac.Water2LightMap.From(water);
                var temperature = almanac.Light2TemperatureMap.From(light);
                var humidity = almanac.Temperature2HumidityMap.From(temperature);
                var location = almanac.Humidity2LocationMap.From(humidity);

                if (location < actual)
                    actual = location;
            }
        }

        actual.Should().Be(expected);
    }

    public class Almanac
    {
        public List<(long, long)> Seeds { get; set; } = new();
        public Map Seed2SoilMap { get; set; } = new();
        public Map Soil2FertilizerMap { get; set; } = new();
        public Map Fertilizer2WaterMap { get; set; } = new();
        public Map Water2LightMap { get; set; } = new();
        public Map Light2TemperatureMap { get; set; } = new();
        public Map Temperature2HumidityMap { get; set; } = new();
        public Map Humidity2LocationMap { get; set; } = new();
    }

    public class Map
    {
        private readonly List<(long source, long destination, long n)> _x = new ();
        public Dictionary<long, long> Exceptions { get; set; } = new();

        public long From(long input)
        {
            foreach (var e in _x)
            {
                if (input >= e.source && input <= e.source + e.n)
                    return input - e.source + e.destination;
            }

            return input;
        }

        public void Add(long source, long destination, long n)
        {
            _x.Add(new (source, destination, n));
        }
    }
}