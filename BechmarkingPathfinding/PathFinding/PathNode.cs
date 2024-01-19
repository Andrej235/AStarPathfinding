namespace BechmarkingPathfinding.PathFinding
{
    public class PathNode
    {
        public readonly int x;
        public readonly int y;

        public int gCost;
        public int hCost;
        public int fCost;

        public List<PathNode> neighbours;
        public PathNode? cameFromNode;

        public PathNode(int x, int y)
        {
            this.x = x;
            this.y = y;
            neighbours = new();
        }

        public override string ToString() => $"{x},{y}";

        internal void CalculateFCost() => fCost = gCost + hCost;
    }
}