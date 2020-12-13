using System;
using System.IO;
namespace AdventOfCode2020 
{
    public class BoatNavigator
    {
        public static void CalcManhattanDist(string navFile)
        {
            WaypointBoat b = new WaypointBoat();
            b.PrintState();
            foreach(string line in File.ReadLines(navFile))
            {
                b.ExecuteInstruction(line);
                b.PrintState();
            }

            Console.WriteLine("Distance: {0}", Math.Abs(b.X) + Math.Abs(b.Y));
        }

        private class Boat
        {
            public int Angle {get; protected set;} = 90; // 90 = EAST
            public int X {get; protected set;} = 0;
            public int Y {get; protected set;} = 0;

            public void ExecuteInstruction(string instruction)
            {
                Console.WriteLine("Executing: " + instruction);
                char dir = instruction[0];
                int val = int.Parse(instruction.Substring(1));

                if(dir == 'L' || dir == 'R')
                {
                    UpdateAngle(dir, val);
                } else {
                    Travel(dir, val);
                }
            }

            public virtual void PrintState(){
                Console.WriteLine("Position: {0},{1} Heading:{2}", X, Y, Angle);
            }

            protected virtual void UpdateAngle(char dir, int rotation)
            {
                Angle = (Angle + rotation * (dir == 'L' ? -1 : 1)) % 360;
                if(Angle < 0) Angle = 360 + Angle;
            }

            protected virtual void Travel(char dir, int units)
            {
                switch(dir){
                    case 'N':
                        Y += units;
                        break;
                    case 'E':
                        X += units;
                        break;
                    case 'S':
                        Y -= units;
                        break;
                    case 'W':
                        X -= units;
                        break;
                    case 'F':
                        TravelForward(units);
                        break;
                }
            }

            protected virtual void TravelForward(int units)
            {
                switch(Angle){
                    case 0: // N
                        Y += units;
                        break;
                    case 90: // E
                        X += units;
                        break;
                    case 180: // S
                        Y -= units;
                        break;
                    case 270: // W
                        X -= units;
                        break;
                }
            }
        }

        private class WaypointBoat : Boat
        {
            public int WaypointX {get; private set;} = 10;
            public int WaypointY {get; private set;} = 1;

            protected override void TravelForward(int units)
            {
                for(int i = 0; i < units; i++)
                {
                    X += WaypointX;
                    Y += WaypointY;
                }
            }

            protected override void Travel(char dir, int units)
            {
                switch(dir){
                    case 'N':
                        WaypointY += units;
                        break;
                    case 'E':
                        WaypointX += units;
                        break;
                    case 'S':
                        WaypointY -= units;
                        break;
                    case 'W':
                        WaypointX -= units;
                        break;
                    case 'F':
                        TravelForward(units);
                        break;
                }
            }

            protected override void UpdateAngle(char dir, int rotation)
            {
                if(rotation == 180)
                {
                    WaypointX = -WaypointX;
                    WaypointY = -WaypointY;
                }
                else if(dir == 'L' && rotation == 90 || dir == 'R' && rotation == 270)
                {
                    int temp = WaypointX;
                    WaypointX = -WaypointY;
                    WaypointY = temp;
                } 
                else if(dir == 'L' && rotation == 270 || dir == 'R' && rotation == 90)
                {
                    int temp = WaypointX;
                    WaypointX = WaypointY;
                    WaypointY = -temp;  
                }
            }

            public override void PrintState()
            {
                Console.WriteLine("Position: {0},{1} Waypoint:{2}, {3}", X, Y, WaypointX, WaypointY);
            }
        }
    }
}