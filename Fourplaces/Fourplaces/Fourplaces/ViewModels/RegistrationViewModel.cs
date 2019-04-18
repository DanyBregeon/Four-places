using Fourplaces.Modele;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using TD.Api.Dtos;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class RegistrationViewModel : ViewModelBase
    {
        public LoginResult lr;

        public RegistrationViewModel()
        {

            Email = "test@test.com";
            FirstName = "FTest";
            LastName = "LTest";
            Password = "Test";
        }


        public String Email { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Password { get; set; }

        public Command CmdRegister
        {
            get
            {
                return new Command(() => Register());
            }
        }

        public async void Register()
        {

            Console.WriteLine("Dev_Send:" + Email + "|" + FirstName + "|" + LastName + "|" + Password);
            lr = await RestServiceSingleton.SingletonRS.RegisterDataAsync(Email, FirstName, LastName, Password);
            if (lr != null)
            {
                LoginResultSingleton.SingletonLR = lr;
                Console.WriteLine("Dev_RDAccessToken:" + LoginResultSingleton.SingletonLR.AccessToken);
                await NavigationService.PopAsync();
            }
        }
    }
}
