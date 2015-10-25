using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Excel_Database_Migration.ViewModel
{
    class MigrationDataConfigViewModel : ViewModelBase
    {

        #region Private Declarations

        private readonly ICommand _selectFilePathCommand;

        #endregion

        #region Constructor

        public MigrationDataConfigViewModel()
        {
            _selectFilePathCommand = new DelegateCommand(ExecuteSelectFilePathCommand, CanExecuteCommand);
        }

        #endregion 

        #region Public Properties

        public ICommand SelectFilePathCommand
        {
            get
            {
                return _selectFilePathCommand;
            }
        }
        #endregion

        #region Private Methods

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }

        private void ExecuteSelectFilePathCommand(object obj)
        {

        }

        #endregion

    }
}
