using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static AdventOfCode.Services.Day12;

namespace AdventOfCode.Services
{
    public class Day12
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day12", "day12.txt");
        char[,] Map = new char[0, 0];
        bool[,] IsExplored = new bool[0, 0];
        bool[,] IsExploredSides = new bool[0,0];
        Sides[,] MapSides = new Sides[0, 0];
        bool print = false;

        private bool InBounds(int x, int y) => x>=0 && y>=0 && x<Map.GetLength(1) && y<Map.GetLength(0);

        public struct Sides
        {
            public bool Up;
            public bool Down;
            public bool Left;
            public bool Right;
        }

        private enum Directions{
            UP,
            DOWN,
            LEFT,
            RIGHT
        }        

        public void RunPart1(){
            ReadFileIntoMap();

            // go through each position (not part of a  region) and calculate the fence price
            var fencingCost = CalculateFencePriceWithPerimeter();

            Console.WriteLine($"Part 1: {fencingCost}");
        }

        public void RunPart2(){
            ReadFileIntoMap();

            var fencingCost = CalculateFencePriceWithSides();
            Console.WriteLine($"Part 2: {fencingCost}");
        }

        public void ReadFileIntoMap(){
            var lines = File.ReadAllLines(_inputFilePath);
            Map = new char[lines.Length, lines[0].Length];
            IsExplored = new bool[lines.Length, lines[0].Length];
            IsExploredSides = new bool[lines.Length, lines[0].Length];
            MapSides = new Sides[lines.Length, lines[0].Length];

            for (int i = 0; i < lines.Length; i++){
                string line = lines[i];
                for (int j = 0; j < line.Length; j++){
                    var value = line[j];
                    Map[j, i] = value;
                    IsExplored[j, i] = false;                  
                    IsExploredSides[j, i] = false;                  
                }
            }

            //PrintReadMap(Map);
        }

        public void PrintReadMap(char [,] currentMap){

            for(int i = 0; i < currentMap.GetLength(0); i++){
                for(int j = 0; j < currentMap.GetLength(1); j++){
                    Console.Write(currentMap[j, i]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public int CalculateFencePriceWithPerimeter(){
            int total = 0;

            for(int i = 0; i < Map.GetLength(0); i++){
                for(int j = 0; j < Map.GetLength(1); j++){
                    if(!IsExplored[i,j]){
                        int perimeter = 0;
                        int area = FindDimensionsOfRegion(i, j, ref perimeter);
                        // Console.WriteLine($"Region {Map[i,j]} starting on {i},{j} => Area: {area}, Perimeter: {perimeter}");
                        total += area*perimeter;
                    }
                }
            }
            return total;
        }

        public int FindDimensionsOfRegion(int x, int y, ref int perimeter){

            //add current position to "exploredPositions"
            if(IsExplored[x,y]) return 0;
            
            IsExplored[x, y] = true;
            int area = 1;

            // UP
            if(!InBounds(x,y-1) || Map[x, y - 1] != Map[x, y])
                perimeter++;
            else if(!IsExplored[x, y-1] && Map[x, y-1] == Map[x,y]){
                area += FindDimensionsOfRegion(x, y-1, ref perimeter);
            }
            //DOWN
            if(!InBounds(x,y+1) || Map[x, y + 1] != Map[x, y])
                perimeter++;
            else if(!IsExplored[x,y+1] && Map[x,y+1] == Map[x,y]){
                area += FindDimensionsOfRegion(x, y+1, ref perimeter);
            }
            //LEFT
            if(!InBounds(x-1,y) || Map[x - 1, y] != Map[x, y])
                perimeter++;
            else if(!IsExplored[x-1,y] && Map[x-1,y] == Map[x,y]){
                area += FindDimensionsOfRegion(x-1, y, ref perimeter);
            }
            //RIGHT
            if(!InBounds(x+1,y) || Map[x + 1, y] != Map[x, y])
                perimeter++;                        
            else if(!IsExplored[x+1,y] && Map[x+1,y] == Map[x,y]){
                area += FindDimensionsOfRegion(x+1, y, ref perimeter);
            }

            return area;
        }

        public int CalculateFencePriceWithSides(){
            int total = 0;

            for (int i = 0; i < Map.GetLength(0); i++){
                for(int j = 0; j < Map.GetLength(1); j++){
                    if(!IsExplored[i,j]){
                        int sides = 0;
                        FindSides(i,j,ref sides);
                        int area = FindAreadOnly(i, j);
                        Console.WriteLine($"Region {Map[i,j]}  => Area: {area}, Sides: {sides}");
                        total += area*sides;
                    }
                }
            }
            return total;
        }

        private int FindAreadOnly(int x, int y) {
            //add current position to "exploredPositions"
            if (IsExplored[x, y]) return 0;

            IsExplored[x, y] = true;
            int area = 1;

            // UP
            if (InBounds(x, y - 1) && !IsExplored[x, y - 1] && Map[x, y - 1] == Map[x, y])
            {
                area += FindAreadOnly(x, y - 1);
            }
            //DOWN
            if (InBounds(x, y + 1) && !IsExplored[x, y + 1] && Map[x, y + 1] == Map[x, y])
            {
                area += FindAreadOnly(x, y + 1);
            }
            //LEFT
            if (InBounds(x - 1, y) && !IsExplored[x - 1, y] && Map[x - 1, y] == Map[x, y])
            {
                area += FindAreadOnly(x - 1, y);
            }
            //RIGHT
            if (InBounds(x + 1, y) && !IsExplored[x + 1, y] && Map[x + 1, y] == Map[x, y])
            {
                area += FindAreadOnly(x + 1, y);
            }

            return area;
        }

        private void FindSides(int x, int y, ref int sides)
        {
            if (IsExploredSides[x, y]) return;

            IsExploredSides[x, y] = true;

            // UP
            if (!InBounds(x, y - 1) || Map[x, y - 1] != Map[x, y])
            {
                MapSides[x, y].Up = true;
                if(!IsThereAdjacentTopSidesToLeft(x, y) && !IsThereAdjacentTopSidesToRight(x, y))
                {
                    PrintSideAdded(x, y, Directions.UP);
                    sides++;
                }
            }
            //DOWN
            if (!InBounds(x, y + 1) || Map[x, y + 1] != Map[x, y])
            {
                MapSides[x, y].Down = true;
                if(!IsThereAdjacentBottomSidesToLeft(x, y) && !IsThereAdjacentBottomSidesToRight(x, y))
                {
                    PrintSideAdded(x, y, Directions.DOWN);
                    sides++;
                }
            }
            //LEFT
            if (!InBounds(x - 1, y) || Map[x - 1, y] != Map[x, y])
            {
                MapSides[x, y].Left = true;
                if(!IsThereAdjacentLeftSidesUp(x, y) && !IsThereAdjacentLeftSidesDown(x, y))
                {
                    PrintSideAdded(x, y, Directions.LEFT);
                    sides++;
                }
            }
            //RIGHT
            if (!InBounds(x + 1, y) || Map[x + 1, y] != Map[x, y])
            {
                MapSides[x, y].Right = true;
                if(!IsThereAdjacentRightSidesUp(x,y) && !IsThereAdjacentRightSidesDown(x, y))
                {
                    PrintSideAdded(x, y, Directions.RIGHT);
                    sides++;
                }
            }

            // UP
            if (InBounds(x, y - 1) && !IsExploredSides[x, y - 1] && Map[x, y - 1] == Map[x, y])
            {
                FindSides(x, y - 1, ref sides);
            }
            //DOWN
            if (InBounds(x, y + 1) && !IsExploredSides[x, y + 1] && Map[x, y + 1] == Map[x, y])
            {
                FindSides(x, y + 1, ref sides);
            }
            //LEFT
            if (InBounds(x - 1, y) && !IsExploredSides[x - 1, y] && Map[x - 1, y] == Map[x, y])
            {
                FindSides(x - 1, y, ref sides);
            }
            //RIGHT
            if (InBounds(x + 1, y) && !IsExploredSides[x + 1, y] && Map[x + 1, y] == Map[x, y])
            {
                FindSides(x + 1, y, ref sides);
            }
        }

        private bool IsThereAdjacentTopSidesToRight(int x, int y) 
        {
            if(InBounds(x+1,y) && Map[x, y] == Map[x + 1, y])
            {
                if (InBounds(x + 1, y - 1) && Map[x, y] == Map[x + 1, y - 1])
                    return false;

                return MapSides[x+1,y].Up || IsThereAdjacentTopSidesToRight(x+1, y);
            }
            return false;
        }
        private bool IsThereAdjacentTopSidesToLeft(int x, int y)
        {
            if (InBounds(x - 1, y) && Map[x, y] == Map[x - 1, y])
            {
                if (InBounds(x - 1, y - 1) && Map[x, y] == Map[x - 1, y - 1])
                    return false;

                return MapSides[x - 1, y].Up || IsThereAdjacentTopSidesToLeft(x - 1, y);
            }
            return false;
        }

        private bool IsThereAdjacentBottomSidesToLeft(int x, int y)
        {
            if (InBounds(x - 1, y) && Map[x, y] == Map[x - 1, y])
            {
                if (InBounds(x - 1, y + 1) && Map[x, y] == Map[x - 1, y + 1])
                    return false;

                return MapSides[x - 1, y].Down || IsThereAdjacentBottomSidesToLeft(x - 1, y);
            }
            return false;
        }

        private bool IsThereAdjacentBottomSidesToRight(int x, int y)
        {
            if (InBounds(x + 1, y) && Map[x, y] == Map[x + 1, y])
            {
                if (InBounds(x + 1, y + 1) && Map[x, y] == Map[x + 1, y + 1])
                    return false;

                return MapSides[x+1,y].Down || IsThereAdjacentBottomSidesToRight(x+1, y);
            }
            return false;
        }

        private bool IsThereAdjacentLeftSidesUp(int x, int y)
        {
            if (InBounds(x, y - 1) && Map[x, y] == Map[x, y - 1])
            {
                if (InBounds(x - 1, y - 1) && Map[x, y] == Map[x - 1, y - 1])
                    return false;

                return MapSides[x,y-1].Left || IsThereAdjacentLeftSidesUp(x, y-1);
            }
            return false;
        }
        private bool IsThereAdjacentLeftSidesDown(int x, int y)
        {
            if (InBounds(x, y + 1) && Map[x, y] == Map[x, y + 1])
            {
                if (InBounds(x - 1, y + 1) && Map[x, y] == Map[x - 1, y + 1])
                    return false;

                return MapSides[x, y + 1].Left || IsThereAdjacentLeftSidesDown(x, y + 1);
            }
            return false;
        }

        private bool IsThereAdjacentRightSidesUp(int x, int y)
        {
            if (InBounds(x, y - 1) && Map[x, y] == Map[x, y - 1])
            {
                if (InBounds(x + 1, y - 1) && Map[x, y] == Map[x + 1, y - 1])
                    return false;

                return MapSides[x,y-1].Right || IsThereAdjacentRightSidesUp(x, y-1);
            }
            return false;
        }

        private bool IsThereAdjacentRightSidesDown(int x, int y)
        {
            if (InBounds(x, y + 1) && Map[x, y] == Map[x, y + 1])
            {
                if (InBounds(x + 1, y + 1) && Map[x, y] == Map[x + 1, y + 1])
                    return false;

                return MapSides[x, y + 1].Right || IsThereAdjacentRightSidesDown(x, y + 1);
            }
            return false;
        }

        private void PrintSideAdded(int x, int y, Directions dir)
        {
            if (print)
                Console.WriteLine($"Side for {Map[x, y]} added:{dir} on {x}, {y}");
        }

    }

}