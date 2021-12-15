# DevExpress Data Processing API (MVP)

DevExpress Data Processing API is a new product that is currently in development and is therefore not yet a part of the DevExpress product line.

This example is created to collect feedback and usage data. If you are interested in DevExpress Data Processing API, leave a comment under our blog post or create a ticket in our Support Center.

## What is the DevExpress Data Processing API

DevExpress Data Processing API is a .NET library that allows you to convert your data (including ETL and data analysis) into usable and desired form.

Typical scenarios:

- Add ETL (Extract, Transform, Load) capabilities to .NET applications.
- Use data shaping (grouping, sorting, filtering, applying analytics functions) before you display data in a UI application, regardless of platform.

The main features of the presented library:

- Connect to different data sources (relational databases, web services, Excel spreadsheets, JSON data, and so on) using a unified interface.
- Process data at runtime in the application memory.
- Embed business logic written in .NET at any point in your data processing.
- Use functions to clean and structure data alongside with analytical functions.
- Debug your app using a wide range of API.
- Transform your data quickly from raw data to final output.

## How to Launch the Project

The following repository contains an example that transforms the user survey data in the JSON format and data about users from an XLSX file in the following way:
- Joins these two data flows to get a one data source. The "Feature list" column is an array of data. The `Unfold` operation creates a new row for each item in the array.
- Aggregates data by "RegionCountryName" and "Feature list".
- Calculates the top 3 achievements for each country.
- Sorts data.
- Uploads data to an XLSX file.

To launch the example, you need to update your DevExpress NuGet packages to **v21.2.4**. You can find the detailed instructions in the following section: [Install DevExpress Controls Using NuGet Packages](https://docs.devexpress.com/GeneralInformation/115912/installation/install-devexpress-controls-using-nuget-packages).

## How to Work with this API

### Common Concepts

The common algorithm: 

1. Create a new data flow (`DataFlow`) and use one of the functions to load data (for example, `FromCsv` or `FromDatabase`).
1. Use functions to clean and structure your data and apply analytical functions (for example, `ProcessColumn`, `AddColumn`, `Join`, `Aggregate`, and so on).
1. Define the output data format (for example, `ToExcel`, `ToDataTable`).
1. Execute the previously defined data flow to generate resulting data (`Execute`).

### Load Data

- From a CVS file: `FromCsv`
- From a database: `FromDatabase`
- From Excel spreadsheets (XLSX and XLS files): `FromExcel`
- From a Web Service (JSON): `FromJsonFile` and `FromJsonUrl`
- From .NET object: `FromObject`

### Transform Data

- Join data from different sources: `Join`
- Unfold array values and display a new data row for every element in the array: `Unfold`
- Add columns: `AddColumn` (using criteria operator or in code)
- Modify column data: `ProcessColumn`
- Filter data: `Filter` (using criteria operator or in code)
- Sort data: `Sort`
- Manage columns: `SelectColumns`, `RenameColumns`, `RemoveColumn`/`RemoveColumns`

### Analyze Data

- TopN: `Top`, `Bottom`
- Data aggregation: `Aggregate`

### Upload Data

- Upload data to a stream that contains an XLSX file: `ToExcel`
- Upload data to a JSON string: `ToJsonString`
- Upload data to a .NET object: `ToDataTable`, `ToEnumerable`

### Debug

- Get data for each step in the processed data flow: `Debug`

## Performance

- Data is stored by column ([column-oriented DBMS](https://en.wikipedia.org/wiki/Column-oriented_DBMS)). This approach allows us to optimize data analysis operations, such as data aggregation, join data from different sources, and so on.
- Data engine supports multi-threaded data calculation to handle a large amount of data efficiently.
- An optimized graph of data operations.

Our experiments showed that DevExpress Data Processing API can be faster or equal to Parallel Linq in aggregation calculation tasks (grouping and sums calculation).

> Note that we made a number of assumptions in the MVP implementation which do not fully reveal the performance. At the same time, performance can depend on many factors (for example, just-in-time (JIT) compilation). If you encounter performance issues, please fell free to describe your scenario in our [Support Center](https://supportcenter.devexpress.com/ticket/list).


## Product Development Plans

- Support more popular data sources and a variety of upload methods.
- Add more features to solve the most popular ETL and analytics problems.
- Create tools for developers to simplify the creating and debugging of data flows, including the development of Visual Studio built-in tools.
- Performance optimization.
- Improve diagnostic logging and error output.
- Integration with DevExpress controls: Winforms, WPF, Blazor (as ASP.NET Core Backend)

Your opinion matters to us. Please share your thoughts in comments in our blog post: []().
