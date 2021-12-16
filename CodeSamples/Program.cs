using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using DevExpress.DataProcessing;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.DataProcessingApi.Model;
using Xunit;
using Newtonsoft.Json;

namespace DevExpress.DataProcessingAPI.CodeSamples {
    public class Program {

        public static void Main(string[] args) {

            LoadDataSamples.FromExcel();
            LoadDataSamples.FromObject();
            //LoadDataSamples.FromJson();

            //TransformDataSamples.AddColumn_Code();
            //TransformDataSamples.AddColumn_CriteriaOperator();
            //TransformDataSamples.Filter_CriteriaOperator();
            //TransformDataSamples.Filter_SingleRowValue_Code();
            //TransformDataSamples.Filter_MultiRowValue_Code();
            //TransformDataSamples.Sort();
            //TransformDataSamples.Join();
            //TransformDataSamples.ProcessColumn();
            //TransformDataSamples.RemoveColumn();
            //TransformDataSamples.RenameColumns();

            //AnalyzeDataSamples.Aggregate();
            //AnalyzeDataSamples.TopN();

            //UploadDataSamples.ToJsonString();
            //UploadDataSamples.ToExcel();
            //UploadDataSamples.ToEnumerable();

            //DebugSamples.DebugNode();
            //DebugSamples.PerformanceLogging();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    public class LoadDataSamples {
        [Fact]
        public static void FromExcel() {
            DataTable result = DataFlow
                .FromExcel(Helper.ExcelFileName, Helper.ExcelWorksheetName)
                .ToDataTable()
                .Execute();

            Assert.Equal(231, result.Rows.Count);
            Helper.PrintDataTable(result);
        }
        [Fact]
        public static void FromObject() {
            DataTable result = DataFlow
                .FromObject(SampleDataRow.CreateData())
                .ToDataTable()
                .Execute();

            Assert.Equal(10, result.Rows.Count);
            Helper.PrintDataTable(result);
        }
        [Fact]
        public static void FromJson() {
            DataTable result = DataFlow
                .FromJsonUrl(options => {
                    options.Url = "https://raw.githubusercontent.com/DevExpress-Examples/DataSources/master/JSON/customers.json";
                    options.RootElement = "Customers";
                })
                .ToDataTable()
                .Execute();
            Assert.Equal(91, result.Rows.Count);
            Helper.PrintDataTable(result);
        }
    }
    public class TransformDataSamples {
        [Fact]
        public static void Join() {
            var salesFlow = DataFlow
                .FromExcel(Helper.ExcelFileName, Helper.ExcelWorksheetName);
            var regionFlow = DataFlow
                .FromObject(SampleDataRow.CreateData());

            DataTable result = salesFlow
                .Join(regionFlow, "RegionId", "Id")
                .ToDataTable()
                .Execute();

            Helper.AssertDataTableColumns(new[] { "ProductName", "RegionId", "Freight", "SubTotal", "Id", "Name" }, result);
            Assert.Equal(231, result.Rows.Count);
            Helper.PrintDataTable(result);
        }
        [Fact]
        public static void Filter_CriteriaOperator() {
            DataTable result = DataFlow
                .FromExcel(Helper.ExcelFileName, Helper.ExcelWorksheetName)
                .Filter("[Freight] > 1500")
                .ToDataTable()
                .Execute();

            Assert.Equal(212, result.Rows.Count);
            Helper.PrintDataTable(result);
        }

        [Fact]
        public static void Filter_SingleRowValue_Code() {
            DataTable result = DataFlow
                .FromExcel(Helper.ExcelFileName, Helper.ExcelWorksheetName)
                .Filter("ProductName", (string value) => value == "Proseware Ink Jet Fax Machine E100 White")
                .ToDataTable()
                .Execute();

            Assert.Equal(10, result.Rows.Count);
            Helper.PrintDataTable(result);

        }

        [Fact]
        public static void Filter_MultiRowValue_Code() {
            DataTable result = DataFlow
                .FromExcel(Helper.ExcelFileName, Helper.ExcelWorksheetName)
                .Filter(row => (string)row["ProductName"] == "Proseware Laser Fax Printer E100 White" && (double)row["Freight"] > 100000)
                .ToDataTable()
                .Execute();

            Assert.Equal(4, result.Rows.Count);
            Helper.PrintDataTable(result);
        }

        [Fact]
        public static void AddColumn_CriteriaOperator() {
            DataTable result = DataFlow
                .FromExcel(Helper.ExcelFileName, Helper.ExcelWorksheetName)
                .AddColumn("NewColumn", "[ProductName] + '_' + [RegionId]")
                .ToDataTable()
                .Execute();

            Helper.AssertDataTableColumns(new[] { "ProductName", "RegionId", "Freight", "SubTotal", "NewColumn" }, result);
            Assert.Equal("Proseware Laser Fax Printer E100 White_9", result.Rows[0]["NewColumn"]);
            Helper.PrintDataTable(result);
        }
        [Fact]
        public static void AddColumn_Code() {
            DataTable result = DataFlow
                .FromObject(SampleDataRow.CreateData())
                .AddColumn("NewColumn", row => $"{((string)row["Name"]).ToUpper()} ({(int)row["Id"]})")
                .ToDataTable()
                .Execute();

            Helper.AssertDataTableColumns(new[] { "Id", "Name", "NewColumn" }, result);
            Assert.Equal("NORTHWEST (1)", result.Rows[0]["NewColumn"]);
            Helper.PrintDataTable(result);
        }
        [Fact]
        public static void RenameColumns() {
            DataTable result = DataFlow
                .FromObject(SampleDataRow.CreateData())
                .RenameColumns(e => {
                    e.ColumnRenameMap.Add("Id", "Region Id");
                    e.ColumnRenameMap.Add("Name", "Region Name");
                })
                .ToDataTable()
                .Execute();

            Helper.AssertDataTableColumns(new[] { "Region Id", "Region Name" }, result);
            Helper.PrintDataTable(result);
        }
        [Fact]
        public static void RemoveColumn() {
            DataTable result = DataFlow
                .FromObject(SampleDataRow.CreateData())
                .RemoveColumn("Id")
                .ToDataTable()
                .Execute();

            Helper.AssertDataTableColumns(new[] { "Name" }, result);
            Helper.PrintDataTable(result);
        }

        [Fact]
        public static void ProcessColumn() {
            DataTable result = DataFlow
                .FromObject(SampleDataRow.CreateData())
                .ProcessColumn("Name", (string regionName) => {
                    return regionName.ToUpper();
                })
                .ToDataTable()
                .Execute();

            Assert.Equal("NORTHWEST", result.Rows[0]["Name"]);
            Helper.PrintDataTable(result);
        }
        [Fact]
        public static void Sort() {
            DataTable result = DataFlow
                .FromObject(SampleDataRow.CreateData())
                .Sort(e => {
                    e.SortColumns.Add("Name", SortOrder.Ascending);
                })
                .ToDataTable()
                .Execute();


            Assert.Equal(10, result.Rows.Count);
            Helper.PrintDataTable(result);
        }

    }
    public class AnalyzeDataSamples {
        [Fact]
        public static void Aggregate() {
            DataTable result = DataFlow
               .FromExcel(Helper.ExcelFileName, Helper.ExcelWorksheetName)
               .Aggregate(options => {
                   options
                       .GroupBy("ProductName")
                       .Summary("Freight", AggregationType.Sum, "Freight (Sum)")
                       .Summary("Freight", AggregationType.Count, "Freight (Count)")
                       .Summary("Freight", AggregationType.Average, "Freight (Avg)")
                       .CountRows("Count");
               })
               .ToDataTable()
               .Execute();

            Helper.AssertDataTableColumns(new[] { "ProductName", "Freight (Sum)", "Freight (Count)", "Freight (Avg)", "Count" }, result);
            Assert.Equal(27, result.Rows.Count);
            Helper.PrintDataTable(result);
        }

        [Fact]
        public static void TopN() {
            DataTable result = DataFlow
                .FromExcel(Helper.ExcelFileName, Helper.ExcelWorksheetName)
                .Top(3, "Freight")
                .ToDataTable()
                .Execute();

            Assert.Equal(3, result.Rows.Count);
            Helper.PrintDataTable(result);
        }

    }
    public class UploadDataSamples {
        [Fact]
        public static void ToExcel() {
            string savedFilePath = DataFlow
                .FromObject(SampleDataRow.CreateData())
                .ToExcelFile(Path.Combine(".", "ExportToExcelSample.xlsx"))
                .Execute();

            Assert.True(File.Exists(savedFilePath));
            Console.WriteLine(savedFilePath);
        }
        [Fact]
        public static void ToJsonString() {
            string jsonString = DataFlow
                .FromExcel(Helper.ExcelFileName, Helper.ExcelWorksheetName)
                .ToJsonString()
                .Execute();
            Assert.NotEmpty(jsonString);
            Console.WriteLine(jsonString);
        }

        class SampleData {
            public string ProductName { get; set; }
            public string Region { get; set; }
            public double RegionId { get; set; }
            public double Freight { get; set; }
            public double SubTotal { get; set; }
        }
        [Fact]
        public static void ToEnumerable() {
            IEnumerable<SampleData> result = DataFlow
                .FromExcel(Helper.ExcelFileName, Helper.ExcelWorksheetName)
                .ToEnumerable<SampleData>()
                .Execute();

            Assert.Equal(231, result.Count());
            Console.WriteLine(result.Count());
        }
    }
    public class DebugSamples {
        [Fact]
        public static void DebugNode() {
            string jsonString = DataFlow
                .FromObject(SampleDataRow.CreateData())
                .Debug(e => {
                    Console.WriteLine("res");
                    e.PrintDataTable();
                    var debugData = e.DataTable;
                    Helper.AssertDataTableColumns(new[] { "Id", "Name" }, debugData);
                    Assert.Equal(10, debugData.Rows.Count);
                })
                .ToJsonString()
                .Execute();
        }
    }

    public static class Helper {
        public static string ExcelFileName { get { return Path.Combine("Data", "ProductSales.xlsx"); } }
        public const string ExcelWorksheetName = "Sheet1";

        public static void AssertDataTableColumns(string[] expectedColumns, DataTable dataTable) {
            Assert.Equal(
                    expectedColumns,
                    dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray()
                );
        }
        public static void PrintDataTable(DataTable dataTable) {
            Console.WriteLine(JsonConvert.SerializeObject(dataTable, Formatting.Indented));
        }
    }
}
