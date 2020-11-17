using System;
namespace Xyz.Service
{
    public class RecordFilter
    {
        public RecordFilter()
        {
        }

        public ColumnFilter RecordColumnFilter { get; set; }
        public RowFilter RecordRowFilter { get; set; }
        public int? RowFilterValue { get; set; }
    }
}
