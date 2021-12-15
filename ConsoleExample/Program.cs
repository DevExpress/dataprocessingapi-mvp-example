using System;
using System.IO;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.DataProcessingApi.Model;

namespace DevExpress.DataProcessingAPI.ConsoleExample {
    class Program {
        static void Main(string[] args) {
            var surveyFlow = DataFlow
                .FromJsonFile(Path.Combine("Data", "survey.json"))
                .Debug(e => {
                    Console.WriteLine("JSON input columns:");
                    e.PrintColumns();
                    Console.WriteLine();
                })
                .SelectColumns("Submitted", "Customer ID", "Which of the following product features are important to you?")
                .RenameColumn("Which of the following product features are important to you?", "Feature list");

            var customersFlow = DataFlow
                .FromExcel(Path.Combine("Data", "Customers.xlsx"), "Grid 1")
                .Debug(e => {
                    Console.WriteLine("Excel input columns:");
                    e.PrintColumns();
                    Console.WriteLine();
                });

            var resultFlow = surveyFlow.Join(customersFlow, "Customer ID", "CustomerKey")
                .Unfold("Feature list")
                .Aggregate(o => {
                    o
                    .GroupBy("RegionCountryName", "Feature list")
                     .CountRows("Count");
                })
                .Top(3, "Count", new[] { "RegionCountryName" })
                .Sort(e => {
                    e.SortColumns.Add("RegionCountryName", SortOrder.Ascending);
                    e.SortColumns.Add("Count", SortOrder.Descending);
                })
                .Debug(e => {
                    Console.WriteLine("Resulting columns:");
                    e.PrintColumns();
                    Console.WriteLine();
                })
                .ToExcelFile(Path.Combine(".", "survey_analysis.xlsx"));

            var filePath = resultFlow.Execute();

            Console.WriteLine($"The resulting file is saved to {filePath}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
