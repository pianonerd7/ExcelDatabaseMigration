using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateSQL.ExcelUtils
{
    public class XLSXToCSVConverter
    {

        public void convert(string xlsxInputPath, string csvOutputPath, string worksheetName)
        {
            if (!File.Exists(xlsxInputPath))
            {
                throw new FileNotFoundException();
            }
            if (!File.Exists(csvOutputPath))
            {
                throw new ArgumentException();
            }

            var connectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};;Extended Properties=\" Excel.0;HDR=Yes;IMEX=1\"", xlsxInputPath);

            OleDbConnection connection = null;
            StreamWriter writer = null;
            OleDbCommand command = null;
            OleDbDataAdapter dataAdapter = null;

            try
            {
                connection = new OleDbConnection(connectionString);
                connection.Open();

                command = new OleDbCommand("SELECT * FROM [" + worksheetName + "$]", connection);
                command.CommandType = CommandType.Text;
                writer = new StreamWriter(csvOutputPath);

                dataAdapter = new OleDbDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string rowString = "";

                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        rowString += "\"" + dataTable.Rows[i][j].ToString() + "\",";
                    }

                    writer.WriteLine(rowString);
                }
            }
            catch(Exception e)
            {
                //TODO handle exception
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                command.Dispose();
                dataAdapter.Dispose();
                writer.Close();
                writer.Dispose();
            }
        }
   }
}