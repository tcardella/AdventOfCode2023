using System.Collections;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/9
/// </summary>
public class Day10

{
    private readonly List<char[]> _map = new();

    private readonly Pipe[] _pipes =
    {
        new('|', Direction.North, Direction.South),
        new('-', Direction.East, Direction.West),
        new('L', Direction.North, Direction.East),
        new('J', Direction.North, Direction.West),
        new('7', Direction.South, Direction.West),
        new('F', Direction.South, Direction.East)
    };

    private readonly List<char[]> _tiles = new();


    [Theory]
    [InlineData("Input/10/example0.txt", 4)]
    [InlineData("Input/10/example1.txt", 8)]
    [InlineData("Input/10/input.txt", 6831)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        foreach (var e in inputs) _tiles.Add(e.Select(f => f).ToArray());

        foreach (var r in inputs) _map.Add(r.Select(_ => '.').ToArray());

        var start = FindS(_tiles);
        _map[start.Y][start.X] = '0';

        Dump(_tiles);

        var next = new Queue<Point>();
        next.Enqueue(start);

        var round = 0;

        while (next.Any())
        {
            var temp = new List<Point>();
            round++;

            while (next.TryDequeue(out var item)) temp.AddRange(FindNextPoint(item, round));

            //Dump(_map);

            foreach (var point in temp) next.Enqueue(point);
        }

        (round - 1).Should().Be(expected);
    }

    public List<Point> FindNextPoint(Point current, int round, char marker = 'X')
    {
        var outputs = new List<Point>();

        foreach (var e in GetValidPipesAndVectors(current))
        {
            var next = current.Add(e.Key);

            if (_tiles.IsPointOnGrid(next))
            {
                var currentTile = _tiles[next.Y][next.X];

                if (currentTile is not ('.' and 'S') && e.Value.Contains(currentTile) && _map[next.Y][next.X] == '.')
                {
                    //_map[next.Y][next.X] = round.ToString().ToCharArray()[0];
                    _map[next.Y][next.X] = marker;
                    outputs.Add(new Point(next.X, next.Y));
                }
            }
        }

        return outputs;
    }

    private Dictionary<Vector2, IEnumerable<char>> GetValidPipesAndVectors(Point input)
    {
        var output = new Dictionary<Vector2, IEnumerable<char>>();

        var up = new Vector2(0, -1);
        var left = new Vector2(-1, 0);
        var right = new Vector2(1, 0);
        var down = new Vector2(0, 1);

        switch (_tiles[input.Y][input.X])
        {
            case 'S':
                output.Add(up, new[] { '|', '7', 'F' });
                output.Add(left, new[] { '-', 'L', 'F' });
                output.Add(right, new[] { '-', 'J', '7' });
                output.Add(down, new[] { '|', 'L', 'J' });
                break;
            case '|':
                output.Add(up, new[] { '|', '7', 'F' });
                output.Add(down, new[] { '|', 'L', 'J' });
                break;
            case '-':
                output.Add(left, new[] { '-', 'L', 'F' });
                output.Add(right, new[] { '-', 'J', '7' });
                break;
            case 'L':
                output.Add(up, new[] { '|', '7', 'F' });
                output.Add(right, new[] { '-', 'J', '7' });
                break;
            case 'J':
                output.Add(left, new[] { '-', 'L', 'F' });
                output.Add(up, new[] { '|', '7', 'F' });
                break;
            case '7':
                output.Add(left, new[] { '-', 'L', 'F' });
                output.Add(down, new[] { '|', 'L', 'J' });
                break;
            case 'F':
                output.Add(right, new[] { '-', 'J', '7' });
                output.Add(down, new[] { '|', 'L', 'J' });
                break;
            default:
                throw new ArgumentOutOfRangeException(_tiles[input.Y][input.X].ToString());
        }

        return output;
    }

    private static Point FindS(List<char[]> input)
    {
        for (var i = 0; i < input.Count; i++)
        for (var j = 0; j < input[i].Length; j++)
            if (input[i][j] == 'S')
                return new Point(j, i);

        throw new Exception("The pipe 'S' could not be found.");
    }

    public void Dump(IEnumerable<IEnumerable<char>> input)
    {
        Console.WriteLine();

        foreach (var row in input)
        {
            foreach (var column in row) Console.Write(column);

            Console.WriteLine();
        }
    }

    [Theory]
    [InlineData("Input/10/example2.txt", 4)]
    [InlineData("Input/10/example3.txt", 8)]
    [InlineData("Input/10/example4.txt", 10)]
    [InlineData("Input/10/input.txt", 948)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        foreach (var e in inputs) _tiles.Add(e.Select(f => f).ToArray());

        foreach (var r in inputs) _map.Add(r.Select(_ => '.').ToArray());

        var start = FindS(_tiles);
        char startChar = 'S';

        //Dump(_tiles);
        //Dump(_map);

        var next = new Queue<Point>();
        next.Enqueue(start);

        var pipePoints = new List<Point> { start };

        var round = 0;

        while (next.Any())
        {
            var temp = new List<Point>();
            round++;

            while (next.TryDequeue(out var item))
            {
                temp.AddRange(FindNextPoint(item, round));


                if (item == start)
                {
                    var a = temp[0];
                    var b = temp[1];

                 startChar = FindMissingPieceBetween2Pieces(_tiles, start, a, b);
                }
            }

            foreach (var point in temp)
            {
                pipePoints.Add(point);
                next.Enqueue(point);
            }
        }

        //Dump(_tiles);

        _tiles[start.Y][start.X] = startChar;

        _map[start.Y][start.X] = 'X';

var map =        ClearJunkPipe(_map, pipePoints);

        //Dump(_tiles);
        //Dump(_map);

        var zoomedMap = ZoomMap(_map, _tiles);

        Dump(zoomedMap);

        //next.Clear();



        //Dump(_map);

        FindAndMarkPeriods(zoomedMap);

var actual =        CountEnclosedTiles(zoomedMap);
        
        //int count = 0;
        //for (int i = 0; i < _map.Count; i++)
        //{
        // for (int j = 0; j < _map[0].Length; j++)
        //    {
        //        if (_map[i][j] == 'I')
        //            count++;
        //    }
        //}


        actual.Should().Be(expected);


    }

    private static int CountEnclosedTiles(List<char[]> map)
    {
        var count = 0;
        
        for (int i = 0; i < map.Count; i+=3)
        {
            for (int j = 0; j < map[i].Count(); j+=3)
            {
                var iStart = i;
                var iEnd = iStart +3;
                
                var jStart = j;
                var jEnd = jStart + 3;

                var points = new List<Point>()
                {
                    new Point(jStart, iStart),
                    new Point(jStart + 1, iStart),
                    new Point(jStart + 2, iStart),
                    new Point(jStart, iStart + 1),
                    new Point(jStart + 1, iStart + 1),
                    new Point(jStart + 2, iStart + 1),
                    new Point(jStart, iStart + 2),
                    new Point(jStart + 1, iStart + 2),
                    new Point(jStart + 2, iStart + 2),
                };

                if (points.All(p => map[p.Y][p.X] == 'I'))
                    count++;
            }
        }

        return count;
    }

    private char FindMissingPieceBetween2Pieces(List<char[]> map, Point start, Point a, Point b)
    {
        var start2AVector = new Vector2(a.X - start.X, a.Y - start.Y);
        var start2BVector = new Vector2(b.X - start.X, b.Y - start.Y);

        var aChar = map[a.Y][a.X];
        var bChar = map[b.Y][b.X];

        var vector = new Vector2(b.X - a.X, b.Y-a.Y);

var v =        Vector2.Normalize(vector);

            switch (v)
            {
                case { X: 0, Y: 1 or -1 }:
                    return '|';
                case { X: 1 or -1, Y: 0 }:
                    return '-';
                case { X: -1, Y: -1 }:
                    return 'L';
                case { X: 1, Y: -1 }:
                    return 'J';
                case { X: 1, Y: 1 }:
                    return '7';
                case { X: -1, Y: 1 }:
                    return 'F';
            }

        return '.';
    }

    private List<char[]> ZoomMap(List<char[]> source, List<char[]> pipes)
    {
        var target = BuildZoomedMap(source);

        for (int i = 0; i < source.Count; i++)
        {
            for (int j = 0; j < source[0].Count(); j++)
            {
                if (source[i][j] == 'X')
                {
                    Copy(i, j, target, pipes[i][j]);
                }
            }
        }

        return target;
    }

    private static List<char[]> BuildZoomedMap(List<char[]> source)
    {
        var height = source.Count * 3;
        var width = source[0].Count() * 3;

        var target = new List<char[]>();

        for (int i = 0; i < height; i++)
        {
            var row = new char[width];

            for (int j = 0; j < row.Length; j++)
            {
                row[j] = '.';
            }

            target.Add(row);
        }

        return target;
    }

    private static void Copy(int startY, int startX, List<char[]> target, char c)
    {
        var t = GetZoomedChars(c);

        for (int i = 0; i < t.Count; i++)
        {
            for (int j = 0; j < t[0].Length; j++)
            {
                var scaledY = (startY * 3) + i;
                var scaledX = (startX * 3) + j;

                target[scaledY][scaledX] = t[i][j];
            }
        }
    }


    private static List<char[]> GetZoomedChars(char c)
    {
        return c switch
        {
            'S'=> new List<char[]>
            {
                new char[] { 'X', '.', 'X' },
                new char[] { '.', 'X', '.' },
                new char[] { 'X', '.', 'X' }
            },
            '|' => new List<char[]>
            {
                new char[] { '.', 'X', '.' },
                new char[] { '.', 'X', '.' },
                new char[] { '.', 'X', '.' }
            },
            '-' =>
            [
                new char[] { '.', '.', '.' },
                new char[] { 'X', 'X', 'X' },
                new char[] { '.', '.', '.' }
            ],
            'L' =>
            [
                new char[] { '.', 'X', '.' },
                new char[] { '.', 'X', 'X' },
                new char[] { '.', '.', '.' }
            ],
            'J' =>
            [
                new char[] { '.', 'X', '.' },
                new char[] { 'X', 'X', '.' },
                new char[] { '.', '.', '.' }
            ],
            '7' =>
            [
                new char[] { '.', '.', '.' },
                new char[] { 'X', 'X', '.' },
                new char[] { '.', 'X', '.' }
            ],
            'F' =>
            [
                new char[] { '.', '.', '.' },
                new char[] { '.', 'X', 'X' },
                new char[] { '.', 'X', '.' }
            ],
            '.' =>
            [
                new char[] { '.', '.', '.' },
                new char[] { '.', '.', '.' },
                new char[] { '.', '.', '.' },
            ]
        };
    }

    private List<char[]> ClearJunkPipe(List<char[]> map, List<Point> points)
    {
        var allPoints = new List<Point>();

        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                allPoints.Add(new Point(j, i));
            }
        }

        var x = allPoints.Except(points);

        foreach (var point in x)
        {
            map[point.Y][point.X] = '.';
        }

        return map;
    }

    private void FindAndMarkPeriods(List<char[]> map)
    {
        var queue = new Queue<Point>();

        var start = FindNextPeriod(map);
        
        while (start.HasValue)
        {
            var group = new List<Point> { start.Value };
            queue.Enqueue(start.Value);

            while (queue.TryDequeue(out var current))
            {
                foreach (var e in GetVectors()
                             .Select(v => current.Add(v))
                             .Where(map.IsPointOnGrid)
                             .Where(e => map[e.Y][e.X] == '.'))
                {
                    map[e.Y][e.X] = 'T';
                    queue.Enqueue(e);
                    group.Add(e);
                }
            }

            var marker = 'I';
            if (group.Any(e => e.X == 0 || e.X == map[0].Length - 1 || e.Y == 0 || e.Y == map.Count - 1))
            {
                marker = 'O';
            }

            foreach (var point in group)
            {
                map[point.Y][point.X] = marker;
            }

            Dump(map);

            start = FindNextPeriod(map);
        }
        
        // check around currentTile for any more '.'
    }

    private Point? FindNextPeriod(List<char[]> map)
    {
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[0].Count(); j++)
            {
                if (map[i][j] == '.')
                    return new Point(j, i);
            }
        }

        return null;
    }

    private IEnumerable<Point> GetPeriods(Point input)
    {
        var outputs = new List<Point>();
        foreach (var e in GetVectors())
        {
            var next = input.Add(e);

            if (_tiles.IsPointOnGrid(next))
            {
                var currentTile = _map[next.Y][next.X];

                if (currentTile == '.')
                {
                    _map[next.Y][next.X] = 'O';
                    outputs.Add(new Point(next.X, next.Y));
                }
            }
        }

        return outputs;
    }

    private IEnumerable<Vector2> GetVectors()
    {
        return new Vector2[]
        {
            new Vector2(-1, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(0, -1)
        };
    }

    private List<List<int>> BuildRows(string input)
    {
        var output = new List<List<int>>();
        var row = input.Split(' ').Select(int.Parse).ToList();

        output.Add(row);

        while (row.Any(e => e != 0))
        {
            row = CalcNextRow(row);
            output.Add(row);
        }

        return output;
    }

    private List<int> CalcNextRow(List<int> input)
    {
        var output = new List<int>();

        for (var i = 1; i < input.Count; i++) output.Add(input[i] - input[i - 1]);

        return output;
    }
}

public record Pipe(char PipeChar, Direction From, Direction To);

public enum Direction
{
    North,
    South,
    East,
    West
}

public static class Extensions
{
    public static Point Add(this Point point, Vector2 vector) =>
        new((int)(point.X + vector.X), (int)(point.Y + vector.Y));

    public static bool IsPointOnGrid(this List<char[]> input, Point point)
    {
        if (point.X < 0 || point.Y < 0) return false;

        if (point.Y >= input.Count || point.X >= input[0].Length) return false;

        return true;
    }
}