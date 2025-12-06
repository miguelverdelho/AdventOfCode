using System.Security.Cryptography;
using System.Text.RegularExpressions;
using static AdventOfCode.Services.Day7;

namespace AdventOfCode._2025;

public class Day6
{
    static string projectRootPath = Directory.GetCurrentDirectory();
    static string _inputFilePath = Path.Combine(projectRootPath, "2025","inputs", "day6.txt");
    public void RunPart1()
    {
        var lines = File.ReadAllLines(_inputFilePath);

        var splitLines = new string[lines.Length][];
        var i = 0;

        foreach (var line in lines)
        {
            splitLines[i] = Regex.Replace(line, @"\s+", " ").Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            i++;
        }

        ulong result = 0;

        for(int column = 0; column < splitLines[0].Length; column++)
        {
            var operation = splitLines[splitLines.Length-1][column];

            if (operation == "+")
            {
                for (int row = 0; row < splitLines.Length - 1; row++)
                {
                    result += ulong.Parse(splitLines[row][column]);
                }
            }
            else if (operation == "*")
            {
                ulong tempResult = 1;
                for (int row = 0; row < splitLines.Length - 1; row++)
                {
                    tempResult *= ulong.Parse(splitLines[row][column]);
                }
                result += tempResult;
            }
        }

        Console.WriteLine(result);

    }

    public void RunPart2()
    {
        var lines = File.ReadAllLines(_inputFilePath);

        // find all indexes where all lines are space - those separate different sections

        List<int> sectionIndexes = new List<int>();

        for (int i = 0; i < lines[0].Length; i++) 
        {
            bool spaceColumn = lines.All(lines => lines[i] == ' ');
            if(spaceColumn) 
                sectionIndexes.Add(i);
        }
        sectionIndexes.Add(lines[0].Length);

        // remove last line to get the operations
        var signs = Regex.Replace(lines[lines.Length - 1], @"\s+", " ").Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        //parse each section into a 2D array and put zeros where spaces are
        var sections = new List<char[][]>();
        int previousIndex = 0;
        foreach (var index in sectionIndexes)
        {
            char[][] section = new char[lines.Length - 1][];
            var size = index - previousIndex;
            for (int l = 0; l < lines.Length - 1; l++)            
                section[l] = new char[size];            
            
            for(int i = 0; i < lines.Length - 1; i++)
            {
                for (int l = previousIndex; l < index; l++)
                {
                    section[i][l- previousIndex] = lines[i][l] == ' '? ' ' : lines[i][l];
                }
            }

            sections.Add(section);
            previousIndex = index+1;
        }

        // parse 2D arrays into vertical numbers
        ulong result = 0;
        int sectionIndex = 0;
        foreach (var section in sections)
        {
            List<ulong> numbers = new List<ulong>();
            for (int col = 0; col < section[0].Length; col++)
            {
                string numberStr = "";
                for (int row = 0; row < section.Length; row++)
                {
                    numberStr += section[row][col];
                }
                numbers.Add(ulong.Parse(numberStr));
            }

            if(signs[sectionIndex] == "+")
            {
                ulong sum = 0;
                foreach (var number in numbers)
                {
                    sum += number;
                }
                result += sum;
            }
            else if (signs[sectionIndex] == "*")
            {
                ulong product = 1;
                foreach (var number in numbers)
                {
                    product *= number;
                }
                result += product;
            }
            sectionIndex++;
        }

        Console.WriteLine(result);
    }
}
