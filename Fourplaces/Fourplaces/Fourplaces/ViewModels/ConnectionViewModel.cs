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
            Login = "";
            Password = "";
            /*Login = "test@test.com";
            Password = "Test";*/
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
            if(!String.IsNullOrEmpty(Login) && !String.IsNullOrEmpty(Password))
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
