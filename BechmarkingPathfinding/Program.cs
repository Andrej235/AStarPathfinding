using BechmarkingPathfinding.PathFinding;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Text.Json;

namespace BechmarkingPathfinding
{
    internal class Program
    {
        public static Pathfinding Pathfinding { get; set; } = null!;
        public static PQPathfinding PQPathfinding { get; set; } = null!;
        public static PQPathfindingHashset PQPathfindingWithHashSet { get; set; } = null!;
        public static CustomPQPathfindingHashset CustomPQPathfindingWithHashSet { get; set; } = null!;

        static void Main()
        {
            PQPathfinding = new(50, 50);
            Pathfinding = new(50, 50);
            PQPathfindingWithHashSet = new(50, 50);
            CustomPQPathfindingWithHashSet = new(50, 50);
            //FullTest();

            BenchmarkRunner.Run<PrioityQueueBenchmark>();
        }

        public static void FullTest()
        {
            List<int> xs = [];
            List<int> ys = [];
            for (int i = 0; i < 100; i++)
            {
                xs.Add(Random.Shared.Next(0, 50));
                ys.Add(Random.Shared.Next(0, 50));
            }

            List<bool> queueResults = [];
            List<bool> hashSetResults = [];
            List<bool> customPQResults = [];
            for (int i = 0; i < xs.Count; i++)
            {
                queueResults.Add(Test_Queue(xs[i], ys[i]));
                hashSetResults.Add(Test_HashSet(xs[i], ys[i]));
                customPQResults.Add(Test_CustomPQ(xs[i], ys[i]));
            }

            Console.WriteLine("Queue " + (!queueResults.Contains(false) ? "worked" : $"failed ({queueResults.Where(x => !x).Count()})"));
            Console.WriteLine("Queue with HashSet " + (!hashSetResults.Contains(false) ? "worked" : $"failed ({hashSetResults.Where(x => !x).Count()})"));
            Console.WriteLine("Custom queue with HashSet " + (!customPQResults.Contains(false) ? "worked" : $"failed ({customPQResults.Where(x => !x).Count()})"));
        }

        private static bool Test_CustomPQ(int x, int y)
        {
            var queue = CustomPQPathfindingWithHashSet.FindPath(0, 0, x, y);
            var normal = Pathfinding.FindPath(0, 0, x, y);

            var normalJSON = JsonSerializer.Serialize(normal);
            var queueJSON = JsonSerializer.Serialize(queue);
            return queueJSON == normalJSON;
        }

        public static bool Test_Queue(int x, int y)
        {
            var queue = PQPathfinding.FindPath(0, 0, x, y);
            var normal = Pathfinding.FindPath(0, 0, x, y);

            var normalJSON = JsonSerializer.Serialize(normal);
            var queueJSON = JsonSerializer.Serialize(queue);
            return queueJSON == normalJSON;
        }

        public static bool Test_HashSet(int x, int y)
        {
            var normal = Pathfinding.FindPath(0, 0, x, y);
            var queueWithHashset = PQPathfindingWithHashSet.FindPath(0, 0, x, y);

            var normalJSON = JsonSerializer.Serialize(normal);
            var queueWithHashsetJSON = JsonSerializer.Serialize(queueWithHashset);
            return queueWithHashsetJSON == normalJSON;
        }
    }

    [MemoryDiagnoser]
    public class PrioityQueueBenchmark
    {
        PQPathfinding pqPathfinding;
        PQPathfindingHashset pqPathfindingHashset;
        CustomPQPathfindingHashset customPQPathfindingWithHashSet;
        //Pathfinding pathfinding;

        public PrioityQueueBenchmark()
        {
            pqPathfinding = new(50, 50);
            pqPathfindingHashset = new(50, 50);
            customPQPathfindingWithHashSet = new(50, 50);
            //pathfinding = new(50, 50);
        }

        /*        [Benchmark]
        public void Normal()
        {
            pathfinding.FindPath(0, 0, 44, 40);
        }*/

        //[Benchmark]
        public void Queue()
        {
            pqPathfinding.FindPath(0, 0, 44, 40);
        }

        //[Benchmark]
        public void Queue_WithHashSet()
        {
            pqPathfindingHashset.FindPath(0, 0, 44, 40);
        }

        /*        [Benchmark]
        public void Normal_Random()
        {
            pathfinding.FindPath(0, 0, Random.Shared.Next(0, 50), Random.Shared.Next(0, 50));
        }*/

        //[Benchmark]
        public void Queue_Random()
        {
            pqPathfinding.FindPath(0, 0, Random.Shared.Next(0, 50), Random.Shared.Next(0, 50));
        }

        //[Benchmark]
        public void Queue_WithHashSet_Random()
        {
            pqPathfindingHashset.FindPath(0, 0, Random.Shared.Next(0, 50), Random.Shared.Next(0, 50));
        }

        [Benchmark]
        public void CustomQueue_WithHashSet_Random()
        {
            customPQPathfindingWithHashSet.FindPath(0, 0, Random.Shared.Next(0, 50), Random.Shared.Next(0, 50));
        }
    }
}

/*
*********************************   RESULTS   **********************************
********************************************************************************
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
**************************************************************************
| Method          | Mean      | Error    | StdDev   | Gen0   | Allocated |
|---------------- |----------:|---------:|---------:|-------:|----------:|
| FindPath_Edge   |  56.30 us | 1.114 us | 0.987 us | 1.0376 |   6.44 KB |
| FindPath_Set    | 269.59 us | 0.529 us | 0.442 us | 0.9766 |   7.46 KB |
| FindPath_Random | 307.87 us | 5.708 us | 5.339 us | 0.9766 |   8.24 KB |
**************************************************************************

Normal - using iteration for finding the lowest fCost
Queue - uses PriorityQueue<> for openList and finding the lowest fCost
*************************************************************************
| Method        | Mean      | Error    | StdDev   | Gen0   | Allocated |
|-------------- |----------:|---------:|---------:|-------:|----------:|
| Queue         |  66.37 us | 0.905 us | 0.803 us | 1.8311 |  11.41 KB |
| Normal        | 216.02 us | 0.535 us | 0.500 us | 1.4648 |   9.45 KB |
| Queue_Random  | 138.55 us | 2.266 us | 1.892 us | 1.9531 |  12.44 KB |
| Normal_Random | 309.28 us | 3.781 us | 3.352 us | 0.9766 |    8.3 KB |
*************************************************************************

Queue - uses PriorityQueue<> for openList and finding the lowest fCost
Queue_WithHashSet - Queue + uses HasSet<> for closedList
********************************************************************************************
| Method                   | Mean      | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|------------------------- |----------:|---------:|---------:|-------:|-------:|----------:|
| Queue                    |  67.09 us | 1.305 us | 1.697 us | 1.8311 |      - |  11.41 KB |
| Queue_WithHashSet        |  32.96 us | 0.506 us | 0.449 us | 2.6855 | 0.0610 |  16.47 KB |
| Queue_Random             | 139.09 us | 1.741 us | 1.544 us | 1.9531 |      - |  12.69 KB |
| Queue_WithHashSet_Random |  56.04 us | 0.386 us | 0.322 us | 2.8687 | 0.1221 |  17.63 KB |
********************************************************************************************

CustomQueue_WithHashSet_Random - uses SimplePriorityQueue made by BlueRaja
***************************************************************************************************
| Method                         | Mean      | Error    | StdDev   | Gen0    | Gen1   | Allocated |
|------------------------------- |----------:|---------:|---------:|--------:|-------:|----------:|
| Queue_WithHashSet_Random       |  57.30 us | 1.072 us | 1.101 us |  2.8687 | 0.1221 |  17.58 KB |
| CustomQueue_WithHashSet_Random | 129.52 us | 1.483 us | 1.239 us | 12.2070 | 1.9531 |  75.81 KB |
***************************************************************************************************

CustomQueue_WithHashSet_Random - uses GenericPriorityQueue made by BlueRaja
*************************************************************************************************
| Method                         | Mean     | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|------------------------------- |---------:|---------:|---------:|-------:|-------:|----------:|
| Queue_WithHashSet_Random       | 58.32 us | 1.160 us | 1.425 us | 2.8687 | 0.1221 |  17.57 KB |
| CustomQueue_WithHashSet_Random | 76.80 us | 0.517 us | 0.484 us | 2.1973 |      - |  13.97 KB |
*************************************************************************************************

CustomQueue_WithHashSet_Random - uses FastPriorityQueue made by BlueRaja >> Stable-ish (it works)
*************************************************************************************************
| Method                         | Mean     | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|------------------------------- |---------:|---------:|---------:|-------:|-------:|----------:|
| Queue_WithHashSet_Random       | 57.25 us | 1.140 us | 1.120 us | 2.8687 | 0.1221 |   17.6 KB |
| CustomQueue_WithHashSet_Random | 60.59 us | 0.541 us | 0.480 us | 1.4648 |      - |   9.42 KB |
*************************************************************************************************

CustomQueue_WithHashSet_Random - uses StablePriorityQueue made by BlueRaja
*************************************************************************************************
| Method                         | Mean     | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|------------------------------- |---------:|---------:|---------:|-------:|-------:|----------:|
| Queue_WithHashSet_Random       | 58.15 us | 1.127 us | 1.253 us | 2.8687 | 0.1221 |  17.63 KB |
| CustomQueue_WithHashSet_Random | 78.06 us | 0.979 us | 0.916 us | 2.1973 | 0.1221 |  13.92 KB |
*************************************************************************************************

CustomQueue_WithHashSet_Random - uses FastPriorityQueue made by BlueRaja >> Stable-ish (it works)
With added support for unwalkable grid cells
Checks if the start and end nodes actually exist
*************************************************************************************************
| Method                         | Mean     | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|------------------------------- |---------:|---------:|---------:|-------:|-------:|----------:|
| Queue_WithHashSet_Random       | 57.91 us | 1.123 us | 1.460 us | 2.8076 | 0.1221 |  17.49 KB |
| CustomQueue_WithHashSet_Random | 61.87 us | 1.209 us | 1.392 us | 1.4648 |      - |   9.36 KB |
*************************************************************************************************

CustomQueue_WithHashSet_Random - uses FastPriorityQueue made by BlueRaja >> Stable-ish (it works)
Removed CalculateFCost method from PathNode and made FCost a property which returns gCost + hCost
****************************************************************************************************
| Method                         | Mean      | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|------------------------------- |----------:|----------:|----------:|-------:|-------:|----------:|
| Queue_WithHashSet_Random       | 55.457 us | 1.1066 us | 1.1841 us | 2.8076 | 0.1221 |   17847 B |
| CustomQueue_WithHashSet_Random |  6.871 us | 0.0335 us | 0.0314 us | 0.0076 |      - |      64 B |
****************************************************************************************************

StablePriorityQueue after the FCost change
******************************************************************************************
| Method                         | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------------------------- |---------:|----------:|----------:|-------:|----------:|
| CustomQueue_WithHashSet_Random | 7.100 us | 0.1238 us | 0.1158 us | 0.0076 |      64 B |
******************************************************************************************

GenericPriorityQueue after the FCost change
******************************************************************************************
| Method                         | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------------------------- |---------:|----------:|----------:|-------:|----------:|
| CustomQueue_WithHashSet_Random | 6.067 us | 0.1171 us | 0.1095 us | 0.0076 |      64 B |
******************************************************************************************

SimplePriorityQueue after the FCost change
******************************************************************************************
| Method                         | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------------------------- |---------:|----------:|----------:|-------:|----------:|
| CustomQueue_WithHashSet_Random | 5.988 us | 0.0484 us | 0.0404 us | 0.0076 |      64 B |
******************************************************************************************
*/