using System;
using System.Globalization;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode.Services
{
    public class Day6
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day6", "day6.txt");

        static char [,]? originalMap;

        const char GUARD_MOVING_UP ='^';
        const char GUARD_MOVING_DOWN ='V';
        const char GUARD_MOVING_LEFT ='<';
        const char GUARD_MOVING_RIGHT ='>';
        const char OBSTACLE = '#'; 

        enum Direction{
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        public void RunPart1()
        {
            ReadFileIntoMap(out char [,] currentMap, out Tuple<int, int> currentPosition, out Tuple<int, int> nextPosition, out char currentGuardIcon);
            SimulateGuardWalk(ref currentMap, currentPosition, nextPosition, currentGuardIcon);            
        }

        private void SimulateGuardWalk(ref char[,] map, Tuple<int, int> currentPosition, Tuple<int, int> nextPosition, char currentGuardIcon)
        {

            //while next position is within map => move 
            while(IsNextPositionInMap(nextPosition, map)){
                // Console.WriteLine($"current position {currentPosition.Item1}, {currentPosition.Item2}");
                UpdateCurrentPosition(ref currentGuardIcon, ref map, ref currentPosition, ref nextPosition);
                CalculateNextPosition(map, currentPosition, ref nextPosition, ref currentGuardIcon);                
                // PrintReadMap(map); 
                // Console.WriteLine($"next position {nextPosition.Item1}, {nextPosition.Item2}");
            }
            map[currentPosition.Item1, currentPosition.Item2] = 'X';       
            CountSteps(map);     
        }

        private void CountSteps(char [,] map){
            int steps = 0;
            for(int i = 0; i < map.GetLength(0); i++){
                for(int j = 0; j < map.GetLength(1); j++){
                    if(map[i, j] == 'X'){
                        steps++;
                    }
                }
            }
            Console.WriteLine($"Steps: {steps}");
        }

        private void CalculateNextPosition(char[,] currentMap, Tuple<int, int> currentPosition, ref Tuple<int, int> nextPosition, ref char currentGuardIcon)
        {
            Direction direction = GetMovingDirection(currentGuardIcon);
            var tempNextPosition = nextPosition;
            var tempGuardIcon = currentGuardIcon;
            if(direction == Direction.UP) {
                tempNextPosition = new Tuple<int, int>(currentPosition.Item1 - 1, currentPosition.Item2);
                tempGuardIcon = GUARD_MOVING_UP;
            }
            if(direction == Direction.DOWN) {
                tempNextPosition = new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2);
                tempGuardIcon = GUARD_MOVING_DOWN;
            }
            if(direction == Direction.LEFT){
                tempNextPosition = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 - 1);
                tempGuardIcon = GUARD_MOVING_LEFT;
            }
            if(direction == Direction.RIGHT){
                tempNextPosition = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 + 1);
                tempGuardIcon = GUARD_MOVING_RIGHT;
            } 

            

            while(IsNextPositionInMap(tempNextPosition,currentMap) && currentMap![tempNextPosition.Item1, tempNextPosition.Item2] == OBSTACLE ){
                // Console.WriteLine($"obstacle at {tempNextPosition.Item1}, {tempNextPosition.Item2}");
                tempNextPosition = nextPosition;
                direction = GetNextTempDirection(direction);
                if(direction == Direction.UP) {
                    tempNextPosition = new Tuple<int, int>(currentPosition.Item1 - 1, currentPosition.Item2);
                    tempGuardIcon = GUARD_MOVING_UP;
                }
                if(direction == Direction.DOWN) {
                    tempNextPosition = new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2);
                    tempGuardIcon = GUARD_MOVING_DOWN;
                }
                if(direction == Direction.LEFT){
                    tempNextPosition = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 - 1);
                    tempGuardIcon = GUARD_MOVING_LEFT;
                }
                if(direction == Direction.RIGHT){
                    tempNextPosition = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 + 1);
                    tempGuardIcon = GUARD_MOVING_RIGHT;
                } 
            }
            // Console.WriteLine($"next position to {tempNextPosition.Item1}, {tempNextPosition.Item2} => direction {direction}");
            currentGuardIcon = tempGuardIcon;
            nextPosition = tempNextPosition;
        }

        private Direction GetNextTempDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.UP:
                    return Direction.RIGHT;
                case Direction.DOWN:
                    return Direction.LEFT;
                case Direction.LEFT:
                    return Direction.UP;
                case Direction.RIGHT:
                    return Direction.DOWN;
                default:
                    return Direction.UP;
            }
        }

        private static Direction GetMovingDirection( char currentGuardIcon)
        {
            return currentGuardIcon switch
            {
                GUARD_MOVING_UP => Direction.UP,
                GUARD_MOVING_DOWN => Direction.DOWN,
                GUARD_MOVING_LEFT => Direction.LEFT,
                GUARD_MOVING_RIGHT => Direction.RIGHT,
                _ => Direction.UP,
            };
        }

        private void UpdateCurrentPosition(ref char currentGuardIcon, ref char[,] currentMap, ref Tuple<int, int> currentPosition, ref Tuple<int, int> nextPosition)
        {
            char temp = currentGuardIcon;
            currentMap![currentPosition.Item1, currentPosition.Item2] = 'X';
            currentPosition = nextPosition;
            currentMap![currentPosition.Item1, currentPosition.Item2] = temp;
        }

        private bool IsNextPositionInMap(Tuple<int, int> nextPosition, char [,] currentMap){
            bool isInMap = currentMap?.GetLength(0) > nextPosition?.Item1 
            && currentMap?.GetLength(1) > nextPosition?.Item2
            && nextPosition.Item1 >= 0
            && nextPosition.Item2 >= 0;
            // Console.WriteLine($"next position {nextPosition.Item1}, {nextPosition.Item2} in map? {isInMap}");
            return isInMap;     
        }

        public void ReadFileIntoMap(out char[,] currentMap, out Tuple<int, int> currentPosition, out Tuple<int, int> nextPosition, out char currentGuardIcon){
            currentPosition = new Tuple<int, int>(0, 0);
            nextPosition = currentPosition;
            currentGuardIcon ='^';
            
            var lines = File.ReadAllLines(_inputFilePath);
            currentMap = new char[lines.Length, lines[0].Length];
            originalMap = new char[lines.Length, lines[0].Length];

            for (int i = 0; i < lines.Length; i++){
                string line = lines[i];
                for (int j = 0; j < line.Length; j++){
                    currentMap[i, j] = line[j];
                    originalMap[i, j] = line[j];
                    if(IsGuard(line[j]))
                    {
                        currentPosition = new Tuple<int, int>(i, j);
                        nextPosition = currentPosition;
                        currentGuardIcon = line[j];
                    } 
                }
            }
            

            // PrintReadMap();
        }

        private bool IsGuard(char c){
            return c == GUARD_MOVING_UP || c == GUARD_MOVING_DOWN || c == GUARD_MOVING_LEFT || c == GUARD_MOVING_RIGHT;
        }

        public void PrintReadMap(char [,] currentMap){
            for(int i = 0; i < currentMap.GetLength(0); i++){
                for(int j = 0; j < currentMap.GetLength(1); j++){
                    Console.Write(currentMap[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void RunPart2(){
            ReadFileIntoMap(out char [,] currentMap, out Tuple<int, int> currentPosition, out Tuple<int, int> nextPosition, out char currentGuardIcon);            
            SimulateGuardWalk(ref currentMap, currentPosition, nextPosition, currentGuardIcon);

            // check for positions where the guard passes
            List<Tuple<int, int>> guardPassingPositions = GetGuardPassingPositions(currentMap);
            
            
            int loops = 0;

            foreach(var guardPassingPosition in guardPassingPositions){
                 Console.WriteLine($"Guard passing position: {guardPassingPosition.Item1}, {guardPassingPosition.Item2}");
                if(IsRunWithObstacleLooped(guardPassingPosition,currentPosition)){
                    loops ++;
                };
                // Console.ReadLine();
            }

            Console.WriteLine($"Part 2: {loops} loops");
        }

        public List<Tuple<int, int>> GetGuardPassingPositions(char [,] currentMap){
            var guardPassingPositions = new List<Tuple<int, int>>();
            for(int i = 0; i < currentMap.GetLength(0); i++){
                for(int j = 0; j < currentMap.GetLength(1); j++){
                    if(currentMap[i, j] == 'X' && originalMap![i, j] != '^'){
                        guardPassingPositions.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            return guardPassingPositions;
        }
        

        public bool IsRunWithObstacleLooped(Tuple<int, int> guardPassingPosition, Tuple<int, int> currentPosition){
            
            char[,] tempMap = (char[,])originalMap!.Clone();
            // tempMap[guardPassingPosition.Item1, guardPassingPosition.Item2] = '0';
            // PrintReadMap(tempMap);
            tempMap[guardPassingPosition.Item1, guardPassingPosition.Item2] = '#';
            // Console.WriteLine($"Obstacle at: {guardPassingPosition.Item1}, {guardPassingPosition.Item2}");
            
            // run guard walk with obstacle 
            return IsWalkLooped(ref tempMap, currentPosition, currentPosition, '^');
        }

        private bool IsWalkLooped(ref char[,] map, Tuple<int, int> currentPosition, Tuple<int, int> nextPosition, char currentGuardIcon)
        {
            var isLooped = false;
            var history = new List<Steps>
            {
                // new (currentPosition, currentGuardIcon)
            };

            //while next position is within map => move 
            do{
                // Console.WriteLine($"current position {currentPosition.Item1}, {currentPosition.Item2}");
                UpdateCurrentPosition(ref currentGuardIcon, ref map, ref currentPosition, ref nextPosition);
                if(history.Any(x => x.position.Item1 == currentPosition.Item1 && x.position.Item2 == currentPosition.Item2 && x.direction == GetMovingDirection(currentGuardIcon))){
                    isLooped = true;
                    // Console.WriteLine($"Looped at {currentPosition.Item1}, {currentPosition.Item2}");
                    break;
                }
                history.Add(new Steps(currentPosition, currentGuardIcon));

                CalculateNextPosition(map, currentPosition, ref nextPosition, ref currentGuardIcon);                
                // PrintReadMap(map); 
                // Console.WriteLine($"next position {nextPosition.Item1}, {nextPosition.Item2}");
            }while(IsNextPositionInMap(nextPosition, map));
            
            return isLooped;
        }

        struct Steps{
            internal Tuple<int, int> position;
            internal Direction direction;

            public Steps(Tuple<int, int> position, char icon){
                this.position = position;
                this.direction = GetMovingDirection(icon);
            }
        }
    }
}