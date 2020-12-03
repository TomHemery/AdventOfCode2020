using System;
using System.IO;
namespace AdventOfCode2020{
    static class PasswordValidator
    {
        public static void ValidatePartOne(string pwdFile) 
        {
            StreamReader sr = File.OpenText(pwdFile);
            string [] line;
            int numValid = 0;
            int total = 0;
            while((line = sr.ReadLine()?.Split()) != null) 
            {
                int min = int.Parse(line[0].Split('-')[0]);
                int max = int.Parse(line[0].Split('-')[1]);
                char targetChar = char.Parse(line[1].TrimEnd(':'));
                int count = 0;
                foreach(char c in line[2]){
                    if(c == targetChar){
                        count++;
                    }
                }
                if(min <= count && count <= max) {
                    Console.WriteLine(line[2]);
                    numValid ++;
                }
                total++;
            }
            Console.WriteLine("Num valid: " + numValid + " / " + total);
        }

        public static void ValidatePartTwo(string pwdFile) 
        {
            StreamReader sr = File.OpenText(pwdFile);
            string [] line;
            int numValid = 0;
            int total = 0;
            while((line = sr.ReadLine()?.Split()) != null) 
            {
                int index0 = int.Parse(line[0].Split('-')[0]) - 1;
                int index1 = int.Parse(line[0].Split('-')[1]) - 1;
                char targetChar = char.Parse(line[1].TrimEnd(':'));
                if(line[2][index0] == targetChar ^ line[2][index1] == targetChar) {
                    Console.WriteLine(line[2]);
                    numValid ++;
                }
                total++;
            }
            Console.WriteLine("Num valid: " + numValid + " / " + total);
        }
    }
}