using Fourplaces.Modele;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using TD.Api.Dtos;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class ConnectionViewModel : ViewModelBase
    {
        public LoginResult lr;

        public ConnectionViewModel()
        {

            Login = "test@test.com";
            Password = "Test";
        }


        public String Login { get; set; }

        public String Password { get; set; }

        public Command CmdConnection
        {
            get
            {
                return new Command(() => Connection());
            }
        }

        public async void Connection()
        {

            Console.WriteLine("Dev_Send:" + Login + "|" + Password);
            lr = await RestServiceSingleton.SingletonRS.ConnectionDataAsync(Login, Password);
            if (lr != null)
            {
                LoginResultSingleton.SingletonLR = lr;
                Console.WriteLine("Dev_RDAccessToken:" + LoginResultSingleton.SingletonLR.AccessToken);
                await NavigationService.PopAsync();
            }
        }
    }
}
