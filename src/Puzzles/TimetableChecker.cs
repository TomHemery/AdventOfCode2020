using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
namespace AdventOfCode2020
{
    public static class TimetableChecker
    {
        public static void CheckTimetable(string timetable)
        {
            string [] lines = File.ReadLines(timetable).ToArray();
            int timestamp = int.Parse(lines[0]);
            List<int> services = new List<int>();

            foreach(string s in lines[1].Split(','))
            {
                if(s != "x")
                {
                    services.Add(int.Parse(s));
                }
            }

            int bestTime = int.MaxValue;
            int bestService = -1;

            foreach(int service in services)
            {
                int time = service;
                while (time < timestamp) {
                    time += service;
                }
                if(bestTime - timestamp > time - timestamp){
                    bestTime = time;
                    bestService = service;
                }
            }

            Console.WriteLine("Timestamp: {0}", timestamp);
            Console.WriteLine("Best service: {0}, Best time: {1}", bestService, bestTime);
            Console.WriteLine("Wait time: {0}, Wait time X ID: {1}", bestTime - timestamp, bestService * (bestTime - timestamp));
        }

        public static void FindSequentialTimestamp(string timetable)
        {
            string [] lines = File.ReadLines(timetable).ToArray();
            List<ServiceOffsetPair> services = new List<ServiceOffsetPair>();

            uint i = 0;
            foreach(string s in lines[1].Split(','))
            {
                if(s != "x")
                {
                    ServiceOffsetPair pair = new ServiceOffsetPair{
                        id = uint.Parse(s),
                        offset = i
                    };
                    services.Add(pair);

                    Console.WriteLine("Service {0}, offset {1}", pair.id, pair.offset);
                }
                i++;
            }   

            ulong step = services[0].id;
            ulong timestamp = services[0].id;

            foreach(ServiceOffsetPair pair in services.Skip(1)){
                while((timestamp + pair.offset) % pair.id != 0){
                    timestamp = checked(timestamp + step);
                }
                Console.WriteLine("Timestamp {0} is valid for service {1}", timestamp, pair.id);
                step = step * pair.id;
            }

            Console.WriteLine("First valid timestamp: " + timestamp);
        }

        private struct ServiceOffsetPair{
            public uint id;
            public uint offset;
        }
    }
}