using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode.Services
{
    public class Day5
    {
        static string projectRootPath = Directory.GetCurrentDirectory();
        static string _inputFilePath = Path.Combine(projectRootPath, "Inputs/day5", "day5.txt");

        static List<int[]> updates = [];
        static List<int[]> validatedUpdates = [];
        static int validSum = 0;
        static Dictionary<int, int[]> rules = [];

        public void RunPart1()
        {
            // Read input to updates list and to int[] list
            ReadFileContent();

            // for each update, check it matches all the rules
            ValidateUpdates();

            Console.WriteLine("Valid sum: " + validSum);
        }

        private void ValidateUpdates(bool fix = false)
        {
            foreach (var update in updates){
                var validUpdate = IsUpdateValid(update);

                if(validUpdate ){
                    // validatedUpdates.Add(update);
                    validSum += update[(update.Length/2)];
                }
                else if(fix){
                    var temp = update;
                    while (!IsUpdateValid(temp = FixUpdate(temp)))
                    {
                    }
                    if(IsUpdateValid(temp))
                        validatedUpdates.Add(temp);
                }
            }
        }

        private int[] FixUpdate(int[] brokenUpdate)
        {
            var updated = brokenUpdate.ToArray();
            // Console.WriteLine("Fixing update: " + string.Join(", ", brokenUpdate));
            
                 for(int i = 0; i < brokenUpdate.Length; i++){
                    if(!rules.ContainsKey(brokenUpdate[i]))
                    { 
                        continue;
                    }
                    else{
                        for(int j = 0; j < i; j++){
                            if(rules[brokenUpdate[i]].Contains(brokenUpdate[j])){
                                updated.SetValue(brokenUpdate[i], j);
                                updated.SetValue(brokenUpdate[j], i);
                                // Console.WriteLine("Fixed update: " + string.Join(", ", updated));
                                return updated;
                            }
                        }  
                    }
                }

            
            // Console.WriteLine("Fixed update: " + string.Join(", ", updated));
            return updated;
        }

        bool IsUpdateValid(int[] update){
            var validUpdate = true;
            for(int i = 0; i < update.Length && validUpdate; i++){
                    if(!rules.ContainsKey(update[i])){ continue;}
                    else
                        for(int j = 0; j < i; j++){
                            if(rules[update[i]].Contains(update[j])){
                                validUpdate = false;
                            }
                        }                                     
                }
            return validUpdate; 
        }

        private void ReadFileContent()
        {
            string[] lines = File.ReadAllLines(_inputFilePath);

            foreach (var line in lines){
                string[] partsRules = line.Split(['|']);
                string[] partsUpdates = line.Split(',');
                if (line.Contains('|')){
                    int key = int.Parse(partsRules[0]);
                    int value = int.Parse(partsRules[1]);

                    if(rules.ContainsKey(key)){
                        rules[key] = [.. rules[key], value];
                    }
                    else{
                        rules.Add(key, [value]);
                    }
                }
                else if ( line.Contains(',')){
                    var update = new int []{};
                    foreach (var number in partsUpdates){
                        update = update.Append(int.Parse(number)).ToArray();
                    }
                    updates.Add(update);
                }
            }

            // Console.WriteLine("Rules: " + rules.Count);
            // Console.WriteLine("Updates: " + updates.Count);

            // foreach (var rule in rules){
            //     Console.WriteLine("Rule: " + rule.Key +  $"({rule.Value.Count()}) - " + string.Join(",", rule.Value));
            // }

            // foreach(var update in updates){
            //     Console.WriteLine("Update: " + string.Join(",", update));
            // }
        }

        public void RunPart2(){
            // Read input to updates list and to int[] list
            updates = [];
            validatedUpdates = [];
            ReadFileContent();
            ValidateUpdates(true);

            var sum = validatedUpdates.Sum(x => x[x.Length/2]);

            Console.WriteLine($"Validated updates: {validatedUpdates.Count}");
            Console.WriteLine("Updates: " + updates.Count);

            Console.WriteLine("Sum part 2: " + sum);
        }
    }
}