using Excel_Database_Migration.ExcelUtils;
using System;
using System.Collections.Generic;
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

        #endregion

        #region Constructor
        public DatabaseAccessViewModel(Window window)
        {
            this._mainWindow = window;
        }

        #endregion

        #region Private Methods

        private void populateGrid(DataTable dataList)
        {

        }

        #endregion
    }
}
