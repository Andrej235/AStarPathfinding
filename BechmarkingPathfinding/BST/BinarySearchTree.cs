namespace BechmarkingPathfinding.BST
{
    public class BinarySearchTree
    {
        public static TreeNode NewNode(int item) => new(item);

        // A utility function to do inorder
        // traversal of BST
        public static void Inorder(TreeNode? root)
        {
            if (root != null)
            {
                Inorder(root.left);
                Console.Write($"{root.val}({root.count})\n");
                Inorder(root.right);
            }
        }

        /* A utility function to insert a new 
        TreeNode with given val in BST */
        public static TreeNode Insert(TreeNode? node, int val)
        {
            /* If the tree is empty, 
            return a new TreeNode */
            if (node == null) return NewNode(val);

            // If val already exists in BST, 
            // increment count and return
            if (val == node.val)
            {
                (node.count)++;
                return node;
            }

            /* Otherwise, recur down the tree */
            if (val < node.val)
                node.left = Insert(node.left, val);
            else
                node.right = Insert(node.right, val);

            /* return the (unchanged) TreeNode pointer */
            return node;
        }

        /* Given a non-empty binary search tree, 
        return the TreeNode with minimum val value 
        found in that tree. Note that the entire tree 
        does not need to be searched. */
        public static TreeNode MinValueNode(TreeNode TreeNode)
        {
            TreeNode current = TreeNode;

            /* loop down to find the leftmost leaf */
            while (current.left != null)
                current = current.left;

            return current;
        }

        /* Given a binary search tree and a val, 
        this function deletes a given val and 
        returns root of modified tree */
        public static TreeNode? DeleteNode(TreeNode? root, int val)
        {
            // base case
            if (root == null) return root;

            // If the val to be deleted is smaller than the
            // root's val, then it lies in left subtree
            if (val < root.val)
                root.left = DeleteNode(root.left, val);

            // If the val to be deleted is greater than 
            // the root's val, then it lies in right subtree
            else if (val > root.val)
                root.right = DeleteNode(root.right, val);

            // if val is same as root's val
            else
            {
                // If val is present more than once, 
                // simply decrement count and return
                if (root.count > 1)
                {
                    (root.count)--;
                    return root;
                }

                // ElSE, delete the TreeNode
                TreeNode? temp;

                // TreeNode with only one child or no child
                if (root.left == null)
                {
                    temp = root.right;
                    return temp;
                }
                else if (root.right == null)
                {
                    temp = root.left;
                    return temp;
                }

                // TreeNode with two children: Get the inorder 
                // successor (smallest in the right subtree)
                temp = MinValueNode(root.right);

                // Copy the inorder successor's 
                // content to this TreeNode
                root.val = temp.val;
                root.count = temp.count;

                // Delete the inorder successor
                root.right = DeleteNode(root.right,
                                        temp.val);
            }
            return root;
        }
    }
}
