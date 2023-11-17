using System.Numerics;
using System.Linq;

namespace AdventOfCode._2022;

public class Day16 : Day<int, int>
{
    public override string Example => @"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II";

    [Test(ExpectedResult = 1651)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 1673)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 1707)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 2343)]
    public override int Two() => Two(Input);

    private static int One(string input)
    {
        var valves = input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.TrimEntries).Select(l => new Valve(l));
        var updated = new List<Valve>();
        var importantConnections = valves.Where(i => i.FlowRate > 0 || i.Name == "AA").Select(r => r.Name);
        var distances = Distances(valves, importantConnections);
        foreach (var v in valves)
        {
            updated.Add(GetDistance(v, distances));
        }

        return DeterminePath(30, updated).Max(x => x.Value);
    }

    private static int Two(string input)
    {
        var valves = input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.TrimEntries).Select(l => new Valve(l));
        var updated = new List<Valve>();
        var distances = Distances(valves, valves.Where(i => i.FlowRate > 0 || i.Name == "AA").Select(r => r.Name));
        updated.AddRange(valves.Select(v => GetDistance(v, distances)));
        var paths = DeterminePath(26, updated).Where(p => p.Value > 750);

        var maxPressure = 0;
        var i = 0;
        foreach (var path in paths)
        {
            foreach (var nextPath in paths.Skip(i + 1))
            {
                var pressure = path.Value + nextPath.Value;
                if ((path.Key & nextPath.Key) > 0 || pressure <= maxPressure) continue;
                maxPressure = pressure;
            }
            i++;
        }

        return maxPressure;
    }

    private static Dictionary<BigInteger, int> DeterminePath(int max, IEnumerable<Valve> valves)
    {
        var valveDistances = valves.First(v => v.Name == "AA").Distances
            .Select((d, i) => (d.Key, (BigInteger)Math.Pow(2, i)))
            .ToDictionary(x => x.Key, x => x.Item2);
        var evaluate = new Dictionary<int, List<State>> { { 0, new List<State> { new State("AA", max) } } };
        var paths = new Dictionary<BigInteger, int>();

        for (int i = 0; i < valveDistances.Count; i++)
        {
            evaluate[i + 1] = new List<State>();
            foreach (var state in evaluate[i])
            {
                foreach (var n in valves.First(v => v.Name == state.Name).Distances)
                {
                    if (Travel(state, valveDistances, n, valves, out State newState))
                    {
                        evaluate[i + 1].Add(newState);
                        var pressure = newState.Pressure + newState.FlowRate * newState.TimeLeft;
                        if (!paths.TryGetValue(newState.Visited, out int compare) || compare <= pressure) paths[newState.Visited] = pressure;
                    }
                }
            }
        }

        return paths;
    }

    private static bool Travel(State state, Dictionary<string, BigInteger> valveDistances, KeyValuePair<string, int> n, IEnumerable<Valve> valves, out State newState)
    {
        var dest = valves.First(v => v.Name == n.Key);
        if (state.TimeLeft - n.Value <= 0 || (state.Visited & valveDistances[n.Key]) == valveDistances[n.Key])
        {
            newState = State.Empty;
            return false;
        }
        var visited = state.Visited + valveDistances[n.Key];
        newState = new State(n.Key, state.TimeLeft - n.Value, state.FlowRate + dest.FlowRate, state.Pressure + state.FlowRate * n.Value, visited);
        return true;
    }

    private static Valve GetDistance(Valve valve, Dictionary<(string, string), int> distances)
    {
        valve.Distances = distances.Where(d => d.Key.Item1 == valve.Name).ToDictionary(x => x.Key.Item2, x => x.Value + 1);
        return valve;
    }

    private static Dictionary<(string, string), int> Distances(IEnumerable<Valve> valves, IEnumerable<string> importantConnections)
    {
        var distances = new Dictionary<(string, string), int>();
        foreach (var valve in valves)
        {
            if (!importantConnections.Any(r => r == valve.Name)) continue;

            var current = new[] { valve.Name };
            var next = Array.Empty<string>();
            var distance = 0;

            distances[(valve.Name, valve.Name)] = 0;
            while (current.Any())
            {
                distance++;
                foreach (var position in current)
                {
                    foreach (var newPosition in valves.First(i => i.Name == position).Neighbors)
                    {
                        if (!distances.ContainsKey((valve.Name, newPosition)))
                        {
                            distances[(valve.Name, newPosition)] = distance;
                            next = next.Append(newPosition).ToArray();
                        }
                    }
                }
                current = next;
                next = Array.Empty<string>();
            }
        }
        return distances;
    }

    private class Valve
    {
        public string Name { get; set; }
        public int FlowRate { get; set; }
        public IEnumerable<string> Neighbors { get; set; }
        public Dictionary<string, int> Distances { get; set; } = new();

        public Valve(string line)
        {
            Name = line[6..8];
            FlowRate = line.Int32s().First();
            var tunnel = line.Split(";")[1];
            Neighbors = tunnel[tunnel.IndexOf(tunnel.First(char.IsUpper))..].Split(",", StringSplitOptions.TrimEntries);
        }

        public Valve SetDistances(Dictionary<(string, string), int> distances)
        {
            Distances = distances.Where(d => d.Key.Item1 == Name && d.Key.Item2 != Name && d.Value > 0).ToDictionary(d => d.Key.Item2, d => d.Value);
            return this;
        }
    }

    private class State
    {
        public string Name { get; set; }
        public int TimeLeft { get; set; }
        public BigInteger Visited { get; set; }
        public int FlowRate { get; set; }
        public int Pressure { get; set; }

        public State(string name, int timeLeft)
        {
            Name = name;
            TimeLeft = timeLeft;
        }

        public State(string name, int timeLeft, int flowRate, int pressure, BigInteger visited)
        {
            Name = name;
            TimeLeft = timeLeft;
            FlowRate = flowRate;
            Pressure = pressure;
            Visited = visited;
        }

        public static State Empty => new("", 0, 0, 0, 0);
    }
}
