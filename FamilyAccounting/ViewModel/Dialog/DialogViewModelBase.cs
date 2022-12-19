using Proconsult.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAccounting.ViewModel
{
    public abstract class DialogViewModelBase : ObservableObject

    {
        public abstract string Title { get; }
        #region Commands

        public EnabledCommand OkCommand { get; }
        public EnabledCommand CancelCommand { get; }

 

        #endregion

        #region Constructor

        protected DialogViewModelBase() 
        {
            OkCommand = new EnabledCommand(OnOk);
            CancelCommand = new EnabledCommand(OnCancel);

    
        }

        #endregion

        #region Command Handlers

        protected abstract void OnOk();

        protected abstract void OnCancel();

       
        #endregion
    }
}
