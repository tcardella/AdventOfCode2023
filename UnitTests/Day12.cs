using System.Drawing;
using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/12
/// </summary>
public class Day12
{
    [Theory]
    [InlineData("Input/12/example.txt", 21)]
    //[InlineData("Input/12/input.txt", 9556712)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var a = from e in inputs
            select e.Split(' ');

        var b = from e in a
            select new { Pattern = e[0], Groups = e[1].Split(',').Select(int.Parse) };
        
        
    }
    
    [Theory]
    [InlineData("Input/12/example.txt", 1030, 10)]
    [InlineData("Input/12/example.txt", 8410, 100)]
    [InlineData("Input/12/example.txt", 82000210, 1_000_000)]
    [InlineData("Input/12/input.txt", 678626199476, 1_000_000)]
    public async Task Part2(string inputFilePath, long expected, long expansion)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

    }
}