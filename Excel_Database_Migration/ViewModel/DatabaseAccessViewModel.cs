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
        private DataTable _queryDataTable;
        private ObservableCollection<String> _columnHeader;
        private Object _selectedOption;
        private readonly ICommand _searchCommand;
        private readonly ICommand _exportCommand;
        private string _searchCriteria;
        private QueryWrapper _queryWrapper;
        private string _attributeString;
        

        #endregion

        #region Constructor
        public DatabaseAccessViewModel(Page window)
        {
            this._mainWindow = window;
            _columnHeader = new ObservableCollection<string>();
            _selectedOption = null;
            _searchCommand = new DelegateCommand(ExecuteSearchCommand, CanExecuteSearchCommand);
            _exportCommand = new DelegateCommand(ExecuteExportCommand, CanExecuteExportCommand);
            _attributeString = null;
            InitializeDataTable();
            ExtractColumnHeader(_queryDataTable);
            
            // add event handlers
            _queryDataTable.AcceptChanges();
            _queryDataTable.RowChanged += new DataRowChangeEventHandler(RowChanged);
            _queryDataTable.ColumnChanged += new DataColumnChangeEventHandler(ColumnChanged);
            _queryDataTable.RowDeleted += new DataRowChangeEventHandler(RowDeleted);

        }

        private void InitializeDataTable()
        {
            _queryWrapper = new QueryWrapper();
            _queryDataTable = _queryWrapper.SelectQuery("*",DatabaseInfo.DatabaseName);
            _attributeString = "";
            int i = 0;
            foreach (DataColumn col in _queryDataTable.Columns)
            {
                if (col.ColumnName == "RowID")
                {
                    continue;
                }
                _attributeString += col.ColumnName;
                if (i < _queryDataTable.Columns.Count - 2) // -2 because there is RowID as well
                {
                    _attributeString += ", ";
                }
                i++;
            }
            // to get rid of the last comma and space
            Console.WriteLine("Attribute string is: " + _attributeString);
        }

        //used for demo @deprecated
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
            _queryDataTable = table;
        }

        #endregion

        #region Public Variables

        public DataTable QueryData
        {
            get
            {
                return _queryDataTable;
            }
        }

        public ObservableCollection<String> ColumnHeader
        {
            get
            {
                return _columnHeader;
            }
        }

        public Object SelectedOption
        {
            get
            {
                return _selectedOption;
            }
            set
            {
                _selectedOption = value;
                OnPropertyChanged("SelectedOption");
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

        private bool CanExecuteSearchCommand(object obj)
        {
            return true;
        }

        private void ExecuteSearchCommand(object obj)
        {
            Console.WriteLine(string.Format("search criteria was: {0}", SearchCriteria));
            if (_selectedOption == null)
            {
                Console.WriteLine("selectedItem was null");
            }
            else
            {
                Console.WriteLine(string.Format("Selected item was: {0}", _selectedOption.ToString()));
            }
            // Gets the proper data table, but can't refresh the data grid view
            _queryDataTable = _queryWrapper.SelectQuery("*",DatabaseInfo.DatabaseName, SelectedOption.ToString(), string.Format("%{0}%", SearchCriteria));
            Console.WriteLine("search command executed");
        }

        private bool CanExecuteExportCommand(object obj)
        {
            return true;
        }

        private void ExecuteExportCommand(object obj)
        {
            DataTable table = _queryDataTable;
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

            DataTableToCSVConverter.WriteDataTableAsCSV(table, csvPath);

            CSVToXLSXConverter.toXLSX(csvPath, savePath);

            MessageBox.Show("Done exporting!", ProjectStrings.APPLICATION_NAME, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Event Handlers
        // Event Handlers

        private void RowChanged(object sender, DataRowChangeEventArgs e)
        {
            Console.WriteLine("RowChanged Event: name={0}; action={1}", e.Row["name"], e.Action);
            switch (e.Action)
            {
                case DataRowAction.Add:
                    Insert(e);
                    break;
                case DataRowAction.Change:
                    break;
                default:
                    Console.WriteLine("Illegal action value");
                    Console.WriteLine("Row_Changed Event: name={0}; action={1}", e.Row["name"], e.Action);
                    break;
            }
        }

        private void Insert(DataRowChangeEventArgs e)
        {
            string value = "";
            int i = 0;
            foreach (DataColumn col in _queryDataTable.Columns)
            {
                Console.WriteLine("column added is " + e.Row[col]);
                if (i == 0)
                {
                    i++;
                    continue;
                }
                if (e.Row[col].ToString().Length == 0)
                {
                    value += "NULL";
                    Console.WriteLine("NULL value");
                }
                else
                {
                    value += string.Format("'{0}'", e.Row[col]);
                }
                if (i<_queryDataTable.Columns.Count - 1)
                {
                    value += ", ";
                }
                i++;
            }
            _queryWrapper.InsertQuery(DatabaseInfo.DatabaseName, _attributeString, value);
        }
        
        
        private void ColumnChanged (object sender, DataColumnChangeEventArgs e)
        {
            _queryWrapper.UpdateQuery(DatabaseInfo.DatabaseName, e.Column.ColumnName, e.Row[e.Column].ToString(), e.Row["RowID"].ToString());
        }

        private void RowDeleted (object sender, DataRowChangeEventArgs e)
        {
            Console.WriteLine("Row_Deleted Event: name={0}; action={1}",
                e.Row["name", DataRowVersion.Original], e.Action);
            switch (e.Action)
            {
                case DataRowAction.Delete:
                    _queryWrapper.DeleteQuery(DatabaseInfo.DatabaseName, string.Format("RowID = {0}", e.Row["RowId", DataRowVersion.Original]));
                    break;
                default:
                    Console.WriteLine("Illegal action value for Delete");
                    break;
            }
        }
        #endregion
    }
}
