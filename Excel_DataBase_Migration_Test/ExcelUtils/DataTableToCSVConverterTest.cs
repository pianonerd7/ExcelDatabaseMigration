using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Excel_Database_Migration.ExcelUtils;
using System.Data;
using System.Diagnostics;

namespace Excel_Database_Migration_Test.ExcelUtils
{
    [TestClass]
    public class DataTableToCSVConverterTest
    {
        [TestMethod]
        public void convert_Empty()
        {
            DataTable table = new DataTable();
            CSVWrapper csv =  DataTableToCSVConverter.ConvertToCSV(table);
            Assert.AreEqual(0, csv.Attributes.Length);
            Assert.AreEqual(0, csv.Data.Count);
        }

        [TestMethod]
        public void convert_small()
        {
            DataTable table = new DataTable();
            table.Clear();
            table.Columns.Add("Name");
            table.Columns.Add("Age");
            DataRow row1 = table.NewRow();
            row1["Name"] = "ted";
            row1["Age"] = "21";
            table.Rows.Add(row1);
            CSVWrapper csv = DataTableToCSVConverter.ConvertToCSV(table);
            Assert.AreEqual(2, csv.Attributes.Length);
            Assert.AreEqual(1, csv.Data.Count);

            Assert.AreEqual("Name", csv.Attributes[0]);
            Assert.AreEqual("Age", csv.Attributes[1]);
            Assert.AreEqual("ted", csv.Data[0][0]);
            Assert.AreEqual("21", csv.Data[0][1]);

        }
    }
}
