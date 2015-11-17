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

        public DataTable SelectQuery(string select, string dbName, string condition)
        {
            string query = 
                String.Format("SELECT {0} FROM dbo.{1} {2}", select, dbName, condition);
            
            return dbAccess.GetQuery(query);
            
        }

        public int DeleteQuery(string dbName, string condition)
        {
            string query =
                String.Format("DELETE FROM {0} WHERE {1}", dbName, condition);

            return dbAccess.GetNonQuery(query);
        }

        public int InsertQuery(string dbName, string parameters, string values)
        {
            string query = 
                String.Format("INSERT {0} ({1}) VALUES ({1})", dbName, parameters, values);

            return dbAccess.GetNonQuery(query);
        }

        public int UpdateQuery(string tableName, string parameters, string newValues)
        {
            string query =
                String.Format("UPDATE dbo.{0} SET {1} where {2}", tableName, parameters, newValues);
            
            return dbAccess.GetNonQuery(query);
        }
        #endregion
    }
}
