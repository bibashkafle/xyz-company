using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xyz.Service
{
    public class XyzSalesService : IXyzSalesService
    {
        public XyzSalesService()
        {
        }

        public virtual (List<ColumnName> columnNames,  Dictionary<string, List<object>> dataTable) GetSalesReport(RecordFilter filter)
        {
            //Read .csv File
            List<XyzSaleRecord> saleRecords = GetSaleRecords();

            //Dicitionary of months 
            var Months = GetMonths();

           
            //Row Filter
            var dataSet = saleRecords.Where(x =>
            {
                if (filter is not null)
                {
                    if (filter.RecordRowFilter == RowFilter.Year && filter.RowFilterValue.HasValue)
                        return x.Date.Year == filter.RowFilterValue.Value;
                    else if (filter.RecordRowFilter == RowFilter.Month && filter.RowFilterValue.HasValue)
                        return x.Date.Month == filter.RowFilterValue.Value;
                }

                return true;
            }).OrderBy(x => x.Date.Month);

            List<FilteredResult> FilteredResult = new List<FilteredResult>();

            //Column Filter
            if (filter.RecordColumnFilter == ColumnFilter.Country)
            {
                FilteredResult = dataSet.GroupBy(c => new { c.Date.Month, c.Country }).Select(g => new FilteredResult(Months[g.Key.Month], g.Key.Month, g.Key.Country.Trim(), g.Where(c => c.Date.Month == g.Key.Month).Sum(c => c.Qty),g.Count())).ToList();

            }
            else if (filter.RecordColumnFilter == ColumnFilter.State)
            {
                FilteredResult = dataSet.GroupBy(c => new { c.Date.Month, c.State }).Select(g => new FilteredResult(Months[g.Key.Month], g.Key.Month, g.Key.State.Trim(), g.Where(c => c.Date.Month == g.Key.Month).Sum(c => c.Qty), g.Count())).ToList();
            }
            else if (filter.RecordColumnFilter == ColumnFilter.City)
            {
                FilteredResult = dataSet.GroupBy(c => new { c.Date.Month, c.City })
                .Select(g => new FilteredResult(Months[g.Key.Month], g.Key.Month, g.Key.City.Trim(), g.Where(c => c.Date.Month == g.Key.Month).Sum(c => c.Qty), g.Count())).ToList();
            }

            //Create columns from filtered result
            var columnNames = FilteredResult.GroupBy(x => x.ColumnName).Select(x => new ColumnName { title = x.Key }).OrderBy(x => x.title).ToList();

            //months in filtered result
            var months = FilteredResult.Select(x => x.MonthName).Distinct().ToList();

            //Column Projected Datatable
            Dictionary<string, List<object>> dataTable = new Dictionary<string, List<object>>();

            months.ForEach(monthsKey =>
            {
                
                columnNames.ForEach(c =>
                {
                    var jsonDatas = FilteredResult.Where(x => x.MonthName == monthsKey && x.ColumnName.Trim() == c.title).Select(x =>  (object)x.Qty).ToList();

                    if (dataTable.ContainsKey(monthsKey))
                        dataTable[monthsKey].AddRange(jsonDatas);
                    else
                        dataTable.Add(monthsKey, jsonDatas);
                });

                //Add months name to the top of list
                dataTable[monthsKey].Insert(0, monthsKey);
            });


            //Bank row after matrix
            List<object> breakPoint = new List<object>();
            columnNames.ForEach(x =>
            {
                breakPoint.Add("____");
            });
            breakPoint.Insert(0, "____");

            dataTable.Add("__", breakPoint);

            //calculate the average, median and total across the region.
            string[] footer = { "Avg.", "Med", "Total" };
            foreach(string item in footer)
            {
                columnNames.ForEach(c => {
                    var list = FilteredResult.Where(x => x.ColumnName == c.title).OrderBy(x => x.Qty).ToList();

                    List<object> valueLise = new List<object>();
                    var sum = list.Sum(x => x.Qty);

                    if (item == "Avg.")
                        valueLise.Add(sum / list.Count());
             
                    else if (item == "Med")
                        valueLise.Add(list[list.Count() / 2].Qty);
                   
                    else if (item == "Total")
                        valueLise.Add(sum);

                    if (dataTable.ContainsKey(item))
                        dataTable[item].AddRange(valueLise);
                    else
                        dataTable.Add(item, valueLise);

                });
                dataTable[item].Insert(0, item);   
            }
            

            columnNames.Insert(0, new ColumnName { title = " " });

            //return tupple columnName and matraix table. 
            return (columnNames, dataTable);
        }



        //Return months of a year. 
        private Dictionary<int, string> GetMonths()
        {
            Dictionary<int, string> months = new Dictionary<int, string>();

            for(int i = 1; i<= 12; i++)
            {
                months.Add(i, new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM"));
            }
  
            return months;
        }

        public virtual List<XyzSaleRecord> GetSaleRecords()
        {
            var filePath = $"{Environment.CurrentDirectory}/xyz_sample_data.csv";

            var lines = File.ReadAllLines(filePath);
            List<XyzSaleRecord> data = new List<XyzSaleRecord>();
            int count = 1;
            for(int i =0; i< lines.Length; i++)
            {
                if (i>=1)
                {
                    var cols = lines[i].Split(",");
                    data.Add(new XyzSaleRecord(cols[0], cols[1], cols[2], Convert.ToDateTime(cols[3]), Convert.ToInt32(cols[4])));
                }
                count++;
            }
            return data;
        }
    }
}
