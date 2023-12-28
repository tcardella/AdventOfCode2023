using System.Drawing;
using System.Numerics;
using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/24
/// </summary>
public class Day24
{
    [Theory]
    [InlineData("Input/24/example.txt", 2)]
    //[InlineData("Input/24/input.txt", 2130)]
    public async Task Part1(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        List<(Point3 position, Vector3 velocity)> y = new();

        foreach (var input in inputs)
        {
            var parts = input.Split('@');
            int[] positionParts = parts[0].Split(',').Select(e => e.Trim()).Select(int.Parse).ToArray();
            var position = new Point3(positionParts[0], positionParts[1], positionParts[2]);

            var velocityParts = parts[1].Split(',').Select(e => e.Trim()).Select(int.Parse).ToArray();
            var velocity = new Vector3(velocityParts[0], velocityParts[1], velocityParts[2]);

            y.Add((position, velocity));
        }

        var box = new Box(new Point3(7, 7, 0), new Point3(27, 27, 0));

        var z = new List<(Point3 start, Point3 end, Vector3 vector)>();

        foreach (var e in y)
        {
            var enterX = (box.Start.X - e.position.X) / e.velocity.X;
            var exitX = (box.End.X - e.position.X) / e.velocity.X;
            var enterY = (box.Start.Y - e.position.Y) / e.velocity.Y;
            var exitY = (box.End.Y - e.position.Y) / e.velocity.Y;

            z.Add((new Point3(enterX, enterY, 0), new Point3(exitX, exitY, 0),
                new Vector3(exitX - enterX, exitY - enterY, 0)));

        }

        var collisionCount = 0;

        for (int i = 0; i < y.Count; i++)
        {
            for (int j = i + 1; j < y.Count; j++)
            {
                var intPoint = CalculateIntersectionPoint(y[i].position, y[i].velocity, y[j].position, y[j].velocity);

                if (intPoint.X >= box.Start.X && intPoint.X <= box.End.X && intPoint.Y >= box.Start.Y && intPoint.Y <= box.End.Y)
                    collisionCount++;

            }
        }

        collisionCount.Should().Be(expected);
    }

    Point3 CalculateIntersectionPoint(Point3 P, Vector3 vectorP, Point3 Q, Vector3 vectorQ)
    {
        // Find the intersection point by setting the parametric equations equal to each other
        double tNumerator = (Q.X - P.X) * vectorQ.Y - (Q.Y - P.Y) * vectorQ.X;
        double sNumerator = (Q.X - P.X) * vectorP.Y - (Q.Y - P.Y) * vectorP.X;
        double denominator = vectorP.X * vectorQ.Y - vectorQ.X * vectorP.Y;

        // Check if the lines are parallel
        if (denominator == 0)
        {
            // Lines are parallel, check if they are coincident
            if ((Q.X - P.X) * vectorP.Y - (Q.Y - P.Y) * vectorP.X == 0)
            {
                // Lines are coincident, return the common point (P or Q)
                return P;
            }
            else
            {
                return new Point3(0, 0, 0);
                // Lines are parallel and non-coincident, no intersection point
                //throw new InvalidOperationException("Lines are parallel and non-coincident");
            }
        }

        // Calculate the parameters
        double t = tNumerator / denominator;
        double s = sNumerator / denominator;

        // Calculate the intersection point
        double intersectionX = P.X + t * vectorP.X;
        double intersectionY = P.Y + t * vectorP.Y;

        return new Point3((float)intersectionX, (float)intersectionY,0);
    }


    [Theory]
    //[InlineData("Input/24/example.txt", 154)]
    //[InlineData("Input/24/input.txt", 284674)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);
    }

    public class Box
    {
        public Point3 Start { get; }
        public Point3 End { get; }

        public Box(Point3 start, Point3 end)
        {
            Start = start;
            End = end;
        }
    }
    
    public class Point3
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public Point3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}