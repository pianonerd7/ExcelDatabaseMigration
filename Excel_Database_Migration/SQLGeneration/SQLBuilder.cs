﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel_Database_Migration.ExcelUtils;

namespace Excel_Database_Migration.SQLGeneration
{
    public class SQLBuilder
    {
        private CSVWrapper csv;
        private string schemaName;
        private string tableName;
        private string datatypePath;
        private StringBuilder builder;

        public SQLBuilder(CSVWrapper csv, string schemaName, string tableName, string datatypePath)
        {
            this.csv = csv;
            this.schemaName = schemaName;
            this.tableName = tableName;
            this.datatypePath = datatypePath;
            builder = new StringBuilder();
        }

        public SQLBuilder createSchema()
        {
            if (schemaName=="" || schemaName== null)
            {
                return this;
            }
            builder.Append(String.Format("CREATE SCHEMA IF NOT EXISTS {0};\n", schemaName));
            return this;
        }

        public SQLBuilder createTable()
        {
            if (tableName=="" || tableName== null)
            {
                return this;
            }
            builder.Append(String.Format("CREATE TABLE {0} (", tableName));
            if (csv.Attributes == null)
            {
                builder.Append(");\n");
                return this;
            }
            for (int i = 0; i<csv.Attributes.Length; i++)
            {
                builder.Append(csv.Attributes[i]);
                builder.Append(" varchar(255)");
                if (i != csv.Attributes.Length - 1)
                {
                builder.Append(", ");
                }
            }
            builder.Append(");\n");
            return this;
        }

        /**
        *Writes the SQL Statement for all the rows of data
        */
        public SQLBuilder createInsert()
        {
            if (csv.Attributes== null)
            {
                return this;
            }
            string attributes = formatAttributes();
            for (int i = 0; i<csv.Data.Count; i++)
            {
                createInsertRow(attributes, i);
            }
            return this;
        }

        /*
        *Writes the SQL Statement for one row insert
        *
        *Insert Statement in SQL looks like
        *INSERT INTO table_name (attributes) VALUES(values);
        */
        private SQLBuilder createInsertRow(String attributes, int index)
        {
            builder.Append(String.Format("INSERT INTO {0}(", tableName));
            builder.Append(attributes);
            builder.Append("VALUES (");
            
            for (int j = 0; j<csv.Data[index].Length; j++)
            {
                builder.Append(csv.Data[index][j]);
                if (j != csv.Data[index].Length - 1)
                {
                    builder.Append(", ");
                }
            }
            builder.Append(");\n");
            return this;
        }

        private String formatAttributes()
        {
            StringBuilder attributeBuilder = new StringBuilder();
            attributeBuilder.Append("(");
            for (int i = 0; i < csv.Attributes.Length; i++)
            {
                attributeBuilder.Append(csv.Attributes[i]);
                if (i != csv.Attributes.Length - 1)
                {
                    attributeBuilder.Append(", ");
                }
            }
            attributeBuilder.Append(")");
            return attributeBuilder.ToString();
        }

        public string build()
        {
            return builder.ToString();
        }
    }
}
