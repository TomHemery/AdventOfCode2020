using System.Collections.Generic;
using System.IO;
using System;

namespace AdventOfCode2020
{
    public static class BoardingPassChecker
    {
        public static void CheckBoardingPasses(string boardingPassFile)
        {
            List<int> allIds = new List<int>();
            foreach (string line in File.ReadLines(boardingPassFile)) {
                string rowDescriptor = line.Substring(0, 7);
                string colDescriptor = line.Substring(7);
                int low = 0;
                int high = 127;
                foreach (char c in rowDescriptor) {
                    if (c == 'F') {
                        high = (low + high) / 2;
                    } else if (c == 'B') {
                        low = (low + high) / 2 + 1;
                    }
                }
                int row = low;
                
                low = 0;
                high = 7;
                foreach (char c in colDescriptor) {
                    if (c == 'L') {
                        high = (low + high) / 2;
                    } else if (c == 'R') {
                        low = (low + high) / 2 + 1;
                    }
                }
                int col = low;
                int id = row * 8 + col;
                allIds.Add(id);
            }
            allIds.Sort();
            for(int i = 1; i < allIds.Count; i++){
                if(allIds[i] - allIds[i - 1] > 1) {
                    Console.WriteLine(allIds[i - 1] + ", " + allIds[i]);
                    Console.WriteLine("Potential seat: " + (allIds[i - 1] + 1));
                }
            }
        }
    }
}