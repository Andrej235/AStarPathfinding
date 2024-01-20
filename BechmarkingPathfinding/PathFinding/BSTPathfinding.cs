using BechmarkingPathfinding.BST;

namespace BechmarkingPathfinding.PathFinding
{
    public class BSTPathfinding
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        public Grid<PathNode> Grid { get; }
        private List<PathNode> openList = [];
        private List<PathNode> closedList = [];

        public TreePathNode? bstRoot;

        public BSTPathfinding(int width, int height)
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

            openList = new() { startNode };
            closedList = new();

            for (int x = 0; x < Grid.Width; x++)
            {
                for (int y = 0; y < Grid.Height; y++)
                {
                    PathNode pathNode = Grid[x, y];
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.cameFromNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();
            BinarySearchTreePathNode.Insert(bstRoot, startNode);

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);
                if (currentNode == endNode)
                    return CalculatePath(endNode);

                openList.Remove(currentNode);
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
                        BinarySearchTreePathNode.Insert(bstRoot, neighbourNode);

                        if (!openList.Contains(neighbourNode))
                            openList.Add(neighbourNode);
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

        private PathNode GetLowestFCostNode(List<PathNode> pathNodes)
        {
            PathNode lowestFCostNode = pathNodes[0];
            for (int i = 1; i < pathNodes.Count; i++)
            {
                if (pathNodes[i].FCost < lowestFCostNode.FCost)
                    lowestFCostNode = pathNodes[i];
            }
            return lowestFCostNode;
        }
    }
}