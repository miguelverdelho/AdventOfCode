using System;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Services
{
    public class Day3
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day3", "day3.txt");
        public void RunPart1()
        {
            Readlines1(out string[] lines);
            var sum = 0;

            foreach(var line in lines){
                var lineCleansed = line.Replace("mul(", "").Replace(")","");
                string[] septValues = lineCleansed.Split(",");
                int[] values = Array.ConvertAll(septValues, int.Parse);
                sum += values[0] * values[1];
            }
            Console.WriteLine("Sum: " + sum);
        }


        public void Readlines1(out string[] lines)
        {
            string pattern = @"mul\(\d{1,3},\d{1,3}\)";

            string contents = File.ReadAllText(_inputFilePath);

            MatchCollection matches = Regex.Matches(contents, pattern);

            string[] substrings = new string[matches.Count];
            lines = new string[substrings.Length];            
            for (int i = 0; i < matches.Count; i++)
            {
                substrings[i] = matches[i].Value;
                // Console.WriteLine(substrings[i]);
                lines[i] = substrings[i];
            }
        }

    
        public void RunPart2(){
            Readlines2(out string[] lines);

            bool doing = true;
            var sum = 0;

            foreach(var line in lines){
                if(doing){
                    if(line.StartsWith("mul")){
                        var lineCleansed = line.Replace("mul(", "").Replace(")","");
                        string[] septValues = lineCleansed.Split(",");
                        int[] values = Array.ConvertAll(septValues, int.Parse);
                        sum += values[0] * values[1];
                    }
                    else if(line.StartsWith("don't()")){
                        Console.WriteLine("Stopped");
                        doing = false;
                    }
                } 
                else if(line.StartsWith("do()")){
                    Console.WriteLine("Started");
                    doing = true;
                }
            }

            Console.WriteLine("Sum: " + sum);

        }
        
        public void Readlines2(out string[] lines)
        {
            string pattern = @"mul\(\d{1,3},\d{1,3}\)|do\(\)|don't\(\)";

            string contents = File.ReadAllText(_inputFilePath);

            MatchCollection matches = Regex.Matches(contents, pattern);

            string[] substrings = new string[matches.Count];
            lines = new string[substrings.Length];            
            for (int i = 0; i < matches.Count; i++)
            {
                substrings[i] = matches[i].Value;
                Console.WriteLine(substrings[i]);
                lines[i] = substrings[i];
            }

        }

    }
}