using Fourplaces.Modele;
using Fourplaces.Views;
using Plugin.Geolocator;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TD.Api.Dtos;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Fourplaces.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        //private DataTemplate dt;
        private ObservableCollection<ItemModel> _TaskItems;

        public List<PlaceItemSummary> listPlaceSummary;

        private PlaceItemSummary placeSummary;

        public Position currentPosition;

        private String toolBarText1 = "";
        private String toolBarText2 = "";
        private String toolBarText3 = "";
        private Command cmd1;
        private Command cmd2;
        private Command cmd3;

        public PlaceItemSummary PlaceSummary
        {
            get
            {
                Console.WriteLine("GETPISS:" + (placeSummary != null));
                return placeSummary;
            }
            set
            {
                SetProperty(ref placeSummary, value);
                ItemTapped();
            }
        }

        public ObservableCollection<ItemModel> TaskItems
        {
            get
            {
                return _TaskItems;

            }
            set
            {
                SetProperty(ref _TaskItems, value);
            }
        }

        public List<PlaceItemSummary> ListPlaceSummary
        {
            get
            {
                return listPlaceSummary;
            }
            set
            {
                SetProperty(ref listPlaceSummary, value);
            }
        }

        public String ToolBarText1
        {
            get
            {
                return toolBarText1;
            }
            set
            {
                SetProperty(ref toolBarText1, value);
            }
        }

        public String ToolBarText2
        {
            get
            {
                return toolBarText2;
            }
            set
            {
                SetProperty(ref toolBarText2, value);
            }
        }

        public String ToolBarText3
        {
            get
            {
                return toolBarText3;
            }
            set
            {
                SetProperty(ref toolBarText3, value);
            }
        }

        public Command Cmd1
        {
            get
            {
                return cmd1;
            }
            set
            {
                SetProperty(ref cmd1, value);
            }
        }

        public Command Cmd2
        {
            get
            {
                return cmd2;
            }
            set
            {
                SetProperty(ref cmd2, value);
            }
        }

        public Command Cmd3
        {
            get
            {
                return cmd3;
            }
            set
            {
                SetProperty(ref cmd3, value);
            }
        }

        public Command Connection
        {
            get
            {
                return new Command(() => GoToConnection());
            }
        }

        public Command Registration
        {
            get
            {

                return new Command(() => GoToRegistration());
            }
        }

        public Command MyAccount
        {
            get
            {
                return new Command(() => GoToMyAccount());
            }
        }

        public Command AddPlace
        {
            get
            {
                return new Command(() => GoToAddPlace());
            }
        }

        public Command CmdSignOut
        {
            get
            {
                return new Command(() => SignOut());
            }
        }

        async private void GoToConnection()
        {
            await NavigationService.PushAsync(new Connection());
        }

        async private void GoToRegistration()
        {
            await NavigationService.PushAsync(new Registration());
        }

        async private void GoToMyAccount()
        {
            //await NavigationService.PushAsync(new MyAccount());
            await NavigationService.PushAsync<MyAccount>();
        }

        async private void GoToAddPlace()
        {
            await NavigationService.PushAsync(new AddPlace());
        }

        private void SignOut()
        {
            LoginResultSingleton.SignOut();
            OnResume();
        }

        //public DataTemplate Dt { get => dt; set => SetProperty(ref dt, value); }

        public MainViewModel()
        {
            /*Console.WriteLine("\nBONJOUR JE SUIS LAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n");
            TaskItems = new ObservableCollection<ItemModel>();
            TaskItems.Add(new ItemModel { Nom = "lieu 1" });
            TaskItems.Add(new ItemModel { Nom = "lieu 2" });
            TaskItems.Add(new ItemModel { Nom = "lieu 3" });*/
            //Dt = new DataTemplate(typeof(CustomCell));
        }

        public override Task OnResume()
        {
            Task t = FindData();
            if (LoginResultSingleton.SingletonLR != null)
            {
                ToolBarText1 = "Sign out";
                ToolBarText2 = "My account";
                ToolBarText3 = "Add place";
                Cmd1 = CmdSignOut;
                Cmd2 = MyAccount;
                Cmd3 = AddPlace;
            }
            else
            {
                ToolBarText1 = "";
                ToolBarText2 = "Sign in";
                ToolBarText3 = "Sign up";
                Cmd1 = null;
                Cmd2 = Connection;
                Cmd3 = Registration;
            }
            return base.OnResume();
        }

        public async Task FindData()
        {
            //ListPlaceSummary = await RestServiceSingleton.SingletonRS.RefreshDataAsync();
            List<PlaceItemSummary> listPlaceSummary2 = await RestServiceSingleton.SingletonRS.RefreshDataAsync();

            currentPosition = await GetLocationAsync();
            ListPlaceSummary = SortListByDistance(listPlaceSummary2);
        }

        async Task<Position> GetLocationAsync()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(2));
                if (position != null)
                {
                    Console.WriteLine(string.Format("Latitude: {0}  Longitude: {1}", position.Latitude, position.Longitude));
                    return new Position(position.Latitude, position.Longitude);
                }
                else
                {
                    Console.WriteLine("DevLoc_Lat:null");
                    return new Position();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: " + ex.ToString());
                return new Position();
            }
        }

        public List<PlaceItemSummary> SortListByDistance(List<PlaceItemSummary> list)
        {
            list.Sort(delegate (PlaceItemSummary pis1, PlaceItemSummary pis2) {
                Position posPlace1 = new Position(pis1.Latitude, pis1.Longitude);
                Position posPlace2 = new Position(pis2.Latitude, pis2.Longitude);
                Distance d1 = DistanceBetweenPoints(currentPosition, posPlace1);
                Distance d2 = DistanceBetweenPoints(currentPosition, posPlace2);
                return d1.Kilometers.CompareTo(d2.Kilometers);
            });

            foreach (PlaceItemSummary pis in list)
            {
                Position posPlace = new Position(pis.Latitude, pis.Longitude);
                Distance d = DistanceBetweenPoints(currentPosition, posPlace);
                Console.WriteLine("Dev_DistSort:" + pis.Title + "|" + pis.ImageId + "|" + d.Kilometers);
            }

            return list;
        }

        Distance DistanceBetweenPoints(Position p1, Position p2)
        {
            double latitude1 = DegreesToRadians(p1.Latitude);
            double latitude2 = DegreesToRadians(p2.Latitude);
            double longitude1 = DegreesToRadians(p1.Longitude);
            double longitude2 = DegreesToRadians(p2.Longitude);

            double distance = Math.Sin((latitude2 - latitude1) / 2.0);
            distance *= distance;

            double intermediate = Math.Sin((longitude2 - longitude1) / 2.0);
            intermediate *= intermediate;

            distance = distance + Math.Cos(latitude1) * Math.Cos(latitude2) * intermediate;
            distance = 2 * 6371 * Math.Atan2(Math.Sqrt(distance), Math.Sqrt(1 - distance));

            return Distance.FromKilometers(distance);

        }

        double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        async private void ItemTapped()
        {
            if (PlaceSummary != null)
            {
                Console.WriteLine("ItemTapped:" + PlaceSummary.Id + " " + PlaceSummary.Latitude + "|" + PlaceSummary.Longitude);

                await NavigationService.PushAsync<DetailsPage>(new Dictionary<string, object>() { { "PlaceSummary", PlaceSummary } });
            }
        }
    }
}
