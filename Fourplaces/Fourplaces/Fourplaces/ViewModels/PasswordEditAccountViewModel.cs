using Fourplaces.Modele;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class PasswordEditAccountViewModel : ViewModelBase
    {
        private String errorLabel;

        public String ErrorLabel
        {
            get
            {
                return errorLabel;
            }
            set
            {
                SetProperty(ref errorLabel, value);
            }
        }

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
            if(OldPassword != "" && NewPassword != "")
            {
                var result = await RestServiceSingleton.SingletonRS.EditPWAsync(OldPassword, NewPassword);
                if(result != null)
                {
                    await NavigationService.PopAsync();
                }
                else
                {
                    ErrorLabel = "Wrong old password";
                }
            }
            else
            {
                ErrorLabel = "All fields must be filled";
            }
        }

        public override Task OnResume()
        {
            ErrorLabel = "";
            return base.OnResume();
        }
    }
}
