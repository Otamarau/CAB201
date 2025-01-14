using static System.Net.Mime.MediaTypeNames;
using System;

namespace OOD_Assignment
{
    public class CodeChecker
    {
        private Grid dotGrid;

        public CodeChecker(Grid grid)
        {
            dotGrid = grid;
        }

        public void Check(string code)
        {
            ObstacleManager obstacleManager = new ObstacleManager(dotGrid);

            switch (code)
            {
                case "g":
                    try
                    {
                        Console.WriteLine("Enter the guard's location (X,Y):");
                        string[] coordinates = Console.ReadLine().Split(',');
                        int x = int.Parse(coordinates[0]);
                        int y = int.Parse(coordinates[1]);
                        Guard guard = new Guard(x, y);
                        obstacleManager.InsertGuard(guard);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid input.");
                    }
                    break;
                case "f":
                    try
                    {
                        Console.WriteLine("Enter the location where the fence starts (X,Y):");
                        string[] fenceStart = Console.ReadLine().Split(',');
                        int sX = int.Parse(fenceStart[0]);
                        int sY = int.Parse(fenceStart[1]);

                        Console.WriteLine("Enter the location where the fence ends (X,Y):");
                        string[] fenceEnd = Console.ReadLine().Split(',');
                        int eX = int.Parse(fenceEnd[0]);
                        int eY = int.Parse(fenceEnd[1]);

                        Fence fence = new Fence(sX, sY, eX, eY);
                        obstacleManager.InsertFence(fence);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid input.");
                    }
                    break;
                case "s":
                    try
                    {
                        Console.WriteLine("Enter the sensor's location (X,Y):");
                        string[] sensorLocation = Console.ReadLine().Split(',');
                        int x = int.Parse(sensorLocation[0]);
                        int y = int.Parse(sensorLocation[1]);

                        Console.WriteLine("Enter the sensor's range:");
                        double inputRange = double.Parse(Console.ReadLine());

                        Sensor sensor = new Sensor(x, y, inputRange);
                        obstacleManager.InsertSensor(sensor);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid input.");
                    }
                    break;
                case "c":
                    try
                    {
                        Console.WriteLine("Enter the camera's location (X,Y):");
                        string[] cameraLocation = Console.ReadLine().Split(',');
                        int x = int.Parse(cameraLocation[0]);
                        int y = int.Parse(cameraLocation[1]);

                        Console.WriteLine("Enter the direction the camera is facing (n, s, e, w):");

                        string inputString = Console.ReadLine();
                        char direction = inputString[0];

                        if (direction == 'n' || direction == 's' || direction == 'e' || direction == 'w')
                        {
                            Camera camera = new Camera(x, y, direction);
                            obstacleManager.InsertCamera(camera);
                        }
                        else
                        {
                            Console.WriteLine("Invalid direction.");
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid input.");
                    }
                    break;

                case "d":
                    try
                    {
                        dotGrid.ShowSafeDirections();
                    } catch (Exception) {
                        Check("d");
                    }
                    break;
                case "m":
                    try
                    {
                        Console.WriteLine("Enter the location of the top-left cell of the map (X,Y):");
                        string[] topLeftCoordinates = Console.ReadLine().Split(',');
                        int topLeftX = int.Parse(topLeftCoordinates[0]);
                        int topLeftY = int.Parse(topLeftCoordinates[1]);

                        Console.WriteLine("Enter the location of the bottom-right cell of the map (X,Y):");
                        string[] bottomRightCoordinates = Console.ReadLine().Split(',');
                        int bottomRightX = int.Parse(bottomRightCoordinates[0]);
                        int bottomRightY = int.Parse(bottomRightCoordinates[1]);

                        string windowedGrid = dotGrid.DisplayMap(topLeftX, topLeftY, bottomRightX, bottomRightY);

                        Console.WriteLine(windowedGrid);

                        if (windowedGrid == "Invalid input.")
                        {
                            Check("m");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid input.");
                        Check("m");
                    }
                    break;
                case "p":
                    try
                    {
                        Console.WriteLine("Enter your current location (X,Y):");
                        string[] agentXY = Console.ReadLine().Split(',');
                        int agentX = int.Parse(agentXY[0]);
                        int agentY = int.Parse(agentXY[1]);

                        Console.WriteLine("Enter the location of your objective (X,Y):");
                        string[] objectiveXY = Console.ReadLine().Split(',');
                        int objectiveX = int.Parse(objectiveXY[0]);
                        int objectiveY = int.Parse(objectiveXY[1]);

                        string path = dotGrid.CalculatePath(agentX, agentY, objectiveX, objectiveY);

                        Console.WriteLine("The following path will take you to the objective:");

                        Console.WriteLine(path);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid input.");
                        Check("p");
                    }
                    break;
                case "x":
                    //throw new Exception("x entered");
                    Environment.Exit(0);
                    break; 
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }
}
