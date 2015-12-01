using Excel_Database_Migration.DatabaseAccess;
using Excel_Database_Migration.ExcelUtils;
using Excel_Database_Migration.SQLGeneration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
        private readonly ICommand _exportCommand;
        private string _searchCriteria;
        private QueryWrapper _queryWrapper;

        #endregion

        #region Constructor
        public DatabaseAccessViewModel(Page window)
        {
            this._mainWindow = window;
            _columnHeader = new ObservableCollection<string>();
            _searchCommand = new DelegateCommand(ExecuteSearchCommand, CanExecuteCommand);
            _exportCommand = new DelegateCommand(ExecuteExportCommand, CanExecuteExportCommand);
            testData();
            ExtractColumnHeader(_queryData);
            // add event handlers
            _queryData.AcceptChanges();
            _queryData.RowChanged += new DataRowChangeEventHandler(RowChanged);
            _queryData.RowDeleted += new DataRowChangeEventHandler(RowDeleted);

        }

        private void testData()
        {
            QueryWrapper wrapper = new QueryWrapper();
            DataTable table = wrapper.SelectQuery("*",DatabaseInfo.DatabaseName,"");
            _queryData = table;
        }

        private void testDataDemo()
        {
            SqlConnection connection = new SqlConnection(DatabaseInfo.ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(string.Format("SELECT * FROM {0}Table;", DatabaseInfo.DatabaseName), connection);
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

        public ICommand ExportCommand
        {
            get
            {
                return _exportCommand;
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
            _queryWrapper.SelectQuery("","","");
        }

        private bool CanExecuteExportCommand(object obj)
        {
            return true;
        }

        private void ExecuteExportCommand(object obj)
        {
            
            DataTable table = _queryData;
            string savePath ="";
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "Excel Files|*xlsx";
            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                savePath = dialog.FileName;
            }
            string filename = Path.GetFileNameWithoutExtension(savePath);
            string csvPath = Path.GetDirectoryName(savePath) + "\\" + filename + ".csv";

            DataTableToCSVConverter.convertToCSV(table, csvPath);

            CSVToXLSXConverter.toXLSX(csvPath, savePath);

            MessageBox.Show("Done exporting!", ProjectStrings.APPLICATION_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
        }


        #endregion

        #region Event Handlers
        // Event Handlers

        private void RowChanged(object sender, DataRowChangeEventArgs e)
        {
            
            switch (e.Action)
            {
                case DataRowAction.Add:
                    break;
                case DataRowAction.Change:
                    break;
                default:
                    Console.WriteLine("Illegal action value");
                    Console.WriteLine("Row_Changed Event: name={0}; action={1}",
    e.Row["name"], e.Action);
                    break;

            }
        }

        private void RowDeleted (object sender, DataRowChangeEventArgs e)
        {
            Console.WriteLine("Row_Deleted Event: name={0}; action={1}",
                e.Row["name", DataRowVersion.Original], e.Action);
            switch (e.Action)
            {
                case DataRowAction.Delete:
                    break;
                default:
                    break;
            }
        }
        #endregion

    }
}
