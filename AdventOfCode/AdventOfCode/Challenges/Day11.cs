using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Services
{
    public class Day11
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day11", "day11.txt");

        long[] initialNumbers = [];

        Dictionary<Tuple<long,int>,long> cache = [];

        public void RunPart1()
        {
            int iterationsPart1 = 25;
            ReadInput();

            long total = 0;
            foreach(long nr in initialNumbers)
                total += Blink182(nr, 0, iterationsPart1);

            Console.WriteLine($"Part1: {total}");
        }

        public void RunPart2(){
             int iterationsPart2 = 75;
             cache = [];

             long total = 0;
            foreach(long nr in initialNumbers)
                total += Blink182(nr, 0, iterationsPart2);
            
            Console.WriteLine($"Part2: {total}");
        }

        public void ReadInput(){
            var fileLines = File.ReadAllLines(_inputFilePath);

            var initialNumbersAsStrings = fileLines[0].Split(' ');

            foreach(var nr in initialNumbersAsStrings){
                initialNumbers = initialNumbers.Append(long.Parse(nr)).ToArray();
            }
        }

        private void FeedZeroCache(int maxIterations){
            var stopwatch = Stopwatch.StartNew();

            for(int i = 0; i < maxIterations; i++){
                Console.WriteLine($"FeedingCache Iteration{i} - time {stopwatch.ElapsedMilliseconds}");
                stopwatch.Restart();
            }

            stopwatch.Stop();

        }


        public long Blink182(long number, int iteration, int maxIterations){
            
            var key = new Tuple<long, int>(number, iteration);
            if (cache.TryGetValue(key, out var value) && value > 0)
            {
                return value;
            }
            
            if(iteration == maxIterations)
                return 1;
            long count = 0;
            var nrToString = number.ToString();
            if(number == 0 ){
                count += Blink182(1, iteration + 1, maxIterations);
            }
            else if(nrToString.Length%2 == 0){
                int halfNrOfDigits = nrToString.Length / 2;

                long firstHalf = long.Parse(nrToString.Substring(0, halfNrOfDigits));
                long secondHalf = long.Parse(nrToString.Substring(halfNrOfDigits));
                // Console.WriteLine($"[Iteration {iteration}] - Spliting {number} to {firstHalf} and {secondHalf}");

                count += Blink182(firstHalf,iteration + 1, maxIterations);
                count += Blink182(secondHalf,iteration + 1, maxIterations);
            }
            else{
                // Console.WriteLine($"[Iteration {iteration}] - Multiplying {number} times 2024 - getting {number*2024}");
                count += Blink182(number*2024, iteration + 1, maxIterations);
            }

            cache[key] = count;

            return count;
        }
    }

}