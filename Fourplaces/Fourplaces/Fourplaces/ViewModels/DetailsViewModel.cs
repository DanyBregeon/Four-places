using Fourplaces.Modele;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TD.Api.Dtos;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Fourplaces.ViewModels
{
    class DetailsViewModel : ViewModelBase
    {
        private PlaceItem place;

        private PlaceItemSummary placeSummary;

        private ImageSource imagePlace;

        private MyMap map;

        private String com = "";

        private bool comNotEmpty = false;

        private bool isConnected;

        public PlaceItem Place
        {
            get
            {
                return place;
            }
            set
            {
                SetProperty(ref place, value);
            }
        }

        [NavigationParameter]
        public PlaceItemSummary PlaceSummary
        {
            get
            {
                return placeSummary;
            }
            set
            {
                SetProperty(ref placeSummary, value);
            }
        }

        public ImageSource ImagePlace
        {
            get
            {
                return imagePlace;
            }
            set
            {
                SetProperty(ref imagePlace, value);
            }
        }

        public MyMap Map
        {
            get
            {
                return map;
            }
            set
            {
                SetProperty(ref map, value);
            }
        }

        public String Com
        {
            get
            {
                return com;
            }
            set
            {
                SetProperty(ref com, value);
                if(value == "")
                {
                    ComNotEmpty = false;
                }
                else
                {
                    ComNotEmpty = true;
                }
            }
        }

        public bool ComNotEmpty
        {
            get
            {
                return comNotEmpty;
            }
            set
            {
                SetProperty(ref comNotEmpty, value);
            }
        }

        public Command CmdCom
        {
            get
            {
                return new Command(() => AddComment());
            }
        }

        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
            set
            {
                SetProperty(ref isConnected, value);
            }
        }

        public DetailsViewModel()
        {

        }

        public override Task OnResume()
        {
            Com = "";
            Task t = FindPlaceItem(PlaceSummary.Id);
            if (LoginResultSingleton.SingletonLR != null)
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
            }
            return base.OnResume();
        }

        private async Task FindPlaceItem(int id)
        {

            Place = await RestServiceSingleton.SingletonRS.PlaceItemDataAsync(id);
            CreateMap();
            await getImage();

        }

        public void CreateMap()
        {
            MyMap map2 = new MyMap();
            var pin = new MyPin
            {
                Type = PinType.Place,
                Position = new Position(Place.Latitude, Place.Longitude),
                Label = Place.Title,

            };

            map2.CustomPins = new List<MyPin> { pin };
            map2.MapType = MapType.Hybrid;
            map2.WidthRequest = 320;
            map2.HeightRequest = 200;
            map2.Pins.Add(pin);
            map2.MoveToRegion(MapSpan.FromCenterAndRadius(
              new Position(Place.Latitude, Place.Longitude), Distance.FromMiles(1.0)));

            Map = map2;
        }

        public async Task getImage()
        {
            ImagePlace = await RestServiceSingleton.SingletonRS.GetRequestImage(Place.ImageId);
        }

        public async void AddComment()
        {
            if (LoginResultSingleton.SingletonLR != null)
            {
                if (Com != "")
                {
                    await RestServiceSingleton.SingletonRS.SendCommentDataAsync(Place.Id, Com, LoginResultSingleton.SingletonLR);
                    await OnResume();
                }
            }

        }
    }
}
