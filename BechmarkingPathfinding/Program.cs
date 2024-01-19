using BechmarkingPathfinding.BST;
using BechmarkingPathfinding.PathFinding;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;

namespace BechmarkingPathfinding
{
    internal class Program
    {
        static void Main()
        {
/*            BSTBench bstBench = new BSTBench();
            bstBench.GetPath_Iterative_Set();
            bstBench.GetPath_BST_Set();*/

            /*            var a = new BSTBench();
                        a.GetLowestFCostNode_Iteration();
                        a.GetLowestFCostNode_BST();*/

            //new Bench().FindPath_Edge();
            BenchmarkRunner.Run<BSTBench>();
            /*            var a = new BSTBench();
                        a.GetLowestFCostNode_Iteration();
                        a.GetLowestFCostNode_BST();*/
        }
    }

    [MemoryDiagnoser]
    public class BSTBench
    {
        //private readonly List<int> vals;
        public BSTBench()
        {
            /*            vals = [];
                        for (int i = 0; i < 100; i++)
                            vals.Add(Random.Shared.Next(0, 1000));

                        bstRoot = BinarySearchTree.Insert(bstRoot, vals[0]);
                        for (int i = 1; i < vals.Count; i++)
                            bstRoot = BinarySearchTree.Insert(bstRoot, vals[i]);*/

            //BinarySearchTree.Inorder(bstRoot);

            //Next step
            //Asign pathNodes and bstPathNodeRoot and test the 2 functions

            /*            Pathfinding pathfinding = new(50, 50);
                        pathfinding.FindPath(0, 15, 49, 49);

                        pathNodes = Pathfinding.testing;

                        bstPathNodeRoot = BinarySearchTreePathNode.Insert(bstPathNodeRoot, pathNodes[0]);
                        for (int i = 1; i < pathNodes.Count; i++)
                            bstPathNodeRoot = BinarySearchTreePathNode.Insert(bstPathNodeRoot, pathNodes[i]);*/

            pathfinding_Iterative = new(50, 50);
            pathfinding_BST = new(50, 50);
        }

        Pathfinding pathfinding_Iterative;
        BSTPathfinding pathfinding_BST;

        readonly Consumer consumer = new();
        //private readonly TreeNode bstRoot;
        //public List<PathNode> pathNodes;
        //public TreePathNode bstPathNodeRoot;

        /*        //[Benchmark]
                public void GetLowestFCostNode_Iteration_Int()
                {
                    int min = vals[0];
                    for (int i = 1; i < vals.Count; i++)
                    {
                        if (vals[i] < min)
                            min = vals[i];
                    }
                    consumer.Consume(min);
                }

                //[Benchmark]
                public void GetLowestFCostNode_BST_Int()
                {
                    var a = BinarySearchTree.MinValueNode(bstRoot).val;
                    consumer.Consume(a);
                }*/

        /*        [Benchmark]
                public void GetLowestFCostNode_Iteration()
                {
                    PathNode lowestFCostNode = pathNodes[0];
                    for (int i = 1; i < pathNodes.Count; i++)
                    {
                        if (pathNodes[i].fCost < lowestFCostNode.fCost)
                            lowestFCostNode = pathNodes[i];
                    }
                    consumer.Consume(lowestFCostNode);
                }

                [Benchmark]
                public void GetLowestFCostNode_BST()
                {
                    var a = BinarySearchTreePathNode.MinValueNode(bstPathNodeRoot).val;
                    consumer.Consume(a);
                }*/

        [Benchmark]
        public void GetPath_Iterative_Corner()
        {
            var res = pathfinding_Iterative.FindPath(0, 0, 49, 49);
            res?.Consume(consumer);
        }

        [Benchmark]
        public void GetPath_BST_Corner()
        {
            var res = pathfinding_BST.FindPath(0, 0, 49, 49);
            res?.Consume(consumer);
        }

        [Benchmark]
        public void GetPath_Iterative_Set()
        {
            var res = pathfinding_Iterative.FindPath(0, 0, 26, 48);
            res?.Consume(consumer);
        }

        [Benchmark]
        public void GetPath_BST_Set()
        {
            var res = pathfinding_BST.FindPath(0, 0, 26, 48);
            res?.Consume(consumer);
        }

        [Benchmark]
        public void GetPath_Iterative_Random()
        {
            var res = pathfinding_Iterative.FindPath(0, 0, Random.Shared.Next(0, 50), Random.Shared.Next(0, 50));
            res?.Consume(consumer);
        }

        [Benchmark]
        public void GetPath_BST_Random()
        {
            var res = pathfinding_BST.FindPath(0, 0, Random.Shared.Next(0, 50), Random.Shared.Next(0, 50));
            res?.Consume(consumer);
        }
    }

    [MemoryDiagnoser]
    public class PathfindingBench
    {
        private static readonly Pathfinding pathfinding = new(50, 50);
        readonly Consumer consumer = new();

        [Benchmark]
        public void FindPath_Edge() => pathfinding.FindPath(0, 0, 49, 49)?.Consume(consumer);

        [Benchmark]
        public void FindPath_Set() => pathfinding.FindPath(0, 0, 35, 27)?.Consume(consumer);

        [Benchmark]
        public void FindPath_Random() => pathfinding.FindPath(0, 0, Random.Shared.Next(0, 50), Random.Shared.Next(0, 50))?.Consume(consumer);
    }
}
/*
 *********************************   RESULTS   *********************************
| Method                | Mean      | Error    | StdDev   | Gen0   | Allocated |
|---------------------- |----------:|---------:|---------:|-------:|----------:|
| FindPath_Edge         |  61.39 us | 0.470 us | 0.367 us | 2.3193 |  14.77 KB |
| FindPath_Set          | 294.65 us | 0.565 us | 0.472 us | 7.8125 |  50.52 KB |
| FindPath_Random       | 337.04 us | 6.440 us | 7.158 us | 7.3242 |  47.52 KB |
| FindPath_Edge_FProp   |  58.89 us | 0.612 us | 0.572 us | 2.3804 |  14.77 KB |
| FindPath_Set_FProp    | 295.14 us | 1.729 us | 1.617 us | 7.8125 |  50.52 KB |
| FindPath_Random_FProp | 336.58 us | 6.539 us | 6.997 us | 7.3242 |  46.87 KB |
********************************************************************************

After precalculating neighbours
******************************   RESULTS   *******************************
| Method          | Mean      | Error    | StdDev   | Gen0   | Allocated |
|---------------- |----------:|---------:|---------:|-------:|----------:|
| FindPath_Edge   |  56.30 us | 1.114 us | 0.987 us | 1.0376 |   6.44 KB |
| FindPath_Set    | 269.59 us | 0.529 us | 0.442 us | 0.9766 |   7.46 KB |
| FindPath_Random | 307.87 us | 5.708 us | 5.339 us | 0.9766 |   8.24 KB |
**************************************************************************
 */
