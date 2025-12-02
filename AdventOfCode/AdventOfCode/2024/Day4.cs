using System;
using System.Text.RegularExpressions;

namespace AdventOfCode.Services
{
    public class Day4
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day4", "day4_test.txt");
        // static Regex pattern = new Regex(@"(?:(?:XMAS|SAMX))");
        static int exes = 0;
        static List<string> lines = new();

        public void RunPart1()
        {
            // get all the possible lines
            ReadFileIntoLines();
            GetAllDiagonals();

            int totalMatches = 0;
            lines.ForEach(x => {
                int count = CheckForXMAS(x);
                totalMatches += count;
            });
            // Console.WriteLine("Lines: " + lines.Count);
            // Console.WriteLine("Total matches: " + totalMatches);        
        }

        public static void ReadFileIntoLines(){
            // read file into matrix
            char[,] charMatrix = ReadFileIntoCharMatrix();

            //convert matrix into lines       
            // horizontal, back and forth
            for(int i = 0; i < charMatrix.GetLength(0); i++)
            {
                string line = "";
                for (int j = 0; j < charMatrix.GetLength(1); j++)
                {
                    line += charMatrix[i, j];
                }
                if(CheckForXMAS(line) > 0){
                    Console.WriteLine($"Line {i} - {line}  - {CheckForXMAS(line) } matches");
                }
                lines.Add(line);
               
            }

            //vertical back and forth
            for (int i = 0; i < charMatrix.GetLength(1); i++)
            {
                string column = "";
                for (int j = 0; j < charMatrix.GetLength(0); j++)
                {
                    column += charMatrix[j, i];
                }
                if(CheckForXMAS(column)  > 0){
                    Console.WriteLine($"Column {i} - {column} - {CheckForXMAS(column) } matches");
                }                
                lines.Add(column);
                // lines.Add(new string(column.Reverse().ToArray()));
            }

            // var diagonals = GetAllDiagonals();
            // diagonals.ForEach(x => lines.Add(new string(x.ToArray())));

        }

        public static List<List<char>> GetAllDiagonals()
        {
            var matrix = ReadFileIntoCharMatrix();
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            // List to store all diagonals
            List<List<char>> diagonals = new List<List<char>>();

            // Step 1: Get all left-to-right diagonals
            for (int startCol = 0; startCol < cols; startCol++)
            {
                // Collect diagonal starting from the first row
                List<char> diagonal = new List<char>();
                int r = 0, c = startCol;
                while (r < rows && c < cols)
                {
                    diagonal.Add(matrix[r, c]);
                    r++;
                    c++;
                }
                diagonals.Add(diagonal); // Add the diagonal to the list
                if(CheckForXMAS(new string(diagonal.ToArray())) > 0){
                    Console.WriteLine($"D - left to right  row{r} - {new string(diagonal.ToArray())} - {CheckForXMAS(new string(diagonal.ToArray()))} matches");
                }    

            }

            for (int startRow = 1; startRow < rows; startRow++)
            {
                // Collect diagonal starting from the first column
                List<char> diagonal = new List<char>();
                int r = startRow, c = 0;
                while (r < rows && c < cols)
                {
                    diagonal.Add(matrix[r, c]);
                    r++;
                    c++;
                }
                diagonals.Add(diagonal); // Add the diagonal to the list
                if(CheckForXMAS(new string(diagonal.ToArray())) > 0){
                    Console.WriteLine($"D - left to right  col{c} - {new string(diagonal.ToArray())} - {CheckForXMAS(new string(diagonal.ToArray()))} matches");
                }       

            }

            // Step 2: Get all right-to-left diagonals
            for (int startCol = cols - 1; startCol >= 0; startCol--)
            {
                // Collect diagonal starting from the first row
                List<char> diagonal = new List<char>();
                int r = 0, c = startCol;
                while (r < rows && c >= 0)
                {
                    diagonal.Add(matrix[r, c]);
                    r++;
                    c--;
                }
                diagonals.Add(diagonal); // Add the diagonal to the list
                if(CheckForXMAS(new string(diagonal.ToArray())) > 0){
                    Console.WriteLine($"D - right to left  row{c} - {new string(diagonal.ToArray())} - {CheckForXMAS(new string(diagonal.ToArray()))} matches");
                } 
            }

            for (int startRow = 1; startRow < rows; startRow++)
            {
                // Collect diagonal starting from the last column
                List<char> diagonal = new List<char>();
                int r = startRow, c = cols - 1;
                while (r < rows && c >= 0)
                {
                    diagonal.Add(matrix[r, c]);
                    r++;
                    c--;
                }
                diagonals.Add(diagonal); // Add the diagonal to the list
                if(CheckForXMAS(new string(diagonal.ToArray())) > 0){
                    Console.WriteLine($"D - right to left  col{c} - {new string(diagonal.ToArray())} - {CheckForXMAS(new string(diagonal.ToArray()))} matches");
                }
            }
            diagonals.ForEach(x => {
                lines.Add(new string(x.ToArray()));
            });

            return diagonals; // Return all diagonals
        }

        public static char[,] ReadFileIntoCharMatrix()
        {
            string[] lines = File.ReadAllLines(_inputFilePath);
            char[,] charMatrix = new char[lines.Length, lines.Max(line => line.Length)];

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                for (int j = 0; j < line.Length; j++)
                {
                    charMatrix[i, j] = line[j];
                }
            }
            return charMatrix;
        }

        static int CheckForXMAS(string input){
            string pattern = @"(?=(XMAS|SAMX))"; // Lookahead regex
            MatchCollection matches = Regex.Matches(input, pattern);
            return matches.Count;
        }

        public void RunPart2(){
            char [,] charMatrix = ReadFileIntoCharMatrix();

            // get all the possible 3x3 matrixes
            GetAllSubMatrixes(charMatrix, 3, 3);
            Console.WriteLine("Exes: " + exes);
        }

        private void GetAllSubMatrixes(char[,] charMatrix, int v1, int v2)
        {
            for (int i = 1; i < charMatrix.GetLength(0)-1; i++){
                for(int j = 1; j < charMatrix.GetLength(1)-1; j++){
                    char [,] subMatrix = new char[v1, v2];
                    subMatrix[0,0] = charMatrix[i-1, j-1];
                    subMatrix[0,1] = charMatrix[i-1, j];
                    subMatrix[0,2] = charMatrix[i-1, j+1];
                    subMatrix[1,0] = charMatrix[i, j-1];
                    subMatrix[1,1] = charMatrix[i, j];
                    subMatrix[1,2] = charMatrix[i, j+1];
                    subMatrix[2,0] = charMatrix[i+1, j-1];
                    subMatrix[2,1] = charMatrix[i+1, j];
                    subMatrix[2,2] = charMatrix[i+1, j+1];

                    if( subMatrix[1,1] == 'A' && CheckforCrossMAS(subMatrix)){
                        exes ++;
                        // Console.WriteLine($"SubMatrix {i} - {j} - {CheckforCrossMAS(subMatrix)} matches");
                    }
                }

            }
        }

        static bool CheckforCrossMAS(char[,] subMatrix){
            //reveive here all the 3x3 matrixes with an A in the middle
            if(subMatrix[0,0] == 'M' && subMatrix[2,2] == 'S'){
                if(subMatrix[0,2] == 'S' && subMatrix[2,0] == 'M') return true;
                if(subMatrix[0,2] == 'M' && subMatrix[2,0] == 'S') return true;
            }
            else if(subMatrix[0,0] == 'S' && subMatrix[2,2] == 'M'){
                if(subMatrix[0,2] == 'S' && subMatrix[2,0] == 'M') return true;
                if(subMatrix[0,2] == 'M' && subMatrix[2,0] == 'S') return true;
            }
            return false;
        }
    }
}