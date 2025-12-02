namespace AdventOfCode._2025;

public class Day2
{
    static string projectRootPath = Directory.GetCurrentDirectory();
    static string _inputFilePath = Path.Combine(projectRootPath, "2025","inputs", "day2.txt");
    public void RunPart1()
    {
        string[] lines = File.ReadAllLines(_inputFilePath);
        string[] ranges = lines[0].Split(',');

        long result = 0;

        foreach (string range in ranges)
        {
            string[] bounds = range.Split('-');
            long.TryParse(bounds[0], out long start);
            long.TryParse(bounds[1], out long end);

            for (long i = start; i <= end; i++) 
            {
                if (i.ToString().Length % 2 != 0) continue;

                if (i.ToString().Substring(0, i.ToString().Length / 2) == i.ToString().Substring(i.ToString().Length / 2))
                    result+= i;
            }
        }

        Console.WriteLine(result);
    }

    public void RunPart2()
    {
        string[] lines = File.ReadAllLines(_inputFilePath);
        string[] ranges = lines[0].Split(',');

        long result = 0;

        foreach (string range in ranges)
        {
            string[] bounds = range.Split('-');
            long.TryParse(bounds[0], out long start);
            long.TryParse(bounds[1], out long end);

            for (long i = start; i <= end; i++)
            {
                string number = i.ToString();

                for (int j = 1; j < number.Length/2 +1; j++) 
                {
                    string subnumber = number.Substring(0, j);
                    int multiplier = (number.Length / subnumber.Length);

                    if (string.Concat(Enumerable.Repeat(subnumber, multiplier)) == number)
                    {
                        result += i;
                        break;
                    }
                }

                //if (i.ToString().Substring(0, i.ToString().Length / 2) == i.ToString().Substring(i.ToString().Length / 2))
                //    result += i;
            }
        }

        Console.WriteLine(result);
    }
}
