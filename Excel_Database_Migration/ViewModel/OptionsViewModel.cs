using Excel_Database_Migration.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Excel_Database_Migration.ViewModel
{
    class OptionsViewModel : ViewModelBase
    {

        #region Private Declaration

        private Window _mainWindow;
        private readonly ICommand _migrateDataCommand;
        private readonly ICommand _accessDatabaseCommand;

        #endregion

        #region Constructor

        public OptionsViewModel(Window window)
        {
            this._mainWindow = window;
            _migrateDataCommand = new DelegateCommand(ExecuteMigrateDataCommand, CanExecuteCommand);
            _accessDatabaseCommand = new DelegateCommand(ExecuteAccessDatabaseCommand, CanExecuteCommand);
        }

        #endregion 

        #region Public Commands

        public ICommand MigrateDataCommand 
        {
            get
            {
                return _migrateDataCommand;
            }
        }

        public ICommand AccessDatabaseCommand
        {
            get
            {
                return _accessDatabaseCommand;
            }
        }

        #endregion

        #region Private Methods

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }

        private void ExecuteMigrateDataCommand(object obj)
        {
            _mainWindow.Content = new MigrateDataConfigControl();
            _mainWindow.DataContext = new MigrationDataConfigViewModel(_mainWindow);
        }

        private void ExecuteAccessDatabaseCommand(object obj)
        {
            _mainWindow.Content = new DatabaseAccessControl();
            _mainWindow.DataContext = new DatabaseAccessViewModel(_mainWindow);
        }

        #endregion

    }
}
