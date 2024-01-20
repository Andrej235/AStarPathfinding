
namespace BechmarkingPathfinding.PathFinding
{
    public class CustomPQPathfindingHashset
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        public Priority_Queue.GenericPriorityQueue<PathNode, int> OpenListQueue { get; set; }

        public Grid<PathNode> Grid { get; }
        private HashSet<PathNode> closedList = [];

        public CustomPQPathfindingHashset(int width, int height)
        {
            Grid = new(width, height, 10, (grid, x, y) => new PathNode(x, y));
            OpenListQueue = new(width * height);

            List<PathNode> GetNeighbourList(PathNode node)
            {
                List<PathNode> neighbours = new();

                if (node.x - 1 >= 0) //Left
                {
                    neighbours.Add(Grid[node.x - 1, node.y]);

                    if (node.y - 1 >= 0) //Down
                        neighbours.Add(Grid[node.x - 1, node.y - 1]);

                    if (node.y + 1 < Grid.Height) //Up
                        neighbours.Add(Grid[node.x - 1, node.y + 1]);

                }
                if (node.x + 1 < Grid.Width) //Right
                {
                    neighbours.Add(Grid[node.x + 1, node.y]);

                    if (node.y - 1 >= 0) //Down
                        neighbours.Add(Grid[node.x + 1, node.y - 1]);

                    if (node.y + 1 < Grid.Height) //Up
                        neighbours.Add(Grid[node.x + 1, node.y + 1]);
                }

                if (node.y - 1 >= 0) //Down
                    neighbours.Add(Grid[node.x, node.y - 1]);

                if (node.y + 1 < Grid.Height) //Up
                    neighbours.Add(Grid[node.x, node.y + 1]);

                return neighbours;
            }
            for (int x = 0; x < Grid.Width; x++)
                for (int y = 0; y < Grid.Height; y++)
                    Grid[x, y].neighbours = GetNeighbourList(Grid[x, y]);
        }

        public List<PathNode>? FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = Grid[startX, startY];
            PathNode endNode = Grid[endX, endY];

            OpenListQueue.Clear();
            closedList = new();

            for (int x = 0; x < Grid.Width; x++)
            {
                for (int y = 0; y < Grid.Height; y++)
                {
                    PathNode pathNode = Grid[x, y];
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.cameFromNode = null;

                    OpenListQueue.ResetNode(pathNode);
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();
            OpenListQueue.Enqueue(startNode, startNode.fCost);

            while (OpenListQueue.Count > 0)
            {
                PathNode currentNode = OpenListQueue.Dequeue();
                if (currentNode == endNode)
                    return CalculatePath(endNode);

                closedList.Add(currentNode);

                foreach (var neighbourNode in currentNode.neighbours)
                {
                    if (closedList.Contains(neighbourNode))
                        continue;

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode); //CalculateDistanceCost returns 10 or 14
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        if (OpenListQueue.Contains(neighbourNode))
                            OpenListQueue.UpdatePriority(neighbourNode, neighbourNode.fCost);
                        else
                            OpenListQueue.Enqueue(neighbourNode, neighbourNode.fCost);
                    }
                }
            }

            //Couldn't find a path
            return null;
        }

        private static List<PathNode> CalculatePath(PathNode node)
        {
            List<PathNode> path = new() { node };
            while (node.cameFromNode != null)
            {
                path.Add(node.cameFromNode);
                node = node.cameFromNode;
            }
            path.Reverse();
            return path;
        }

        private static int CalculateDistanceCost(PathNode a, PathNode b)
        {
            float xDistance = MathF.Abs(a.x - b.x);
            float yDistance = MathF.Abs(a.y - b.y);
            float remaining = MathF.Abs(xDistance - yDistance);
            return (int)(MOVE_DIAGONAL_COST * MathF.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining);
        }
    }
}