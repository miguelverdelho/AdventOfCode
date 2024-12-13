using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static AdventOfCode.Services.Day12;

namespace AdventOfCode.Services
{
    public class Day13
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day13", "day13.txt");

        List<Game> games = [];

        struct Game{
            public long xT;
            public long yT;
            public long xA;
            public long yA;
            public long xB;
            public long yB;
        }

        public void RunPart1(){
            ReadFile();
            IterateThroughGames();
        }

        public void RunPart2(){
            games = [];
            ReadFile(true);
            IterateThroughGames();
        }

        private void ReadFile( bool part2 = false){
            string[] lines = File.ReadAllLines(_inputFilePath);

            for (int i = 0; i < lines.Length; i += 4)
            {
                 var game = new Game();
                if (i + 2 < lines.Length)
                {
                    string[] partsA = lines[i].Split(':');
                    string[] partsB = lines[i + 1].Split(':');
                    string[] partsPrize = lines[i + 2].Split(':');

                    game.xA = int.Parse(partsA[1].Split(',')[0].Replace("X+", ""));
                    game.yA = int.Parse(partsA[1].Split(',')[1].Replace("Y+", ""));

                    game.xB = int.Parse(partsB[1].Split(',')[0].Replace("X+", ""));
                    game.yB = int.Parse(partsB[1].Split(',')[1].Replace("Y+", ""));

                    game.xT = int.Parse(partsPrize[1].Split(',')[0].Replace("X=", ""));
                    game.yT = int.Parse(partsPrize[1].Split(',')[1].Replace("Y=", ""));

                    game.xT += 10000000000000;
                    game.yT += 10000000000000;
                }

                games.Add(game);
            }
        }

        private void IterateThroughGames(){

            long total = 0;
            foreach(var game in games){
                long neededTokens = CalculateNeededTokens(game);
                total += neededTokens;
                // Console.WriteLine($"Needed tokens for game {game.xT} {game.yT}: {neededTokens}");
            }

            Console.WriteLine($"Total: {total}");
        }

        private long CalculateNeededTokens(Game game){
            long minPrice;
            decimal A = (decimal)(game.yT*game.xB - game.xT*game.yB)/(decimal)(game.yA*game.xB - game.xA*game.yB);
            
            decimal B = (decimal)(game.xT - A*game.xA)/(decimal)(game.xB);

            if (A % 1 == 0 && B % 1 == 0)
            {
                minPrice = (long)(3*A + B);
            }
            else 
                return 0;
            return minPrice;
        }

    }

}