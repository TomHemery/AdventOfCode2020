using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
namespace AdventOfCode2020 
{
    public static class SeatingSimulator
    {
        public static void Simulate(string seatingFile)
        {
            IEnumerable<string> rows = File.ReadLines(seatingFile);
            char [][] seatArray = new char[rows.Count()][];
            int i = 0;
            foreach(string row in rows){
                seatArray[i] = row.Trim().ToArray();
                i++;
            }

            bool changed = true;
            int updates = 0;
            
            while(changed){
                seatArray = Update(seatArray, out changed);
                updates++;
            }

            Console.WriteLine("Occupied seats: " + CountOccupied(seatArray));
        }

        private static char [][] Update(char [][] seatArray, out bool changed)
        {
            changed = false;
            char [][] next = new char[seatArray.Length][];
            for(int y = 0; y < seatArray.Length; y++){
                next[y] = new char[seatArray[y].Length];
                for(int x = 0; x < seatArray[y].Length; x++){
                    next[y][x] = seatArray[y][x] == '.' ? '.' : GetNextState(x, y, seatArray);
                    if(next[y][x] != seatArray[y][x]) changed = true;
                }
            }
            return next;
        }

        private static char GetNextState(int x, int y, char[][] seatArray)
        {
            int occupiedSeats = 0;
            char currState = seatArray[y][x];

            for(int yDir = -1; yDir <= 1; yDir++){
                for(int xDir = -1; xDir <= 1; xDir++){
                    if(xDir != 0 || yDir != 0){
                        int xPos = x + xDir;
                        int yPos = y + yDir;
                        while(yPos >= 0 && yPos < seatArray.Length && xPos >= 0 && xPos < seatArray[yPos].Length){
                            if(seatArray[yPos][xPos] == '#'){
                                occupiedSeats++;
                                break;
                            } else if(seatArray[yPos][xPos] == 'L'){
                                break;
                            }
                            xPos += xDir;
                            yPos += yDir;
                        }
                    }
                }
            }

            if(currState == 'L' && occupiedSeats == 0) {
                return '#';
            } else if (currState == '#' && occupiedSeats >= 5) {
                return 'L';
            }

            return currState;
        }

        private static int CountOccupied(char [][] seatArray)
        {
            int count = 0;
            foreach(char [] row in seatArray)
            {
                foreach(char seat in row)
                {
                    if(seat == '#') count++;
                }
            }
            return count;
        }

        private static void PrintSeats(char [][] seatArray)
        {
            foreach(char [] row in seatArray)
            {
                foreach(char seat in row)
                {
                    Console.Write(seat);
                }
                Console.WriteLine();
            }
        }
    }
}