using System.Data.SqlTypes;
using System.Text;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
namespace AdventOfCode2020
{
    public static class MemoryInitializer
    {
        public static void RunProgram(string programFile)
        {
            Dictionary<int, ulong> memory = new Dictionary<int, ulong>();
            string mask = "";
            Regex memRegex = new Regex("mem\\[([0-9]*)\\]");
            foreach(string line in File.ReadLines(programFile))
            {
                string instruction = line.Split(" = ")[0];
                string value = line.Split(" = ")[1];

                if(instruction == "mask")
                {
                    mask = value;
                }
                else
                {
                    int temp = int.Parse(value);
                    value = Convert.ToString(temp, 2);

                    //add leading 0s
                    int offset = mask.Length - value.Length;
                    StringBuilder sb = new StringBuilder();
                    for(int i = 0; i < offset; i++){
                        sb.Append("0");
                    }
                    sb.Append(value);
                    value = sb.ToString();

                    Console.WriteLine(value);
                    Console.WriteLine(mask);

                    int addr = int.Parse(memRegex.Match(instruction).Groups[1].Value);
                    char[] valueArray = value.Trim().ToCharArray();
                    for(int i = 0; i < valueArray.Length; i++)
                    {
                        valueArray[i] = mask[i] == 'X' ? valueArray[i] : 
                            mask[i] == '0' ? '0' : '1';
                    }
                    value = new string(valueArray);
                    Console.WriteLine(value);
                    Console.WriteLine();
                    memory[addr] = Convert.ToUInt64(value.Trim(), 2);
                }
            }

            ulong total = 0;
            foreach(ulong value in memory.Values)
            {
                total += value;
            }
            Console.WriteLine("Total in memory: {0}", total);
        }

        public static void RunProgramV2(string programFile)
        {
            Dictionary<ulong, ulong> memory = new Dictionary<ulong, ulong>();
            string mask = "";
            Regex memRegex = new Regex("mem\\[([0-9]*)\\]");
            foreach(string line in File.ReadLines(programFile))
            {
                string instruction = line.Split(" = ")[0];
                string value = line.Split(" = ")[1];

                if(instruction == "mask")
                {
                    mask = value;
                }
                else
                {
                    int temp = int.Parse(memRegex.Match(instruction).Groups[1].Value);
                    string addrString = Convert.ToString(temp, 2);

                    //add leading 0s
                    int offset = mask.Length - addrString.Length;
                    StringBuilder sb = new StringBuilder();
                    for(int i = 0; i < offset; i++){
                        sb.Append("0");
                    }
                    sb.Append(addrString);
                    addrString = sb.ToString();

                    Console.WriteLine(addrString);
                    Console.WriteLine(mask);

                    char[] addrArray = addrString.Trim().ToCharArray();
                    int numFloating = 0;
                    for(int i = 0; i < addrArray.Length; i++)
                    {
                        addrArray[i] = mask[i] == 'X' ? 'X' : 
                            mask[i] == '0' ? addrArray[i] : '1';  
                        if(addrArray[i] == 'X') numFloating ++;                      
                    }
                    Console.WriteLine("Floating values: " + numFloating);
                    for(int i = 0; i < Math.Pow(2, numFloating); i++){ // i = 0 -> 2^floating
                        string vals = Convert.ToString(i, 2); // binary rep 
                        vals = vals.PadLeft(numFloating, '0');
                        Console.WriteLine("Values: " + vals);
                        int valIndex = 0; 
                        char[] finalAddr = new char[addrArray.Length];
                        for(int j = 0; j < finalAddr.Length; j++){
                            finalAddr[j] = addrArray[j] != 'X' ? addrArray[j] : 
                                vals[valIndex ++];
                        }
                        memory[Convert.ToUInt64(new string(finalAddr).Trim(), 2)] = Convert.ToUInt64(value.Trim());
                    }
                }
            }

            ulong total = 0;
            foreach(ulong value in memory.Values)
            {
                total += value;
            }
            Console.WriteLine("Total in memory: {0}", total);
        }
    }
}