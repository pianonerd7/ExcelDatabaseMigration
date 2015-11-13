using Excel_Database_Migration.DatabaseAccess;
using Excel_Database_Migration.ExcelUtils;
using Excel_Database_Migration.SQLGeneration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Excel_Database_Migration.ViewModel
{
    public class DatabaseAccessViewModel : ViewModelBase
    {
         #region Private Declaration

        private Page _mainWindow;
        private DataTable _queryData;
        private ObservableCollection<String> _columnHeader;
        private readonly ICommand _searchCommand;
        private string _searchCriteria;
        private QueryWrapper _queryWrapper;

        #endregion

        #region Constructor
        public DatabaseAccessViewModel(Page window)
        {
            this._mainWindow = window;
            _columnHeader = new ObservableCollection<string>();
            _searchCommand = new DelegateCommand(ExecuteSearchCommand, CanExecuteCommand);
            _queryWrapper = new QueryWrapper();
            testData();
            ExtractColumnHeader(_queryData);
        }

        private void testData()
        {
            _queryWrapper.SelectQuery("*", SQLGenerator._dbName);
            /*
            DataTable table = new DataTable("Test");

            table.Columns.Add("Name");
            table.Columns.Add("Animal");
            table.Columns.Add("Favorite Number");
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
           
            _queryData = table; */
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

        public string SearchCriteria
        {
            get
            {
                return _searchCriteria;
            }
            set
            {
                _searchCriteria = value;
                OnPropertyChanged("SearchCriteria");
            }
        }

        #endregion

        #region Commands

        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand;
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

        #region Private Methods

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }

        private void ExecuteSearchCommand(object obj)
        {
            //TODO INSERT SEARCH QUERY METHOD AND UPDATE DATATABLE
            QueryWrapper queryWrapper = new QueryWrapper();
            //queryWrapper.SelectQuery();
            //_searchCriteria = "";
        }


        #endregion

    }
}
