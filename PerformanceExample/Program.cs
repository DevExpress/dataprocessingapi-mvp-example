using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using DevExpress.DataProcessing;
using DevExpress.DataProcessingApi.Model;

namespace DevExpress.DataProcessingAPI.PerformanceExample {
    public class Program {
        public static void Main(string[] args) {
            Console.WriteLine($"Preparing data...");
            var data = PerformanceTestData.Generate(5_000_000);

            RunTest("DevExpress DataProcessingApi", () => RunDataProcessingApi(data));
            RunTest("Microsoft Linq", () => RunLinq(data));
            RunTest("Microsoft Parallel Linq", () => RunParallelLinq(data));

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        
        static void RunTest(string testName, Action action) {
            const int warmUpCount = 2;
            const int testCount = 10;
            for(int i = 0; i < warmUpCount; i++) {
                action();
            }
            Console.WriteLine($"Running {testName}");
            var result = new double[testCount];
            for(int i = 0; i < testCount; i++) {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                action();
                stopWatch.Stop();
                result[i] = stopWatch.Elapsed.TotalMilliseconds;
            }

            Console.WriteLine($"{ testName } - average time: { Math.Round(result.Average()) }ms");
        }

        static void RunDataProcessingApi(IList<PerformanceTestData> data) {
            var result = DataFlow
                .FromObject(data)
                .Aggregate(e => e
                    .GroupBy("StringProperty1", "StringProperty2", "StringProperty3", "StringProperty4")
                    .Summary("IntProperty", AggregationType.Sum, "IntPropertySum")
                    .Summary("DoubleProperty", AggregationType.Sum, "DoublePropertySum")
                )
                .ToEnumerable<PerformanceTestDataResult>()
                .Execute()
                .ToArray();
        }
        static void RunParallelLinq(IList<PerformanceTestData> data) {
            var result = from row in data.AsParallel()
                         group row by new { c1 = row.StringProperty1, c2 = row.StringProperty2, c3 = row.StringProperty3, c4 = row.StringProperty4 } into g
                         select new PerformanceTestDataResult() {
                             StringProperty1 = g.Key.c1,
                             StringProperty2 = g.Key.c2,
                             StringProperty3 = g.Key.c3,
                             StringProperty4 = g.Key.c4,
                             IntPropertySum = g.Sum(row => row.IntProperty),
                             DoublePropertySum = g.Sum(row => row.DoubleProperty),
                         };
            result.ToArray();
        }
        static void RunLinq(IList<PerformanceTestData> data) {
            var result = from row in data
                         group row by new { c1 = row.StringProperty1, c2 = row.StringProperty2, c3 = row.StringProperty3, c4 = row.StringProperty4 } into g
                         select new PerformanceTestDataResult() {
                             StringProperty1 = g.Key.c1,
                             StringProperty2 = g.Key.c2,
                             StringProperty3 = g.Key.c3,
                             StringProperty4 = g.Key.c4,
                             IntPropertySum = g.Sum(row => row.IntProperty),
                             DoublePropertySum = g.Sum(row => row.DoubleProperty),
                         };
            result.ToArray();
        }
    }

    class PerformanceTestData {
        public string StringProperty1 { get; set; }
        public string StringProperty2 { get; set; }
        public string StringProperty3 { get; set; }
        public string StringProperty4 { get; set; }
        public int IntProperty { get; set; }
        public double DoubleProperty { get; set; }

        public static IList<PerformanceTestData> Generate(int rowCount) {
            var res = new PerformanceTestData[rowCount];
            var random = new Random();
            for(int i = 0; i < rowCount; i++) {
                res[i] = new PerformanceTestData {
                    StringProperty1 = "StringProperty1_" + random.Next(0, 5),
                    StringProperty2 = "StringProperty2_" + random.Next(0, 15),
                    StringProperty3 = "StringProperty3_" + random.Next(0, 30),
                    StringProperty4 = "StringProperty4_" + random.Next(0, 50),
                    IntProperty = random.Next(0, 10000),
                    DoubleProperty = random.NextDouble(),
                };
            }
            return res;
        }
    }
    class PerformanceTestDataResult {
        public string StringProperty1 { get; set; }
        public string StringProperty2 { get; set; }
        public string StringProperty3 { get; set; }
        public string StringProperty4 { get; set; }

        public Int64 IntPropertySum { get; set; }
        public double DoublePropertySum { get; set; }
        public double DoublePropertyAverage { get; set; }
    }  
}
