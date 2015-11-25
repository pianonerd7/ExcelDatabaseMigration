﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Excel_Database_Migration.ViewModel
{
    public class SelectConnectionStringViewModel : ViewModelBase
    {

        #region Private Declarations

        private Page _mainWindow;
        private ICommand _selectConStrCommand;
        private ICommand _continueCommand;

        #endregion

        #region Constructor

        public SelectConnectionStringViewModel(Page mainWindow)
        {
            _mainWindow = mainWindow;
            _selectConStrCommand = new DelegateCommand(ExecuteSelectConStrCommand, CanExecuteCommand);
            _continueCommand = new DelegateCommand(ExecuteContinueCommand, CanExecuteCommand);
        }

        #endregion

        #region Public Fields

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

        private bool CanExecuteCommand(object obj)
        {
            return true;
        }

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

        private void ExecuteContinueCommand(object obj)
        {

        }

        #endregion
    }
}