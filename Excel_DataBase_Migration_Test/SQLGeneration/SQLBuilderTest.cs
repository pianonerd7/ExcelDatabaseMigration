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
            Assert.AreEqual("", builder.build());
            builder.createSchema();
            Assert.AreEqual("", builder.build());
        }

        [TestMethod]
        public void DropTableTest_name()
        {
            SQLBuilder builder = new SQLBuilder(null, "", "nameTable", "");
            Assert.AreEqual("", builder.build());
            builder.dropTable();
            Assert.AreEqual("If Exists(Select object_id From sys.tables Where name = 'nameTable') Drop Table nameTable;\n", builder.build());
        }

        [TestMethod]
        public void CreateDatabaseTest_empty()
        {
            SQLBuilder builder = new SQLBuilder(null, "", "", "");
            Assert.AreEqual("", builder.build());
            builder.createDatabase();
            Assert.AreEqual("", builder.build());
        }

        [TestMethod]
        public void CreateDatabaseTest_name()
        {
            SQLBuilder builder = new SQLBuilder(null, "nameDB", "", "");
            Assert.AreEqual("", builder.build());
            builder.createDatabase();
            Assert.AreEqual("IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'nameDB') CREATE DATABASE nameDB;\n", builder.build());
        }

        [TestMethod]
        public void CreateUse()
        {
            SQLBuilder builder = new SQLBuilder(null, "useME", "", "");
            Assert.AreEqual("", builder.build());
            builder.createUse();
            Assert.AreEqual("USE useME;\n", builder.build());
        }

        [TestMethod]
        public void CreateTableTest_empty()
        {
            SQLBuilder builder = new SQLBuilder(null, "", "", "");
            Assert.AreEqual("", builder.build());
            builder.createTable();
            Assert.AreEqual("", builder.build());
        }

        [TestMethod]
        public void CreateTableTest_emptyCSV()
        {
            SQLBuilder builder = new SQLBuilder(new CSVWrapper(), "", "", "");
            Assert.AreEqual("", builder.build());
            builder.createTable();
            Assert.AreEqual("", builder.build());
        }

        [TestMethod]
        public void CreateTableTest_name_noAttributes()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(FormatPath.formatPath("empty.csv"));
            SQLBuilder builder = new SQLBuilder(csv, "", "Account", "");
            Assert.AreEqual("", builder.build());
            builder.createTable();
            Assert.AreEqual("CREATE TABLE Account (RowID int IDENTITY (1,1) PRIMARY KEY);\n", builder.build());
        }

        [TestMethod]
        public void CreateTableTest_name_smallCSV()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(FormatPath.formatPath("test1.csv"));
            SQLBuilder builder = new SQLBuilder(csv, "", "Employee", "");
            Assert.AreEqual("", builder.build());
            builder.createTable();
            Assert.AreEqual("CREATE TABLE Employee (RowID int IDENTITY (1,1) PRIMARY KEY, Name text, Gender text, Salary text);\n", builder.build());
        }

        [TestMethod]
        public void CreateInsertTest_emptyCSV()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(FormatPath.formatPath("empty.csv"));
            SQLBuilder builder = new SQLBuilder(csv, "", "Employee", "");
            Assert.AreEqual("", builder.build());
            builder.createInsert();
            Assert.AreEqual("", builder.build());
        }

        [TestMethod]
        public void CreateInsertTest_smallCSV()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(FormatPath.formatPath("test1.csv"));
            SQLBuilder builder = new SQLBuilder(csv, "", "Employee", "");
            Assert.AreEqual("", builder.build());
            builder.createInsert();
            string[] insertLines = builder.build().Split('\n');
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
            Assert.AreEqual("", builder.build());
            builder.createInsert();
            string[] insertLines = builder.build().Split('\n');
            Assert.AreEqual("INSERT INTO Employee(Name, Gender, Salary) VALUES ('Tom_The_Cat', 'M', '20');", insertLines[0]);
            Assert.AreEqual("INSERT INTO Employee(Name, Gender, Salary) VALUES ('Jerry_The_Mouse', 'M', '30');", insertLines[1]);
        }

        [TestMethod]
        public void CleanStringTestSpecialCharacter()
        {
            
        }
    }
}