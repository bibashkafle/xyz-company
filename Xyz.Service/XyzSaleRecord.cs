using System;

namespace Xyz.Service
{
    public class XyzSaleRecord
    {

        public XyzSaleRecord()
        {

        }

        public XyzSaleRecord(string country, string state, string city, DateTime date, int qty)
        {
            Country = country;
            State = state;
            City = city;
            Date = date;
            Qty = qty;
        }

        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public int Qty { get; set; }


    }
}
