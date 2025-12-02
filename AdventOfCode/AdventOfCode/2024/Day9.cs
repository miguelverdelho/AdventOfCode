using System;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Services
{
    public class Day9
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day9", "day9.txt");

        static string initialInput = string.Empty;
        static string[] parsedInput = [];

        public void RunPart1()
        {
            ReadInput();
            ParseInitialInput();
            SortInputWithMultiDigits();
            CalculateCheckSum(1);
        }

        public void RunPart2(){
           parsedInput =[];
           ParseInitialInput();
           SortInputWithFullFilesTogether();
           CalculateCheckSum(2);
        }

        public void ReadInput(){
            initialInput = File.ReadAllText(_inputFilePath);            
        }

        public void ParseInitialInput(){
            var isFree = false;
            var index = 0;

            for (int i = 0; i < initialInput.Length; i++) { 
                int value = int.Parse(initialInput[i].ToString());
                for (int j = 0; j < value; j++) { 
                    parsedInput = parsedInput.Append(isFree ? "." : index.ToString()).ToArray();
                }
                index = isFree ? index + 1 : index;
                isFree = !isFree;
            }
        }

        private void CalculateCheckSum(int part)
        {
            long result = 0;

            for(int i = 0; i < parsedInput.Length; i++){
                if (parsedInput[i] == ".") continue;
                result += int.Parse(parsedInput[i]) * i;
            }
            
            Console.WriteLine($"Part {part} Checksum: {result}");
        }

        private void SortInputWithMultiDigits(){

            var result = parsedInput;

            for(int i = parsedInput.Length-1; i >= 0; i--){
                if (parsedInput[i] != "."){
                    int newPosition = Array.IndexOf(parsedInput,".");
                    if(newPosition == -1 || newPosition > i) break;
                    result[newPosition] = parsedInput[i];
                    result[i] = ".";
                    parsedInput = result;
                }
            }
        }

        private void SortInputWithFullFilesTogether(){

            for(int i = parsedInput.Length-1; i >= 0; i--){
                if (parsedInput[i] != "."){
                    // count how many inputs with id == parsedInput[i]
                    var nrIds = CountHowManyIds(parsedInput[i]);

                    // look for the first sequence of X . together
                    var firstIndex = GetIndexofDotSequence(nrIds);
                    // Console.WriteLine($"Moving {nrIds} ids {parsedInput[i]} to {firstIndex}");
                    
                    //if no space continue
                    if (firstIndex == -1) continue;
                    
                    // moving to right continue
                    if(firstIndex > i) continue;    

                    // swap
                    string[] temp = new string[nrIds];
                    string[] parsedInputCopy = (string[])parsedInput.Clone();

                    // Copy values from [i-nrIds] to [i] to temp
                    Array.Copy(parsedInputCopy, i - nrIds + 1, temp, 0, nrIds);

                    // Move values from [firstIndex] to [firstIndex + nrIds] to [i-nrIds] to [i]
                    Array.Copy(parsedInputCopy, firstIndex, parsedInput, i - nrIds +1, nrIds);

                    // Move values from temp to [firstIndex] to [firstIndex + nrIds]
                    Array.Copy(temp, 0, parsedInput, firstIndex, nrIds);

                    // Console.WriteLine(string.Join("", parsedInput));
                }
            }            
        }

        private int CountHowManyIds(string id){
            return parsedInput.Where(x => x == id).Count();            
        }

        private int GetIndexofDotSequence(int size){

            int index = -1;
            int count = 0;

            for (int i = 0; i < parsedInput.Length; i++)
            {
                if (parsedInput[i] == ".")
                {
                    count++;
                    if (count == size)
                    {
                        index = i - size + 1;
                        break;
                    }
                }
                else
                {
                    count = 0;
                }
            }

            return index;
        }
    }
}