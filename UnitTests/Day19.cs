using System.Drawing;
using System.Numerics;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/19
/// </summary>
public class Day19
{
    [Theory]
    [InlineData("Input/19/example.txt", 62)]
    [InlineData("Input/19/input.txt", 511498)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var input = await File.ReadAllTextAsync(inputFilePath);

        var inputs = input.Split("\r\n\r\n");
        if (inputs.Length == 1)
            inputs = input.Split("\n\n");
        
        var rawWorkflows = inputs[0].Split("\n");
        var workflows = new Dictionary<string, IEnumerable<string>>();

        foreach (var rawWorkflow in rawWorkflows)
        {
            var first = rawWorkflow.IndexOf('{');
            var last = rawWorkflow.IndexOf('}');

            workflows.Add(rawWorkflow.Substring(0, first), rawWorkflow.Substring(first+1, last-first).Split(','));
        }

        var parts = inputs[1].Split("\n").Select(e => e.Substring(1, e.Length - 2).Split(','));

        var sum = 0;
        
        // iterate over the parts
        
        // iterate over the workflows until the part matches the first condition of a workflow
        
        // execute the workflow
        
        // if the part is accepted
            // sum += x + m+a+s

            sum.Should().Be(expected);
    }

    private void Dump(char[][] map)
    {
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[0].Length; j++)
            {
                Console.Write(map[i][j]);
            }
            
            Console.WriteLine();
        }
    }

    [Theory]
    [InlineData("Input/19/example1.txt", 145)]
    [InlineData("Input/19/input.txt", 284674)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

    }
}