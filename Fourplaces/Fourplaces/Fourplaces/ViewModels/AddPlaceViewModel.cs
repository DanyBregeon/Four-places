using Fourplaces.Modele;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class AddPlaceViewModel : ViewModelBase
    {
        private ImageSource image;
        private byte[] imageB;
        private bool cam = false;
        private String errorLabel;

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

        public bool Cam
        {
            get
            {
                return (cam);
            }
            set
            {
                SetProperty(ref cam, value);
            }
        }

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
                if(imageB != null)
                {
                    bool send = await RestServiceSingleton.SingletonRS.SendPlaceDataAsync(Nom, Description, Latitude, Longitude, imageB, LoginResultSingleton.SingletonLR);
                    if (send)
                    {
                        await NavigationService.PopAsync();
                    }
                    else
                    {
                        ErrorLabel = "Invalid fields";
                    }
                }
                else
                {
                    ErrorLabel = "You need to add a picture";
                }
                
            }
            else
            {
                Console.WriteLine("Dev_CPPasEncoreConnecte:");
            }
        }

        public async void Picture()
        {
            imageB = await RestServiceSingleton.SingletonRS.SendPicture(Cam);
            if(imageB != null)
            {
                Image = ImageSource.FromStream(() => new MemoryStream(imageB));
            }
        }

        public override Task OnResume()
        {
            ErrorLabel = "";
            return base.OnResume();
        }
    }
}
