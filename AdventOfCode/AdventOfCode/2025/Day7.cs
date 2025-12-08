using System.Data;
using System.Linq;

namespace AdventOfCode._2025;

public class Day7
{
    static string projectRootPath = Directory.GetCurrentDirectory();
    static string _inputFilePath = Path.Combine(projectRootPath, "2025","inputs", "day7.txt");

    private const char START = 'S';
    private const char BEAM = '|';
    private const char SPLITTER = '^';
    private const char EMPTY = '.';

    public void RunPart1()
    {
        var lines = File.ReadAllLines(_inputFilePath);

        var map = new char[lines.Length][];
        
        for(int i = 0; i < lines.Length; i++)
        {
            map[i] = lines[i].ToCharArray();
        }

        var splits = 0;

        for(int i = 1; i < map.Length; i++)
        {
            for(int j = 0; j < map[i].Length; j++)
            {
                char currentChar = map[i][j];
                char aboveChar = map[i - 1][j];

                // check what character is above
                //if S put a beam below
                if (aboveChar == START)
                {
                    map[i][j] = BEAM;
                    continue;
                }
                // if beam 
                else if(aboveChar == BEAM) 
                {
                    // if current is empty put beam
                    if(currentChar == EMPTY)
                    {
                        map[i][j] = BEAM;
                        continue;
                    }
                    else if(currentChar == SPLITTER && aboveChar == BEAM)
                    {
                        var splitted = false;
                        // put beam below and to the left and right                        
                        if(j > 0 && map[i][j - 1] == EMPTY)
                        {
                            map[i][j - 1] = BEAM;
                            splitted = true;
                        }
                        if(j < map[i].Length - 1 && map[i][j + 1] == EMPTY)
                        {
                            map[i][j + 1] = BEAM;
                            splitted |= true;
                        }
                        if(splitted)
                        {
                            splits++;
                        }
                        continue;
                    }
                }
            }
        }

        Console.WriteLine($"Total splits: {splits}");
    }

    public void RunPart2()
    {
        var lines = File.ReadAllLines(_inputFilePath);

        var map = new char[lines.Length][];
        ulong[][] counterTimelines = new ulong[map.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            map[i] = lines[i].ToCharArray();
            counterTimelines[i] = new ulong[map[i].Length];
            Array.Fill(counterTimelines[i], (ulong)0);
        }

        for (int i = 1; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                char currentChar = map[i][j];
                char aboveChar = map[i - 1][j];

                // check what character is above
                //if S put a beam below
                if (aboveChar == START || (aboveChar == BEAM && currentChar == EMPTY))
                {
                    map[i][j] = BEAM;
                    counterTimelines[i][j] = aboveChar == START ? 1 : counterTimelines[i - 1][j];
                    continue;
                }
                // if beam 
                else if (aboveChar == BEAM)
                {
                    if (currentChar == SPLITTER)
                    {
                        // put beam below and to the left and right                        
                        if (j > 0)
                        {
                            map[i][j - 1] = BEAM;
                            if(counterTimelines[i][j - 1] == 0)
                                counterTimelines[i][j - 1] += counterTimelines[i-1][j-1];
                        }
                        if (j < map[i].Length - 1)
                        {
                            map[i][j + 1] = BEAM;
                            if (counterTimelines[i][j + 1] == 0)
                                counterTimelines[i][j + 1] += counterTimelines[i-1][j+1];
                        }
                        UpdateCounter(map,ref counterTimelines, i, j);
                        continue;
                    }
                }
            }
        }

        PrintCounter(counterTimelines,map);
        PrintMap(map);
    }

    private void UpdateCounter(char [][] map,ref ulong[][] counter, int row, int col)
    {
        counter[row][col - 1] += counter[row - 1][col];
        counter[row][col + 1] += counter[row - 1][col];

    }

    private void PrintMap(char[][] map)
    {
        for(int i = 0; i < map.Length; i++)
        {
            Console.WriteLine(string.Join(" ", map[i]));
        }
    }
    private void PrintCounter(ulong[][] map, char[][] mmap)
    {
        for (int i = 0; i < map.Length; i++)
        {
            var row = new string[map[i].Length];
            for (int j = 0; j < map[i].Length; j++)
            {
                if (mmap[i][j] == SPLITTER)
                    row[j] = "^";
                else if (map[i][j] == 0)
                    row[j] = " ";
                else
                    row[j] = map[i][j].ToString();
            }
            var rowUpdated = row.Select(val => val == "0" ? " " : val.ToString())
                .ToArray();
            Console.WriteLine(string.Join(" ", rowUpdated));
        }

        // add all values in last row
        ulong total = map[map.Length - 1].Aggregate(0UL, (acc, val) => acc + val); 
        Console.WriteLine("TimeLines: " + total);
    }

    private int PerformTimeLine(char[][] map, int row)
    {
        if (row >= map.Length)
        {
            PrintMap(map);
            return 1;
        }

        int totalPaths = 0;


        for (int i = row; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                char currentChar = map[i][j];
                char aboveChar = map[i - 1][j];

                // check what character is above
                if (aboveChar == START || (aboveChar == BEAM && currentChar == EMPTY))
                {
                    var newMap = CloneMap(map);
                    newMap[i][j] = BEAM;
                    totalPaths += PerformTimeLine(newMap, i + 1);
                }
                else if (aboveChar == BEAM &&currentChar == SPLITTER )
                {
                    // put beam below and to the left and right                        
                    if (j > 0)
                    {
                        var leftMap = CloneMap(map);
                        leftMap[i][j - 1] = BEAM;
                        //PrintMap(map);
                        totalPaths += PerformTimeLine (leftMap, i+1);
                        map[i][j - 1] = EMPTY;
                    }
                    if (j < map[i].Length - 1)
                    {
                        var rightMap = CloneMap(map);
                        rightMap[i][j + 1] = BEAM;
                        //PrintMap(map);
                        totalPaths += PerformTimeLine(rightMap, i+1);
                    }                    
                }
            }
        }

        return totalPaths;
    }

    private char[][] CloneMap(char[][] map)
    {
        var clone = new char[map.Length][];
        for (int i = 0; i < map.Length; i++)
            clone[i] = (char[])map[i].Clone();
        return clone;
    }

    private void CountDivisions(char[][] map)
    {

        for (int i = 1; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                char currentChar = map[i][j];
                char aboveChar = map[i - 1][j];

                // check what character is above
                //if S put a beam below
                if (aboveChar == START)
                {
                    map[i][j] = BEAM;
                    continue;
                }
                // if beam 
                else if (aboveChar == BEAM)
                {
                    // if current is empty put beam
                    if (currentChar == EMPTY)
                    {
                        map[i][j] = BEAM;
                        continue;
                    }
                    else if (currentChar == SPLITTER && aboveChar == BEAM)
                    {
                        // put beam below and to the left and right                        
                        if (j > 0 && map[i][j - 1] == EMPTY)
                        {
                            map[i][j - 1] = BEAM;
                        }
                        if (j < map[i].Length - 1 && map[i][j + 1] == EMPTY)
                        {
                            map[i][j + 1] = BEAM;
                        }
                        continue;
                    }
                }
            }
        }

        int useless = 0;
        int usefull = 0;

        for (int i = 0; i < map.Length; i++)
        {
            for(int j = 0; j < map[i].Length; j++)
            {
                if(map[i][j] == SPLITTER)
                {

                    if (map[i-1][j] == EMPTY)
                    {
                        useless++;
                    }
                    else
                        usefull++;
                }
            }
        }

        Console.WriteLine("Useless splitters : " + useless);
        Console.WriteLine("Usefull splitters : " + usefull);

        PrintMap(map);
        return;
    }
}
