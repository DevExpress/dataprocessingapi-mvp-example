using System;
using System.Collections.Generic;

namespace DevExpress.DataProcessingAPI.CodeSamples {
    public class SampleDataRow {
        public static List<SampleDataRow> CreateData(){
            List<SampleDataRow> data = new List<SampleDataRow>();
            data.Add(new SampleDataRow() { Id = 1, Name = "Northwest"});
            data.Add(new SampleDataRow() { Id = 2, Name = "Northeast" });
            data.Add(new SampleDataRow() { Id = 3, Name = "Central" });
            data.Add(new SampleDataRow() { Id = 4, Name = "Southwest" });
            data.Add(new SampleDataRow() { Id = 5, Name = "Southeast" });
            data.Add(new SampleDataRow() { Id = 6, Name = "Canada" });
            data.Add(new SampleDataRow() { Id = 7, Name = "France" });
            data.Add(new SampleDataRow() { Id = 8, Name = "Germany" });
            data.Add(new SampleDataRow() { Id = 9, Name = "Australia" });
            data.Add(new SampleDataRow() { Id = 10, Name = "United Kingdom" });
            return data;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
