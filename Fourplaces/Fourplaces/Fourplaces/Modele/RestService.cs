using Common.Api.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TD.Api.Dtos;
using Xamarin.Forms;

namespace Fourplaces
{
    class RestService
    {
        HttpClient client;
        public String url = "https://td-api.julienmialon.com/";

        public RestService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<List<PlaceItemSummary>> RefreshDataAsync()
        {

            var uri = new Uri(string.Format(url + "places", string.Empty));

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Response<List<PlaceItemSummary>> r = JsonConvert.DeserializeObject<Response<List<PlaceItemSummary>>>(content);
                return r.Data;
            }

            return null;



        }

        public async Task<PlaceItem> PlaceItemDataAsync(int id)
        {

            var uri = new Uri(string.Format(url + "places/" + id, string.Empty));

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Response<PlaceItem> r = JsonConvert.DeserializeObject<Response<PlaceItem>>(content);
                return r.Data;
            }

            return null;

        }

        //int? : int qui peut être nul
        public async Task<ImageSource> GetRequestImage(int? id)
        {
            if (id == null)
            {
                return "profilDef.png";
            }
            else
            {
                Console.WriteLine("Image:" + id);
                var uri = new Uri(string.Format(url + "images/" + id, string.Empty));
                var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                Console.WriteLine("Status:" + response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    byte[] content = await response.Content.ReadAsByteArrayAsync();
                    ImageSource ims = ImageSource.FromStream(() => new MemoryStream(content));
                    return ims;
                }
                return "profilDef.png";
            }
        }
    }
}
