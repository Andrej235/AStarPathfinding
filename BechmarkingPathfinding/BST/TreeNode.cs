namespace BechmarkingPathfinding.BST
{
    public class TreeNode(int val, TreeNode? left = null, TreeNode? right = null)
    {
        public int val = val;
        public int count = 1;
        public TreeNode? left = left;
        public TreeNode? right = right;
    }
}
