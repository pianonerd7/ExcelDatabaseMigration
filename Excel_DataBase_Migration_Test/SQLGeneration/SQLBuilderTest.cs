using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Excel_Database_Migration.SQLGeneration;
using Excel_Database_Migration.ExcelUtils;
using Excel_DataBase_Migration_Test.TestHelpers;

namespace Excel_DataBase_Migration_Test.SQLGeneration
{
    [TestClass]
    public class SQLBuilderTest
    {
        [TestMethod]
        public void CreateSchemaTest_empty()
        {
            SQLBuilder builder = new SQLBuilder(null, "", "", "");
            Assert.AreEqual("", builder.Build());
            builder.createSchema();
            Assert.AreEqual("", builder.Build());
        }

        [TestMethod]
        public void DropTableTest_name()
        {
            SQLBuilder builder = new SQLBuilder(null, "", "nameTable", "");
            Assert.AreEqual("", builder.Build());
            builder.DropTable();
            Assert.AreEqual("If Exists(Select object_id From sys.tables Where name = 'nameTable') Drop Table nameTable;\n", builder.Build());
        }

        [TestMethod]
        public void CreateDatabaseTest_empty()
        {
            SQLBuilder builder = new SQLBuilder(null, "", "", "");
            Assert.AreEqual("", builder.Build());
            builder.CreateDatabase();
            Assert.AreEqual("", builder.Build());
        }

        [TestMethod]
        public void CreateDatabaseTest_name()
        {
            SQLBuilder builder = new SQLBuilder(null, "nameDB", "", "");
            Assert.AreEqual("", builder.Build());
            builder.CreateDatabase();
            Assert.AreEqual("IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'nameDB') CREATE DATABASE nameDB;\n", builder.Build());
        }

        [TestMethod]
        public void CreateUse()
        {
            SQLBuilder builder = new SQLBuilder(null, "useME", "", "");
            Assert.AreEqual("", builder.Build());
            builder.CreateUse();
            Assert.AreEqual("USE useME;\n", builder.Build());
        }

        [TestMethod]
        public void CreateTableTest_empty()
        {
            SQLBuilder builder = new SQLBuilder(null, "", "", "");
            Assert.AreEqual("", builder.Build());
            builder.CreateTable();
            Assert.AreEqual("", builder.Build());
        }

        [TestMethod]
        public void CreateTableTest_emptyCSV()
        {
            SQLBuilder builder = new SQLBuilder(new CSVWrapper(), "", "", "");
            Assert.AreEqual("", builder.Build());
            builder.CreateTable();
            Assert.AreEqual("", builder.Build());
        }

        [TestMethod]
        public void CreateTableTest_name_noAttributes()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(FormatPath.formatPath("empty.csv"));
            SQLBuilder builder = new SQLBuilder(csv, "", "Account", "");
            Assert.AreEqual("", builder.Build());
            builder.CreateTable();
            Assert.AreEqual("CREATE TABLE Account (RowID int IDENTITY (1,1) PRIMARY KEY);\n", builder.Build());
        }

        [TestMethod]
        public void CreateTableTest_name_smallCSV()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(FormatPath.formatPath("test1.csv"));
            SQLBuilder builder = new SQLBuilder(csv, "", "Employee", "");
            Assert.AreEqual("", builder.Build());
            builder.CreateTable();
            Assert.AreEqual("CREATE TABLE Employee (RowID int IDENTITY (1,1) PRIMARY KEY, Name text, Gender text, Salary text);\n", builder.Build());
        }

        [TestMethod]
        public void CreateInsertTest_emptyCSV()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(FormatPath.formatPath("empty.csv"));
            SQLBuilder builder = new SQLBuilder(csv, "", "Employee", "");
            Assert.AreEqual("", builder.Build());
            builder.CreateInsert();
            Assert.AreEqual("", builder.Build());
        }

        [TestMethod]
        public void CreateInsertTest_smallCSV()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(FormatPath.formatPath("test1.csv"));
            SQLBuilder builder = new SQLBuilder(csv, "", "Employee", "");
            Assert.AreEqual("", builder.Build());
            builder.CreateInsert();
            string[] insertLines = builder.Build().Split('\n');
            Assert.AreEqual("INSERT INTO Employee(Name, Gender, Salary) VALUES ('Tom', 'M', '20');", insertLines[0]);
            Assert.AreEqual("INSERT INTO Employee(Name, Gender, Salary) VALUES ('Adam', 'M', '30');", insertLines[1]);
            Assert.AreEqual("INSERT INTO Employee(Name, Gender, Salary) VALUES ('Sara', 'F', '40');", insertLines[2]);
            Assert.AreEqual("INSERT INTO Employee(Name, Gender, Salary) VALUES ('Serena', 'F', '50');", insertLines[3]);
        }

        [TestMethod]
        public void CleanStringTestSpace()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(FormatPath.formatPath("space.csv"));
            SQLBuilder builder = new SQLBuilder(csv, "", "Employee", "");
            Assert.AreEqual("", builder.Build());
            builder.CreateInsert();
            string[] insertLines = builder.Build().Split('\n');
            Assert.AreEqual("INSERT INTO Employee(Name, Gender, Salary) VALUES ('Tom_The_Cat', 'M', '20');", insertLines[0]);
            Assert.AreEqual("INSERT INTO Employee(Name, Gender, Salary) VALUES ('Jerry_The_Mouse', 'M', '30');", insertLines[1]);
        }

        [TestMethod]
        public void CleanStringTestSpecialCharacter()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(FormatPath.formatPath("space.csv"));
            SQLBuilder builder = new SQLBuilder(csv, "", "Employee", "");
            Assert.AreEqual("", builder.Build());
            builder.CreateInsert();
            string[] insertLines = builder.Build().Split('\n');
            Assert.AreEqual("INSERT INTO Employee(amt_in, amt1, amt2, sum) VALUES ('10', '50', '50','100');", insertLines[0]);
            Assert.AreEqual("INSERT INTO Employee(amt_in, amt1, amt2, sum) VALUES ('12', '20', '70', '90');", insertLines[1]);
        }
    }
}