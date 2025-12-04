namespace AdventOfCode._2025;

public class Day4
{
    static string projectRootPath = Directory.GetCurrentDirectory();
    static string _inputFilePath = Path.Combine(projectRootPath, "2025", "inputs", "day4.txt");

    private readonly char ROLL = '@';

    public void RunPart1()
    {
        //  read file into matrix
        var lines = File.ReadAllLines(_inputFilePath);
        var size = lines.Length;
        var map = new char[size][];

        for (int i = 0; i < size; i++)
        {
            map[i] = new char[size];
            for (int j = 0; j < size; j++)
            {
                map[i][j] = lines[i][j];
            }
        }

        int validRolls = CountRemovableRolls(map);

        Console.WriteLine(validRolls);
    }

    public int CountAdjacentRolls(char[][] map, int x, int y)
    {
        int count = 0;

        // check all 8 directions
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;
                if (GetPositionValue(map, x + dx, y + dy) == ROLL)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public char GetPositionValue(char[][] map, int x, int y)
    {
        if (x >= 0 && x < map.Length && y >= 0 && y < map[x].Length)
        {
            return map[x][y];
        }
        return '-';
    }

    public void RunPart2()
    {
        var lines = File.ReadAllLines(_inputFilePath);
        var size = lines.Length;
        var map = new char[size][];

        for (int i = 0; i < size; i++)
        {
            map[i] = new char[size];
            for (int j = 0; j < size; j++)
            {
                map[i][j] = lines[i][j];
            }
        }

        int removedRolls = 0;
        int validRolls = CountRemovableRolls(map);

        while (validRolls != 0)
        {
            removedRolls += RemoveRolls(map);
            validRolls = CountRemovableRolls(map);
        }

        Console.WriteLine(removedRolls);
    }

    private int CountRemovableRolls(char[][] map)   
    {
        int count = 0;
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == ROLL && CountAdjacentRolls(map, i, j) < 4)
                {
                    count++;
                }
            }
        }
        return count;
    }

    private int RemoveRolls(char[][] map)
    {
        int removedCount = 0;
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == ROLL && CountAdjacentRolls(map, i, j) < 4)
                {
                    map[i][j] = '.';
                    removedCount++;
                }
            }
        }
        return removedCount;
    }

}
