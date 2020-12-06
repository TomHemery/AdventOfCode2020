using System.Collections.Generic;
using System;
using System.IO;
namespace AdventOfCode2020
{
    public static class CustomsDeclarationsChecker
    {
        public static void CheckCustomsDeclarations(string customsDecsFile)
        {
            Dictionary<char, int> answers = new Dictionary<char, int>();
            long total = 0;
            int groupSize = 0;
            foreach(string line in File.ReadLines(customsDecsFile)){
                if(String.IsNullOrWhiteSpace(line.Trim())){
                    foreach(int val in answers.Values){
                        if(val == groupSize){
                            total++;
                        }
                    }
                    answers.Clear();
                    groupSize = 0;
                } else {
                    foreach(char c in line.Trim()){
                        answers[c] = answers.ContainsKey(c) ? answers[c] + 1 : 1;
                    }
                    groupSize++;
                }
            }
            if(answers.Count > 0){
                foreach(int val in answers.Values){
                    if(val == groupSize){
                        total++;
                    }
                }
            }
            Console.WriteLine("Total: " + total);
        }
    }
}