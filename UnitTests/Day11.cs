using System.Drawing;
using System.Numerics;
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
        //universe = ExpandUniverse(universe, 1);
        
        var expandingRowIndexes = GetExpandingRowIndexes(universe);
        var expandingColumnIndexes = GetExpandingColumnIndexes(universe);

        var galaxies = ParseGalaxies(universe);

        Dump(galaxies);
        
        var start = new Point(0, 0);
        var vector = new Vector2(1, 1);
        var expansion = 1;
        var temp = start;

        var expansionPoints = expandingRowIndexes.SelectMany(r => expandingColumnIndexes, (r, c) => new Point(r, c)).Revers.ToArray();
        
        Dump(expansionPoints);

        for (var i = 0; i < expansionPoints.Length; i++)
        {
            for (var j = 0; j < galaxies.Count; j++)
            {
                var galaxy = galaxies[j];

                var x = galaxy.X;
                var y = galaxy.Y;

                if (galaxy.X > expansionPoints[i].X)
                {
                    x += expansion;
                }

                if (galaxy.Y > expansionPoints[i].Y)
                {
                    y += expansion;
                }

                galaxies[j] = new Point(x, y);
            }
            
            var lastX = expansionPoints[i].X;
            var lastY = expansionPoints[i].Y;

            //var x = galaxies.Where(g => g.X > expansionPoint.X).ToArray();

            //for (int i = 0; i < x.Count(); i++)
            //{
            //    x[i].X += expansion;
            //}

            //var y = galaxies.Where(g => g.Y > expansionPoint.Y).ToArray();

            //for (var i = 0; i < y.Count(); i++)
            //{
            //    y[i].Y += expansion;
            //}
        }

        Dump(galaxies);
        
        var pairs = GetPairs(galaxies.ToArray());

        var sum = pairs.Sum(pair => Math.Abs(pair.Item1.X - pair.Item2.X) + Math.Abs(pair.Item1.Y - pair.Item2.Y));

        sum.Should().Be(expected);
    }

    private void Dump(IEnumerable<Point> galaxies)
    {
        foreach (var galaxy in galaxies)
        {
            Console.WriteLine(galaxy.ToString());
        }
        
        Console.WriteLine();
    }

    private static List<(Point, Point)> GetGalaxyPairs(List<List<char>> universe)
    {
        var galaxies = ParseGalaxies(universe);

        return GetPairs(galaxies.ToArray());
    }

    private static List<Point> ParseGalaxies(List<List<char>> universe)
    {
        var galaxies = new List<Point>();

        for (var i = 0; i < universe.Count; i++)
        for (var j = 0; j < universe[0].Count; j++)
            if (universe[i][j] == '#')
                galaxies.Add(new Point(j, i));
        return galaxies;
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
    [InlineData("Input/11/example.txt", 8410, 1_000_000)]
    //[InlineData("Input/11/input.txt", 948)]
    public async Task Part2(string inputFilePath, int expected, int expansion)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var universe = BuildUniverse(inputs);
        universe = ExpandUniverse(universe, expansion);

        //universe[0].Count.Should().Be(13);

        var pairs = GetGalaxyPairs(universe);
    }
}