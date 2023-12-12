using System.Drawing;
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
        var galaxies = ParseGalaxies(universe).Select(g=> new GalaxyOffsets(g,0,0)).ToArray();

        Dump(galaxies);
        
        var expandingRowIndexes = GetExpandingRowIndexes(universe);
        for (int i = 0; i < expandingRowIndexes.Count; i++)
        {
            var temp = galaxies.Where(e => e.Point.Y > expandingRowIndexes[i]);
            foreach (var t in temp)
            {
                t.OffsetY += 1;
            }
        }

        var expandingColumnIndexes = GetExpandingColumnIndexes(universe);

        for (int i = 0; i < expandingColumnIndexes.Count; i++)
        {
            var temp = galaxies.Where(e => e.Point.X > expandingColumnIndexes[i]);
            foreach (var t in temp)
            {
                t.OffsetX += 1;
            }
        }

        Dump(galaxies);
        
        var newGalaxies = galaxies.Select(e => new Point(e.Point.X + e.OffsetX, e.Point.Y + e.OffsetY)).ToArray();

        //universe = ExpandUniverse(universe, 1);
        
        var pairs = GetPairs(newGalaxies);

        var sum = pairs.Sum(pair => Math.Abs(pair.Item1.X - pair.Item2.X) + Math.Abs(pair.Item1.Y - pair.Item2.Y));

        sum.Should().Be(expected);
    }

    private void Dump(GalaxyOffsets[] galaxies)
    {
        foreach (var galaxy in galaxies)
        {
            Console.WriteLine($"{galaxy.Point}: {galaxy.OffsetX}   {galaxy.OffsetY}");
        }
        
        Console.WriteLine();
    }

    public class GalaxyOffsets(Point point, long offsetX, long offsetY)
    {
        public Point Point { get; } = point;
        public long OffsetX { get; set; } = offsetX;
        public long OffsetY { get;set; } = offsetY;
    }

    private IEnumerable<Point> ParseGalaxies(List<List<char>> universe)
    {
        for (int i = 0; i < universe.Count; i++)
        {
            for (int j = 0; j < universe[0].Count; j++)
            {
                if (universe[i][j] == '#')
                    yield return new Point(j, i);
            }
        }
    }
    
    private static List<(Point, Point)> GetGalaxyPairs(List<List<char>> universe)
    {
        var galaxies = new List<Point>();

        for (var i = 0; i < universe.Count; i++)
        for (var j = 0; j < universe[0].Count; j++)
            if (universe[i][j] == '#')
                galaxies.Add(new Point(j, i));

        //galaxies.Should().HaveCount(9);

        var pairs = GetPairs(galaxies.ToArray());
        //pairs.Should().HaveCount(36);
        return pairs;
    }

    private List<List<char>> ExpandUniverse(List<List<char>> universe, int expansion = 1)
    {
        var expandingRowIndexes = GetExpandingRowIndexes(universe);
        var expandingColumnIndexes = GetExpandingColumnIndexes(universe);

        var emptyRow = Enumerable.Range(0, universe[0].Count).Select(_ => '.').ToList();
        var emptyRows = Enumerable.Range(0, expansion).Select(_ => emptyRow).ToArray();
        
        for (var i = expandingRowIndexes.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < expansion; j++)
            {
                var row = expandingRowIndexes[i];
                universe.InsertRange(row, emptyRows);
            }
        }

        var emptyColumns = Enumerable.Range(0, expansion).Select(_ => '.').ToArray();

        for (var i = expandingColumnIndexes.Count - 1; i >= 0; i--)
        {
            var column = expandingColumnIndexes[i];

            for (var j = 0; j < universe.Count; j++)
            {
                universe[j].InsertRange(column, emptyColumns);
            }
        }

        return universe;
    }

    private static List<int> GetExpandingColumnIndexes(List<List<char>> universe)
    {
        var expandingColumnIndexes = new List<int>();

        for (var column = 0; column < universe[0].Count; column++)
        {
            var canAdd = true;

            for (var row = 0; row < universe.Count; row++)
                if (universe[row][column] != '.')
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
        var universe = new List<List<char>>();


        foreach (var input in inputs) universe.Add(input.Select(e => e).ToList());
        return universe;
    }

    public static List<(Point, Point)> GetPairs(Point[] inputs)
    {
        var pairs = new List<(Point, Point)>();
        for (var i = 0; i < inputs.Length; i++)
        for (var j = i + 1; j < inputs.Length; j++)
            pairs.Add((inputs[i], inputs[j]));
        return pairs;
    }

    public void Dump(List<List<char>> universe)
    {
        for (var i = 0; i < universe.Count; i++)
        {
            for (var j = 0; j < universe[0].Count; j++) Console.Write(universe[i][j]);

            Console.WriteLine();
        }

        Console.WriteLine();
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

        Dump(galaxies);

        var expandingRowIndexes = GetExpandingRowIndexes(universe);
        for (int i = 0; i < expandingRowIndexes.Count; i++)
        {
            var temp = galaxies.Where(e => e.Point.Y > expandingRowIndexes[i]);
            foreach (var t in temp)
            {
                t.OffsetY += expansion-1;
            }
        }

        var expandingColumnIndexes = GetExpandingColumnIndexes(universe);

        for (int i = 0; i < expandingColumnIndexes.Count; i++)
        {
            var temp = galaxies.Where(e => e.Point.X > expandingColumnIndexes[i]);
            foreach (var t in temp)
            {
                t.OffsetX += expansion-1;
            }
        }

        Dump(galaxies);

        var newGalaxies = galaxies.Select(e => new Point(e.Point.X + e.OffsetX, e.Point.Y + e.OffsetY)).ToArray();

        //universe = ExpandUniverse(universe, 1);

        var pairs = GetPairs(newGalaxies);

        long sum = pairs.Sum(pair => Math.Abs(pair.Item1.X - pair.Item2.X) + Math.Abs(pair.Item1.Y - pair.Item2.Y));

        sum.Should().Be(expected);
    }

    public record Point(long X, long Y);
}