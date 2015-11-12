using Excel_Database_Migration.Controls;
using Excel_Database_Migration.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Excel_Database_Migration.ViewModel
{
    class OptionsViewModel : ViewModelBase
    {

        #region Private Declaration

        private Page _mainWindow;
        private readonly ICommand _migrateDataCommand;
        private readonly ICommand _accessDatabaseCommand;

        #endregion

        #region Constructor

        public OptionsViewModel(Page window)
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
            //_mainWindow.Content = new DatabaseAccessControl();
            //_mainWindow.DataContext = new DatabaseAccessViewModel(_mainWindow);
            NavigationService navService = NavigationService.GetNavigationService(_mainWindow);
            //navService.Navigate(new System.Uri("Pages/DatabaseAccessPage.xaml", UriKind.RelativeOrAbsolute));
            navService.Navigate(new DatabaseAccessPage());
        }

        #endregion

    }
}
