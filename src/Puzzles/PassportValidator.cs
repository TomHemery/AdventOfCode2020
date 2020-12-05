using System.Text.RegularExpressions;
using System.Text;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020{
    public static class PassportValidator{

        static string [] VALID_EYE_COLOURS = new string []{
            "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
        };
        public static void ValidatePassports(string passportsFile){
            List<Passport> allPassports = new List<Passport>();
            StringBuilder sb = new StringBuilder();
            int numValid = 0;
            foreach(string line in File.ReadLines(passportsFile)){
                if(String.IsNullOrWhiteSpace(line)){
                    Passport passport = new Passport(sb.ToString());
                    if(passport.IsValid()){
                        numValid ++;
                    }
                    allPassports.Add(passport);
                    sb.Clear();
                } else {
                    sb.AppendLine(line);
                }
            }
            if(sb.Length != 0){
                Passport passport = new Passport(sb.ToString());
                if(passport.IsValid()){
                    numValid ++;
                }
                allPassports.Add(passport);
                sb.Clear();
            }
            Console.WriteLine("Number of valid passports: " + numValid);
        }

        public class Passport{
            public Dictionary<string, string> contents = new Dictionary<string, string>(){
                {"byr", ""},
                {"iyr", ""},
                {"eyr", ""},
                {"hgt", ""},
                {"hcl", ""},
                {"ecl", ""},
                {"pid", ""},
            };

            public Passport(string stringRep){
                string [] pairs = stringRep.Split();
                Console.WriteLine("======");
                foreach(string pair in pairs){
                    if(pair != "" && !String.IsNullOrWhiteSpace(pair)){
                        Console.WriteLine(pair);
                        string key = pair.Split(':')[0];
                        string val = pair.Split(':')[1];
                        contents[key] = val.Trim();
                    }
                }
                Console.WriteLine("======");
            }

            public bool IsValid(){
                foreach(KeyValuePair<string, string> kvp in contents){
                    int minYear = int.MinValue;
                    int maxYear = int.MaxValue;
                    if(kvp.Value == "" || String.IsNullOrEmpty(kvp.Value))
                        return false;
                    switch(kvp.Key){
                        case "byr":
                            minYear = 1920;
                            maxYear = 2002;
                            goto default;
                        case "iyr":
                            minYear = 2010;
                            maxYear = 2020;
                            goto default;
                        case "eyr":
                            minYear = 2020;
                            maxYear = 2030;
                            goto default;
                        case "hgt":
                            string pattern = @"([0-9]*)(cm|in)";
                            Match m = Regex.Match(kvp.Value, pattern);
                            if(m.Success){
                                if(int.TryParse(m.Groups[1].Value, out int height)){
                                    if(m.Groups[2].Value == "cm" && (height < 150 || height > 193)){
                                        return false;
                                    } else if (m.Groups[2].Value == "in" && (height < 59 || height > 76)){
                                        return false;
                                    }
                                } else {
                                    return false;
                                }
                            } else {
                                return false;
                            }
                            break;
                        case "hcl":
                            if(kvp.Value[0] != '#' || kvp.Value.Length != 7)
                                return false;
                            string hex = kvp.Value.Substring(1);
                            try{
                                int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                            } catch(FormatException) {
                                return false;
                            }
                            break;
                        case "ecl": 
                            if(!VALID_EYE_COLOURS.Contains(kvp.Value)){
                                return false;
                            }
                            break;
                        case "pid":
                            if(kvp.Value.Length != 9)
                                return false;
                            if(int.TryParse(kvp.Value, out int num)){
                                if(num < 0)
                                    return false;
                            } else {
                                return false;
                            }
                            break;
                        case "cid":
                            break;
                        default: // Validates year
                            int year = 0;
                            if(int.TryParse(kvp.Value, out year)){
                                if(year < minYear || year > maxYear){
                                    return false;
                                }
                            } else {
                                return false;
                            }
                            break;
                    }
                }
                return true;
            }
        }

    }
}