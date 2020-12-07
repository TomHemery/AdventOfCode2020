using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
namespace AdventOfCode2020{
    public static class LuggageChecker
    {
        public static void CheckLuggage(string luggageFile)
        {
            Dictionary<string, Bag> bags = ParseBags(luggageFile);
            int total = 0;
            Bag bagOfInterest = bags["shiny gold"];
            foreach(Bag b in bags.Values){
                if(b.canContain(bagOfInterest)){
                    Console.WriteLine("Bag: " + b.name + " can contain shiny gold");
                    total++;
                }
            }
            Console.WriteLine("Total: " + total);

            Console.WriteLine("A shiny gold bag contains: " + bagOfInterest.countContents() + " bags");
        }

        private static Dictionary<string, Bag> ParseBags(string luggageFile){
            Dictionary<string, Bag> allBags = new Dictionary<string, Bag>();

            foreach (string rule in File.ReadLines(luggageFile)) {
                string bagName = (rule.Split()[0] + " " + rule.Split()[1]).Trim();
                Bag newBag = null;
                if(allBags.ContainsKey(bagName)){
                    newBag = allBags[bagName];
                } else {
                    newBag = new Bag(bagName);
                    allBags.Add(bagName, newBag);
                }
                
                string [] contentsRule = rule.Split(" contain ")[1].Split();

                StringBuilder sb = new StringBuilder();
                int count = 0;
                foreach(string symbol in contentsRule){
                    if(symbol.StartsWith("bag")){
                        string childName = sb.ToString().Trim();
                        if(childName != "no other"){
                            if(allBags.ContainsKey(childName)) {
                                newBag.children.Add(allBags[childName], count);
                            } else {
                                Bag childBag = new Bag(childName);
                                newBag.children.Add(childBag, count);
                                allBags.Add(childName, childBag);
                            }
                        }
                        sb.Clear();
                    } else if (int.TryParse(symbol, out int num)){
                        count = num;
                    } else {
                        sb.Append(symbol).Append(" ");
                    }
                }
            }            
            return allBags;             
        }

        private class Bag
        {
            public string name;

            public Dictionary<Bag, int> children = new Dictionary<Bag, int>();

            public Bag(string name){
                this.name = name;
            }

            public bool canContain(Bag b){
                if(children.ContainsKey(b)){
                    return true;
                }
                foreach(Bag child in children.Keys){
                    if(child.canContain(b)){
                        return true;
                    }
                }
                return false;
            }

            public long countContents(){
                long count = 0;
                foreach(KeyValuePair<Bag, int> kvp in children){
                    count += kvp.Value * (kvp.Key.countContents() + 1);
                }
                return count;
            }
        }
    }
}