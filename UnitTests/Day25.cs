using System.Drawing;
using System.Net.Http.Headers;
using System.Numerics;
using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/25
/// </summary>
public class Day25
{
    [Theory]
    [InlineData("Input/25/example.txt", 54)]
    //[InlineData("Input/25/input.txt", 2130)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var graph = inputs.Select(input => input.Split(':'))
            .Where(e=> e.Any())
            .ToDictionary(e => e[0], e => e[1].Split(' ').Select(f => f.Trim()).ToHashSet());

        var groups = DivideGraph(graph);

        var product = groups.Aggregate(1, (current, group) => current * group.Count);

        product.Should().Be(expected);
    }

    [Theory]
    //[InlineData("Input/25/example.txt", 154)]
    //[InlineData("Input/25/input.txt", 284674)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);
    }
    static List<List<string>> DivideGraph(Dictionary<string, HashSet<string>> graph)
    {
        var originalGraph = graph.ToDictionary(entry => entry.Key, entry => entry.Value.ToHashSet());
        var nodes = new HashSet<string>(graph.Keys.Concat(graph.Values.SelectMany(v => v))); // Include both keys and values

        var minCutNodes = MinCut(originalGraph, nodes);

        // Split the graph into two groups based on the minimum cut
        var group1 = nodes.Except(minCutNodes).ToList();
        var group2 = minCutNodes.ToList();

        return new List<List<string>> { group1, group2 };
    }

    static HashSet<string> MinCut(Dictionary<string, HashSet<string>> graph, HashSet<string> nodes)
    {
        // Stoer-Wagner algorithm for finding the minimum cut
        var bestMinCut = int.MaxValue;
        HashSet<string> bestMinCutNodes = null;

        while (nodes.Count > 1)
        {
            var visited = new HashSet<string>();
            var startNode = nodes.First();

            // Perform a depth-first search
            var minCutNodes = DFS(graph, startNode, visited);

            // Calculate the size of the cut
            var cutSize = 0;
            foreach (var node in minCutNodes)
            {
                cutSize += graph[node].Count;
                graph[node].ExceptWith(minCutNodes); // Contract the nodes in the cut
            }

            // Update the best minimum cut if necessary
            if (cutSize < bestMinCut)
            {
                bestMinCut = cutSize;
                bestMinCutNodes = minCutNodes;
            }

            // Remove the contracted nodes from the set of nodes
            nodes.ExceptWith(minCutNodes);
        }

        return bestMinCutNodes;
    }

    static HashSet<string> DFS(Dictionary<string, HashSet<string>> graph, string startNode, HashSet<string> visited)
    {
        var stack = new Stack<string>();
        stack.Push(startNode);
        visited.Add(startNode);

        while (stack.Count > 0)
        {
            var currentNode = stack.Pop();

            foreach (var neighbor in graph[currentNode])
            {
                if (!visited.Contains(neighbor))
                {
                    stack.Push(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        return visited;
    }
}