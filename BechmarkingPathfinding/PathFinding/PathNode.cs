using Priority_Queue;

namespace BechmarkingPathfinding.PathFinding
{
    public class PathNode : GenericPriorityQueueNode<int>
    {
        public readonly int x;
        public readonly int y;
        public bool isWalkable;

        public int gCost;
        public int hCost;
        public int FCost => gCost + hCost;

        public List<PathNode> neighbours;
        public PathNode? cameFromNode;

        public PathNode(int x, int y, bool isWalkable = true)
        {
            this.x = x;
            this.y = y;
            this.isWalkable = isWalkable;
            neighbours = new();
        }

        public override string ToString() => $"{x},{y}";

        internal void CalculateFCost()
        {

        }
    }
}