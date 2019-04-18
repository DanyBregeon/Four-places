using Fourplaces.Modele;
using Fourplaces.Views;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TD.Api.Dtos;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class MyAccountViewModel : ViewModelBase
    {
        private UserItem user;

        public UserItem User
        {
            get
            {
                return user;
            }
            set
            {
                SetProperty(ref user, value);
            }
        }

        public Command EditAccount
        {
            get
            {
                return new Command(() => GoToEditAccount());
            }

        }

        public Command PasswordEditAccount
        {
            get
            {
                return new Command(() => GoToPasswordEditAccount());
            }

        }

        async private void GoToEditAccount()
        {
            await NavigationService.PushAsync<EditAccount>(new Dictionary<string, object>() { { "User", User } });
        }

        async private void GoToPasswordEditAccount()
        {
            await NavigationService.PushAsync<PasswordEditAccount>();
        }

        public MyAccountViewModel()
        {

        }

        public override Task OnResume()
        {
            Task t = DataUser();
            return base.OnResume();
        }

        public async Task DataUser()
        {
            User = await RestServiceSingleton.SingletonRS.UserDataAsync();
        }
    }
}
