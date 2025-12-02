namespace AdventOfCode._2025;

public class Day1
{
    static string projectRootPath = Directory.GetCurrentDirectory();
    static string _inputFilePath = Path.Combine(projectRootPath, "2025","inputs", "day1_sample.txt");
    public void RunPart1()
    {
        //read lines from file
        string[] lines = File.ReadAllLines(@"C:\Users\migue\source\repos\miguelverdelho\AdventOfCode\AdventOfCode\AdventOfCode\2025\inputs\day1_test.txt"); 
        int x = 50;
        int result = 0;

        foreach (string line in lines)
        {
            // parse first char from line as L or R
            char direction = line[0];

            int.TryParse(line[1..], out int value);

            if (direction == 'L')
            {
                x -= value;
            }
            else if (direction == 'R')
            {
                x += value;
            }

            if (x%100 == 0)
            {
                result++;
            }
        }

        Console.WriteLine(result);
    }

    public void RunPart2()
    {

        //read lines from file
        string[] lines = File.ReadAllLines(@"C:\Users\migue\source\repos\miguelverdelho\AdventOfCode\AdventOfCode\AdventOfCode\2025\inputs\day1.txt");

        int x = 50;
        int prevX = 0;
        int result = 0;
        int i = 0;

        foreach (string line in lines)
        {
            i++;
            char direction = line[0];
            int.TryParse(line[1..], out int instruction);
            prevX = x;

            if (direction == 'L')
            {
                x -= instruction;
            }
            else if (direction == 'R')
            {
                x += instruction;
            }

            if (x < 0)
            {
                if (prevX != 0) result++;

                result += Math.Abs(x / 100);
                var overflow = Math.Abs((x) % 100);

                x = overflow == 0? 0: 100 - overflow;
            }
            else if (x > 99)
            {
                result += x / 100;

                var overflow = Math.Abs((x) % 100);

                x = overflow;
            }
            else if (x == 0)
            {
                result++;
            }
            Console.WriteLine(i +": " +prevX +""+ line.Replace('R', '+').Replace('L','-') + "=" + x + "=>" + result);

        }

        Console.WriteLine(result);
    }
}
