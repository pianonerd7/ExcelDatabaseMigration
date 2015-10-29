using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


using Excel_Database_Migration.ExcelUtils;
using Excel_Database_Migration.SQLGeneration;

namespace Excel_Database_Migration.SQLGeneration
{
    class SQLGenerator
    {
        public static void generate (string xlsxPath, string datatypePath = null)
        {
            string filename = Path.GetFileNameWithoutExtension(xlsxPath);
            string pathWOExtension = Path.GetDirectoryName(xlsxPath) + filename;
            
            //convert from xlsx to csv
            string csvPath =  pathWOExtension + ".csv";
            XLSXToCSVConverter.toCSV(xlsxPath, csvPath);

            //represent csv as an object
            CSVWrapper csv = new CSVWrapper();
            csv.openCSV(csvPath);

            //make the contents of the sql script
            string sqlPath = pathWOExtension + ".sql";
            string sqlContent = new SQLBuilder(csv, filename, filename, datatypePath).
                createSchema().createTable().createInsert().build();

            //write sql to file
            System.IO.File.WriteAllText(sqlPath, sqlContent);
        }
    }
}
