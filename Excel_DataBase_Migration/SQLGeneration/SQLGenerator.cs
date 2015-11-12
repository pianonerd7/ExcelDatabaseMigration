using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using Excel_Database_Migration.ExcelUtils;
using System.Windows;

namespace Excel_Database_Migration.SQLGeneration
{
    public class SQLGenerator
    {
        public static void generate (string xlsxPath, string datatypePath = null)
        {
            string filename = Path.GetFileNameWithoutExtension(xlsxPath);
            string pathWOExtension = Path.GetDirectoryName(xlsxPath)+ "\\" + filename;
            
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
            string sqlContent = new SQLBuilder(csv, filename, filename, datatypePath).
                createSchema().createTable().createInsert().build();
            //createDatabaseFromSql(filename);
            populateDatabaseFromSql(sqlContent, filename);
            //write sql to file
            System.IO.File.WriteAllText(sqlPath, sqlContent);
        }

        private static string createConnectionStringFromDbName(string dbName)
        {
            return string.Format("Server=localhost;Integrated security=True;database=master", dbName); 
        }

        private static void createDatabaseFromSql(string dbName)
        {
            string connectionString = createConnectionStringFromDbName(dbName);
            SqlConnection connection = new SqlConnection(connectionString);
            string databaseQuery1 = string.Format("CREATE DATABASE {0} ON PRIMARY ", dbName) +
                string.Format("(NAME = {0}, ", dbName) +
                string.Format("FILENAME = '{0}.mdf', ", dbName) +
                "SIZE = 10MB, MAXSIZE = 2GB, FILEGROWTH = 10%) " +
                string.Format("LOG ON (NAME = {0}_Log, ", dbName) +
                string.Format("FILENAME = '{0}_Log.ldf', ", dbName) +
                "SIZE = 5MB, MAXSIZE = 1GB, FILEGROWTH = 10%)";
            string databaseQuery = string.Format("CREATE DATABASE {0} IF NOT EXISTS", dbName);
            SqlCommand creationCommand = new SqlCommand(databaseQuery, connection);
            try
            {
                connection.Open();
                creationCommand.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.ToString(), ProjectStrings.APPLICATION_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private static void populateDatabaseFromSql(string sqlContent, string dbName )
        {
            string connectionString = createConnectionStringFromDbName(dbName);
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string[] lines = sqlContent.Split('\n');
            foreach (string line in lines)
            {
                try
                {
                    SqlCommand command = new SqlCommand(line, connection);
                    command.ExecuteNonQuery();
                }
                catch (System.Exception e)
                {
                    MessageBox.Show(e.ToString(), ProjectStrings.APPLICATION_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            connection.Close();
        }
    }
}
