using Fourplaces.Modele;
using Newtonsoft.Json;
using Storm.Mvvm;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TD.Api.Dtos
{
    public class UserItem : NotifierBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("image_id")]
        public int? ImageId { get; set; }

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

        public async Task GetImageResource()
        {
            if (sourceImage == null)
            {
                SourceImage = await RestServiceSingleton.SingletonRS.GetRequestImage(ImageId);
            }
        }
    }
}