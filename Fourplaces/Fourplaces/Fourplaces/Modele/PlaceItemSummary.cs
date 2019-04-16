using Fourplaces.Modele;
using Newtonsoft.Json;
using Storm.Mvvm;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TD.Api.Dtos
{
	public class PlaceItemSummary : NotifierBase
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		
		[JsonProperty("title")]
		public string Title { get; set; }
		
		[JsonProperty("description")]
		public string Description { get; set; }
		
		[JsonProperty("image_id")]
		public int ImageId { get; set; }

        public ImageSource sourceImage;
        public ImageSource SourceImage
        {
            get
            {
                Task t = GetImageResource();
                return sourceImage;
            }
            set
            {
                SetProperty(ref sourceImage, value);
            }
        }

        [JsonProperty("latitude")]
		public double Latitude { get; set; }
		
		[JsonProperty("longitude")]
		public double Longitude { get; set; }

        public async Task GetImageResource()
        {
            if (sourceImage == null)
            {
                SourceImage = await RestServiceSingleton.SingletonRS.GetRequestImage(ImageId);
            }
        }
    }
}