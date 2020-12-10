using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
namespace AdventOfCode2020 
{
    public static class GamesConsoleBootChecker
    {
        public static void CheckBootProgram(string bootProgramFile)
        {
            string [] lines = File.ReadLines(bootProgramFile).ToArray();
            long accumulator = 0;
            int index = 0;

            List<int> visitedInstructions = new List<int>();
            while(index < lines.Length)
            {
                if(visitedInstructions.Contains(index)){
                    Console.WriteLine("Reached start of loop. Accumulator value: " + accumulator);
                    break;
                } else {
                    visitedInstructions.Add(index);
                }
                string instruction = lines[index].Split()[0].Trim();
                switch(instruction){
                    case "nop":
                        index++;
                        break;
                    case "acc":
                        int val = int.Parse(lines[index].Split()[1].Trim());
                        accumulator += val;
                        index++;
                        break;
                    case "jmp":
                        int offset = int.Parse(lines[index].Split()[1].Trim());
                        index += offset;
                        break;
                }
            }
        }

        public static void FixBootProgram(string bootProgramFile){
            int fixLine = 0;
            long accumulator = 0;
            string [] lines = File.ReadLines(bootProgramFile).ToArray();

            while(true){

                while(lines[fixLine].Split()[0].Trim() == "acc"){
                    fixLine++;
                }
                Console.WriteLine("Checking fix on line: " + fixLine);

                accumulator = 0;
                int index = 0;
                bool errored = false;

                List<int> visitedInstructions = new List<int>();
                while(index < lines.Length)
                {
                    if(visitedInstructions.Contains(index)){
                        Console.WriteLine("Reached start of an infinite loop. Accumulator value: " + accumulator);
                        errored = true;
                        fixLine++;
                        break;
                    } else {
                        visitedInstructions.Add(index);
                    }
                    string instruction = lines[index].Split()[0].Trim();

                    if(index == fixLine){
                        if(instruction == "nop") instruction = "jmp";
                        else if (instruction == "jmp") instruction = "nop";
                    }

                    switch(instruction){
                        case "nop":
                            index++;
                            break;
                        case "acc":
                            int val = int.Parse(lines[index].Split()[1].Trim());
                            accumulator += val;
                            index++;
                            break;
                        case "jmp":
                            int offset = int.Parse(lines[index].Split()[1].Trim());
                            index += offset;
                            break;
                    }
                } 
                if(!errored)
                    break;               
            }
            Console.WriteLine("Reached end. Accumulator value: " + accumulator);

        }
    }
}