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
            csv.Attributes = attributes.ToArray<string>();
            foreach(DataRow row in table.Rows)
            {

                List<string> dataRow = new List<string>();
                foreach(DataColumn col in table.Columns)
                {
                    dataRow.Add("" + row[col]);
                }
                csv.addRow(dataRow.ToArray<string>());
            }
            csv.writeCSV(path);
        }

    }
}
