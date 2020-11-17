using System;
namespace Xyz.Service
{
    public class FilteredResult
    {
        public FilteredResult(string monthName, int monthNumber, string columnName, int qty, int groupCount)
        {
            MonthName = monthName;
            MonthNumber = monthNumber;
            ColumnName = columnName;
            Qty = qty;
            GroupCount = groupCount;
        }

        public string MonthName { get; set; }
        public int MonthNumber { get; set; }
        public string ColumnName { get; set; }
        public int Qty { get; set; }
        public int GroupCount { get; set; }
     
    }

    public class ColumnName
    {
        public string title { get; set; }
    }
}
