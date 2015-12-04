﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Windows;

using Excel_Database_Migration.ExcelUtils;
using Excel_Database_Migration.DatabaseAccess;


namespace Excel_Database_Migration.SQLGeneration
{
    public class SQLGenerator
    {
        public static void generate (string xlsxPath, string datatypePath = null)
        {
            string filename = Path.GetFileNameWithoutExtension(xlsxPath);
            filename = filename.Replace(" ", "_");
            string pathWOExtension = Path.GetDirectoryName(xlsxPath)+ "\\" + filename;

            DatabaseInfo.DatabaseName = filename;
            //convert from xlsx to csv
            string csvPath =  pathWOExtension + ".csv";
            Console.WriteLine("csvPath is: " + csvPath);
            XLSXToCSVConverter.toCSV(xlsxPath, csvPath);

            //represent csv as an object
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(csvPath);

            //make the contents of the sql script
            string sqlPath = pathWOExtension + ".sql";
            Console.WriteLine("sqlPath is: " + sqlPath);
            string sqlContent = new SQLBuilder(csv, filename, filename+"Table", datatypePath).
                CreateDatabase().DropTable().CreateUse().CreateTable().CreateInsert().Build();
            //make the contents of the connection string file
            string dbPath = pathWOExtension + ProjectStrings.CONNECTION_STRING_FILE_EXTENSION;
            string dbContent = DatabaseInfo.DatabaseName;

            //write sql to file
            System.IO.File.WriteAllText(sqlPath, sqlContent);
            System.IO.File.WriteAllText(dbPath, dbContent);
            string[] lines = sqlContent.Split('\n');
            createDatabaseFromSql(lines, filename);
            populateDatabaseFromSql(lines, filename);
        }

        public static string createConnectionStringFromDbName(string dbName)
        {
            return string.Format("Server=localhost;Integrated security=True;database={0}", dbName);
        }

        public static void generateFromSql(string sqlPath)
        {
            string[] lines = File.ReadAllLines(sqlPath);
            string filename = Path.GetFileNameWithoutExtension(sqlPath);
            createDatabaseFromSql(lines, filename);
            populateDatabaseFromSql(lines, filename);
        }

        private static void createDatabaseFromSql(string[] lines, string dbName)
        {
            string connectionString ="Server=localhost;Integrated security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = null;
            try
            {
                command = new SqlCommand(lines[0], connection);
                command.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
                MessageBox.Show(command.CommandText, ProjectStrings.APPLICATION_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                MessageBox.Show(e.ToString(), ProjectStrings.APPLICATION_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            connection.Close();
        }
        
        /// <summary>
        /// Goes through each line of the sql script and
        /// executes a non query on them
        /// </summary>
        /// <param name="sqlContent"></param>
        /// <param name="dbName"></param>
        private static void populateDatabaseFromSql(string[] lines, string dbName )
        {
            string connectionString = createConnectionStringFromDbName(dbName);
            DatabaseInfo.ConnectionString = connectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            
            foreach (string line in lines)
            {
                if (line == "")
                {
                    continue;
                }
                SqlCommand command = null;
                try
                {
                    command = new SqlCommand(line, connection);
                    command.ExecuteNonQuery();
                }
                catch (System.Exception e)
                {
                    MessageBox.Show(command.CommandText, ProjectStrings.APPLICATION_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                    MessageBox.Show(e.ToString(), ProjectStrings.APPLICATION_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            connection.Close();
        }
    }
}
