using Excel_Database_Migration.SQLGeneration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel_Database_Migration.DatabaseAccess
{
    public class QueryWrapper
    {
        DatabaseAccess dbAccess;

        public QueryWrapper()
        {
            dbAccess = new DatabaseAccess(SQLGenerator.createConnectionStringFromDbName(DatabaseInfo.DatabaseName)); //need to insert connection string to constructor
        }

        #region Query Methods
        public DataTable SelectQuery(string select, string dbName)
        {
            string query =
                String.Format("SELECT {0} FROM {1}Table", select, dbName);

            return dbAccess.GetQuery(query);
        }

        public DataTable SelectQuery(string select, string dbName, string condition)
        {
            string query = 
                String.Format("SELECT {0} FROM {1}Table WHERE {2}", select, dbName, condition);
            
            return dbAccess.GetQuery(query);
        }

        public int DeleteQuery(string dbName, string condition)
        {
            string query =
                String.Format("DELETE FROM {0}Table WHERE {1}", dbName, condition);

            return dbAccess.GetNonQuery(query);
        }

        public int InsertQuery(string dbName, string parameters, string values)
        {
            string query = 
                String.Format("INSERT INTO {0}Table ({1}) VALUES ({2})", dbName, parameters, values);
            Console.WriteLine("query is: "+query);
            return dbAccess.GetNonQuery(query);
        }

        public int UpdateQuery(string tableName, string parameters, string newValues, string rowID)
        {
            string query =
                String.Format("UPDATE {0}Table SET {1}='{2}' WHERE RowID='{3}'", tableName, parameters, newValues, rowID);
            Console.WriteLine("query is: " + query);

            return dbAccess.GetNonQuery(query);
        }
        #endregion
    }
}
