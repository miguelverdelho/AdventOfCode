namespace AdventOfCode.Services
{
    public class Day15
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day15", "day15_test.txt");

        static char[] moves = [];

        const char UP = '^', DOWN = 'v', LEFT = '<', RIGHT = '>', WALL = '#', BOX = 'O', GUARD = '@', EMPTY = '.', LEFTBOX = '[', RIGHTBOX = ']';

        static char [,] map = new char[0,0];
        static char [,] duplicateMap = new char[0,0];

        static Tuple<int, int> currentPosition = new (0, 0);

        public void RunPart1(){
            ReadFile();

            MoveGuard();

            CountGPS();
        }

        public void RunPart2(){
            map = new char[map.GetLength(0), map.GetLength(1)];
            ReadFile();
            DuplicateMapWidth(map);
            MoveGuardDuplicate();
        }

        #region part 1


        private void CountGPS()
        {
            int total = 0;
            for (int i = 0; i < map.GetLength(0); i++) { 
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == BOX)
                    {
                        total += CountBoxGPS(new Tuple<int, int>(i, j));
                    }
                }
            }

            Console.WriteLine($"Part 1: {total}");
        }

        public void ReadFile(){

            var lines = File.ReadAllLines(_inputFilePath);
            
            map = new char[lines[0].Length, lines[0].Length];
            int i = 0;
            foreach (var line in lines){
                if(line.StartsWith(WALL)){
                    for(int j = 0; j < line.Length; j++){
                        map[j, i] = line[j];
                        if (line[j] == GUARD) 
                            currentPosition = new Tuple<int, int>(i, j);
                    }
                    i++;
                }
                else if(line.StartsWith(' ')){
                    continue;
                }
                else{
                    moves = line.ToCharArray();
                }
            }
        }

        public void MoveGuard(){
            PrintMap();
            foreach (var move in moves){
                // Console.WriteLine($"Move {move}");
                TryMove(move);
                PrintMap();
            }
        }

        public void TryMove(char direction) {

            var nextPosition = GetMapValue(currentPosition, direction);

            if (nextPosition == WALL)
            {
            }
            else if ( nextPosition == BOX) 
            {
                var boxPosition = GetNextPosition(currentPosition, direction);
                TryPushBox(boxPosition, direction);
            }
            else{
                SetMapValue(currentPosition, EMPTY);
                SetMapValue(GetNextPosition(currentPosition, direction), GUARD);
            }           
        }

        private void TryPushBox(Tuple<int, int> boxPostion, char direction)
        {
            var nextBoxPosition = boxPostion;
            while (true)
            {
                nextBoxPosition = GetNextPosition(nextBoxPosition, direction);
                var nextPositionValue = GetMapValue(nextBoxPosition);
                if (nextPositionValue == WALL)
                    return;
                else if (nextPositionValue == EMPTY)
                {
                    SetMapValue(nextBoxPosition, BOX);
                    SetMapValue(boxPostion, EMPTY);
                    SetMapValue(currentPosition, EMPTY);

                    currentPosition = GetNextPosition(currentPosition, direction);
                    SetMapValue(currentPosition, GUARD);
                    return;
                }
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

        public void SetMapValue(Tuple<int,int> pos, char value) {
            map[pos.Item1, pos.Item2] = value;
            if(value == GUARD)
                currentPosition = new Tuple<int, int>(pos.Item1, pos.Item2);
        }

        public void PrintMap()
        {
            // for (int i = 0; i < map.GetLength(0); i++)
            // {
            //     for (int j = 0; j < map.GetLength(1); j++)
            //     {
            //         Console.Write(map[j, i]);
            //     }
            //     Console.WriteLine();
            // }
        }

        public int CountBoxGPS(Tuple<int, int> pos) {
            return 100*pos.Item2 + pos.Item1;
        }
        #endregion

        public void DuplicateMapWidth(char[,] map)
        {
            int width = map.GetLength(1);
            int height = map.GetLength(0);
            duplicateMap = new char[width * 2, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    char tile = map[x, y];
                    switch (tile)
                    {
                        case '#':
                            duplicateMap[x * 2, y] = '#';
                            duplicateMap[x * 2 + 1, y] = '#';
                            break;
                        case 'O':
                            duplicateMap[x * 2, y] = '[';
                            duplicateMap[x * 2 + 1, y] = ']';
                            break;
                        case '.':
                            duplicateMap[x * 2, y] = '.';
                            duplicateMap[x * 2 + 1, y] = '.';
                            break;
                        case '@':
                            duplicateMap[x * 2, y] = '@';
                            duplicateMap[x * 2 + 1, y] = '.';
                            currentPosition = new Tuple<int, int>(x * 2, y);
                            break;
                    }
                }
            }
        }

        public void PrintDuplicateMap()
        {
            for (int i = 0; i < duplicateMap.GetLength(1); i++)
            {
                for (int j = 0; j < duplicateMap.GetLength(0); j++)
                {
                    Console.Write(duplicateMap[j, i]);
                }
                Console.WriteLine();
            }
        }

        public void MoveGuardDuplicate()
        {
            PrintDuplicateMap();
            foreach (var move in moves)
            {
                Console.WriteLine($"Move {move}");
                TryMoveDuplicate(move);
                PrintDuplicateMap();
            }
        }

        public void TryMoveDuplicate(char direction)
        {

            var nextPosition = GetDuplicateMapValue(currentPosition, direction);

            if (nextPosition == WALL)
            {
            }
            else if (nextPosition == LEFTBOX || nextPosition == RIGHTBOX)
            {
                var boxPosition = GetNextPosition(currentPosition, direction);
                TryPushDuplicateBox(boxPosition, direction);
            }
            else
            {
                SetDuplicateMapValue(currentPosition, EMPTY);
                SetDuplicateMapValue(GetNextPosition(currentPosition, direction), GUARD);
            }
        }

        public char GetDuplicateMapValue(Tuple<int, int> pos, char? direction = null)
        {
            switch (direction)
            {
                case UP:
                    return duplicateMap[pos.Item1, pos.Item2 - 1];
                case DOWN:
                    return duplicateMap[pos.Item1, pos.Item2 + 1];
                case LEFT:
                    return duplicateMap[pos.Item1 - 1, pos.Item2];
                case RIGHT:
                    return duplicateMap[pos.Item1 + 1, pos.Item2];
                default:
                    return duplicateMap[pos.Item1, pos.Item2];
            }
        }

        public void SetDuplicateMapValue(Tuple<int, int> pos, char value)
        {
            duplicateMap[pos.Item1, pos.Item2] = value;
            if (value == GUARD)
                currentPosition = new Tuple<int, int>(pos.Item1, pos.Item2);
        }

        private void TryPushDuplicateBox(Tuple<int, int> boxPostion, char direction)
        {
            if (direction  == UP || direction == DOWN)
            {
                // TODO 
                // when pushing up or down, needs to be checked if any of the  [ ] hit other boxes and if those can be pushed before pushing
            }
            else if (direction == LEFT || direction == RIGHT)
            {
                var nextBoxPosition = boxPostion;
                while (true) {
                    nextBoxPosition = GetNextPosition(nextBoxPosition, direction);
                    var nextPositionValue = GetDuplicateMapValue(nextBoxPosition);
                    if (nextPositionValue == WALL)
                        return;
                    else if (nextPositionValue == EMPTY)
                    {
                        SetDuplicateMapValue(currentPosition, EMPTY);
                        SetDuplicateMapValue(boxPostion, GUARD);
                        // from box position to nexPosition
                        if(direction == LEFT)
                            FillBoxesLeftToRight(nextBoxPosition, GetNextPosition(boxPostion, direction));
                        else
                            FillBoxesRightToLeft(GetNextPosition(boxPostion, direction), nextBoxPosition);
                        return;
                    }
                }
            }
        }

        public void FillBoxesLeftToRight(Tuple<int, int> start, Tuple<int, int> end)
        {
            int startX = start.Item1;
            int endX = end.Item1;

            for (int x = startX; x <= endX; x++)
            {
                duplicateMap[x, start.Item2] = '[';
                if (x < endX)
                {
                    duplicateMap[x + 1, start.Item2] = ']';
                    x++; // increment x to skip the next position
                }
            }
        }

        public void FillBoxesRightToLeft(Tuple<int, int> start, Tuple<int, int> end)
        {
            int startX = start.Item1;
            int endX = end.Item1;

            for (int x = endX; x >= startX; x--)            
                if (x > startX)                
                    duplicateMap[x, start.Item2] = ']';                
                else                
                    duplicateMap[x, start.Item2] = '[';
        }

        public void FillBoxesTopToBottom(Tuple<int, int> start, Tuple<int, int> end)
        {
            int startX = start.Item1;
            int startY = start.Item2;
            int endY = end.Item2;

            for (int y = startY; y <= endY; y += 2)
            {
                if (duplicateMap[startY, startX] == '[')
                {
                    duplicateMap[y, startX] = '[';
                    duplicateMap[y, startX + 1] = ']';
                }
                else
                {
                    duplicateMap[y, startX] = ']';
                    duplicateMap[y, startX + 1] = '[';
                }
                if (y > startY)
                {
                    duplicateMap[y - 2, startX] = '.';
                    duplicateMap[y - 2, startX + 1] = '.';
                }
            }
        }

        public void FillBoxesBottomToTop(Tuple<int, int> start, Tuple<int, int> end)
        {
            int startX = start.Item1;
            int startY = start.Item2;
            int endY = end.Item2;

            for (int y = endY; y >= startY; y -= 2)
            {
                if (duplicateMap[startY, startX] == '[')
                {
                    duplicateMap[y, startX] = '[';
                    duplicateMap[y, startX + 1] = ']';
                }
                else
                {
                    duplicateMap[y, startX] = ']';
                    duplicateMap[y, startX + 1] = '[';
                }
                if (y < endY)
                {
                    duplicateMap[y + 2, startX] = '.';
                    duplicateMap[y + 2, startX + 1] = '.';
                }
            }
        }
    }

}