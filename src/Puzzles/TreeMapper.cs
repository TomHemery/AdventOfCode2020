using System.Xml;
using System;
using System.Linq;
using System.IO;

namespace AdventOfCode2020{
    public static class TreeMapper
    {
        public static void MapTrees(string treeMapFile)
        {
            string [] lines = File.ReadLines(treeMapFile).ToArray();

            int [][] slopes = new int[5][] {
                new int[2] {1, 1},
                new int[2] {3, 1},
                new int[2] {5, 1},
                new int[2] {7, 1},
                new int[2] {1, 2}
            };

            long treesMult = 1;
            foreach(int[] slope in slopes){
                int trees = 0;
                int x = 0;
                for(int y = 0; y < lines.Length; y += slope[1])
                {
                    if(lines[y][x] == '#') {
                        trees++;
                    } 
                    for(int i = 0; i < slope[0]; i++){
                        x = x + 1 == lines[y].Length ? 0 : x + 1;
                    }
                }
                Console.WriteLine("Slope: " + slope[0] + ", " + slope[1] + ". Trees: " + trees);
                treesMult = treesMult * trees;
            }
            Console.WriteLine("Trees mult: " + treesMult);
        }
    }
}
