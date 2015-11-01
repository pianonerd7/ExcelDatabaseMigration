using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Excel_Database_Migration.ExcelUtils;
using System.IO;
using System.Diagnostics;

namespace ExcelDatabaseMigrationTest.ExcelUtils
{
    [TestClass]
    public class CSVWrapperTest
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void openCSV_emptyPath()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV("");
        }

        [TestMethod]
        public void directory()
        {
            Trace.WriteLine("environment is: " + Environment.CurrentDirectory);
            Trace.WriteLine("environment is: " + formatPath("test1.csv"));
        }

        [TestMethod]
        public void openCSV_existing_EmptyFile()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(formatPath("empty.csv"));
            Assert.IsNull(csv.Attributes);
        }

        [TestMethod]
        public void openCSV_existing_smallFile()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(formatPath("test1.csv"));
            Assert.IsNotNull(csv.Attributes);
            string[] expectedAttributes = {"Name", "Gender", "Salary" };
            checkAttributes(expectedAttributes, csv);

            string[][] expectedData = { new string[] { "Tom", "M", "20" },
            new string[] {"Adam", "M", "30" },
            new string[] {"Sara", "F", "40" },
            new string[] {"Serena", "F", "50" } };
            checkData(expectedData, csv);
        }

        private string formatPath (string filename)
        {
            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);
            Trace.WriteLine(d.ToString());
            
            DirectoryInfo twoup = d.Parent.Parent;
            Trace.WriteLine(twoup.FullName.ToString());
            return twoup.FullName.ToString() + "\\" + "testData" + "\\" + filename;
        }

        private void checkAttributes(String[] expected, CSVWrapper csv)
        {
            for (int i = 0; i < csv.Attributes.Length; i++)
            {
                Assert.AreEqual(expected[i], csv.Attributes[i]);
            }
        }

        private void checkData (String[][] expected, CSVWrapper csv)
        {
            for (int i = 0; i<csv.Data.Count; i++)
            {
                for (int j = 0; j<csv.Data[i].Length; j++)
                {
                    Assert.AreEqual(expected[i][j], csv.Data[i][j]);
                }
            }
        }
        
    }
}
