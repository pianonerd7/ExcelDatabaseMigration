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
            string sqlContent = new SQLBuilder(csv, filename, filename+"Table", datatypePath).
                createDatabase().dropTable().createUse().createTable().createInsert().build();

            //write sql to file
            System.IO.File.WriteAllText(sqlPath, sqlContent);
            string[] lines = sqlContent.Split('\n');
            createDatabaseFromSql(lines, filename);
            populateDatabaseFromSql(lines, filename);
        }

        public static string createConnectionStringFromDbName(string dbName)
        {
            return string.Format("Server=localhost;Integrated security=True;database={0}", dbName);
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
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            
            foreach (string line in lines)
            {
                SqlCommand command = null;
                try
                {
                    command = new SqlCommand(line, connection);
                    //command.CommandText = line;
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
