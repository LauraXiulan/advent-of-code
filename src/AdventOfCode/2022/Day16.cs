using Newtonsoft.Json.Linq;
using NUnit.Framework.Internal;

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

    [Test(ExpectedResult = 1)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 1)]
    public override int Two() => Two(Input);

    private static int One(string input)
    {
        var valves = input.Split("\r\n").Select(l => new Valve(l));
        var importantConnections = valves.Where(i => i.FlowRate > 0 || i.Name == "AA").Select(r => r.Name);
        valves = valves.Select(v => Distances(v, valves));
        //var distances = Distances(valves, importantConnections);

        //valves = valves.Select(v => v.Connect(valves, distances));

        var bestFlow = Test(valves);
        return bestFlow;
    }

    private static int Test(IEnumerable<Valve> valves)
    {
        var nonZeroValves = valves.First(v => v.Name == "AA").Distances.Select(d => d.Key).ToArray();
        var evaluate = new Dictionary<int, List<State>> { { 0, new List<State> { new State("AA", 30) } } };
        var paths = new Dictionary<int, int>();

        for (int i = 0; i < nonZeroValves.Length; i++)
        {
            evaluate[i + 1] = new List<State>();
            foreach (var state in evaluate[i])
            {
                foreach (var n in valves.First(v => v.Name == state.Name).Distances)
                {
                    var dest = valves.First(v => v.Name == n.Key);
                    if (state.TimeLeft + n.Value <= 0 || state.Visited.Contains(n.Key)) continue;
                    state.Visited.Add(n.Key);
                    var newState = new State(n.Key, state.TimeLeft - n.Value, state.FlowRate + dest.FlowRate, state.Pressure + state.FlowRate * n.Value, state.Visited);
                    evaluate[i + 1].Add(newState);
                    var pressure = newState.Pressure + newState.FlowRate * newState.TimeLeft;
                    var key = nonZeroValves.Select((v, i) => newState.Visited.Contains(v) ? (int)Math.Pow(2, i) : 0).Sum();

                    if (paths.TryGetValue(key, out int compare) && compare > pressure) continue;
                    paths[key] = pressure;
                }
            }
        }

        return paths.Max(x => x.Value);
    }

    private static Valve Distances(Valve valve, IEnumerable<Valve> valves)
    {
        var neighbors = valve.Neighbors.Select(n => n);
        var done = new Dictionary<string, int> { { valve.Name, 0 } };
        var steps = 0;

        while (neighbors.Any())
        {
            steps++;
            var newNeighbors = new HashSet<string>();
            foreach (var n in neighbors)
            {
                done[n] = steps;
                foreach (var nested in valves.First(v => v.Name == n).Neighbors)
                {
                    if (done.ContainsKey(nested) || newNeighbors.Contains(nested)) continue;
                    newNeighbors.Add(nested);
                }
            }
            neighbors = newNeighbors;
        }
        valve.Distances = done.Where(d => valves.First(v => v.Name == d.Key).FlowRate > 0 && d.Key != valve.Name).ToDictionary(k => k.Key, k => k.Value + 1);
        return valve;
    }

    //private static Dictionary<(string, string), int> Distances(IEnumerable<Valve> valves, IEnumerable<string> importantConnections)
    //{
    //    var distances = new Dictionary<(string, string), int>();
    //    foreach (var valve in valves)
    //    {
    //        if (!importantConnections.Any(r => r == valve.Name)) continue;

    //        var current = new[] { valve.Name };
    //        var next = Array.Empty<string>();
    //        var distance = 0;

    //        distances[(valve.Name, valve.Name)] = 0;
    //        while (current.Any())
    //        {
    //            distance++;
    //            foreach (var position in current)
    //            {
    //                foreach (var newPosition in valves.First(i => i.Name == position).Neighbors)
    //                {
    //                    if (!distances.ContainsKey((valve.Name, newPosition)))
    //                    {
    //                        distances[(valve.Name, newPosition)] = distance;
    //                        next = next.Append(newPosition).ToArray();
    //                    }
    //                }
    //            }
    //            current = next;
    //            next = Array.Empty<string>();
    //        }
    //    }
    //    return distances;
    //}

    class Valve
    {
        public string Name { get; set; }
        public int FlowRate { get; set; }
        public string[] Neighbors { get; set; }
        public Dictionary<string, int> Distances { get; set; } = new();

        public Valve(string line)
        {
            Name = line[6..8];
            FlowRate = line.Int32s().First();
            var tunnel = line.Split(";")[1];
            Neighbors = tunnel[tunnel.IndexOf(tunnel.First(char.IsUpper))..].Split(",", StringSplitOptions.TrimEntries);
        }

        public Valve Connect(IEnumerable<Valve> valves, Dictionary<(string, string), int> distances)
        {
            Distances = distances.Where(d => d.Key.Item1 == Name && d.Key.Item2 != Name && d.Value > 0).ToDictionary(d => d.Key.Item2, d => d.Value);
            return this;
        }
    }

    class State
    {
        public string Name { get; set; }
        public int TimeLeft { get; set; }
        public HashSet<string> Visited { get; set; } = new();
        public int FlowRate { get; set; }
        public int Pressure { get; set; }

        public State(string name, int timeLeft)
        {
            Name = name;
            TimeLeft = timeLeft;
        }

        public State(string name, int timeLeft, int flowRate, int pressure, HashSet<string> visited)
        {
            Name = name;
            TimeLeft = timeLeft;
            FlowRate = flowRate;
            Pressure = pressure;
            Visited = visited;
        }
    }

    private static int Two(string input)
    {
        return 0;
    }
}
