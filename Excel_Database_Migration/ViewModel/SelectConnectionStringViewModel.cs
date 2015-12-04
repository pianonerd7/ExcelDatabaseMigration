using Excel_Database_Migration.Pages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Excel_Database_Migration.DatabaseAccess;
using System.IO;

namespace Excel_Database_Migration.ViewModel
{
    public class SelectConnectionStringViewModel : ViewModelBase
    {

        #region Private Declarations

        private Page _mainWindow;
        private string _connectionStringFilePath;
        private ICommand _selectConStrCommand;
        private ICommand _continueCommand;

        #endregion

        #region Constructor

        public SelectConnectionStringViewModel(Page mainWindow)
        {
            _mainWindow = mainWindow;
            _selectConStrCommand = new DelegateCommand(ExecuteSelectConStrCommand, CanExecuteSelectionCommand);
            _continueCommand = new DelegateCommand(ExecuteContinueCommand, CanExecuteContinueCommand);
        }

        #endregion

        #region Public Fields

        public string ConnectionStringFilePath
        {
            get
            {
                return _connectionStringFilePath;
            }
            set
            {
                _connectionStringFilePath = value;
                OnPropertyChanged("ConnectionStringFilePath");
            }
        }

        public ICommand SelectConStrCommand
        {
            get
            {
                return _selectConStrCommand;
            }
        }

        public ICommand ContinueCommand
        {
            get
            {
                return _continueCommand;
            }
        }

        #endregion

        #region Private Methods

        private bool CanExecuteSelectionCommand(object obj)
        {
            return true;
        }

        private bool CanExecuteContinueCommand(object obj)
        {
            return ConnectionStringFilePath != null;
        }

        private void ExecuteSelectConStrCommand(object obj)
        {

            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Database Connection Files|*"+ProjectStrings.CONNECTION_STRING_FILE_EXTENSION;
            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                ConnectionStringFilePath = dialog.FileName;
                string[] fileContent = File.ReadAllLines(dialog.FileName);
                DatabaseInfo.DatabaseName = fileContent[0];
                ((DelegateCommand)_continueCommand).RaiseCanExecuteChanged();
            }

        }

        private void ExecuteContinueCommand(object obj)
        {
            NavigationService navService = NavigationService.GetNavigationService(_mainWindow);
            navService.Navigate(new DatabaseAccessPage());
        }

        #endregion
    }
}
