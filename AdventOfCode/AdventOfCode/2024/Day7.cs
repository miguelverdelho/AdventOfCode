using System;

namespace AdventOfCode.Services
{
    public class Day7
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day7", "day7.txt");
        static List<Line> lines = [];

        public struct Line{
            public long Result { get; set; }
            public int[] Operands { get; set; }
        }

        public void RunPart1()
        {
            ReadFileIntoLines();
            long sum = 0;

            foreach (var line in lines){
               if(CanGetResult(line.Operands, line.Result)){
                   sum += line.Result;
               }
            }

           Console.WriteLine($"Sum: {sum}");
        }
    
        public void ReadFileIntoLines(){
            List<string> fileLines = [.. File.ReadAllLines(_inputFilePath)];
            
            foreach (string line in fileLines){
                string[] parts = line.Split([':'], StringSplitOptions.RemoveEmptyEntries);
                long result = long.Parse(parts[0].Trim());
                int[] operands = parts[1].Split([' '], StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                lines.Add(new Line{ Result = result, Operands = operands });
            }
        }

        public void RunPart2()
        {
            long sum = 0;
            foreach (var line in lines){
               if(CanGetResult(line.Operands, line.Result, true)){
                   sum += line.Result;
               }
            }

            Console.WriteLine($"Sum part 2: {sum}");
        }

        private bool CanGetResult(int[] operands, long result, bool concat = false)
        {
            var operations = new char [operands.Length-1];
            operations = operations.Select(x => '+').ToArray();
            return Calculate(operands, operations, result, 0, concat);
        }

        private bool Calculate(int[] operands, char[] operations, long result, int index, bool concat = false){

            long res = operands[0];
            for(int i = 0; i < operations.Length; i++){
                if(operations[i] == '+'){
                    res += operands[i+1];
                }
                else if(operations[i] == '*'){
                    res *= operands[i+1];
                }
                else if( concat && operations[i] == '|'){
                    res = long.Parse(res.ToString() + operands[i+1].ToString());
                }
            }

            if(res == result){
                return true;
            }
            else if(index == operations.Length){
                return false;
            }
            else{
                operations[index] = '+';
                if(Calculate(operands, operations, result, index+1, concat)){
                    return true;
                }
                operations[index] = '*';
                if(Calculate(operands, operations, result, index+1, concat)){
                    return true;
                }
                operations[index] = '|';
                if(concat && Calculate(operands, operations, result, index+1, concat)){
                    return true;
                }
                return false;
            }

        }


    }
}