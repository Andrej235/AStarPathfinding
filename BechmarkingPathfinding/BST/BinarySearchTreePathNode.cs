using BechmarkingPathfinding.PathFinding;

namespace BechmarkingPathfinding.BST
{
    public class BinarySearchTreePathNode
    {
        public static TreePathNode NewNode(PathNode item) => new(item);

        // A utility function to do inorder
        // traversal of BST
        public static void Inorder(TreePathNode? root)
        {
            if (root != null)
            {
                Inorder(root.left);
                Console.Write($"{root.val.fCost}({root.count})\n");
                Inorder(root.right);
            }
        }

        /* A utility function to insert a new 
        TreePathNode with given val in BST */
        public static TreePathNode Insert(TreePathNode? node, PathNode val)
        {
            /* If the tree is empty, 
            return a new TreePathNode */
            if (node == null) return NewNode(val);

            // If val already exists in BST, 
            // increment count and return
            if (val.fCost == node.val.fCost)
            {
                (node.count)++;
                return node;
            }

            /* Otherwise, recur down the tree */
            if (val.fCost < node.val.fCost)
                node.left = Insert(node.left, val);
            else
                node.right = Insert(node.right, val);

            /* return the (unchanged) TreePathNode pointer */
            return node;
        }

        /* Given a non-empty binary search tree, 
        return the TreePathNode with minimum val value 
        found in that tree. Note that the entire tree 
        does not need to be searched. */
        public static TreePathNode MinValueNode(TreePathNode TreePathNode)
        {
            TreePathNode current = TreePathNode;

            /* loop down to find the leftmost leaf */
            while (current.left != null)
                current = current.left;

            return current;
        }

        /* Given a binary search tree and a val, 
        this function deletes a given val and 
        returns root of modified tree */
        public static TreePathNode? DeleteNode(TreePathNode? root, PathNode val)
        {
            // base case
            if (root == null) return root;

            // If the val to be deleted is smaller than the
            // root's val, then it lies in left subtree
            if (val.fCost < root.val.fCost)
                root.left = DeleteNode(root.left, val);

            // If the val to be deleted is greater than 
            // the root's val, then it lies in right subtree
            else if (val.fCost > root.val.fCost)
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

                // ElSE, delete the TreePathNode
                TreePathNode? temp;

                // TreePathNode with only one child or no child
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

                // TreePathNode with two children: Get the inorder 
                // successor (smallest in the right subtree)
                temp = MinValueNode(root.right);

                // Copy the inorder successor's 
                // content to this TreePathNode
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
