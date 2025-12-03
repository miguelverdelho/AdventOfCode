using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode._2025;

public class Day3
{
    static string projectRootPath = Directory.GetCurrentDirectory();
    static string _inputFilePath = Path.Combine(projectRootPath, "2025","inputs", "day3.txt");
    public void RunPart1()
    {
        // read line as array of ints
        var lines = File.ReadAllLines(_inputFilePath);
        var totalJoltage = 0;
        foreach (var line in lines)
        {
            //parse to int array
            int[] bank = line.Select(i => int.Parse(i.ToString())).ToArray();
                        
            int max = bank.Max();
            int maxIndex = Array.IndexOf(bank, max);

            if(maxIndex != bank.Length - 1)
            {
                // check sub array from index to end
                int[] subArray = bank[(maxIndex + 1)..];
                int subMax = subArray.Max();

                var joltage = max*10 + subMax;
                totalJoltage += joltage;
            }
            else if (maxIndex == bank.Length - 1)
            {
                // check sub array from start to maxIndex
                int[] subArray = bank[0..(maxIndex)];
                int subMax = subArray.Max();
                var joltage = subMax*10 + max;

                totalJoltage += joltage;
            }
            
        }

        Console.WriteLine(totalJoltage);
    }
    //17155

    public void RunPart2()
    {
        // read line as array of ints
        var lines = File.ReadAllLines(_inputFilePath);
        long totalJoltage = 0;
        foreach (var line in lines)
        {
            //parse to int array
            int[] bank = line.Select(i => int.Parse(i.ToString())).ToArray();

            var joltage = GetBiggestNumberFromArrayWithSize(bank, 12);
            totalJoltage += long.Parse(joltage);
        }
        Console.WriteLine(totalJoltage);
    }
    //169685670469164

    public string GetBiggestNumberFromArrayWithSize(int[] array, int size)
    {
        if (size == 1)
            return array.Max().ToString();
        if (array.Length == size)
            return string.Join("", array);

        // if i need size X then I'll find the max number outside that range
        int[] findMaxArray = array[..(array.Length - size+1)];
        int max = findMaxArray.Max();
        int maxIndex = Array.IndexOf(findMaxArray, max);

        var newSubArray = array[(maxIndex + 1)..];

        return string.Concat(array[maxIndex], GetBiggestNumberFromArrayWithSize(newSubArray, size - 1));
    }
}
