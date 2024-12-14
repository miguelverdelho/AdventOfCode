
namespace AdventOfCode.Services
{
    public class Day14
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day14", "day14.txt");

        public struct Robot{
            public int initialX;
            public int initialY;
            public int speedX;
            public int speedY;
            public int FinalX;
            public int FinalY;
        }

        public int mapHeight = 103;
        public int mapWidth = 101;

        public Robot[] robots = [];

        public long topLeftQuadrant = 0;
        public long topRightQuadrant = 0;
        public long bottomLeftQuadrant = 0;
        public long bottomRightQuadrant = 0;

        public long[] safetyVals = []; 


        public void RunPart1(){
            ReadFile();
            CalculateRobotPositionAfterSeconds(100);

            Console.WriteLine($"Top Left: {topLeftQuadrant}, Top Right: {topRightQuadrant}, Bottom Left: {bottomLeftQuadrant}, Bottom Right: {bottomRightQuadrant}");
            Console.WriteLine($"Total: {topLeftQuadrant * topRightQuadrant * bottomLeftQuadrant * bottomRightQuadrant}");
        }

        public void RunPart2(){

            robots = [];
            ReadFile();
            for(int i = 0;i <10000; i++){                
                if( CalculateRobotPositionAfterSeconds(i) > 0){
                    Console.WriteLine($"Seconds: {i}");
                    PrintMapWithRobots(i);
                    break;
                }                
            }
            


        }

        public void ReadFile(){
            string[] lines = File.ReadAllLines(_inputFilePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(' ');
                string[] coords = parts[0].Split(',');
                string[] speed = parts[1].Split(',');

                Robot robot = new ();
                robot.initialX = int.Parse(coords[0].TrimStart('p', '=').Trim());
                robot.initialY = int.Parse(coords[1].TrimStart('p', '=').Trim());
                robot.speedX = int.Parse(speed[0].TrimStart('v', '=').Trim());
                robot.speedY = int.Parse(speed[1].TrimStart('v', '=').Trim());

                robots = [..robots, robot];
            }

            // foreach (Robot robot in robots)
            // {
            //     Console.WriteLine($"Robot: {robot.initialX}, {robot.initialY}, {robot.speedX}, {robot.speedY}");
            // }
        }
    
        public int CalculateRobotPositionAfterSeconds(int seconds){            
            for (int i = 0; i < robots.Length; i++)
            {
                var tempX = robots[i].speedX * seconds%mapWidth + robots[i].initialX;

                if (tempX > mapWidth - 1)
                    tempX -= mapWidth;                
                else if (tempX < 0)
                    tempX += mapWidth;
                robots[i].FinalX = tempX;


                int tempY = robots[i].speedY * seconds%mapHeight + robots[i].initialY;

                if (tempY > mapHeight - 1)
                    tempY -= mapHeight;                
                else if (tempY < 0)
                    tempY += mapHeight;
                robots[i].FinalY = tempY;

                FindQuadrant(robots[i].FinalX, robots[i].FinalY);
            }


            int size = 7; // size of adjacent Xs to find

            var groups = robots.GroupBy(r => r.FinalY);

            foreach (var group in groups)
            {
                var sortedGroup = group.OrderBy(r => r.FinalX).ToArray();

                for (int i = 0; i < sortedGroup.Length - size + 1; i++)
                {
                    bool adjacent = true;
                    for (int j = 0; j < size - 1; j++)
                    {
                        if (sortedGroup[i + j + 1].FinalX != sortedGroup[i + j].FinalX + 1)
                        {
                            adjacent = false;
                            break;
                        }
                    }
                    if (adjacent)
                    {
                        return 1;
                    }
                }
            }


            return 0;
        }
    
        public void FindQuadrant(int x, int y){
            if(x < mapWidth/2 && y < mapHeight/2){
                topLeftQuadrant++;
            }
            else if(x > mapWidth/2 && y < mapHeight/2){
                topRightQuadrant++;
            }
            else if(x < mapWidth/2 && y > mapHeight/2){
                bottomLeftQuadrant++;
            }
            else if(x > mapWidth/2 && y > mapHeight/2){
                bottomRightQuadrant++;
            }
        }
    
        public void PrintMapWithRobots(int seconds){
            char[,] map = new char[mapHeight, mapWidth];

            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    map[i, j] = ' ';
                }
            }

            foreach (Robot robot in robots)
            {
                map[robot.FinalY, robot.FinalX] = 'X';
            }

            for(int i = 0; i < mapHeight; i++){
                for(int j = 0; j < mapWidth; j++){
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Seconds: {seconds}");
        }
    }

}