using System;

namespace AdventOfCode.Services
{
    public class Day8
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day8", "day8.txt");
        static char [,]? Map;
        public List<Antenna> Antennas = [];

        public List<Tuple<int, int>> AntiNodes = [];

        public struct Antenna{
            public char Name {get; set;}
            public Tuple<int, int> Position {get; set;}
            public List<Tuple<int, int>> EqualAntennas {get; set;} = [];

            public Antenna(char name, Tuple<int, int> position){
                Name = name;
                Position = position;
            }

            public override string ToString() => $"{Name} {Position} {{{string.Join(",", EqualAntennas)}}}";
        }

        public void RunPart1()
        {
            ReadFileIntoMap();

            // for each antenna, calculate all its antinodes
            foreach (var antenna in Antennas){
                CalculateAntinodes(antenna);
            }

            Console.WriteLine($"Part 1: {AntiNodes.Count} antinodes");
        }

        private void CalculateAntinodes(Antenna antenna)
        {
            foreach (var equalAntenna in antenna.EqualAntennas){
                var differenceBetweenAntennas = new Tuple<int, int>(equalAntenna.Item1 - antenna.Position.Item1, equalAntenna.Item2 - antenna.Position.Item2);
                var newAntinode = new Tuple<int, int>(equalAntenna.Item1 + differenceBetweenAntennas.Item1, equalAntenna.Item2 + differenceBetweenAntennas.Item2);

                if(!AntiNodes.Contains(newAntinode) && IsInMap(newAntinode))
                {
                    // Console.WriteLine($"{antenna.Position} - {equalAntenna} -> new antinode: {newAntinode} - difference: {differenceBetweenAntennas}");
                    AntiNodes.Add(newAntinode);
                }

            }
        }

        private void CalculateAntinodesWithHarmonica(Antenna antenna)
        {
            foreach (var equalAntenna in antenna.EqualAntennas){
                var differenceBetweenAntennas = new Tuple<int, int>(equalAntenna.Item1 - antenna.Position.Item1, equalAntenna.Item2 - antenna.Position.Item2);
                var newAntinode = equalAntenna;

                while(IsInMap(newAntinode)){
                    if(!AntiNodes.Contains(newAntinode))
                    {
                        // Console.WriteLine($"{antenna.Position} - {equalAntenna} -> new antinode: {newAntinode} - difference: {differenceBetweenAntennas}");
                        AntiNodes.Add(newAntinode);
                        Map![newAntinode.Item1, newAntinode.Item2] = '#';
                    }
                    newAntinode = new Tuple<int, int>(newAntinode.Item1 + differenceBetweenAntennas.Item1, newAntinode.Item2 + differenceBetweenAntennas.Item2);
                }
            }
        }

        public void ReadFileIntoMap(){
            var lines = File.ReadAllLines(_inputFilePath);
            Map = new char[lines.Length, lines[0].Length];

            for (int i = 0; i < lines.Length; i++){
                string line = lines[i];
                for (int j = 0; j < line.Length; j++){
                    var value = line[j];
                    Map[i, j] = value;
                    if(value != '.'){
                        Antennas.Add(new Antenna(value, new Tuple<int, int>(i, j)));
                    }
                }
            }

            foreach (var antenna in Antennas){
                var equalAntennas = Antennas.Where(x => x.Name == antenna.Name).ToList();
                antenna.EqualAntennas.AddRange(equalAntennas.Where(x => x.Position != antenna.Position).Select(x => x.Position).ToList());
            }

            PrintReadMap(Map);
        }

        public void PrintReadMap(char [,] currentMap){

            for(int i = 0; i < currentMap.GetLength(0); i++){
                for(int j = 0; j < currentMap.GetLength(1); j++){
                    Console.Write(currentMap[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            // Antennas.ForEach(x => Console.WriteLine(x));
            // Console.WriteLine($"Antennas: {Antennas.Count}");
        }

        public void RunPart2()
        {
            AntiNodes = [];
            foreach (var antenna in Antennas){
                CalculateAntinodesWithHarmonica(antenna);
            }

            Console.WriteLine($"Part 2: {AntiNodes.Count} antinodes");

            PrintReadMap(Map!);
        }

        public bool IsInMap(Tuple<int, int> position){
            return position.Item1 >= 0
            && position.Item2 >= 0
            && position.Item1 < Map!.GetLength(0)
            && position.Item2 < Map!.GetLength(1);
        }
    }
}