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
        public static PQPathfinding PQPathfinding { get; set; } = null!;
        public static Pathfinding Pathfinding { get; set; } = null!;

        static void Main()
        {
            PQPathfinding = new(50, 50);
            Pathfinding = new(50, 50);

            //PrioityQueueBenchmark benchmark = new PrioityQueueBenchmark();
            BenchmarkRunner.Run<PrioityQueueBenchmark>();

            /*            for (int i = 0; i < 50; i++)
                            Console.WriteLine(Test(benchmark, Random.Shared.Next(0, 50), Random.Shared.Next(0, 50)));*/
        }

        public static bool Test(PrioityQueueBenchmark benchmark, int x, int y)
        {
            var q = PQPathfinding.FindPath(0, 0, x, y);
            var n = Pathfinding.FindPath(0, 0, x, y);

            var qJ = JsonSerializer.Serialize(q);
            var nJ = JsonSerializer.Serialize(n);
            return qJ == nJ;
        }
    }

    [MemoryDiagnoser]
    public class PrioityQueueBenchmark
    {
        PQPathfinding pqPathfinding;
        Pathfinding pathfinding;

        public PrioityQueueBenchmark()
        {
            pqPathfinding = new(50, 50);
            pathfinding = new(50, 50);
        }

        [Benchmark]
        public void Queue()
        {
            pqPathfinding.FindPath(0, 0, 44, 40);
        }

        [Benchmark]
        public void Normal()
        {
            pathfinding.FindPath(0, 0, 44, 40);
        }

        [Benchmark]
        public void Queue_Random()
        {
            pqPathfinding.FindPath(0, 0, Random.Shared.Next(0, 50), Random.Shared.Next(0, 50));
        }

        [Benchmark]
        public void Normal_Random()
        {
            pathfinding.FindPath(0, 0, Random.Shared.Next(0, 50), Random.Shared.Next(0, 50));
        }
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
*****************************   RESULTS   ********************************
| Method          | Mean      | Error    | StdDev   | Gen0   | Allocated |
|---------------- |----------:|---------:|---------:|-------:|----------:|
| FindPath_Edge   |  56.30 us | 1.114 us | 0.987 us | 1.0376 |   6.44 KB |
| FindPath_Set    | 269.59 us | 0.529 us | 0.442 us | 0.9766 |   7.46 KB |
| FindPath_Random | 307.87 us | 5.708 us | 5.339 us | 0.9766 |   8.24 KB |
**************************************************************************

Normal - using iteration for finding the lowest fCost
Queue - uses PriorityQueue<> for openList and finding the lowest fCost
****************************   RESULTS   *******************************
| Method        | Mean      | Error    | StdDev   | Gen0   | Allocated |
|-------------- |----------:|---------:|---------:|-------:|----------:|
| Queue         |  66.37 us | 0.905 us | 0.803 us | 1.8311 |  11.41 KB |
| Normal        | 216.02 us | 0.535 us | 0.500 us | 1.4648 |   9.45 KB |
| Queue_Random  | 138.55 us | 2.266 us | 1.892 us | 1.9531 |  12.44 KB |
| Normal_Random | 309.28 us | 3.781 us | 3.352 us | 0.9766 |    8.3 KB |
*************************************************************************
 */
