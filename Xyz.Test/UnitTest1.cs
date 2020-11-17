using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xyz.Service;

namespace Xyz.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetSaleRecords()
        {
            var mockIXyzSalesService = new Mock<XyzSalesService>();
            List<XyzSaleRecord> records = new List<XyzSaleRecord>();
            records.Add(new XyzSaleRecord("Australia", "Victoria", "Melbourne", Convert.ToDateTime("2020/1/1"), 10));
            records.Add(new XyzSaleRecord("Australia", "Victoria", "Melbourne", Convert.ToDateTime("2020/2/1"), 20));
            records.Add(new XyzSaleRecord("Australia", "Victoria", "Melbourne", Convert.ToDateTime("2020/3/1"), 4));
            mockIXyzSalesService.Setup(M=>M.GetSaleRecords()).Returns(records);
           
            var xyzService = new XyzServiceTest(mockIXyzSalesService.Object);

            var data = xyzService.GetSaleRecords();
          
            Assert.AreEqual(3, data.Count);
        }

        [TestMethod]
        public void TestGetSalesReport()
        {
            var mockIXyzSalesService = new Mock<XyzSalesService>();

            RecordFilter recordFilter = new RecordFilter();
            recordFilter.RecordColumnFilter = ColumnFilter.State;

            List<XyzSaleRecord> records = new List<XyzSaleRecord>();
            records.Add(new XyzSaleRecord("Australia", "Victoria", "Melbourne", Convert.ToDateTime("2020/1/1"), 10));
            records.Add(new XyzSaleRecord("Australia", "Victoria", "Melbourne", Convert.ToDateTime("2020/2/1"), 20));
            records.Add(new XyzSaleRecord("Australia", "Victoria", "Melbourne", Convert.ToDateTime("2020/3/1"), 4));
            mockIXyzSalesService.Setup(M => M.GetSaleRecords()).Returns(records);
            mockIXyzSalesService.Setup(M => M.GetSalesReport(recordFilter)).CallBase();

            var xyzService = new XyzServiceTest(mockIXyzSalesService.Object);
            var data = xyzService.GetSalesReport(recordFilter);

            //should be 2 column name
            Assert.AreEqual(2, data.columnNames.Count);
            //second column name should be state name
            Assert.AreEqual("Victoria", data.columnNames[1].title);

            // total data should be 7
            //Jan,Feb, March and Avg. Med and Total
            Assert.AreEqual(7, data.dataTable.Count);

        }
    }
}
