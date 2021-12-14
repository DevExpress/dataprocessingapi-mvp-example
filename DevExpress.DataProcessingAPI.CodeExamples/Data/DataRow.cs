using System;
using System.Collections.Generic;

namespace DevExpress.DataProcessingAPI.CodeExamples {
    public class DataRow {
        public static List<DataRow> CreateData(){
            List<DataRow> data = new List<DataRow>();
            data.Add(new DataRow() { RegionId = 1, Name = "Northwest"});
            data.Add(new DataRow() { RegionId = 2, Name = "Northeast" });
            data.Add(new DataRow() { RegionId = 3, Name = "Central" });
            data.Add(new DataRow() { RegionId = 4, Name = "Southwest" });
            data.Add(new DataRow() { RegionId = 5, Name = "Southeast" });
            data.Add(new DataRow() { RegionId = 6, Name = "Canada" });
            data.Add(new DataRow() { RegionId = 7, Name = "France" });
            data.Add(new DataRow() { RegionId = 8, Name = "Germany" });
            data.Add(new DataRow() { RegionId = 9, Name = "Australia" });
            data.Add(new DataRow() { RegionId = 10, Name = "United Kingdom" });
            return data;
        }

        public int RegionId { get; set; }
        public string Name { get; set; }
    }
}
