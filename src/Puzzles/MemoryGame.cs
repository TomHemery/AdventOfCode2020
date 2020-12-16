using System;
using System.Collections.Generic;
namespace AdventOfCode2020
{
    public static class MemoryGame
    {
        static int [] startingNumbers = {1, 12, 0, 20, 8, 16};

        public static void Part1()
        {
            List<int> spoken = new List<int>();
            for(int i = 0; i < 2020; i++)
            {
                if(i < startingNumbers.Length)
                {
                    spoken.Add(startingNumbers[i]);
                }
                else
                {
                    spoken.Add(GetNextNumber(i - 1, spoken));
                }
                Console.Write(spoken[i] + ", ");
            }

            Console.WriteLine();
            Console.WriteLine(spoken[spoken.Count - 1]);
        }

        public static void Part2()
        {
            Dictionary<int, int> spoken = new Dictionary<int, int>();
            int prev = -1;
            for(int i = 0; i < 30000000; i++)
            {
                if(i < startingNumbers.Length)
                {
                    spoken[startingNumbers[i]] = i;
                    prev = startingNumbers[i];
                }
                else
                {
                    int newNum = GetNextNumber(prev, i - 1, spoken);
                    spoken[prev] = i-1;
                    prev = newNum;
                }
            }
            Console.WriteLine(prev);

        }

        private static int GetNextNumber(int curr, int index, Dictionary<int, int> prev)
        {
            if(prev.ContainsKey(curr))
            {
                return index - prev[curr];
            }
            return 0;
        }

        private static int GetNextNumber(int index, List<int> vals)
        {
            for(int i = index - 1; i >= 0; i--)
            {
                if(vals[i] == vals[index])
                {
                    return index - i;
                }
            }
            return 0;
        }
    }
}