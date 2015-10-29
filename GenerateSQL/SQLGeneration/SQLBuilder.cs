using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateSQL.ExcelUtils;

namespace GenerateSQL.SQLGeneration
{
    class SQLBuilder
    {
        private CSVWrapper csv;
        private string schemaName;
        private string tableName;
        private string sqlPath;
        private StringBuilder builder;

        public SQLBuilder(CSVWrapper csv, string schemaName, string tableName, string sqlPath)
        {
            this.csv = csv;
            this.schemaName = schemaName;
            this.sqlPath = sqlPath;
            builder = new StringBuilder();
        }

        public SQLBuilder createSchema()
        {
            builder.Append(String.Format("{0}", schemaName));
            return this;
        }

        public SQLBuilder createTable()
        {
            builder.Append(String.Format("CREATE TABLE {0} (", tableName));
            for (int i = 0; i<csv.Attributes.Length; i++)
            {
                builder.Append(csv.Attributes[i]);
                builder.Append(" varchar(255)");
                if (i != csv.Attributes.Length - 1)
                {
                builder.Append(", ");
                }
            }
            builder.Append(")");
            return this;
        }

        public SQLBuilder createInsert()
        {
            builder.Append(String.Format("INSERT "));
            return this;
        }
    }
}
