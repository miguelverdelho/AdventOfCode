namespace AdventOfCode._2025;

public class Day9
{
    static string projectRootPath = Directory.GetCurrentDirectory();
    static string _inputFilePath = Path.Combine(projectRootPath, "2025","inputs", "day9.txt");

    List<(ulong x, ulong y)> _tiles = new();

    public void RunPart1()
    {
         var lines = File.ReadAllLines(_inputFilePath);
         foreach(var line in lines)
         {
            var coords = line.Split(',');
            _tiles.Add(new(ulong.Parse(coords[0]), ulong.Parse(coords[1])));
         }

         ulong maxArea = 0;

        foreach (var tile in _tiles)
        {
            foreach (var otherTile in _tiles)
            {
                if (otherTile != tile)
                {
                    ulong localarea = (ulong)(Math.Abs((long)(tile.x - otherTile.x)) + 1) * (ulong)(Math.Abs((long)(tile.y - otherTile.y)) + 1);
                    if(localarea > maxArea)
                        maxArea = localarea;
                }
            }
        }

        Console.WriteLine($"Max Area: {maxArea}");
    }

    public void RunPart2()
    {

    }
}
