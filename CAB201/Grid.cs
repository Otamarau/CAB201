

using System;
using System.Collections.Generic;
using System.Linq;

namespace OOD_Assignment
{

    /// <summary>
    /// Represents a grid with various obstacles and utilities to navigate and manipulate it.
    /// </summary>
    public class Grid
    {
        private readonly Dictionary<Coordinate, char> virtualGrid;
        private readonly int maxBound;
        private readonly Pathfinder pathfinder;

        public Grid(int maxBound)
        {
            virtualGrid = new Dictionary<Coordinate, char>();
            this.maxBound = maxBound;
            pathfinder = new Pathfinder(this);
        }

        public char GetFromGrid(int x, int y)
        {
            if (virtualGrid.TryGetValue(new Coordinate(x, y), out char value))
            {
                return value;
            }
            return '.';  // default to safe if the coordinate isn't in the virtualGrid
        }


        public void SetOnGrid(int x, int y, char value)
        {
            virtualGrid[new Coordinate(x, y)] = value;
        }

        public bool IsValidCoordinate(int x, int y)
        {
            return x >= -maxBound && x <= maxBound && y >= -maxBound && y <= maxBound;
        }

        public void AddGuard(Guard guard)
        {
            int x = guard.X;
            int y = guard.Y;

            if (IsValidCoordinate(x, y))
            {
                // Check if there's already an obstacle at the location
                char? currentVal = GetFromGrid(x, y);
                if (currentVal == null || currentVal == '.')
                {
                    SetOnGrid(x, y, 'g');
                }
                else
                {
                    Console.WriteLine("There's already an obstacle at the given coordinates!");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }

        public void AddFence(Fence fence)
        {
            if (IsValidCoordinate(fence.StartX, fence.StartY) && IsValidCoordinate(fence.EndX, fence.EndY))
            {
                if (fence.StartX == fence.EndX) // Vertical Fence
                {
                    for (int j = Math.Min(fence.StartY, fence.EndY); j <= Math.Max(fence.StartY, fence.EndY); j++)
                    {
                        SetOnGrid(fence.StartX, j, 'f');
                    }
                }
                else if (fence.StartY == fence.EndY) // Horizontal Fence
                {
                    for (int i = Math.Min(fence.StartX, fence.EndX); i <= Math.Max(fence.StartX, fence.EndX); i++)
                    {
                        SetOnGrid(i, fence.StartY, 'f');
                    }
                }
                else
                {
                    // Handle non-straight fences, 
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }

        public void AddSensor(Sensor sensor)
        {
            int x = sensor.X;
            int y = sensor.Y;
            double range = sensor.Range;

            if (IsValidCoordinate(x, y))
            {
                SetOnGrid(x, y, 's');

                // Only loop over the potential circle area
                for (int i = (int)Math.Floor(x - range); i <= (int)Math.Ceiling(x + range); i++)
                {
                    for (int j = (int)Math.Floor(y - range); j <= (int)Math.Ceiling(y + range); j++)
                    {
                        if (IsInCircularRange(i, j, x, y, range))
                        {
                            var currentKey = new OOD_Assignment.Coordinate(i, j);


                            // Check if this point hasn't been marked by another entity or is not outside the bounds
                            if (IsValidCoordinate(i, j) &&
                                (!virtualGrid.ContainsKey(currentKey) ||
                                 virtualGrid[currentKey] == '.' ||
                                 virtualGrid[currentKey] == 'c'))
                            {
                                SetOnGrid(i, j, 's');
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }

        // Helper method to determine if a point (i, j) is within a circular range of center (x, y) and radius 'range'
        private bool IsInCircularRange(int i, int j, int x, int y, double range)
        {
            double distance = Math.Sqrt(Math.Pow((i - x), 2) + Math.Pow((j - y), 2));
            return distance <= range;
        }


        public void AddCamera(Camera camera)
        {
            int x = camera.X;
            int y = camera.Y;
            int visionRange = 1000;  // Camera's VisionRange

            int dx = 0; // Change in x direction based on camera's facing direction
            int dy = 0; // Change in y direction based on camera's facing direction

            switch (camera.Facing)
            {
                case 'n': dy = -1; break;
                case 'e': dx = 1; break;
                case 's': dy = 1; break;
                case 'w': dx = -1; break;
            }

            for (int i = 1; i <= visionRange; i++)
            {
                int xPos = x + i * dx;
                int yPos = y + i * dy;

                if (IsValidCoordinate(xPos, yPos)) 
                {
                    SetOnGrid(xPos, yPos, 'c');
                }

                for (int j = 1; j <= i; j++)
                {
                    int sideX1 = xPos - j * dy;
                    int sideY1 = yPos + j * dx;
                    if (IsValidCoordinate(sideX1, sideY1)) 
                    {
                        SetOnGrid(sideX1, sideY1, 'c');
                    }

                    int sideX2 = xPos + j * dy;
                    int sideY2 = yPos - j * dx;
                    if (IsValidCoordinate(sideX2, sideY2)) 
                    {
                        SetOnGrid(sideX2, sideY2, 'c');
                    }
                }
            }

            SetOnGrid(x, y, 'c');
        }



        public string DisplayMap(int topLeftX, int topLeftY, int bottomRightX, int bottomRightY)
        {
            string windowedGrid = "";
            for (int i = topLeftY; i <= bottomRightY; i++)
            {
                for (int b = topLeftX; b <= bottomRightX; b++)
                {
                    char? cellValue = GetFromGrid(b, i);
                    windowedGrid += cellValue.HasValue ? cellValue.Value : '.';
                }
                windowedGrid += "\n";
            }
            return windowedGrid;
        }


        public void ShowSafeDirections()
        {
            Console.WriteLine("Enter your current location (X,Y):");

            string[] input = Console.ReadLine().Split(',');

            if (input.Length != 2 || !int.TryParse(input[0], out int x) || !int.TryParse(input[1], out int y))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            if (!IsValidCoordinate(x, y))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            if (IsOntop(x, y))
            {
                Console.WriteLine("Agent, your location is compromised.");
               
            }

            if (IsSurroundedByObstacles(x, y)) {
                Console.WriteLine("Agent, your location is compromised.");
            }

            string checkDir = "";

            if (CheckDirection(x, y, 'N')) checkDir += 'N';
            if (CheckDirection(x, y, 'S')) checkDir += 'S';
            if (CheckDirection(x, y, 'E')) checkDir += 'E';
            if (CheckDirection(x, y, 'W')) checkDir += 'W';

            if (string.IsNullOrEmpty(checkDir))
            {
                Console.WriteLine("You cannot safely move in any direction. Abort mission.");
                return;
            }

            Console.WriteLine("You can safely take any of the following directions: " + checkDir);
        }

        public bool CheckDirection(int x, int y, char dir)
        {
            int deltaX = 0, deltaY = 0;

            switch (dir)
            {
                case 'N':
                    deltaY = -1;
                    break;
                case 'S':
                    deltaY = 1;
                    break;
                case 'E':
                    deltaX = 1;
                    break;
                case 'W':
                    deltaX = -1;
                    break;
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }

            // Check just the next position, not all the way to the edge
            x += deltaX;
            y += deltaY;

            if (!IsValidCoordinate(x, y) || GetFromGrid(x, y) != '.')
            {
                return false;
            }

            return true;
        }


    
        public bool IsOntop(int x, int y)
        {
            return GetFromGrid(x, y) != '.'; 
        }


        public bool IsSurroundedByObstacles(int x, int y)
        {
            if (GetFromGrid(x + 1, y) != '.' &&
                GetFromGrid(x - 1, y) != '.' &&
                GetFromGrid(x, y + 1) != '.' &&
                GetFromGrid(x, y - 1) != '.')
            {
                return true;
            }
            return false;
        }

        public string CalculatePath(int agentX, int agentY, int objectiveX, int objectiveY)
        {
            return pathfinder.FindPath(agentX, agentY, objectiveX, objectiveY);
        }

    }
}




