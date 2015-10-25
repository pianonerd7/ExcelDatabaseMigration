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

        public static void convertExcelToCSV(string sourceFile, string worksheetName, string targetFile)
        {
            //Msexcl40.dll is needed
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sourceFile + ";Extended Properties=\" Excel 8.0;HDR=Yes;IMEX=1\"";
            OleDbConnection conn = null;
            StreamWriter wrtr = null;
            OleDbCommand cmd = null;
            OleDbDataAdapter da = null;
            try
            {
                conn = new OleDbConnection(strConn);
                conn.Open();

                cmd = new OleDbCommand("SELECT * FROM [" + worksheetName + "$]", conn);
                cmd.CommandType = CommandType.Text;
                
                wrtr = new StreamWriter(targetFile);

                da = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    string rowString = "";
                    for (int y = 0; y < dt.Columns.Count; y++)
                    {
                        rowString += "\"" + dt.Rows[x][y].ToString() + "\",";
                    }
                    wrtr.WriteLine(rowString);
                }
                Console.WriteLine();
                Console.WriteLine("Done! Your " + sourceFile + " has been converted into " + targetFile + ".");
                Console.WriteLine();
            }
            catch (Exception exc)
            {
                Console.WriteLine("error happened");
                Console.WriteLine(exc.ToString());
                Console.ReadLine();
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                conn.Dispose();
                cmd.Dispose();
                da.Dispose();
                wrtr.Close();
                wrtr.Dispose();
            }
        }
   }
}