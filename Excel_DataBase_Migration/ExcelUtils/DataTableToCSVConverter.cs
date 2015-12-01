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
        public static void convertToCSV(DataTable table, string path)
        {
            CSVWrapper csv = new CSVWrapper();

            List<string> attributes = new List<string>();
            foreach (DataColumn dc in table.Columns)
            {
                attributes.Add(dc.ColumnName);
            }

            foreach(DataRow row in table.Rows)
            {
                csv.addRow(row.ItemArray as string[]);
            }
            csv.writeCSV(path);
        }

    }
}
