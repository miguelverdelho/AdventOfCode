using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace AdventOfCode._2025;

public class Day8
{
    static string projectRootPath = Directory.GetCurrentDirectory();
    static string _inputFilePath = Path.Combine(projectRootPath, "2025", "inputs", "day8.txt");

    public class Box
    {
        public int Id;
        public int X;
        public int Y;
        public int Z;
    }

    public class Connection
    {
        public HashSet<int> Boxes = new();
    }

    public List<Box> boxes = new();
    public List<Connection> connections = new();

    public void RunPart1()
    {
        var lines = File.ReadAllLines(_inputFilePath);
        int id = 1;
        foreach (var line in lines)
        {
            var coords = line.Split(',').Select(x => int.Parse(x)).ToArray();
            boxes.Add(new Box { Id = id, X = coords[0], Y = coords[1], Z = coords[2] });
            id++;
        }

        var pairs = new List<(int Box1, int Box2, double Distance)>();
        for (int i = 0; i < boxes.Count; i++)
        {
            for (int j = i + 1; j < boxes.Count; j++)
            {
                var b1 = boxes[i];
                var b2 = boxes[j];
                double dist = Math.Sqrt(
                    Math.Pow(b1.X - b2.X, 2) +
                    Math.Pow(b1.Y - b2.Y, 2) +
                    Math.Pow(b1.Z - b2.Z, 2)
                );
                pairs.Add((b1.Id, b2.Id, dist));
            }
        }

        pairs.Sort((a, b) => a.Distance.CompareTo(b.Distance));

        var parent = new Dictionary<int, int>();
        var size = new Dictionary<int, int>();
        foreach (var box in boxes)
        {
            parent[box.Id] = box.Id;
            size[box.Id] = 1;
        }

        int Find(int x)
        {
            if (parent[x] != x)
                parent[x] = Find(parent[x]);
            return parent[x];
        }

        void Union(int x, int y)
        {
            int px = Find(x);
            int py = Find(y);

            // print connection being made
            //Console.WriteLine("Connecting Box " + x + " and Box " + y + " | Root1: " + px + " Root2: " + py);

            if (px == py) return;
            if (size[px] < size[py])
            {
                parent[px] = py;
                size[py] += size[px];
            }
            else
            {
                parent[py] = px;
                size[px] += size[py];
            }

            if(size[px] == parent.Count || size[py] == parent.Count)
            {
                CalculatePart2(x, y);
            }
        }

        int attempts = 0;
        int maxConnections = 1000; 

        foreach (var (box1, box2, dist) in pairs)
        {
            //if (attempts >= maxConnections)
            //    break;

            if (Find(box1) != Find(box2))
            {
                Union(box1, box2);
            }
            attempts++;
        }

        var groupSizes = new Dictionary<int, int>();
        foreach (var box in boxes)
        {
            int root = Find(box.Id);
            if (!groupSizes.ContainsKey(root))
                groupSizes[root] = 0;
            groupSizes[root]++;
        }

        var largest = groupSizes.Values.OrderByDescending(x => x).Take(3).ToList();
        int result = largest.Aggregate(1, (acc, val) => acc * val);

        //Console.WriteLine($"Part 1: {result}");
    }

    public void RunPart2()
    {

    }

    public void CalculatePart2(int box1Id, int box2Id)
    {
        ulong result = 0;

        var box1 = boxes.First(b => b.Id == box1Id);
        var box2 = boxes.First(b => b.Id == box2Id);

        //Console.WriteLine(box2.X);
        //Console.WriteLine(box2.Y);

        result = (ulong)(box1.X * box2.X);
        Console.WriteLine("Part 2: " + result);
    }


}
