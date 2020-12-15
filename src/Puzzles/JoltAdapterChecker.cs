using System.Diagnostics;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
namespace AdventOfCode2020
{
    public static class JoltAdapterChecker
    {
        public static void CheckAdapters(string adapterFile)
        {
            int [] vals = GetOrderedValues(adapterFile);
                
            int oneDiffs = 0;
            int threeDiffs = 0;

            for(int i = 0; i <= vals.Length; i++)
            {
                int diff = i == 0 ? vals[i] : +
                    i == vals.Length ? 3 : vals[i] - vals[i-1];

                if(i < vals.Length) Console.WriteLine(vals[i]);

                if(diff == 1) oneDiffs ++;
                else if(diff == 3) threeDiffs ++;
            }

            Console.WriteLine("One: " + oneDiffs + " Three: " + threeDiffs + " Mult: " + (oneDiffs * threeDiffs));
        }

        public static void CountArrangements(string adapterFile)
        {
            var timer = new Stopwatch();

            timer.Start();
            int [] vals = GetOrderedValues(adapterFile);
            long count = CountLeaves(-1, vals, new Dictionary<int, long>());
            timer.Stop();

            Console.WriteLine("Leaf node count: " + count);
            Console.WriteLine("Elapsed: " + timer.ElapsedMilliseconds);
        }

        private static long CountLeaves(int curr, int [] vals, Dictionary<int, long> validPaths)
        {
            if(curr == vals.Length - 1){
                return 1;
            }
            else if(validPaths.ContainsKey(curr)) return validPaths[curr];

            long total = 0;
            for(int i = curr + 1; i < vals.Length; i++){
                int diff = curr >= 0 ? vals[i] - vals[curr] : vals[i];
                if(diff <= 3){
                    total += CountLeaves(i, vals, validPaths);
                } else {
                    break;
                }
            }
            validPaths[curr] = total;
            return total;
        }

        private static int [] GetOrderedValues(string adapterFile){
            return File.ReadLines(adapterFile)
                    .Select(line => int.Parse(line))
                    .OrderBy(val => val)
                    .ToArray();
        }
    }
}