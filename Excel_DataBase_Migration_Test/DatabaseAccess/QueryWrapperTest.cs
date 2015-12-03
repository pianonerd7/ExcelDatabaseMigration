using System;
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
        public void TestInsert()
        {

        }

        private void CreateTestDatabase()
        {
            SQLGenerator.generateFromSql(FormatPath.formatPath("testDB.sql"));
        }

        private void Populate()
        {

        }
    }
}
