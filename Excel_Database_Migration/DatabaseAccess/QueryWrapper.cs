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
            dbAccess = new DatabaseAccess(); //need to insert connection string to constructor
        }

        #region Query Methods

        public DataTable SelectQuery(string select, string tableName)
        {
            string query = 
                String.Format("SELECT {0} FROM dbo.{1} ", select, tableName);
            
            return dbAccess.GetQuery(query);
            
        }
        #endregion
    }
}
