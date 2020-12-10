using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
namespace AdventOfCode2020 
{
    public static class CypherValidator 
    {
        public static void FindWeakness(string cypherFile)
        {
            long target = FindInvalidNumber(cypherFile);
            string [] lines = File.ReadLines(cypherFile).ToArray();
            long [] vals = lines.Select(line => long.Parse(line)).ToArray();
            for(int startIndex = 0; startIndex < vals.Length; startIndex++){
                bool found = true;
                long sum = 0;
                for(int i = startIndex; i < vals.Length; i++){
                    sum += vals[i];
                    if(sum > target){
                        found = false;
                        break;
                    } else if (sum == target){
                        long [] series = new long[i + 1 - startIndex];
                        Array.Copy(vals, startIndex, series, 0, i + 1 - startIndex);
                        PrintMinMax(series);
                        break;
                    }
                }
                if(found) break;
            }
        }

        private static void PrintMinMax(long [] vals)
        {
            long min = long.MaxValue;
            long max = long.MinValue;
            long sum = 0;
            foreach(long val in vals){
                sum += val;
                if (val < min) min = val;
                if (val > max) max = val;
                Console.WriteLine(val);
            }
            Console.WriteLine("Min: " + min + ", Max: " + max);
            Console.WriteLine("Combined: " + (min + max));
        }

        public static long FindInvalidNumber(string cypherFile)
        {
            Queue<long> queue = new Queue<long>();
            foreach(string line in File.ReadLines(cypherFile))
            {
                long val = long.Parse(line);
                if(queue.Count == 25){
                    bool valid = false;
                    foreach(long a in queue){
                        foreach(long b in queue){
                            if(a != b && a + b == val){
                                valid = true;
                                break;
                            }
                        }
                        if(valid) break;
                    }
                    if(!valid) {
                        Console.WriteLine(val + " does not have the required property.");
                        return val;
                    } else {
                        queue.Dequeue();
                        queue.Enqueue(val);
                    }
                } else {
                    queue.Enqueue(val);
                }
            }
            Console.WriteLine("No invalid number found");
            return -1;
        }
    }
}