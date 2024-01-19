using BechmarkingPathfinding.PathFinding;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;

namespace BechmarkingPathfinding
{
    internal class Program
    {
        static void Main()
        {
            //new Bench().FindPath_Edge();
            BenchmarkRunner.Run<Bench>();
        }

    }

    [MemoryDiagnoser]
    public class Bench
    {
        private static Pathfinding pathfinding = new(50, 50);
        Consumer consumer = new();

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
