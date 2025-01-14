using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_Assignment
{
    public class Coordinate : IEquatable<Coordinate>
    {
        public int X { get; }
        public int Y { get; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Coordinate);
        }

        public bool Equals(Coordinate other)
        {
            return other != null && X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            return hash;
        }
    }

    public class Node : IComparable<Node>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Node Parent { get; set; }
        public double G { get; set; } // Cost from start
        public double H { get; set; } // Heuristic (straight-line distance to goal)
        public double F => G + H;

        public int CompareTo(Node other)
        {
            return F.CompareTo(other.F);
        }
    }

    public class Pathfinder
    {
        private readonly Grid grid;

        public Pathfinder(Grid grid)
        {
            this.grid = grid;
        }

        public string FindPath(int agentX, int agentY, int objectiveX, int objectiveY)
        {
            const int MAX_MOVES = 1000;

            var openList = new List<Node>();
            var closedList = new HashSet<Tuple<int, int>>();
            openList.Add(new Node { X = agentX, Y = agentY, G = 0, H = GetHeuristic(agentX, agentY, objectiveX, objectiveY) });

            while (openList.Any())
            {
                var currentNode = openList.OrderBy(node => node.F).First();

                if (currentNode.X == objectiveX && currentNode.Y == objectiveY)
                {
                    var path = "";
                    while (currentNode.Parent != null)
                    {
                        var direction = GetDirectionFromParent(currentNode);
                        path = direction + path;
                        currentNode = currentNode.Parent;
                    }
                    return path;
                }

                openList.Remove(currentNode);
                closedList.Add(new Tuple<int, int>(currentNode.X, currentNode.Y));

                foreach (var neighbour in GetNeighbors(currentNode))
                {
                    if (closedList.Any(closedNode => closedNode.Item1 == neighbour.X && closedNode.Item2 == neighbour.Y))
                        continue;

                    if (CanMove(neighbour.X, neighbour.Y))
                    {
                        if (!openList.Any(openNode => openNode.X == neighbour.X && openNode.Y == neighbour.Y))
                        {
                            openList.Add(neighbour);
                        }
                        else
                        {
                            var existingNode = openList.First(node => node.X == neighbour.X && node.Y == neighbour.Y);
                            if (existingNode.G > neighbour.G)
                            {
                                openList.Remove(existingNode);
                                openList.Add(neighbour);
                            }
                        }
                    }
                }
            }

            return "No path found.";
        }

        private string GetDirectionFromParent(Node currentNode)
        {
            if (currentNode.Parent == null) return "";
            if (currentNode.X == currentNode.Parent.X + 1) return "E";
            if (currentNode.X == currentNode.Parent.X - 1) return "W";
            if (currentNode.Y == currentNode.Parent.Y + 1) return "S";
            if (currentNode.Y == currentNode.Parent.Y - 1) return "N";
            return "";
        }

        private List<Node> GetNeighbors(Node currentNode)
        {
            var neighbors = new List<Node>();
            int x = currentNode.X;
            int y = currentNode.Y;

            // Top neighbor
            neighbors.Add(new Node { X = x, Y = y - 1, Parent = currentNode });
            // Bottom neighbor
            neighbors.Add(new Node { X = x, Y = y + 1, Parent = currentNode });
            // Left neighbor
            neighbors.Add(new Node { X = x - 1, Y = y, Parent = currentNode });
            // Right neighbor
            neighbors.Add(new Node { X = x + 1, Y = y, Parent = currentNode });

            return neighbors;
        }

        private bool CanMove(int x, int y)
        {
            char gridValue = grid.GetFromGrid(x, y);
            return gridValue == '.';
        }

        private double GetHeuristic(int x, int y, int targetX, int targetY)
        {
            return Math.Abs(x - targetX) + Math.Abs(y - targetY);
        }
    }
}
