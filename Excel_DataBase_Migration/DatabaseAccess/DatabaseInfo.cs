using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel_Database_Migration.DatabaseAccess
{
    public static class DatabaseInfo
    {
        private static string connectionString;
        public static string ConnectionString {
            get { return connectionString; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }
                connectionString = value;
            }
        }

        private static string databaseName;
        public static string DatabaseName
        {
            get { return databaseName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }
                databaseName = value;
            }
        }


    }
}
