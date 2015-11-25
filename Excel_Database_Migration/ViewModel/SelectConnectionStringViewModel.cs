using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Excel_Database_Migration.ViewModel
{
    public class SelectConnectionStringViewModel : ViewModelBase
    {

        #region Private Declarations

        private ICommand _SelectConStrCommand;
        private ICommand _continueCommand;

        #endregion

        #region Constructor

        #endregion

        #region Public Fields

        public ICommand SelectConStrCommand
        {
            get
            {
                return _SelectConStrCommand;
            }
        }

        public ICommand 
        #endregion

        #region Private Methods

        private void ExecuteSelectConStrCommand(object obj)
        {

            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "CSV Files|*.csv";
            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                AttributeFilePath = dialog.FileName;
            }

        }

        #endregion
    }
}
