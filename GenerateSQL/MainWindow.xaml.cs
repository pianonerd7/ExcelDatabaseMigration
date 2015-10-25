using GenerateSQL.ExcelUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GenerateSQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //InitializeComponent();

            /*
            string sourceFile, worksheetName, targetFile;
            sourceFile = "Test.xlsx"; worksheetName = "Sheet1"; targetFile = "target.csv";

            XLSXToCSVConverter Converter = new XLSXToCSVConverter();
            Converter.Convert(sourceFile, targetFile, worksheetName);
             */


            string sourceFile, worksheetName, targetFile;
            sourceFile = "source.xls"; worksheetName = "sheet1"; targetFile = "target.csv";

            XLSXToCSVConverter Converter = new XLSXToCSVConverter();
            Converter.convertExcelToCSV(sourceFile, worksheetName, targetFile);

        }
    }
}
