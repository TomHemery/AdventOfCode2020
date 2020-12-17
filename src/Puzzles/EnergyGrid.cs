using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
namespace AdventOfCode2020
{
    public static class EnergyGrid
    {
        public static void Simulate(string startFile)
        {
            Console.WriteLine("Initial state:");
            int y = 0;
            foreach(string line in File.ReadLines(startFile))
            {
                int x = 0;
                foreach(char state in line.Trim())
                {
                    new ConwayCube(x, y, 0, 0, state == '#');
                    x++;
                    Console.Write(state == '#' ? 1 : 0);
                }
                Console.WriteLine();
                y++;
            }
            Console.WriteLine(
                "Active cubes: {0}. Total cubes: {1}", 
                ConwayCube.CountActiveCubes(),
                ConwayCube.CubeDict.Count
            );
            for(int i = 0; i < 6; i++){
                ConwayCube.Update();
                Console.WriteLine(
                    "Active cubes after {0} cycles: {1}. Total cubes: {2}", 
                    i + 1, 
                    ConwayCube.CountActiveCubes(), 
                    ConwayCube.CubeDict.Count
                );
            }
            Console.WriteLine("Active cubes: {0}", ConwayCube.CountActiveCubes());
        }
        

        private class ConwayCube
        {
            public static Dictionary<(int, int, int, int), ConwayCube> CubeDict {get; private set;} = new Dictionary<(int, int, int, int), ConwayCube>();
            int x, y, z, w;
            public (int, int, int, int) Id {get; private set;}
            public bool active;
            public bool nextState;

            public ConwayCube(int x, int y, int z, int w, bool active = false){
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
                this.active = active;
                this.nextState = active;

                this.Id = (x, y, z, w); 

                CubeDict[Id] = this;
            }

            /// <summary>
            /// Populates all missing neighbours in the CubeDict with inactive cubes
            /// </summary>
            public void CreateMissingNeighbours()
            {
                for(int xTest = x - 1; xTest <= x + 1; xTest ++)
                {
                    for(int yTest = y - 1; yTest <= y + 1; yTest ++)
                    {
                        for(int zTest = z - 1; zTest <= z + 1; zTest ++)
                        {
                            for(int wTest = w - 1; wTest <= w + 1; wTest ++){
                            if(!CubeDict.ContainsKey((xTest, yTest, zTest, wTest)))
                                {
                                    new ConwayCube(xTest, yTest, zTest, wTest);
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Sets the next state of this cube based on neighbours
            /// </summary>
            public void SetNextState()
            {
                int activeNeighbours = CountActiveNeighbours();
                nextState = active && activeNeighbours == 2 || activeNeighbours == 3;
            }

            /// <summary>
            /// Counts how many of this cube's 26 neighbours are active
            /// </summary>
            /// <returns>Count of active neighbours</returns>
            public int CountActiveNeighbours()
            {
                int activeNeighbours = 0;
                for(int xTest = x - 1; xTest <= x + 1; xTest ++)
                {
                    for(int yTest = y - 1; yTest <= y + 1; yTest ++)
                    {
                        for(int zTest = z - 1; zTest <= z + 1; zTest ++)
                        {
                            for(int wTest = w - 1; wTest <= w + 1; wTest ++){
                                if(xTest != x || yTest != y || zTest != z || wTest != w){
                                    if(CubeDict.ContainsKey((xTest, yTest, zTest, wTest)))
                                    {
                                        if(CubeDict[(xTest, yTest, zTest, wTest)].active)
                                            activeNeighbours ++;
                                    }
                                }
                            }
                        }
                    }
                }
                return activeNeighbours;
            }

            /// <summary>
            /// Counts how many cubes are active in total across the whole grid
            /// </summary>
            /// <returns>Count of active cubes</returns>
            public static int CountActiveCubes()
            {
                int activeCount = 0;
                foreach(ConwayCube cube in CubeDict.Values){
                    if(cube.active) activeCount++;
                }
                return activeCount;
            }

            /// <summary>
            /// Updates the entire energy grid
            /// </summary>
            public static void Update()
            {
                // Add new cubes that are needed, only cubes neighbouring active cubes have a chance of becoming active
                var currCubes = CubeDict.Values.ToArray();
                foreach(ConwayCube cube in currCubes)
                {
                    if(cube.active) cube.CreateMissingNeighbours();
                }

                // For every cube we are considering set the next state based on neighbours
                foreach(ConwayCube cube in CubeDict.Values)
                {
                    cube.SetNextState();
                }
                
                // Move on to the next state
                foreach(ConwayCube cube in CubeDict.Values)
                {
                    cube.active = cube.nextState;
                }
            }
        }
    }
}