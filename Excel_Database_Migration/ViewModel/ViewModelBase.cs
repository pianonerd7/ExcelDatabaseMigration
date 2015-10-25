using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel_Database_Migration.ViewModel
{
 public class ViewModelBase : INotifyPropertyChanged
    {

        #region Private Properties



        #endregion

        #region Public Properties

    
        #endregion

        #region INotifyProperty

        public event PropertyChangedEventHandler PropertyChanged;

        internal void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}