using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelDatabaseMigrationTest.TestHelpers
{
    class FormatPath
    {
        public static string formatPath(string filename)
        {
            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);
            

            DirectoryInfo twoup = d.Parent.Parent;
            
            return twoup.FullName.ToString() + "\\" + "testData" + "\\" + filename;
        }
    }
}
