using System;
using System.Collections.Generic;

namespace Xyz.Service
{
    public interface IXyzSalesService
    {
        List<XyzSaleRecord> GetSaleRecords();

        (List<ColumnName> columnNames, Dictionary<string, List<object>> dataTable) GetSalesReport(RecordFilter filter);
    }
}
