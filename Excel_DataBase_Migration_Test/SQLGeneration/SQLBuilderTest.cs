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
        public void createDatabaseTest_empty()
        {
            SQLBuilder builder = new SQLBuilder(null, "", "", "");
            Assert.AreEqual("", builder.build());
            builder.createDatabase();
            Assert.AreEqual("", builder.build());
        }

        [TestMethod]
        public void createDatabaseTest_name()
        {
            SQLBuilder builder = new SQLBuilder(null, "nameDB", "", "");
            Assert.AreEqual("", builder.build());
            builder.createDatabase();
            Assert.AreEqual("IF NOT EXISTS (select * from sys.databases where name = 'nameDB') CREATE DATABASE nameDB;\n", builder.build());
        }

        [TestMethod]
        public void createTableTest_empty()
        {
            SQLBuilder builder = new SQLBuilder(null, "", "", "");
            Assert.AreEqual("", builder.build());
            builder.createTable();
            Assert.AreEqual("", builder.build());
        }

        [TestMethod]
        public void createTableTest_emptyCSV()
        {
            SQLBuilder builder = new SQLBuilder(new CSVWrapper(), "", "", "");
            Assert.AreEqual("", builder.build());
            builder.createTable();
            Assert.AreEqual("", builder.build());
        }

        [TestMethod]
        public void createTableTest_name_noAttributes()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(FormatPath.formatPath("empty.csv"));
            SQLBuilder builder = new SQLBuilder(csv, "", "Account", "");
            Assert.AreEqual("", builder.build());
            builder.createTable();
            Assert.AreEqual("CREATE TABLE Account ();\n", builder.build());
        }

        [TestMethod]
        public void createTableTest_name_smallCSV()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(FormatPath.formatPath("test1.csv"));
            SQLBuilder builder = new SQLBuilder(csv, "", "Employee", "");
            Assert.AreEqual("", builder.build());
            builder.createTable();
            Assert.AreEqual("CREATE TABLE Employee (Name varchar(255), Gender varchar(255), Salary varchar(255));\n", builder.build());
        }

        [TestMethod]
        public void createInsertTest_emptyCSV()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(FormatPath.formatPath("empty.csv"));
            SQLBuilder builder = new SQLBuilder(csv, "", "Employee", "");
            Assert.AreEqual("", builder.build());
            builder.createInsert();
            Assert.AreEqual("", builder.build());
        }

        [TestMethod]
        public void createInsertTest_smallCSV()
        {
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(FormatPath.formatPath("test1.csv"));
            SQLBuilder builder = new SQLBuilder(csv, "", "Employee", "");
            Assert.AreEqual("", builder.build());
            builder.createInsert();
            Assert.AreEqual("INSERT INTO Employee((Name, Gender, Salary)VALUES (Tom, M, 20);\nINSERT INTO Employee((Name, Gender, Salary)VALUES (Adam, M, 30);\nINSERT INTO Employee((Name, Gender, Salary)VALUES (Sara, F, 40);\nINSERT INTO Employee((Name, Gender, Salary)VALUES (Serena, F, 50);\n", builder.build());
        }
    }
}