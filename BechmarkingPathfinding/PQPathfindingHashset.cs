namespace BechmarkingPathfinding.PathFinding
{
    public class PQPathfindingHashset
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        public PriorityQueue<PathNode, int> OpenListQueue { get; set; } = new();

        public Grid<PathNode> Grid { get; }
        private HashSet<PathNode> closedList = [];

        public PQPathfindingHashset(int width, int height)
        {
            Grid = new(width, height, 10, (grid, x, y) => new PathNode(x, y));

            for (int x = 0; x < Grid.Width; x++)
            {
                for (int y = 0; y < Grid.Height; y++)
                {
                    Grid[x, y].neighbours = GetNeighbourList(Grid[x, y]);
                }
            }
        }

        public List<PathNode>? FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = Grid[startX, startY];
            PathNode endNode = Grid[endX, endY];

            OpenListQueue = new();
            closedList = new();

            for (int x = 0; x < Grid.Width; x++)
            {
                for (int y = 0; y < Grid.Height; y++)
                {
                    //Make a more effiecient way to change these - every time Grid[x, y] is accessed it goes through 4 checks (ifs)
                    PathNode pathNode = Grid[x, y];
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.cameFromNode = null;
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

                        OpenListQueue.Enqueue(neighbourNode, neighbourNode.fCost);
                    }
                }
            }

            //Couldn't find a path
            return null;
        }

        private List<PathNode> GetNeighbourList(PathNode node)
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

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new() { endNode };
            var currentNode = endNode;
            while (currentNode.cameFromNode != null)
            {
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }
            path.Reverse();
            return path;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            float xDistance = MathF.Abs(a.x - b.x);
            float yDistance = MathF.Abs(a.y - b.y);
            float remaining = MathF.Abs(xDistance - yDistance);
            return (int)(MOVE_DIAGONAL_COST * MathF.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining);
        }
    }
}