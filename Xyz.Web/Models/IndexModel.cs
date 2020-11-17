using System;
namespace Xyz.Web.Models
{
    public class IndexModel
    {
        public IndexModel()
        {
        }


        public string ColumnFilter { get; set; }
        public string RowFilter { get; set; }
        public int? RowFilterValue { get; set; }

        public object DataColumn { get; set; }
        public object DataTable { get; set; }
  
    }
}
