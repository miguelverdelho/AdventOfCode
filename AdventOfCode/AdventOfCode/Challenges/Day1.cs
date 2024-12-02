using System;

namespace AdventOfCode.Services
{
    public class Day1
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day1", "day1.txt");
        public void RunPart1()
        {
            ReadColumns(out double[] left, out double[] right, out int nrLines);

            var difference = new double[nrLines];

            for (int i = 0; i < nrLines; i++)
            {
                difference[i] = Math.Abs(left[i] - right[i]);
            }

            var sum = difference.Sum();

            Console.WriteLine("Sum: " + sum);
        }
    
        public void RunPart2(){
            ReadColumns(out double[] left, out double[] right, out int nrLines);

            double similarityScore = 0;

            for (int i = 0; i < nrLines; i++){
                int appearancesOnRightColumn = right.Count(x => x == left[i]);
                similarityScore += appearancesOnRightColumn*left[i];
            }

            Console.WriteLine("Similarity score: " + similarityScore);
        }

        public void ReadColumns(out double[] left, out double[] right, out int nrLines){
            string[] lines = File.ReadAllLines(_inputFilePath);
            nrLines = lines.Length;
            left = new double[nrLines];
            right = new double[nrLines];

            for (int i = 0; i < nrLines; i++)
            {
                string[] parts = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                left[i] = double.Parse(parts[0]);
                right[i] = double.Parse(parts[1]);                
            }

            left = left.OrderBy(x => x).ToArray();
            right = right.OrderBy(x => x).ToArray();
        }
    }
}