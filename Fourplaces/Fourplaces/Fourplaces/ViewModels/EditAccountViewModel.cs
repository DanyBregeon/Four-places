using Fourplaces.Modele;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TD.Api.Dtos;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class EditAccountViewModel : ViewModelBase
    {
        private UserItem user;

        private ImageSource image;
        private byte[] imageB;

        private bool cam = false;

        public EditAccountViewModel()
        {

        }

        [NavigationParameter]
        public UserItem User
        {

            get
            {
                return user;
            }
            set
            {
                SetProperty(ref user, value);
                Image = User.SourceImage;
            }
        }

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

        public Command CmdEdit
        {
            get
            {
                return new Command(() => Edit());
            }
        }

        public Command CmdEditImg
        {
            get
            {
                return new Command(() => EditImg());
            }
        }

        async private void Edit()
        {
            Console.WriteLine("?????EditTest:" + User.FirstName + "|" + User.LastName + "|");
            //USER.ImageId = int.Parse(IMAGEID, System.Globalization.CultureInfo.InvariantCulture);
            var result = await RestServiceSingleton.SingletonRS.EditCountAsync(User.FirstName, User.LastName, User.ImageId, imageB);
            if(result != null)
            {
                Console.WriteLine(";;;;;EditTest:" + User.FirstName + "|" + User.LastName + "|");
                await NavigationService.PopAsync();
            }
            else
            {
                Console.WriteLine("!!!!!EditTest:" + User.FirstName + "|" + User.LastName + "|");
            }
        }

        public async void EditImg()
        {
            imageB = await RestServiceSingleton.SingletonRS.SendPicture(Cam);
            Image = ImageSource.FromStream(() => new MemoryStream(imageB));
        }
    }
}
