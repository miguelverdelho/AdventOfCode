using System;
using System.Net;
using System.Runtime.InteropServices;

namespace AdventOfCode.Services
{
    public class Day16
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day16", "day16.txt");
        const char WALL = '#', EMPTY = '.', START = 'S', END = 'E';
        const char UP = 'U', DOWN = 'D', LEFT = 'L', RIGHT = 'R';
        static Tuple<int,int> currentPosition = new Tuple<int, int>(0, 0);
        static char[,] map = new char[0, 0];
        static long[,] costMap = new long[0, 0];

        List<Tuple<int, int>> visitedOnIdealPath = new List<Tuple<int, int>>();

        long minCost = long.MaxValue;

        long part1MinCost = long.MaxValue;
        public void RunPart1()
        {
            // read map
            ReadFile();

            Move(currentPosition, RIGHT, 0);
            Console.WriteLine($"Min Cost: {minCost}");
            part1MinCost = minCost;
            minCost = long.MaxValue;
            MoveTracking(currentPosition, RIGHT, 0, [currentPosition]);


            PrintTempMap(visitedOnIdealPath);

        }
    
        public void RunPart2(){
           
        }

        public void ReadFile(){

            var lines = File.ReadAllLines(_inputFilePath);            
            map = new char[lines[0].Length, lines[0].Length];
            costMap = new long[lines[0].Length, lines[0].Length];
            int i = 0;
            foreach (var line in lines){                
                for(int j = 0; j < line.Length; j++){
                    map[j, i] = line[j];
                    if (line[j] == START) 
                        currentPosition = new Tuple<int, int>(j, i);
                }
                i++;                
            }
        }

        public void PrintMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[j, i]);
                }
                Console.WriteLine();
            }
        }

        public void PrintMap(Tuple<int, int> Robot)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if(j == Robot.Item1 && i == Robot.Item2)
                        Console.Write("@");
                    else
                        Console.Write(map[j, i]);
                }
                Console.WriteLine();
            }
        }

        public void PrintMap(char [,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[j, i]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }


        public void PrintTempMap(List<Tuple<int, int>> path)
        {
            var idealTiles = new HashSet<Tuple<int, int>>(path);

            var tempMap = map;

            foreach (var tile in idealTiles)
            {
                tempMap[tile.Item1, tile.Item2] = 'O';
            }
            PrintMap(tempMap);


            Console.WriteLine($"Ideal Tiles: {idealTiles.Count}");

        }


        public void Move(Tuple<int, int> position, char prevDirection, long cost){

            if (costMap[position.Item1, position.Item2] != 0 && costMap[position.Item1, position.Item2] <= cost)
                return;

            costMap[position.Item1, position.Item2] = cost;

            // PrintDistanceMap();
            //PrintMap(position);
            //Console.WriteLine($"Cost: {cost}");

            if (GetMapValue(position) == END){
                ////PrintTempMap(path);
                //if (cost < minCost)
                //{
                //    //minCost = cost;
                //    visitedOnIdealPath = path;
                //}
                //else if (cost == minCost)
                //{
                //    visitedOnIdealPath.AddRange(path);
                //}

                minCost = Math.Min(minCost, cost);
                return;
            }

            long tempCost;
            // UP
            if(GetMapValue(position, UP) != WALL && !GoingBack(prevDirection, UP))
            {                    
                tempCost = cost + CountTurnsCost(prevDirection, UP) +1;
                var nextPos =GetNextPosition(position, UP);
                //var tempPath = new List<Tuple<int, int>>(path)
                //{
                //    nextPos
                //};
                Move(nextPos, UP, tempCost);
            }
            if(GetMapValue(position, LEFT) != WALL && !GoingBack(prevDirection, LEFT)){

                tempCost = cost + CountTurnsCost(prevDirection, LEFT) + 1;
                var nextPos =GetNextPosition(position, LEFT);
                //var tempPath = new List<Tuple<int, int>>(path)
                //{
                //    nextPos
                //};
                Move(nextPos, LEFT, tempCost);
            }
            if(GetMapValue(position, RIGHT) != WALL && !GoingBack(prevDirection, RIGHT)){
                tempCost = cost + CountTurnsCost(prevDirection, RIGHT) + 1;
                var nextPos =GetNextPosition(position, RIGHT);
                //var tempPath = new List<Tuple<int, int>>(path)
                //{
                //    nextPos
                //};
                Move(nextPos, RIGHT, tempCost);
            }
            if(GetMapValue(position, DOWN) != WALL && !GoingBack(prevDirection, DOWN)){
                tempCost = cost + CountTurnsCost(prevDirection, DOWN) + 1;
                var nextPos =GetNextPosition(position, DOWN);
                //var tempPath = new List<Tuple<int, int>>(path)
                //{
                //    nextPos
                //};
                Move(nextPos, DOWN, tempCost);
            }
            
            return;
        }

        public Tuple<int, int> GetNextPosition(Tuple<int, int> currentPosition, char direction)
        {
            switch (direction)
            {
                case UP:
                    return new (currentPosition.Item1, currentPosition.Item2 - 1);
                case DOWN:
                    return new(currentPosition.Item1, currentPosition.Item2 + 1);
                case LEFT:
                    return new(currentPosition.Item1 - 1, currentPosition.Item2);
                case RIGHT:
                    return new(currentPosition.Item1 + 1, currentPosition.Item2);
                default:
                    return new(currentPosition.Item1, currentPosition.Item2);
            }
        }

        public char GetMapValue(Tuple<int,int> pos, char? direction = null){
            switch (direction)
            {
                case UP:
                    return map[pos.Item1, pos.Item2 - 1];
                case DOWN:
                    return map[pos.Item1, pos.Item2 + 1];
                case LEFT:
                    return map[pos.Item1 - 1, pos.Item2];
                case RIGHT:
                    return map[pos.Item1 + 1, pos.Item2];
                default:
                    return map[pos.Item1, pos.Item2];
            }
        }
    
        public int CountTurnsCost(char prevDirection, char nextDirection){
            if(prevDirection == nextDirection)
                return 0;
            if((prevDirection == UP || prevDirection == DOWN) && (nextDirection == LEFT || nextDirection == RIGHT))
                return 1000;
            else if ((prevDirection == LEFT || prevDirection == RIGHT) && (nextDirection == UP || nextDirection == DOWN))
                return 1000;
            return 0;
        }

        public bool GoingBack(char prevDirection, char nextDirection){
            if(prevDirection == UP && nextDirection == DOWN)
                return true;
            if(prevDirection == DOWN && nextDirection == UP)
                return true;
            if(prevDirection == LEFT && nextDirection == RIGHT)
                return true;
            if(prevDirection == RIGHT && nextDirection == LEFT)
                return true;
            return false;
        }

        public void MoveTracking(Tuple<int, int> position, char prevDirection, long cost, List<Tuple<int, int>> path)
        {
            if(cost > part1MinCost) return;

            if (costMap[position.Item1, position.Item2] != 0 && costMap[position.Item1, position.Item2] + 1001 <= cost)
                return;

            costMap[position.Item1, position.Item2] = cost;

            // PrintDistanceMap();
            //PrintMap(position);
            //Console.WriteLine($"Cost: {cost}");

            if (GetMapValue(position) == END)
            {
                //PrintTempMap(path);
                if (cost < minCost)
                {
                    //minCost = cost;
                    visitedOnIdealPath = path;
                }
                else if (cost == minCost)
                {
                    visitedOnIdealPath.AddRange(path);
                }

                minCost = Math.Min(minCost, cost);
                return;
            }

            long tempCost;
            // UP
            if (GetMapValue(position, UP) != WALL && !GoingBack(prevDirection, UP))
            {
                tempCost = cost + CountTurnsCost(prevDirection, UP) + 1;
                var nextPos = GetNextPosition(position, UP);
                var tempPath = new List<Tuple<int, int>>(path)
                {
                    nextPos
                };
                MoveTracking(nextPos, UP, tempCost, tempPath);
            }
            if (GetMapValue(position, LEFT) != WALL && !GoingBack(prevDirection, LEFT))
            {

                tempCost = cost + CountTurnsCost(prevDirection, LEFT) + 1;
                var nextPos = GetNextPosition(position, LEFT);
                var tempPath = new List<Tuple<int, int>>(path)
                {
                    nextPos
                };
                MoveTracking(nextPos, LEFT, tempCost, tempPath);
            }
            if (GetMapValue(position, RIGHT) != WALL && !GoingBack(prevDirection, RIGHT))
            {
                tempCost = cost + CountTurnsCost(prevDirection, RIGHT) + 1;
                var nextPos = GetNextPosition(position, RIGHT);
                var tempPath = new List<Tuple<int, int>>(path)
                {
                    nextPos
                };
                MoveTracking(nextPos, RIGHT, tempCost, tempPath);
            }
            if (GetMapValue(position, DOWN) != WALL && !GoingBack(prevDirection, DOWN))
            {
                tempCost = cost + CountTurnsCost(prevDirection, DOWN) + 1;
                var nextPos = GetNextPosition(position, DOWN);
                var tempPath = new List<Tuple<int, int>>(path)
                {
                    nextPos
                };
                MoveTracking(nextPos, DOWN, tempCost, tempPath);
            }

            return;
        }
    }
}