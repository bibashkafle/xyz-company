using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xyz.Web.Models;
using Xyz.Service;
using System.Text.Json;

namespace Xyz.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
       
        public IActionResult Index()
        {
            var model = GetModel(new IndexModel());
            return  View(model);
        }

        [HttpPost]
        public IActionResult Index(IndexModel model)
        {
            model = GetModel(model);
            return View(model);
        }

        [NonAction]
        private IndexModel GetModel(IndexModel indexModel)
        {

            ColumnFilter _columnFilter = ColumnFilter.State;
            RowFilter _rowFilter = RowFilter.Month;

            RecordFilter recordFilter = new RecordFilter();

            if (indexModel is not null)
            {
                if (!string.IsNullOrWhiteSpace(indexModel.ColumnFilter))
                {
                    Enum.TryParse(indexModel.ColumnFilter, out _columnFilter);
                }

                if (!string.IsNullOrWhiteSpace(indexModel.RowFilter))
                {
                    Enum.TryParse(indexModel.RowFilter, out _rowFilter);
                }

                if (indexModel.RowFilterValue.HasValue && indexModel.RowFilterValue.Value > 0)
                {
                    recordFilter.RowFilterValue = indexModel.RowFilterValue;
                }

                recordFilter.RecordColumnFilter = _columnFilter;
                recordFilter.RecordRowFilter = _rowFilter;
            }

            IXyzSalesService service = new XyzSalesService();
            var data = service.GetSalesReport(recordFilter);
            indexModel = new IndexModel { DataColumn = JsonSerializer.Serialize(data.columnNames), DataTable = JsonSerializer.Serialize(data.dataTable.Values) };

            return indexModel;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
