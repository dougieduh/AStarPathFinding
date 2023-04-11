using System;
using System.Collections.Generic;

namespace AStarPathfinding
{
    class Program
    {
        static void Main(string[] args)
        {
            char[,] grid = {
                { 'S', '.', '.', '.', '.', '.', '.' },
                { '.', '#', '#', '#', '.', '#', '.' },
                { '.', '.', '.', '.', '.', '#', '.' },
                { '.', '#', '#', '#', '#', '#', '.' },
                { '.', '#', '.', '.', '.', '.', '.' },
                { '.', '#', '#', '#', '#', '#', '.' },
                { '.', '.', '.', '.', '.', '.', 'E' }
            };

            Node start = FindStart(grid);
            Node end = FindEnd(grid);
            List<Node> path = AStar(grid, start, end);

            Console.WriteLine("A* Pathfinding Algorithm");
            PrintGrid(grid, path);
        }

        class Node
        {
            public int X;
            public int Y;
            public int G;
            public int H;
            public Node Parent;

            public Node(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int F()
            {
                return G + H;
            }
        }

        static List<Node> AStar(char[,] grid, Node start, Node end)
        {
            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();
            openList.Add(start);

            while (openList.Count > 0)
            {
                Node current = FindLowestF(openList);

                if (current.X == end.X && current.Y == end.Y)
                {
                    return BuildPath(current);
                }

                openList.Remove(current);
                closedList.Add(current);

                List<Node> neighbors = GetNeighbors(grid, current);

                foreach (Node neighbor in neighbors)
                {
                    if (closedList.Contains(neighbor))
                    {
                        continue;
                    }

                    int tentativeG = current.G + 1;

                    if (!openList.Contains(neighbor) || tentativeG < neighbor.G)
                    {
                        neighbor.Parent = current;
                        neighbor.G = tentativeG;
                        neighbor.H = ManhattanDistance(neighbor, end);

                        if (!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }

        static List<Node> BuildPath(Node current)
        {
            List<Node> path = new List<Node>();

            while (current != null)
            {
                path.Add(current);
                current = current.Parent;
            }

            path.Reverse();
            return path;
        }

        static Node FindLowestF(List<Node> nodes)
        {
            Node lowestFNode = nodes[0];

            foreach (Node node in nodes)
            {
                if (node.F() < lowestFNode.F())
                {
                    lowestFNode = node;
                }
            }

            return lowestFNode;
        }

        static List<Node> GetNeighbors(char[,] grid, Node current)
        {
            List<Node> neighbors = new List<Node>();

            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                int x = current.X + dx[i];
                int y = current.Y + dy[i];

                if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1) && grid[x, y] != '#')
                {
                    Node neighbor = new Node(x, y);
                    neighbors.Add(neighbor);
                }
            }
            return neighbors;
        }

        static int ManhattanDistance(Node start, Node end)
        {
            return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
        }

        static Node FindStart(char[,] grid)
        {
            Node start = null;

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == 'S')
                    {
                        start = new Node(i, j);
                        break;
                    }
                }

                if (start != null)
                {
                    break;
                }
            }

            return start;
        }

        static Node FindEnd(char[,] grid)
        {
            Node end = null;

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == 'E')
                    {
                        end = new Node(i, j);
                        break;
                    }
                }

                if (end != null)
                {
                    break;
                }
            }

            return end;
        }

        static void PrintGrid(char[,] grid, List<Node> path)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (path != null && path.Exists(node => node.X == i && node.Y == j))
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write(grid[i, j]);
                    }
                }

                Console.WriteLine();
            }
        }
    }
}