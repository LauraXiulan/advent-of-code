using System.Net.Sockets;
using System.Numerics;

namespace AdventOfCode._2022;

public class Day18 : Day<int, int>
{
    public override string Example => "2,2,2;1,2,2;3,2,2;2,1,2;2,3,2;2,2,1;2,2,3;2,2,4;2,2,6;1,2,5;3,2,5;2,1,5;2,3,5";

    [Test(ExpectedResult = 64)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 4364)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 58)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 2508)]
    public override int Two() => Two(Input);

    private static int One(string input) => SumOfCubes(Grid3D.Parse(input), 0);

    private static int Two(string input) => SumOfCubes(Grid3D.Parse(input).Vents(), 2);

    private static int SumOfCubes(Grid3D grid, int pocket)
    {
        var sum = 0;
        for (int x = 0; x <= grid.Width; x++)
        {
            for (int y = 0; y <= grid.Height; y++)
            {
                for (int z = 0; z <= grid.Depth; z++)
                {
                    if (grid.Grid[x, y, z] == 1)
                    {
                        if (x == 0)
                        {
                            sum++;
                            if (grid.Grid[x + 1, y, z] == pocket) sum++;
                        }
                        else if (x == grid.Width)
                        {
                            sum++;
                            if (grid.Grid[x - 1, y, z] == pocket) sum++;
                        }
                        else
                        {
                            if (grid.Grid[x + 1, y, z] == pocket) sum++;
                            if (grid.Grid[x - 1, y, z] == pocket) sum++;
                        }
                    }
                }
            }
        }

        for (int y = 0; y <= grid.Height; y++)
        {
            for (int x = 0; x <= grid.Width; x++)
            {
                for (int z = 0; z <= grid.Depth; z++)
                {
                    if (grid.Grid[x, y, z] == 1)
                    {
                        if (y == 0)
                        {
                            sum++;
                            if (grid.Grid[x, y + 1, z] == pocket) sum++;
                        }
                        else if (y == grid.Height)
                        {
                            sum++;
                            if (grid.Grid[x, y - 1, z] == pocket) sum++;
                        }
                        else
                        {
                            if (grid.Grid[x, y + 1, z] == pocket) sum++;
                            if (grid.Grid[x, y - 1, z] == pocket) sum++;
                        }
                    }
                }
            }
        }

        for (int z = 0; z <= grid.Depth; z++)
        {
            for (int y = 0; y <= grid.Height; y++)
            {
                for (int x = 0; x <= grid.Width; x++)
                {
                    if (grid.Grid[x, y, z] == 1)
                    {
                        if (z == 0)
                        {
                            sum++;
                            if (grid.Grid[x, y, z + 1] == pocket) sum++;
                        }
                        else if (z == grid.Depth)
                        {
                            sum++;
                            if (grid.Grid[x, y, z - 1] == pocket) sum++;
                        }
                        else
                        {
                            if (grid.Grid[x, y, z + 1] == pocket) sum++;
                            if (grid.Grid[x, y, z - 1] == pocket) sum++;
                        }
                    }
                }
            }
        }

        return sum;
    }

    record Grid3D(int[,,] Grid, int Width, int Height, int Depth)
    {
        public static Grid3D Parse(string input)
        {
            var lines = input.Lines();
            var cubes = new HashSet<Vector3>();
            foreach (var line in lines)
            {
                cubes.Add(Cube(line));
            }

            var maxX = (int)cubes.Max(c => c.X);
            var maxY = (int)cubes.Max(c => c.Y);
            var maxZ = (int)cubes.Max(c => c.Z);
            var vectorGrid = new int[maxX + 1, maxY + 1, maxZ + 1];
            foreach (var cube in cubes)
            {
                vectorGrid[(int)cube.X, (int)cube.Y, (int)cube.Z] = 1;
            }

            return new(vectorGrid, maxX, maxY, maxZ);
        }

        public static Vector3 Cube(string input)
        {
            var ints = input.Int32s().ToArray();
            return new(ints[0], ints[1], ints[2]);
        }

        public Grid3D Vents()
        {
            for (int x = 0; x <= Width; x += Width)
            {
                for (int y = 0; y <= Height; y++)
                {
                    for (int z = 0; z <= Depth; z++)
                    {
                        if (Grid[x, y, z] == 0)
                        {
                            Grid[x, y, z] = 2;
                            var addition = x == Width ? -1 : 1;
                            var nextPoint = Grid[x + 1, y, z];
                            if (nextPoint == 0)
                            {
                                NeighboringVents(x + addition, y, z);
                            }

                        }
                    }
                }
            }

            /* // Y and Z loop not needed for this input
            for (int y = 0; y <= Height; y += Height)
            {
                for (int x = 0; x <= Width; x++)
                {
                    for (int z = 0; z <= Depth; z++)
                    {
                        TestContext.Progress.WriteLine($"Checking {x}, {y}, {z} in y loop");
                        if (Grid[x, y, z] == 0)
                        {
                            Grid[x, y, z] = 2;
                            var addition = y == Height ? -1 : 1;
                            var nextPoint = Grid[x, y + 1, z];
                            if (nextPoint == 0)
                            {
                                CheckNeighboringPockets(x, y + addition, z);
                            }
                        }
                    }
                }
            }

            for (int z = 0; z <= Depth; z += Depth)
            {
                for (int y = 0; y <= Height; y++)
                {
                    for (int x = 0; x <= Width; x++)
                    {
                        TestContext.Progress.WriteLine($"Checking {x}, {y}, {z} in z loop");
                        if (Grid[x, y, z] == 0)
                        {
                            Grid[x, y, z] = 2;
                            var addition = z == Depth ? -1 : 1;
                            var nextPoint = Grid[x, y, z + 1];
                            if (nextPoint == 0)
                            {
                                CheckNeighboringPockets(x, y, z + addition);
                            }

                        }
                    }
                }
            }*/

            return this;
        }

        private void NeighboringVents(int x, int y, int z)
        {
            var queue = new Queue<Vector3>();
            queue.Enqueue(new(x, y, z));

            while (queue.Any())
            {
                var total = queue.Count;
                var current = queue.Dequeue();

                for (int i = 0; i < total; i++)
                {
                    if (Grid[(int)current.X, (int)current.Y, (int)current.Z] == 2) continue;
                    Grid[(int)current.X, (int)current.Y, (int)current.Z] = 2;

                    foreach (var n in Neighbors(current))
                    {
                        queue.Enqueue(n);
                    }
                }
            }
        }

        public HashSet<Vector3> Neighbors(Vector3 point)
        {
            var neighbors = new HashSet<Vector3>
            {
                new Vector3(point.X + 1, point.Y, point.Z),
                new Vector3(point.X - 1, point.Y, point.Z),
                new Vector3(point.X, point.Y + 1, point.Z),
                new Vector3(point.X, point.Y - 1, point.Z),
                new Vector3(point.X, point.Y, point.Z + 1),
                new Vector3(point.X, point.Y, point.Z - 1)
            };

            bool OutOfBounds(Vector3 point)
                => point.X < 0 || point.Y < 0 || point.Z < 0
                    || point.X > Width || point.Y > Height || point.Z > Depth;

            foreach (var n in neighbors)
            {
                if (OutOfBounds(n) || Grid[(int)n.X, (int)n.Y, (int)n.Z] != 0)
                {
                    neighbors.Remove(n);
                }
            }

            return neighbors;
        }
    }
}
