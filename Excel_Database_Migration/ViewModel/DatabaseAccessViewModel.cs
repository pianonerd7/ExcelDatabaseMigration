using Excel_Database_Migration.ExcelUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<String> _columnHeader;

        #endregion

        #region Constructor
        public DatabaseAccessViewModel(Window window)
        {
            this._mainWindow = window;
            _columnHeader = new ObservableCollection<string>();
            testData();
            ExtractColumnHeader(_queryData);
        }

        private void testData()
        {
            DataTable table = new DataTable("Test");

            table.Columns.Add("Name");
            table.Columns.Add("Animal");
            table.Columns.Add("super looooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooong");
            table.Columns.Add("Fruit");

            table.Rows.Add("Bob", "Dog", "100", "Banana");
            table.Rows.Add("John", "cat", "200", "apple");
            table.Rows.Add("Jerry", "Panda", "300", "pineapple");
            table.Rows.Add("Bob", "Dog", "100", "Banana");
            table.Rows.Add("John", "cat", "200", "apple");
            table.Rows.Add("Jerry", "Panda", "300", "pineapple");
            table.Rows.Add("Bob", "Dog", "100", "Banana");
            table.Rows.Add("John", "cat", "200", "apple");
            table.Rows.Add("Jerry", "Panda", "300", "pineapple");
            table.Rows.Add("Bob", "Dog", "100", "Banana");
            table.Rows.Add("John", "cat", "200", "apple");
            table.Rows.Add("Jerry", "Panda", "300", "pineapple");
            table.Rows.Add("Bob", "Dog", "100", "Banana");
            table.Rows.Add("John", "cat", "200", "apple");
            table.Rows.Add("Jerry", "Panda", "300", "pineapple");
           
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

        public ObservableCollection<String> ColumnHeader
        {
            get
            {
                return _columnHeader;
            }
        }

        #endregion

        #region Private Methods

        private void DataTableToObservableCollection(DataTable dataList)
        {

        }

        private void ExtractColumnHeader(DataTable dataList)
        {
            for (int i = 0; i < dataList.Columns.Count; i++)
            {
                _columnHeader.Add(dataList.Columns[i].ColumnName.ToString());
            }

        }

        #endregion

    }
}
