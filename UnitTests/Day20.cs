using FluentAssertions;

namespace UnitTests;

/// <summary>
///     Prompt: https://adventofcode.com/2023/day/20
/// </summary>
public class Day20
{
    [Theory]
    [InlineData("Input/20/example0.txt", 32000000)]
    [InlineData("Input/20/example1.txt", 11687500)]
    [InlineData("Input/20/input.txt", 511498)] // TODO: should be greater than 850885266
    public async Task Part1(string inputFilePath, long expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);

        var modules = new Dictionary<string, ModuleBase>();

        string firstModule = null;

        for (var i = 0; i < inputs.Length; i++)
        {
            var parts = inputs[i].Split("->").Select(e => e.Trim()).ToArray();

            if (i == 0) firstModule = parts[0];

            var destinations = parts[1].Split(',').Select(e => e.Trim()).ToArray();

            if (parts[0] == "broadcaster") modules.Add(parts[0], new BroadcastModule(parts[0], destinations));

            var name = parts[0].Substring(1);

            if (parts[0].StartsWith('%')) modules.Add(name, new FlipFlopModule(name, destinations));

            if (parts[0].StartsWith('&')) modules.Add(name, new ConjunctionModule(name, destinations));
        }

        var conjunctionModules = modules.Values.Where(e => e is ConjunctionModule).Cast<ConjunctionModule>();
        foreach (var conjunctionModule in conjunctionModules)
        {
            var sourceModules = modules.Values.Where(e => e.Destinations.Contains(conjunctionModule.Name)).Distinct();

            //Console.WriteLine(string.Join(',', sourceModules.Select(e=> e.Name)));

            conjunctionModule.Watch(sourceModules.Select(e => e.Name));
        }

        modules.Should().HaveSameCount(inputs);

        long lowCount = 0;
        long highCount = 0;

        var messageQueue = new Queue<Message>();

        var button = new ButtonModule(new[] { "broadcaster" }!);

        for (var i = 0; i < 1000; i++)
        {
            foreach (var message in button.Send(button.Name, Pulse.Low)) messageQueue.Enqueue(message);

            while (messageQueue.TryDequeue(out var message))
            {
                Console.WriteLine($"{message.From} -{message.Pulse}-> {message.Destination}");

                if (message.Pulse == Pulse.Low)
                    lowCount++;
                else
                    highCount++;

                if (modules.TryGetValue(message.Destination, out var module))
                    foreach (var newMessage in module.Send(message.From, message.Pulse))
                        messageQueue.Enqueue(newMessage);
            }

            Console.WriteLine();
        }

        (lowCount * highCount).Should().Be(expected);
    }

    [Theory]
    [InlineData("Input/20/example.txt", 145)]
    [InlineData("Input/20/input.txt", 284674)]
    public async Task Part2(string inputFilePath, int expected)
    {
        var inputs = await File.ReadAllLinesAsync(inputFilePath);
    }

    public class FlipFlopModule : ModuleBase
    {
        private bool state;

        public FlipFlopModule(string name, IEnumerable<string> destinations) : base(name, destinations)
        {
        }

        public override IEnumerable<Message> Send(string from, Pulse pulse)
        {
            if (pulse == Pulse.High)
                return Array.Empty<Message>();

            if (state == false)
            {
                state = true;
                return Destinations.Select(d => new Message(Name, d, Pulse.High));
            }

            state = false;
            return Destinations.Select(d => new Message(Name, d, Pulse.Low));
        }
    }

    public class ConjunctionModule : ModuleBase
    {
        private readonly Dictionary<string, Pulse> _memory = new();

        public ConjunctionModule(string name, IEnumerable<string> destinations) : base(name, destinations)
        {
        }

        public override IEnumerable<Message> Send(string from, Pulse pulse)
        {
            _memory[from] = _memory[from] == Pulse.Low
                ? Pulse.High
                : Pulse.Low;

            var output = Pulse.High;

            if (_memory.Values.All(e => e == Pulse.High))
                output = Pulse.Low;

            return Destinations.Select(d => new Message(Name, d, output));
        }

        public void Watch(IEnumerable<string> sourceModules)
        {
            foreach (var sourceModule in sourceModules) _memory.Add(sourceModule, Pulse.Low);
        }
    }

    public class BroadcastModule : ModuleBase
    {
        public BroadcastModule(string name, IEnumerable<string> destinations) : base(name, destinations)
        {
        }

        public override IEnumerable<Message> Send(string from, Pulse pulse)
        {
            return Destinations.Select(d => new Message(Name, d, pulse));
        }
    }

    public class ButtonModule : ModuleBase
    {
        public ButtonModule(IEnumerable<string> destinations) : base("Button", destinations)
        {
        }

        #region Overrides of ModuleBase

        public override IEnumerable<Message> Send(string from, Pulse pulse) =>
            Destinations.Select(d => new Message(Name, d, pulse));

        #endregion
    }
}

public abstract class ModuleBase
{
    protected ModuleBase(string name, IEnumerable<string> destinations)
    {
        Name = name;
        Destinations = destinations;
    }

    public string Name { get; }
    public IEnumerable<string> Destinations { get; }

    public abstract IEnumerable<Message> Send(string from, Pulse pulse);
}

public record Message(string From, string Destination, Pulse Pulse);

public enum Pulse
{
    Low,
    High
}