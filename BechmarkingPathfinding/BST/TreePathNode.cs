using BechmarkingPathfinding.PathFinding;

namespace BechmarkingPathfinding.BST
{
    public class TreePathNode(PathNode val, TreePathNode? left = null, TreePathNode? right = null)
    {
        public PathNode val = val;
        public int count = 1;
        public TreePathNode? left = left;
        public TreePathNode? right = right;
    }
}
