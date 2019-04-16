using Fourplaces.Modele;
using Fourplaces.Views;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TD.Api.Dtos;
using Xamarin.Forms;

namespace Fourplaces.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        //private DataTemplate dt;
        private ObservableCollection<ItemModel> _TaskItems;

        public List<PlaceItemSummary> listPISummary;

        private PlaceItemSummary placeSummary;

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

        public List<PlaceItemSummary> ListPISummary
        {

            get
            {

                Console.WriteLine("GETLPIS:" + (listPISummary != null));
                return listPISummary;
            }

            set => SetProperty(ref listPISummary, value);

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
            return base.OnResume();
        }

        public async Task FindData()
        {
            ListPISummary = await RestServiceSingleton.SingletonRS.RefreshDataAsync();
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
