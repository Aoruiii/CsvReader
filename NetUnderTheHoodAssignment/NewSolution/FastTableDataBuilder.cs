using CsvDataAccess.CsvReading;
using CsvDataAccess.Interface;

namespace CsvDataAccess.NewSolution;


// problem 1: loop data with a lot of nulls, slow down the performance
// problem 2: boxing and unboxing, consuming memories


public class FastTableDataBuilder : ITableDataBuilder
{
    public ITableData Build(CsvData csvData)
    {
        var resultRows = new List<Row>();

        foreach (var row in csvData.Rows)
        {
            var newRow = new Row();

            for (int columnIndex = 0; columnIndex < csvData.Columns.Length; ++columnIndex)
            {
                var column = csvData.Columns[columnIndex];
                string valueAsString = row[columnIndex];
                if (string.IsNullOrEmpty(valueAsString))
                {
                    continue;
                }
                else if (valueAsString == "TRUE")
                {
                    newRow.AssignCell(column, true);
                }
                else if (valueAsString == "FALSE")
                {
                    newRow.AssignCell(column, false);
                }
                else if (valueAsString.Contains(".") && decimal.TryParse(valueAsString, out var valueAsDecimal))
                {
                    newRow.AssignCell(column, valueAsDecimal);
                }
                else if (int.TryParse(valueAsString, out var valueAsInt))
                {
                    newRow.AssignCell(column, valueAsInt);
                }
                else
                {
                    newRow.AssignCell(column, valueAsString);
                }

            }

            resultRows.Add(newRow);
        }

        return new TableData(csvData.Columns, resultRows);
    }
}

public class Row
{
    private Dictionary<string, int> _intsData = new();
    private Dictionary<string, bool> _boolsData = new();
    private Dictionary<string, decimal> _decimalsData = new();
    private Dictionary<string, string> _stringsData = new();

    public void AssignCell(string columnName, int data)
    {
        _intsData.Add(columnName, data);
    }

    public void AssignCell(string columnName, bool data)
    {
        _boolsData.Add(columnName, data);
    }

    public void AssignCell(string columnName, decimal data)
    {
        _decimalsData.Add(columnName, data);
    }

    public void AssignCell(string columnName, string data)
    {
        _stringsData.Add(columnName, data);
    }

    public object GetAtColumn(string columnName)
    {
        if (_intsData.ContainsKey(columnName))
        {
            return _intsData[columnName];
        }
        if (_boolsData.ContainsKey(columnName))
        {
            return _boolsData[columnName];
        }
        if (_decimalsData.ContainsKey(columnName))
        {
            return _decimalsData[columnName];
        }
        if (_stringsData.ContainsKey(columnName))
        {
            return _stringsData[columnName];
        }
        return null;
    }
}

public class TableData : ITableData
{
    private readonly List<Row> _rows;
    public int RowCount => _rows.Count;
    public IEnumerable<string> Columns { get; }

    public TableData(IEnumerable<string> columns, List<Row> rows)
    {
        _rows = rows;
        Columns = columns;
    }

    public object GetValue(string columnName, int rowIndex)
    {
        return _rows[rowIndex].GetAtColumn(columnName);
    }
}