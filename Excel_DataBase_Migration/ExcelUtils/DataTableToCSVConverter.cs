using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel_Database_Migration.ExcelUtils
{
    public class DataTableToCSVConverter
    {
        /// <summary>
        /// 
        /// Actually writes the CSVWrapper object into file
        /// </summary>
        /// <param name="table"></param>
        /// <param name="path"></param>
        public static void WriteDataTableAsCSV(DataTable table, string path)
        {
            CSVWrapper csv = ConvertToCSV(table);
            csv.writeCSV(path);
        }
        
        /// <summary>
        /// 
        /// Converts a Data Table into a CSVWrapper object
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static CSVWrapper ConvertToCSV(DataTable table)
        {
            CSVWrapper csv = new CSVWrapper();

            List<string> attributes = new List<string>();
            foreach (DataColumn dc in table.Columns)
            {
                attributes.Add(dc.ColumnName);
            }
            csv.Attributes = attributes.ToArray<string>();
            foreach (DataRow row in table.Rows)
            {
                List<string> dataRow = new List<string>();
                foreach (DataColumn col in table.Columns)
                {
                    dataRow.Add("" + row[col]);
                }
                csv.addRow(dataRow.ToArray<string>());
            }
            return csv;
        }

    }
}
