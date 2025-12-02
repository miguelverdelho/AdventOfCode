using System;

namespace AdventOfCode.Services
{
    public class Day2
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day2", "day2.txt");
        public void RunPart1()
        {
            Readlines(out int[][] lines);
            
            CountValidLines(lines);
        }
    
        public void RunPart2(){
            Readlines(out int[][] lines);
            CountValidLines(lines, true);
        }


        public void Readlines(out int[][] lines)
        {
            string[] fileLines = File.ReadAllLines(_inputFilePath);
            lines = new int[fileLines.Length][];

            for (int i = 0; i < fileLines.Length; i++)
            {
                string[] parts = fileLines[i].Split([' '], StringSplitOptions.RemoveEmptyEntries);
                lines[i] = new int[parts.Length];

                for (int j = 0; j < parts.Length; j++)
                    lines[i][j] = int.Parse(parts[j]);
            }
        }

        public bool IsLineOrdered(int[] line){            
            return line.SequenceEqual(line.OrderBy(x => x).ToArray()) || line.SequenceEqual(line.OrderByDescending(x => x).ToArray());
        }

        public bool IsLineDifferencesValid(int[] line){            
            for(int i = 0; i < line.Length - 1; i++){
                var difference = Math.Abs(line[i] - line[i+1]);
                if(difference > 3 || difference < 1) 
                    return false;                
            }
            return true;
        }   

        public void CountValidLines(int[][] lines, bool subArrays = false){
            int validLines = 0;

            foreach(var line in lines)
                if(IsLineOrdered(line) && IsLineDifferencesValid(line)) 
                    validLines++;                
                else if(subArrays && IsSubArrayValid(line)) 
                    validLines++;
            
            Console.WriteLine("Valid lines: " + validLines);
        }

        public int[] GetSubArray(int [] original, int removeIndex){
            List<int> list = new List<int>(original);
            list.RemoveAt(removeIndex);

            var validSubArray = list.ToArray();
            Console.WriteLine($"Original Array: [{string.Join(", ", original)}] Valid Sub Array: [{string.Join(", ", validSubArray)}] Removed Value: {original[removeIndex]} at Index {removeIndex}");
            return validSubArray;
        }

        public bool IsSubArrayValid(int[] original) {            
            for(int i = 0; i <= original.Length - 1; i++){
                var subArray = GetSubArray(original, i);
                if(IsLineDifferencesValid(subArray) && IsLineOrdered(subArray)){
                    Console.WriteLine("Valid");
                    return true;
                }
            }
                Console.WriteLine("");
            return false;
        }
    }
}