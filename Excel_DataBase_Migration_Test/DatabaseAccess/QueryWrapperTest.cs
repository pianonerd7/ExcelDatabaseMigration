﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Excel_DataBase_Migration_Test.TestHelpers;
using Excel_Database_Migration.DatabaseAccess;
using System.Data;
using Excel_Database_Migration.SQLGeneration;

namespace Excel_Database_Migration_Test.DatabaseAccess
{
    [TestClass]
    public class QueryWrapperTest
    {
        private const string testDBName = "testDB";
        private string connectionString = "Server=localhost;Integrated security=True;database=TestDB";
        private QueryWrapper query;

        [TestMethod]
        public void TestSearch_empty()
        {
            CreateTestDatabase();
            query = new QueryWrapper(connectionString);
            
            DataTable table = query.SelectQuery("*", testDBName);
            Assert.AreEqual(0, table.Rows.Count);
        }

        [TestMethod]
        public void TestInsert_singleValue()
        {
            CreateTestDatabase();
            query = new QueryWrapper(connectionString);

            query.InsertQuery(testDBName, "Name", "'Bob'");
            DataTable table = query.SelectQuery("*", testDBName);
            Assert.AreEqual(1, table.Rows.Count);
            Assert.AreEqual("Bob", table.Rows[0]["Name"].ToString());
        }

        [TestMethod]
        public void TestInsert_allValue()
        {
            CreateTestDatabase();
            query = new QueryWrapper(connectionString);

            query.InsertQuery(testDBName, "Name, Gender, Salary", "'Bob', 'M', '80'");
            DataTable table = query.SelectQuery("*", testDBName);
            Assert.AreEqual(1, table.Rows.Count);
            Assert.AreEqual("Bob", table.Rows[0]["Name"].ToString());
            Assert.AreEqual("M", table.Rows[0]["Gender"].ToString());
            Assert.AreEqual("80", table.Rows[0]["Salary"].ToString());
        }

        [TestMethod]
        public void TestInsert_allValue_withNULL()
        {
            CreateTestDatabase();
            query = new QueryWrapper(connectionString);

            query.InsertQuery(testDBName, "Name, Gender, Salary", "'Bob', NULL, '80'");
            DataTable table = query.SelectQuery("*", testDBName);
            Assert.AreEqual(1, table.Rows.Count);
            Assert.AreEqual("Bob", table.Rows[0]["Name"].ToString());
            Assert.AreEqual("", table.Rows[0]["Gender"].ToString());
            Assert.AreEqual("80", table.Rows[0]["Salary"].ToString());
        }

        [TestMethod]
        public void TestUpdate_singleValue()
        {
            CreateTestDatabase();
            query = new QueryWrapper(connectionString);

            query.InsertQuery(testDBName, "Name, Gender, Salary", "'Bob', NULL, '80'");
            DataTable table = query.SelectQuery("*", testDBName);

            query.UpdateQuery(testDBName, "Salary", "120", "1");

            table = query.SelectQuery("*", testDBName);

            Assert.AreEqual(1, table.Rows.Count);
            Assert.AreEqual("Bob", table.Rows[0]["Name"].ToString());
            Assert.AreEqual("", table.Rows[0]["Gender"].ToString());
            Assert.AreEqual("120", table.Rows[0]["Salary"].ToString());
        }

        [TestMethod]
        public void TestUpdate_singleValue_moreThanOneRow()
        {
            CreateTestDatabase();
            query = new QueryWrapper(connectionString);

            query.InsertQuery(testDBName, "Name, Gender, Salary", "'Bob', NULL, '80'");
            query.InsertQuery(testDBName, "Name, Gender, Salary", "'Tom', NULL, '84'");
            query.InsertQuery(testDBName, "Name, Gender, Salary", "'Teresa', NULL, '100'");
            DataTable table = query.SelectQuery("*", testDBName);

            query.UpdateQuery(testDBName, "Salary", "120", "2");

            table = query.SelectQuery("*", testDBName);

            Assert.AreEqual(3, table.Rows.Count);
            Assert.AreEqual("Tom", table.Rows[1]["Name"].ToString());
            Assert.AreEqual("", table.Rows[1]["Gender"].ToString());
            Assert.AreEqual("120", table.Rows[1]["Salary"].ToString());
        }
        
        [TestMethod]
        public void TestDelete_rowID()
        {
            CreateTestDatabase();
            query = new QueryWrapper(connectionString);

            query.InsertQuery(testDBName, "Name, Gender, Salary", "'Bob', NULL, '80'");
            query.InsertQuery(testDBName, "Name, Gender, Salary", "'Tom', NULL, '84'");
            query.InsertQuery(testDBName, "Name, Gender, Salary", "'Teresa', NULL, '100'");
            DataTable table = query.SelectQuery("*", testDBName);

            Assert.AreEqual(3, table.Rows.Count);
            query.DeleteQuery(testDBName, "rowID = 1");

            table = query.SelectQuery("*", testDBName);

            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual("Tom", table.Rows[0]["Name"].ToString());
            Assert.AreEqual("", table.Rows[0]["Gender"].ToString());
            Assert.AreEqual("84", table.Rows[0]["Salary"].ToString());


            Assert.AreEqual("Teresa", table.Rows[1]["Name"].ToString());
            Assert.AreEqual("", table.Rows[1]["Gender"].ToString());
            Assert.AreEqual("100", table.Rows[1]["Salary"].ToString());

        }

        private void CreateTestDatabase()
        {
            SQLGenerator.generateFromSql(FormatPath.formatPath("testDB.sql"));
        }
        
    }
}
