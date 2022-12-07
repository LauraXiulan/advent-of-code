using System.Diagnostics;

namespace AdventOfCode._2022;

public class Day07 : Day<int, int>
{
    public override string Example => @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";

    [Test(ExpectedResult = 95437)]
    public int One_Example() => One(Example);

    [Test(ExpectedResult = 1307902)]
    public override int One() => One(Input);

    [Test(ExpectedResult = 24933642)]
    public int Two_Example() => Two(Example);

    [Test(ExpectedResult = 7068748)]
    public override int Two() => Two(Input);

    private static int One(string input)
        => Parse(input).All().Where(ch => ch.Size <= 100000).Sum(s => s.Size);

    private static int Two(string input)
    {
        var root = Parse(input);
        var required = root.Size - (70000000 - 30000000);

        return root.All().OrderBy(s => s.Size).First(s => s.Size >= required).Size;
    }

    private static Directory Parse(string input)
    {
        var root = Directory.Root();
        var current = root;
        foreach (var line in input.Lines())
        {
            switch (line)
            {
                case "$ ls":
                    break;
                case "$ cd /":
                    current = root;
                    break;
                case "$ cd ..":
                    current = current.Parent;
                    break;
                case string s when s.StartsWith("$ cd"):
                    current = current.Children.First(ch => ch.Name == line[5..]);
                    break;
                case string s when s.StartsWith("dir"):
                    current.Add(new Directory(line[4..], current));
                    break;
                default:
                    current.Add(new File(line));
                    break;
            }
        }

        return root;
    }

    [DebuggerDisplay("{Name}: {Size}")]
    public class Directory
    {
        public Directory(string name, Directory parent)
        {
            Name = name;
            Parent = parent;
        }

        public string Name { get; }
        public Directory Parent { get; }
        public IList<Directory> Children { get; } = new List<Directory>();
        public IList<File> Files { get; } = new List<File>();

        public static Directory Root() => new("/", null!);

        public void Add(Directory directory) => Children.Add(directory);

        public void Add(File file) => Files.Add(file);

        public IEnumerable<Directory> All() => Children.SelectMany(c => c.All()).Concat(new[] { this });

        public int Size => All().SelectMany(c => c.Files).Sum(s => s.Size);
    }

    [DebuggerDisplay("{Name}: {Size}")]
    public class File
    {
        public File(string input)
        {
            var split = input.Split(" ").ToArray();
            Size = int.Parse(split[0]);
            Name = split[1];
        }

        public string Name { get; }
        public int Size { get; }
    }
}
