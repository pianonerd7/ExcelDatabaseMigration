using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Excel_Database_Migration.ExcelUtils
{
    //uses VB's FileIO so that framework needs to be referenced
    public class CSVWrapper 
    {
        private string[] attributes;
        private List<string[]> data;

        public CSVWrapper()
        {
            data = new List<string[]>();
        }
        
        public CSVWrapper(string[] attributes, List<string[]> data)
        {
            this.attributes = attributes;
            this.data = data;
        }

        public string[] Attributes
        {
            get { return this.attributes; }
            set { this.attributes = value; }
        }

        public List<string[]> Data
        {
            get { return this.data; }
        }

        public void addRow(string[] row)
        {
            data.Add(row);
        }

        public void openCSV(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(String.Format("csv file was not found at: {0}", path));
            }
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                string[] fields = parser.ReadFields();
                attributes = fields;
                while (!parser.EndOfData)
                {
                    fields = parser.ReadFields();
                    data.Add(fields);
                }
            }
        }

        public void writeCSV(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            List<String> csvFileContent = new List<string>();
            csvFileContent.Add(string.Join(",", attributes));
            foreach(string[] line in data)
            {
                if (line == null)
                {
                    continue;
                }
                csvFileContent.Add(string.Join(",", line));
            }

            File.WriteAllLines(path, csvFileContent.ToArray<string>());
        }
    }
}
