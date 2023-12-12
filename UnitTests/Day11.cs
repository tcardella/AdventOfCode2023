using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/11
/// </summary>
public class Day11
{
    [Theory]
    [InlineData("Input/11/example.txt", 374)]
    [InlineData("Input/11/input.txt", 9556712)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var universe = BuildUniverse(inputs);
        var galaxies = ParseGalaxies(universe).Select(g => new GalaxyOffsets(g, 0, 0)).ToArray();

        foreach (var t in GetExpandingRowIndexes(universe).Select(r => galaxies.Where(e => e.Point.Y > r))
                     .SelectMany(e => e)) t.OffsetY += 1;

        foreach (var t in GetExpandingColumnIndexes(universe).Select(c => galaxies.Where(e => e.Point.X > c))
                     .SelectMany(e => e)) t.OffsetX += 1;

        var expandedGalaxies = galaxies.Select(e => new Point(e.Point.X + e.OffsetX, e.Point.Y + e.OffsetY)).ToArray();

        var sum = GetPairs(expandedGalaxies)
            .Sum(pair => Math.Abs(pair.Item1.X - pair.Item2.X) + Math.Abs(pair.Item1.Y - pair.Item2.Y));

        sum.Should().Be(expected);
    }

    private IEnumerable<Point> ParseGalaxies(List<List<char>> universe)
    {
        for (var i = 0; i < universe.Count; i++)
        for (var j = 0; j < universe[0].Count; j++)
            if (universe[i][j] == '#')
                yield return new Point(j, i);
    }

    private static List<int> GetExpandingColumnIndexes(List<List<char>> universe)
    {
        var expandingColumnIndexes = new List<int>();

        for (var column = 0; column < universe[0].Count; column++)
        {
            var canAdd = true;

            foreach (var row in universe)
                if (row[column] != '.')
                    canAdd = false;

            if (canAdd)
                expandingColumnIndexes.Add(column);
        }

        return expandingColumnIndexes;
    }

    private static List<int> GetExpandingRowIndexes(List<List<char>> universe)
    {
        var expandingRowIndexes = new List<int>();
        for (var row = 0; row < universe.Count; row++)
        {
            var canAdd = true;

            for (var column = 0; column < universe[0].Count; column++)
                if (universe[row][column] != '.')
                    canAdd = false;

            if (canAdd)
                expandingRowIndexes.Add(row);
        }

        return expandingRowIndexes;
    }

    private static List<List<char>> BuildUniverse(string[] inputs)
    {
        return inputs.Select(input => input.Select(e => e).ToList()).ToList();
    }

    private static List<(Point, Point)> GetPairs(Point[] inputs)
    {
        var pairs = new List<(Point, Point)>();
        for (var i = 0; i < inputs.Length; i++)
        for (var j = i + 1; j < inputs.Length; j++)
            pairs.Add((inputs[i], inputs[j]));
        return pairs;
    }

    [Theory]
    [InlineData("Input/11/example.txt", 1030, 10)]
    [InlineData("Input/11/example.txt", 8410, 100)]
    [InlineData("Input/11/example.txt", 82000210, 1_000_000)]
    [InlineData("Input/11/input.txt", 678626199476, 1_000_000)]
    public async Task Part2(string inputFilePath, long expected, long expansion)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var universe = BuildUniverse(inputs);
        var galaxies = ParseGalaxies(universe).Select(g => new GalaxyOffsets(g, 0, 0)).ToArray();

        foreach (var t in GetExpandingRowIndexes(universe).Select(r => galaxies.Where(e => e.Point.Y > r))
                     .SelectMany(e => e)) t.OffsetY += expansion - 1;

        foreach (var t in GetExpandingColumnIndexes(universe).Select(c => galaxies.Where(e => e.Point.X > c))
                     .SelectMany(e => e)) t.OffsetX += expansion - 1;

        var newGalaxies = galaxies.Select(e => new Point(e.Point.X + e.OffsetX, e.Point.Y + e.OffsetY)).ToArray();

        var pairs = GetPairs(newGalaxies);

        var sum = pairs.Sum(pair => Math.Abs(pair.Item1.X - pair.Item2.X) + Math.Abs(pair.Item1.Y - pair.Item2.Y));

        sum.Should().Be(expected);
    }

    private class GalaxyOffsets(Point point, long offsetX, long offsetY)
    {
        public Point Point { get; } = point;
        public long OffsetX { get; set; } = offsetX;
        public long OffsetY { get; set; } = offsetY;
    }

    private record Point(long X, long Y);
}