using Excel_Database_Migration.ExcelUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Excel_Database_Migration.ViewModel
{
    public class DatabaseAccessViewModel : ViewModelBase
    {
         #region Private Declaration

        private Window _mainWindow;
        private DataTable _queryData;

        #endregion

        #region Constructor
        public DatabaseAccessViewModel(Window window)
        {
            this._mainWindow = window;
            testData();
        }

        private void testData()
        {
            DataTable table = new DataTable("Test");

            table.Columns.Add("Name");
            table.Columns.Add("Animal");
            table.Columns.Add("Number");
            table.Columns.Add("Fruit");
            
            DataRow row1 = table.NewRow();
            row1["Name"] = "Bob";
            row1["Animal"] = "Dog";
            row1["Number"] = "100";
            row1["Fruit"] = "Banana";

            table.Rows.Add(row1);
            _queryData = table;
        }

        #endregion

        #region Public Variables

        public DataTable QueryData
        {
            get
            {
                return _queryData;
            }
        }

        #endregion

        #region Private Methods

        private void populateGrid(DataTable dataList)
        {

        }

        #endregion

    }
}
