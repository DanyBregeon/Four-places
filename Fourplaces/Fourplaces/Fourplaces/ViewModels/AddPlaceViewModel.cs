using Fourplaces.Modele;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class AddPlaceViewModel : ViewModelBase
    {
        private ImageSource image;
        private byte[] imageB;

        public String Nom { get; set; }

        public String Description { get; set; }

        public String Latitude { get; set; }

        public String Longitude { get; set; }

        public ImageSource Image
        {
            get
            {
                return image;
            }
            set
            {
                SetProperty(ref image, value);
            }
        }

        public AddPlaceViewModel()
        {
            Nom = "Quelque part";
            Description = "un lieu";
            Latitude = "42";
            Longitude = "42";
            Image = "profilDef.png";
        }

        public Command CmdAdd
        {
            get
            {
                return new Command(() => Add());
            }
        }

        public Command CmdPicture
        {
            get
            {
                return new Command(() => Picture());
            }
        }

        public async void Add()
        {
            if (LoginResultSingleton.SingletonLR != null)
            {
                Console.WriteLine("Dev_CPAccessToken:" + LoginResultSingleton.SingletonLR.AccessToken);
                bool send = await RestServiceSingleton.SingletonRS.SendPlaceDataAsync(Nom, Description, Latitude, Longitude, imageB, LoginResultSingleton.SingletonLR);
                if (send)
                {
                    await NavigationService.PopAsync();
                }
            }
            else
            {
                Console.WriteLine("Dev_CPPasEncoreConnecte:");
            }
        }

        public async void Picture()
        {
            /*imageB = await RestServiceSingleton.SingletonRS.SendPicture(Cam);
            IMAGE = ImageSource.FromStream(() => new MemoryStream(imageB));*/
        }
    }
}
