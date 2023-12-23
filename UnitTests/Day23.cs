using System.Drawing;
using System.Numerics;
using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/23
/// </summary>
public class Day23
{
    private Vector2[] _directions;
    private Point _end;
    private char[][] _map;

    [Theory]
    [InlineData("Input/23/example.txt", 94)]
    [InlineData("Input/23/input.txt", 2130)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        _map = inputs.Select(e => e.ToCharArray()).ToArray();

        _directions = new[] { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };

        var tree = BuildTree();

        ConnectTree(tree);

        var start = tree.Nodes.First().Value;
        _end = tree.Nodes.Last().Key;

        var visitedNodes = new List<Node>();
        
        var actual = DFS(start, visitedNodes)+2;

        actual.Should().Be(expected);
    }

    private int DFS(Node parentNode, List<Node> visitedNodes)
    {
        Console.WriteLine(parentNode.Point);
        
        var maxLength = 0;
        
        visitedNodes.Add(parentNode);
        
        foreach (var childNode in parentNode.Children)
        {
            if (childNode.Point == _end)
                return maxLength;
            
            if (!visitedNodes.Contains(childNode))
            {
                var length = 1 + DFS(childNode, visitedNodes);
                maxLength = Math.Max(maxLength, length);
            }
            else
            {
                var i = 0;
            }
        }

        visitedNodes.Remove(parentNode);

        return maxLength;
    }
    
    private void ConnectTree(Tree tree)
    {
        foreach (var point in tree.Nodes.Keys)
        {
            var currentNode = tree.Nodes[point];
           
            var x = point.Add(new Vector2(0, -1));
            if (tree.Nodes.TryGetValue(x, out var up))
            {
                if (up.Type is '.' or '^')
                    currentNode.Children.Add(tree.Nodes[x]);
            }

            x = point.Add(new Vector2(0, 1));
            if (tree.Nodes.TryGetValue(x, out var down))
            {
                if (down.Type is '.' or 'v')
                    currentNode.Children.Add(tree.Nodes[x]);
            }

            x = point.Add(new Vector2(1, 0));
            if (tree.Nodes.TryGetValue(x, out var right))
            {
                if (right.Type is '.' or '>')
                    currentNode.Children.Add(tree.Nodes[x]);
            }

            x = point.Add(new Vector2(-1, 0));
            if (tree.Nodes.TryGetValue(x, out var left))
            {
                if (left.Type is '.' or '<')
                    currentNode.Children.Add(tree.Nodes[x]);
            }
        }
    }

    private Tree BuildTree()
    {
        var tree = new Tree();

        for (var i = 0; i < _map.Length; i++)
        for (var j = 0; j < _map[i].Length; j++)
            if (_map[i][j] != '#')
                tree.Add(new Point(j, i), _map[i][j]);
        return tree;
    }

    private static bool IsValidPoint(char[][] map, Point point) =>
        point.X >= 1 && point.X < map.Length && point.Y >= 0 && point.Y < map[0].Length;

    private Point FindStart(char[] row)
    {
        return new Point(row.First(e => e == '.'), 0);
    }

    [Theory]
    [InlineData("Input/23/example.txt", 145)]
    [InlineData("Input/23/input.txt", 284674)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);
    }

    public class Tree
    {
        public Dictionary<Point, Node> Nodes { get; set; } = new();

        public void Add(Point point, char type)
        {
            Nodes.Add(point, new Node(point, type));
        }
    }

    public class Node
    {
        public Node(Point point, char type)
        {
            Point = point;
            Type = type;
        }

        public Point Point { get; }
        public char Type { get; }
        public List<Node> Children { get; } = new();
    }
}