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
    class RegistrationViewModel : ViewModelBase
    {
        public LoginResult lr;

        private String errorLabel;
        private String successLabel;

        public RegistrationViewModel()
        {
            Email = "";
            FirstName = "";
            LastName = "";
            Password = "";
            /*Email = "test@test.com";
            FirstName = "FTest";
            LastName = "LTest";
            Password = "Test";*/
        }


        public String Email { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

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

        public String SuccessLabel
        {
            get
            {
                return successLabel;
            }
            set
            {
                SetProperty(ref successLabel, value);
            }
        }

        public Command CmdRegister
        {
            get
            {
                return new Command(() => Register());
            }
        }

        public async void Register()
        {
            if(!String.IsNullOrEmpty(Email) && !String.IsNullOrEmpty(FirstName) && !String.IsNullOrEmpty(LastName) && !String.IsNullOrEmpty(Password))
            {
                lr = await RestServiceSingleton.SingletonRS.RegisterDataAsync(Email, FirstName, LastName, Password);
                if (lr != null)
                {
                    //LoginResultSingleton.SingletonLR = lr;
                    SuccessLabel = "Successful registration. Now sign in to add new places or comments !";
                    ErrorLabel = "";
                    //await NavigationService.PopAsync();
                }
                else
                {
                    SuccessLabel = "";
                    ErrorLabel = "Invalid fields or email already exist";
                }
            }
            else
            {
                SuccessLabel = "";
                ErrorLabel = "All fields must be filled";
            }  
        }

        public override Task OnResume()
        {
            SuccessLabel = "";
            ErrorLabel = "";
            return base.OnResume();
        }
    }
}
