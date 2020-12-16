using System.Reflection;
using System.Text;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
namespace AdventOfCode2020
{
    public static class TicketScanner
    {
        public static void ScanTickets(string ticketFile)
        {
            string [] lines = File.ReadLines(ticketFile).ToArray();
            long totalInvalid = 0;
            int i = 0;
            List<Field> allFields = new List<Field>();
            List<int []> validTickets = new List<int[]>();

            // parse fields
            while(!string.IsNullOrWhiteSpace(lines[i]))
            {
                string [] ranges = lines[i].Split(": ")[1].Trim().Split(" or ");
                Field newField = new Field();
                newField.name = lines[i].Split(": ")[0].Trim();
                newField.lowMin = int.Parse(ranges[0].Split('-')[0]);
                newField.lowMax = int.Parse(ranges[0].Split('-')[1]);

                newField.highMin = int.Parse(ranges[1].Split('-')[0]);
                newField.highMax = int.Parse(ranges[1].Split('-')[1]);
                allFields.Add(newField);

                i++;
            }
            List<int> indices = Enumerable.Range(0, allFields.Count).ToList();
            foreach(Field field in allFields)
            {
                field.potentialIndices = new List<int>(indices);
            }

            // Your ticket is always valid
            while(lines[i].Trim() != "nearby tickets:")
            {
                if(lines[i].Split(',').Length > 1)
                {
                    validTickets.Add(lines[i].Split(',').Select(int.Parse).ToArray());
                }
                i++;
            }

            // Test every other ticket
            i++;
            for(; i < lines.Length; i++)
            {
                int [] vals = lines[i].Split(',').Select(int.Parse).ToArray();
                bool validTicket = true;
                foreach(int val in vals)
                {
                    bool valid = false;
                    foreach(Field field in allFields)
                    {
                        valid = field.isValid(val);
                        if(valid)
                            break;
                    }

                    if(!valid){
                        totalInvalid += val;
                        validTicket = false;
                    }
                }
                if(validTicket)
                    validTickets.Add(vals);
            }

            // Part 1
            Console.WriteLine("Error rate: {0}", totalInvalid);

            // Remove invalid indices
            foreach(int [] ticket in validTickets)
            {
                for(i = 0; i < ticket.Length; i++){
                    int val = ticket[i];
                    foreach(Field field in allFields)
                    {
                        if(!field.isValid(val)){
                            if(field.potentialIndices.Contains(i))
                            {
                                field.potentialIndices.Remove(i);
                            }
                        }
                    }
                }
            }

            // O(n.. squared?) systematically get all the fields down to one possible index
            List<Field> setFields = new List<Field>();

            while(allFields.Count > 0){
                for(int j = allFields.Count - 1; j >= 0; j--)
                {
                    Field field = allFields[j];
                    if(field.potentialIndices.Count == 1){
                        foreach(Field other in allFields){
                            if(other != field){
                                other.potentialIndices.Remove(field.potentialIndices[0]);
                            }
                        }
                        setFields.Add(field);
                        allFields.Remove(field);
                    }
                }
            }

            // get all the departure esque indices
            List<int> departureIndices = new List<int>();
            foreach(Field field in setFields)
            {
                Console.WriteLine(field);
                if(field.name.StartsWith("departure"))
                {
                    departureIndices.Add(field.potentialIndices[0]);
                }
            }

            //work out the total of multiplying them all
            long total = 1;
            foreach(int index in departureIndices)
            {
                total *= validTickets[0][index];
            }
            Console.WriteLine("Multiplying all departure values on your ticket gives: {0}", total);
        }

        private class Field
        {
            public string name = "";
            public int lowMin;
            public int lowMax;

            public int highMin;
            public int highMax;

            public bool isValid(int value)
            {
                return value >= lowMin && value <= lowMax
                    || value >= highMin && value <= highMax;
            }

            public List<int> potentialIndices;

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                foreach(int index in potentialIndices){
                    sb.Append(index);
                    sb.Append(", ");
                }
                return $"\"{name}\": {lowMin} - {lowMax}; {highMin} - {highMax} [{(potentialIndices.Count == 1 ? potentialIndices[0] : sb.ToString())}]";
            }
        }
    }
}