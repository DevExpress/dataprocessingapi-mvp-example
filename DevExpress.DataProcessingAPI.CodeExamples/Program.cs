using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.DataAccess.Excel;
using DevExpress.DataProcessing;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.DataProcessingApi.Model;

namespace DevExpress.DataProcessingAPI.CodeExamples {
    class Program {
        #region Entry point (Main)
        static void Main(string[] args) {
            JoinSample();
            //TopNSample();
            //DebugSample();
            //ExportToExcelSample();
            //FilterSample_CriteriaOperator();
            //FilterSample_SingleRowValue_Code();
            //FilterSample_MultiRowValue_Code();
            //AddColumnSample_Code();
            //AddColumnSample_CriteriaOperator();
            //ProcessColumnSample();
            //AggregateSample();
            //SortSample();
            //RenameColumnsSample();
            //RemoveColumnSample();
            //ToDataTableSample();
            //ToEnumerableSample();

            //PerfromanceTestSample();
            //PerfromanceLoggingSample();

            Console.WriteLine("DataFlow was executed");
            Console.ReadLine();
        }
        #endregion
        #region open and read Excel file to the flow
        static ProcessingDataFlowOperation CreateExcelFlow() {
            return DataFlow
                .FromExcel(Path.Combine("Data", "ProductSales.xlsx"), "Sheet1")
                .SelectColumns("ProductName", "Region", "RegionId", "Freight", "SubTotal"); //schema
        }
        #endregion
        #region create object flow
        static ProcessingDataFlowOperation CreateObjectFlow() {
            return DataFlow.FromObject(DataRow.CreateData());
        }
        #endregion
        #region get JSON string and display it
        static void PrintJsonStringToConsole(ProcessingDataFlowOperation flow) {
            string json = flow.ToJsonString().Execute();
            Console.WriteLine(json);
        }
        #endregion
        #region join data from different sources (Join Sample)
        static void JoinSample() {
            var flow1 = CreateExcelFlow();
            var flow2 = CreateObjectFlow();
            var resultFlow = flow1.Join(flow2, "RegionId", "RegionId");
            PrintJsonStringToConsole(resultFlow);
        }
        #endregion
        #region you can attach debug node to any operation (Debug Sample)
        static void DebugSample() {
            var flow = CreateExcelFlow();
            flow.Debug(e => {
                Console.WriteLine("res");
                e.PrintDataTable();
                var currentData = e.DataTable;
            });
        }
        #endregion
        #region export data to Excel stream (Export To Excel Sample)
        static void ExportToExcelSample() {
            var flow = CreateObjectFlow();
            var fullPath = flow.ToExcelFile(Path.Combine(".", "ExportToExcelSample.xlsx")).Execute();
            Console.WriteLine(fullPath);
        }
        #endregion
        #region filter data by CriteriaOperator (Filter Sample)
        static void FilterSample_CriteriaOperator() {
            var filteredFlow = CreateExcelFlow()
                .Filter("[Freight] > 1500");
            PrintJsonStringToConsole(filteredFlow);
        }
        #endregion
        #region filter data by single row compare (Filter Sample)
        static void FilterSample_SingleRowValue_Code() {
            var filteredFlow = CreateExcelFlow()
                .Filter("Region", (string value) => value != "Australia");
            PrintJsonStringToConsole(filteredFlow);
        }
        #endregion
        #region filter data by multi row compare (Filter Sample)
        static void FilterSample_MultiRowValue_Code() {
            var filteredFlow = CreateExcelFlow()
                .Filter(row => (string)row["Region"] == "United Kingdom" && (double)row["Freight"] > 1500);
            PrintJsonStringToConsole(filteredFlow);
        }
        #endregion
        #region add new column by CriteriaOperator (AddColumn Sample)
        static void AddColumnSample_CriteriaOperator() {
            var flow = CreateObjectFlow()
                .AddColumn("NewColumn", "[Name] + '_' + [RegionId]");
            PrintJsonStringToConsole(flow);
        }
        #endregion
        #region add new column by code (AddColumn Sample)
        static void AddColumnSample_Code() {
            var flow = CreateObjectFlow()
                .AddColumn("NewColumn", row => (string)row["Name"] + "_" + (int)row["RegionId"]);
            PrintJsonStringToConsole(flow);
        }
        #endregion
        #region rename existing columns (Rename DataColumns Sample)
        static void RenameColumnsSample() {
            var flow = CreateObjectFlow()
                .RenameColumns(e => {
                    e.ColumnRenameMap.Add("RegionId", "Id");
                    e.ColumnRenameMap.Add("Name", "RegionName");
                });
            PrintJsonStringToConsole(flow);
        }
        #endregion
        #region remove existing column (Remove DataColumn Sample)
        static void RemoveColumnSample() {
            var flow = CreateObjectFlow()
                .RemoveColumn("RegionId");
            PrintJsonStringToConsole(flow);
        }
        #endregion
        #region process all data in column (ProcessColumn Sample)
        static void ProcessColumnSample() {
            var flow = CreateObjectFlow()
                .ProcessColumn("RegionId", (int id) => {
                    return id * 10;
                });
            PrintJsonStringToConsole(flow);
        }
        #endregion
        #region aggregate data sample (Aggregate Sample)
        static void AggregateSample() {
            var flow = CreateExcelFlow()
               .Aggregate(options => {
                   options
                       .GroupBy("ProductName")
                       .Summary("Freight", AggregationType.Sum, "Freight (Sum)")
                       .Summary("Freight", AggregationType.Count, "Freight (Count)")
                       .Summary("Freight", AggregationType.Average, "Freight (Avg)")
                       .CountRows("Count");
               });
            PrintJsonStringToConsole(flow);
        }
        #endregion
        #region sort data by column (Sort Sample)
        static void SortSample() {
            var flow = CreateExcelFlow()
                .Sort(e => {
                    e.SortColumns.Add("ProductName", SortOrder.Ascending);
                });
            PrintJsonStringToConsole(flow);
        }
        #endregion
        #region select top N data (TopN Sample)
        static void TopNSample() {
            var flow = CreateExcelFlow()
                .Top(3, "Freight");
            PrintJsonStringToConsole(flow);
        }
        #endregion
        #region extract data to DataTable (ExportTo DataTable Sample)
        static void ToDataTableSample() {
            var flow = CreateExcelFlow()
                .ToDataTable();

            var data = flow.Execute();
            Console.WriteLine(data.Rows.Count);
        }
        #endregion
        #region PerformanceTest
        static void PerfromanceTestSample() {
            Console.WriteLine($"Preparing data...");
            var data = PerfromanceTestData.Generate(5_000_000);

            PerformanceTest.RunTest("DevExpress DataProcessingApi", () => PerformanceTest.RunDataProcessingApi(data));
            PerformanceTest.RunTest("Microsoft Linq", () => PerformanceTest.RunLinq(data));
            PerformanceTest.RunTest("Microsoft Parallel Linq", () => PerformanceTest.RunParallelLinq(data));
        }
        #endregion
        #region Performance Logging
        static void PerfromanceLoggingSample() {
            var flow = CreateExcelFlow()
                .Filter(row => (string)row["Region"] == "United Kingdom")
                .Aggregate(e => e
                        .GroupBy("ProductName")
                        .Summary("Freight", AggregationType.Sum, "Freight (Sum)")
                        )
                .Sort("Freight (Sum)", SortOrder.Ascending)
                
                .ToDataTable();

            var data = flow.Execute(ExecutionMode.Release, (performanceLog) => { 
                Console.WriteLine($"Total time: {performanceLog.TotalTime}ms");
                DataFlow.FromObject(performanceLog.PerformanceTable).ToExcelFile("./perfromanceTable.xlsx").Execute();
            });
        }
        #endregion

        class SampleData {
            public string ProductName { get; set; }
            public string Region { get; set; }
            public double RegionId { get; set; }
            public double Freight { get; set; }
            public double SubTotal { get; set; }
        }
        static void ToEnumerableSample() {
            var flow = CreateExcelFlow()
                    .ToEnumerable<SampleData>();

            var data = flow.Execute();

            Console.WriteLine(data.Count());
        }
    }
}
