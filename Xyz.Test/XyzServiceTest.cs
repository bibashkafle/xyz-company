using System;
using System.Collections.Generic;
using Xyz.Service;

namespace Xyz.Test
{
    public class XyzServiceTest
    {
        IXyzSalesService _service;
        public XyzServiceTest(IXyzSalesService service)
        {
            _service = service;
        }


        public List<XyzSaleRecord> GetSaleRecords()
        {
            return _service.GetSaleRecords();
        }

        public (List<ColumnName> columnNames, Dictionary<string, List<object>> dataTable) GetSalesReport(RecordFilter filter)
        {
            return _service.GetSalesReport(filter);
        }

    }
}
