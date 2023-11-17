namespace AdventOfCode._2022;

public partial class Day19 : Day<int, int>
{
    public override string Example => @"Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.";

    [Test(ExpectedResult = 33)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 1719)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 3472)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 19530)]
    public override int Two() => Two(Input);

    private static int One(string input)
        => input.Lines(print => new Blueprint(print))
            .Select((b, index) => TryBuild(b, new(24, default, new(Ore: 1)), new()) * (index + 1))
            .Sum();

    private static int Two(string input)
        => input.Lines(print => new Blueprint(print))
            .Take(3)
            .Select((b, index) => TryBuild(b, new(32, default, new(Ore: 1)), new()))
            .Aggregate(1, (a, b) => a * b);

    private static int TryBuild(Blueprint print, State state, Dictionary<State, int> done)
    {
        if (state.Time == 0 || (state.Produces.Obsidian == 0 && print.Geode.Cost.Obsidian > state.Time * 2))
        {
            return state.Current.Geode;
        }

        if (done.TryGetValue(state, out int geode))
        {
            return geode;
        }
        else
        {
            var mostGeodes = state.PossibleNewStates(print).Select(s => TryBuild(print, s.Next(state.Produces), done)).Max();
            done[state] = mostGeodes;
            return mostGeodes;
        }
    }

    record struct Resource(int Ore = 0, int Clay = 0, int Obsidian = 0, int Geode = 0)
    {
        public bool CanBuild(Robot robot) => Ore >= robot.Cost.Ore && Clay >= robot.Cost.Clay && Obsidian >= robot.Cost.Obsidian && Geode >= robot.Cost.Geode;

        public static Resource operator +(Resource l, Resource r) => new(l.Ore + r.Ore, l.Clay + r.Clay, l.Obsidian + r.Obsidian, l.Geode + r.Geode);
        public static Resource operator -(Resource l, Resource r) => new(l.Ore - r.Ore, l.Clay - r.Clay, l.Obsidian - r.Obsidian, l.Geode - r.Geode);
    }

    record State(int Time, Resource Current, Resource Produces)
    {
        public State Next(Resource produces) => this with { Time = Time - 1, Current = Current + produces };
        public State Build(Robot robot) => this with { Current = Current - robot.Cost, Produces = Produces + robot.Produces };
        private Resource Prev => Current - Produces;

        public IEnumerable<State> PossibleNewStates(Blueprint print)
        {
            if (Current.CanBuild(print.Geode)) { yield return Build(print.Geode); }
            else
            {
                //foreach (var rb in print.BuildingRobots.Where(rb => /*!Prev.CanBuild(rb.Key) &&*/ Current.CanBuild(rb.Key) && (rb.Value != "Ore" || NotEnoughOre(print))))
                //{
                //    yield return Build(rb.Key);
                //}

                //yield return this;

                if (Current.CanBuild(print.Obsidian))
                {
                    yield return Build(print.Obsidian);
                }

                if (!Prev.CanBuild(print.Clay) && Current.CanBuild(print.Clay))
                {
                    yield return Build(print.Clay);
                }

                if (!Prev.CanBuild(print.Ore) && Current.CanBuild(print.Ore) && NotEnoughOre(print))
                {
                    yield return Build(print.Ore);
                }

                yield return this;
            }
        }

        bool NotEnoughOre(Blueprint print) => Produces.Ore < print.Geode.Cost.Ore || Produces.Ore < print.Obsidian.Cost.Ore || Produces.Ore < print.Clay.Cost.Ore;
        //bool NotEnoughOre(Blueprint print) => true;
    }

    class Blueprint
    {
        public int Id { get; set; }
        public Robot Ore { get; set; }
        public Robot Clay { get; set; }
        public Robot Obsidian { get; set; }
        public Robot Geode { get; set; }

        public Dictionary<Robot, string> BuildingRobots => new() { { Ore, nameof(Ore) }, { Clay, nameof(Clay) }, { Obsidian, nameof(Obsidian) } };

        public Blueprint(string input)
        {
            var lines = input.Int32s().ToArray();

            Id = lines[0];
            Ore = new(new(Ore: lines[1]), new(Ore: 1));
            Clay = new(new(Ore: lines[2]), new(Clay: 1));
            Obsidian = new(new(Ore: lines[3], Clay: lines[4]), new(Obsidian: 1));
            Geode = new(new(Ore: lines[5], Obsidian: lines[6]), new(Geode: 1));
        }
    }

    record Robot(Resource Cost, Resource Produces);
}
