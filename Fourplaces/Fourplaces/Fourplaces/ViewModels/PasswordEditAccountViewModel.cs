using Fourplaces.Modele;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class PasswordEditAccountViewModel : ViewModelBase
    {
        public PasswordEditAccountViewModel()
        {

        }

        public String OldPassword { get; set; }

        public String NewPassword { get; set; }

        public Command CmdEdit
        {
            get
            {
                return new Command(() => Edit());
            }
        }

        async private void Edit()
        {
            await RestServiceSingleton.SingletonRS.EditPWAsync(OldPassword, NewPassword);
        }
    }
}
