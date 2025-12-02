using System;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Services
{
    public class Day10
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day10", "day10.txt");
        int[,] _map = new int[0, 0];
        int score = 0;

        List<Trails> trails = new ();

        struct Trails {
            public Tuple<int, int> head;
            public Tuple<int, int> tail;
        }

        public void RunPart1()
        {
            ReadInput();
            ParseFromTrailHeads();
            Console.WriteLine($"Part 1 Score: {score}");
        }

        public void RunPart2(){
            score = 0;
            ParseFromTrailHeads(true);
            Console.WriteLine($"Part 2 Score: {score}");
        }

        public void ReadInput(){
            var fileLines = File.ReadAllLines(_inputFilePath);

            _map = new int[fileLines.Length, fileLines[0].Length];

            for (int i = 0; i < fileLines.Length; i++)
                for (int j = 0; j < fileLines[i].Length; j++)
                    _map[i, j] = int.Parse(fileLines[i][j].ToString());

            // for (int i = 0; i < _map.GetLength(0); i++){
            //     for (int j = 0; j < _map.GetLength(1); j++)
            //         Console.Write(_map[i, j]);            
            //     Console.WriteLine();
            // }
        }


        private void ParseFromTrailHeads(bool rating = false){
            for (int i = 0; i < _map.GetLength(0); i++)
                for (int j = 0; j < _map.GetLength(1); j++)
                    if (_map[i, j] == 0)
                        IsTrailAvailable(i, j, new Tuple<int, int>(i, j), rating);
            
        }

        private void IsTrailAvailable(int x, int y, Tuple<int, int> trailHead, bool rating = false){

            if(_map[x, y] == 9){
                if(rating || !trails.Any(t => t.head.Item1 == trailHead.Item1 && t.head.Item2 == trailHead.Item2 && t.tail.Item1 == x && t.tail.Item2 == y)){
                    score++;
                    if(!rating) 
                        trails.Add(new Trails{head = trailHead, tail = new Tuple<int, int>(x, y)});
                }
                return;
            }
            foreach(Tuple<int, int> nextPos in GetAvailableNextPositions(x, y)){
                if(_map[nextPos.Item1, nextPos.Item2] == _map[x, y] + 1){
                    IsTrailAvailable(nextPos.Item1, nextPos.Item2, trailHead, rating);
                }
            }
        }

        private List<Tuple<int, int>> GetAvailableNextPositions(int x, int y){
            var result = new List<Tuple<int, int>>();

            if( x > 0){
                    result.Add(new Tuple<int, int>(x - 1, y));
            }
            if (x < _map.GetLength(0) - 1){
                    result.Add(new Tuple<int, int>(x + 1, y));
            }
            if(y > 0){
                    result.Add(new Tuple<int, int>(x, y - 1));
            }
            if(y < _map.GetLength(1) - 1){
                    result.Add(new Tuple<int, int>(x, y + 1));
            }

            return result;

        }

        
    }

}