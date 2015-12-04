﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel_Database_Migration.ExcelUtils;
using System.IO;

namespace Excel_Database_Migration.SQLGeneration
{
    public class SQLBuilder
    {
        private CSVWrapper csv;
        private string schemaName;
        private string tableName;
        private string[] datatype;
        private StringBuilder builder;

        public SQLBuilder(CSVWrapper csv, string schemaName, string tableName, string datatypePath)
        {
            this.csv = csv;
            
            this.schemaName = schemaName;
            this.tableName = tableName;
            CleanNames();
            builder = new StringBuilder();
            datatype = null;
            if (!string.IsNullOrEmpty(datatypePath) && File.Exists(datatypePath))
            {
                datatype = File.ReadAllText(datatypePath).Split(',');
            }
        }

        private void CleanNames()
        {
            schemaName = CleanString(schemaName);
            tableName = CleanString(tableName);
        }
        
        public SQLBuilder DropTable()
        {
            builder.Append(String.Format(
                "If Exists(Select object_id From sys.tables Where name = '{0}') Drop Table {1};\n", tableName, tableName));
            return this;
        }

        /// <summary>
        /// 
        /// @Deprecated
        /// </summary>
        /// <returns></returns>
        public SQLBuilder createSchema()
        {
            if (schemaName=="" || schemaName== null)
            {
                return this;
            }
            builder.Append(String.Format("CREATE SCHEMA {0};\n", schemaName));
            return this;
        }

        public SQLBuilder CreateDatabase()
        {
            if (schemaName == "" || schemaName == null)
            {
                return this;
            }
            builder.Append(String.Format("IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{0}') CREATE DATABASE {0};\n", schemaName, schemaName));
            return this;
        }
        
        public SQLBuilder CreateUse()
        {
            builder.Append(String.Format("USE {0};\n", schemaName));
            return this;
        }

        public SQLBuilder CreateTable()
        {
            if (tableName=="" || tableName== null)
            {
                return this;
            }
            builder.Append(String.Format("CREATE TABLE {0} (", tableName));
            builder.Append("RowID int IDENTITY (1,1) PRIMARY KEY");
            if (csv.Attributes == null)
            {
                builder.Append(");\n");
                return this;
            }
            for (int i = 0; i<csv.Attributes.Length; i++)
            {
                builder.Append(", ");
                builder.Append(CleanString(csv.Attributes[i]));
                if (datatype == null)
                {
                    builder.Append(" text");
                }
                else
                {
                    builder.Append(" " + datatype[i]);
                }
            }
            builder.Append(");\n");
            return this;
        }

        /**
        *Writes the SQL Statement for all the rows of data
        */
        public SQLBuilder CreateInsert()
        {
            if (csv.Attributes== null)
            {
                return this;
            }
            string attributes = formatAttributes();
            for (int i = 0; i<csv.Data.Count; i++)
            {
                createInsertRow(attributes, i);
                if (i != csv.Data.Count - 1)
                {
                    builder.Append("\n");
                }
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
            builder.Append(String.Format("INSERT INTO {0}", tableName));
            builder.Append(attributes);
            builder.Append(" VALUES (");
            
            for (int j = 0; j<csv.Data[index].Length; j++)
            {
                builder.Append("'");
                builder.Append(csv.Data[index][j]);
                builder.Append("'");
                if (j != csv.Data[index].Length - 1)
                {
                    builder.Append(", ");
                }
            }
            builder.Append(");");
            return this;
        }

        private String formatAttributes()
        {
            StringBuilder attributeBuilder = new StringBuilder();
            attributeBuilder.Append("(");
            for (int i = 0; i < csv.Attributes.Length; i++)
            {
                attributeBuilder.Append(CleanString(csv.Attributes[i]));
                if (i != csv.Attributes.Length - 1)
                {
                    attributeBuilder.Append(", ");
                }
            }
            attributeBuilder.Append(")");
            return attributeBuilder.ToString();
        }

        public string Build()
        {
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// Gets rid of dirty characters like μ
        /// </summary>
        /// <param name="dirty"></param>
        /// <returns></returns>
        private string CleanString(string dirty)
        {
            return dirty.Replace("\\W", "").Replace(" ", "_");
        }
        
    }
}
