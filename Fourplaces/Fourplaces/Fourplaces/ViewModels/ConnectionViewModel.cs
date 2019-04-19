using Fourplaces.Modele;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TD.Api.Dtos;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class ConnectionViewModel : ViewModelBase
    {
        public LoginResult lr;

        private String errorLabel;

        public ConnectionViewModel()
        {

            Login = "test@test.com";
            Password = "Test";
        }

        public String Login { get; set; }

        public String Password { get; set; }

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

        public Command CmdConnection
        {
            get
            {
                return new Command(() => Connection());
            }
        }

        public async void Connection()
        {

            lr = await RestServiceSingleton.SingletonRS.ConnectionDataAsync(Login, Password);
            if (lr != null)
            {
                LoginResultSingleton.SingletonLR = lr;
                await NavigationService.PopAsync();
            }
            else
            {
                ErrorLabel = "Wrong email or password";
            }
        }

        public override Task OnResume()
        {
            ErrorLabel = "";
            return base.OnResume();
        }
    }
}
