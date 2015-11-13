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

        #endregion

        #region Constructor
        public DatabaseAccessViewModel(Page window)
        {
            this._mainWindow = window;
            _columnHeader = new ObservableCollection<string>();
            _searchCommand = new DelegateCommand(ExecuteSearchCommand, CanExecuteCommand);
            testData();
            ExtractColumnHeader(_queryData);
        }

        private void testData()
        {
            SqlConnection connection = new SqlConnection(SQLGenerator.ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(string.Format("SELECT * FROM {0}Table;", SQLGenerator.Name), connection);
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            try {
                adapter.SelectCommand = command;
                adapter.Fill(table);
            }
            catch(System.Exception e)
            {

                MessageBox.Show(command.CommandText, ProjectStrings.APPLICATION_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
                MessageBox.Show(e.ToString(), ProjectStrings.APPLICATION_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
            
        }


        #endregion

    }
}
