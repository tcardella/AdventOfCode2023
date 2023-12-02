namespace _02.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var availableCubes = new Dictionary<string, int>() { { "red", 12 }, { "green", 13 }, { "blue", 14 } };
            var inputs = await File.ReadAllLinesAsync("Inputs/actual-1.txt");
            var sum = 0;

            foreach (var input in inputs)
            {
                var line = input.Split(':');
                var gameNumber = line.First().Split(' ').Last();
                var game = line.TakeLast(1).First();
                var sets = game.Split(';');

                var isValid = true;
                var d = new Dictionary<string, int>();

                foreach (var set in sets)
                {
                    var cubes = set.Split(',')
                        .Select(e => e.Trim())
                        .Select(e => e.Split(' '))
                        .ToDictionary(e => e[1], e => int.Parse( e[0]));
                    
                    foreach (var cube in cubes)
                    {
                        if(!d.ContainsKey(cube.Key))
                            d.Add(cube.Key, cube.Value);
                        else
                        {
                            if (d[cube.Key] < cube.Value)
                                d[cube.Key] = cube.Value;
                        }

                        //if (availableCubes[cube.Key] < cube.Value)
                        //{
                        //    isValid = false;
                        //    break;
                        //}
                    }
                }

                //if (isValid)
                //    sum += int.Parse(gameNumber);

                var aggregate = d.Values.Aggregate((a, b) => a * b);
                sum += aggregate;
            }

            Assert.Equal(8, sum);
        }
    }
}