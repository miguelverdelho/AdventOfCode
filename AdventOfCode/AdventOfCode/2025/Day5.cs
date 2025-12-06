using System;

namespace AdventOfCode._2025;

public class Day5
{
    static string projectRootPath = Directory.GetCurrentDirectory();
    static string _inputFilePath = Path.Combine(projectRootPath, "2025","inputs", "day5.txt");
    public void RunPart1()
    {
        var lines = File.ReadAllLines(_inputFilePath);

        List<Tuple<ulong, ulong>> ranges = new List<Tuple<ulong, ulong>>();
        List<ulong> numbers = new List<ulong>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            if (line.Contains("-"))
            {
                var parts = line.Split('-');
                ranges.Add(new Tuple<ulong, ulong>(ulong.Parse(parts[0]), ulong.Parse(parts[1])));
            }
            else
            {
                numbers.Add(ulong.Parse(line));
            }
        }

        var validingredients = 0;
        foreach (var number in numbers)
        {
            var isValid = ranges.Any(x => number >= x.Item1 && number <= x.Item2);
            if (isValid)
            {
                validingredients++;
            }
        }

        Console.WriteLine($"Valid ingredients: {validingredients}");

    }

    public void RunPart2()
    {
        var lines = File.ReadAllLines(_inputFilePath);

        List<Tuple<ulong, ulong>> ranges = new List<Tuple<ulong, ulong>>();
        List<ulong> numbers = new List<ulong>();
        ulong totalValidNumbers = 0;
        List<Tuple<ulong, ulong>> mergedRanges = new List<Tuple<ulong, ulong>>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            if (line.Contains("-"))
            {
                var parts = line.Split('-');
                ranges.Add(new Tuple<ulong, ulong>(ulong.Parse(parts[0]), ulong.Parse(parts[1])));
            }
            else
            {
                numbers.Add(ulong.Parse(line));
            }
        }

        foreach (var range in ranges)
        {
            // if range not overlaps with any other range, add its full length
            var overlaps = mergedRanges.Any(x => x != range && !(range.Item2 < x.Item1 || range.Item1 > x.Item2));
            var embbededInOther = mergedRanges.Any(x => range.Item1 >= x.Item1 && range.Item2 <= x.Item2);
            if (!overlaps)
            {
                mergedRanges.Add(range);
            }
            else if (embbededInOther)
            {
                continue;
            }
            else
            {
                // find all overlapping ranges
                var overlappingRanges = mergedRanges.Where(x => !(range.Item2 < x.Item1 || range.Item1 > x.Item2)).ToList();
                // merge them
                var newStart = Math.Min(range.Item1, overlappingRanges.Min(x => x.Item1));
                var newEnd = Math.Max(range.Item2, overlappingRanges.Max(x => x.Item2));
                // remove overlapping ranges
                foreach (var overlappingRange in overlappingRanges)
                {
                    mergedRanges.Remove(overlappingRange);
                }
                // add merged range
                mergedRanges.Add(new Tuple<ulong, ulong>(newStart, newEnd));
            }
        }

        foreach (var range in mergedRanges)
        {
            totalValidNumbers += (range.Item2 - range.Item1 + 1);
        }

        Console.WriteLine($"Total valid numbers: {totalValidNumbers}");
    }
}
