using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Excel_Database_Migration.ExcelUtils;
using System.Data;

namespace Excel_Database_Migration_Test.ExcelUtils
{
    [TestClass]
    public class DataTableToCSVConverterTest
    {
        [TestMethod]
        public void convert_Empty()
        {
            DataTable table;
            DataTableToCSVConverter.ConvertToCSV(table);
        }
    }
}
