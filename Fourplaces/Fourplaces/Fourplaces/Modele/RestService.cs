using Common.Api.Dtos;
using Fourplaces.Modele;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async Task<LoginResult> RegisterDataAsync(String email, String fname, string lname, string password)
        {
            var uri = new Uri(string.Format(url + "auth/register", string.Empty));

            Console.WriteLine("Dev_RegisterData:");

            RegisterRequest rr = new RegisterRequest();
            rr.Email = email;
            rr.FirstName = fname;
            rr.LastName = lname;
            rr.Password = password;
            var jsonRequest = JsonConvert.SerializeObject(rr);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
            //var response = client.PostAsync(uri, content).Result;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "__access__token__");
            request.Content = content;
            HttpResponseMessage response = await client.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Dev_RDResponse:" + result);
                Response<LoginResult> r = JsonConvert.DeserializeObject<Response<LoginResult>>(result);
                Console.WriteLine("Dev_is_sucess:" + r.IsSuccess);
                Console.WriteLine("Dev_error_code:" + r.ErrorCode);
                Console.WriteLine("Dev_error_message:" + r.ErrorMessage);
                if (r.IsSuccess)
                {

                    return r.Data;
                }
            }
            else
            {
                Debugger.Break();
            }

            return null;

        }

        public async Task<LoginResult> ConnectionDataAsync(String login, string password)
        {

            var uri = new Uri(string.Format(url + "auth/login", string.Empty));

            Console.WriteLine("Dev_ConnexionData:");

            LoginRequest lr = new LoginRequest();
            lr.Email = login;

            lr.Password = password;
            var jsonRequest = JsonConvert.SerializeObject(lr);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
            //var response = client.PostAsync(uri, content).Result;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "__access__token__");
            request.Content = content;
            HttpResponseMessage response = await client.SendAsync(request);

            string result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Dev_CDResponse:" + result);
                Response<LoginResult> r = JsonConvert.DeserializeObject<Response<LoginResult>>(result);
                Console.WriteLine("Dev_is_sucess:" + r.IsSuccess);
                Console.WriteLine("Dev_error_code:" + r.ErrorCode);
                Console.WriteLine("Dev_error_message:" + r.ErrorMessage);
                if (r.IsSuccess)
                {
                    return r.Data;
                }
            }
            else
            {
                Debugger.Break();
            }
            return null;
        }

        public async Task<UserItem> UserDataAsync()
        {
            var uri = new Uri(string.Format(url + "me", string.Empty));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Authorization = new AuthenticationHeaderValue(LoginResultSingleton.SingletonLR.TokenType, LoginResultSingleton.SingletonLR.AccessToken);
            HttpResponseMessage response = await client.SendAsync(request);

            string result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Dev_UDResponse:" + result);
                Response<UserItem> r = JsonConvert.DeserializeObject<Response<UserItem>>(result);
                Console.WriteLine("Dev_is_sucess:" + r.IsSuccess);
                Console.WriteLine("Dev_error_code:" + r.ErrorCode);
                Console.WriteLine("Dev_error_message:" + r.ErrorMessage);

                if (r.IsSuccess)
                {

                    return r.Data;
                }
            }
            else
            {
                Debugger.Break();
            }
            return null;
        }

        public async Task<UserItem> EditCountAsync(String FName, string LName, int? imageId, byte[] imageData)
        {


            var uri = new Uri(string.Format(url + "me", string.Empty));

            Console.WriteLine("Dev_ECA:");


            UpdateProfileRequest upr = new UpdateProfileRequest();
            upr.FirstName = FName;
            upr.LastName = LName;

            if (imageData == null) //ne pas mettre a jour l'image de profil
            {
                upr.ImageId = imageId;
            }
            else
            {
                ImageItem iItem = await UploadImage(imageData);
                upr.ImageId = iItem.Id;
            }




            var jsonRequest = JsonConvert.SerializeObject(upr);



            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
            //var response = client.PostAsync(uri, content).Result;



            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), uri);
            request.Headers.Authorization = new AuthenticationHeaderValue(LoginResultSingleton.SingletonLR.TokenType, LoginResultSingleton.SingletonLR.AccessToken);
            request.Content = content;
            HttpResponseMessage response = await client.SendAsync(request);

            string result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Dev_ECAesponse:" + result);
            Console.WriteLine("Dev_ECAtatusCode:" + response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Dev_ECAResponse:" + result);
                Response<UserItem> r = JsonConvert.DeserializeObject<Response<UserItem>>(result);
                Console.WriteLine("Dev_is_sucess:" + r.IsSuccess);
                Console.WriteLine("Dev_error_code:" + r.ErrorCode);
                Console.WriteLine("Dev_error_message:" + r.ErrorMessage);

                if (r.IsSuccess)
                {

                    return r.Data;
                }




            }
            else
            {
                Debugger.Break();

            }

            return null;

        }

        public async Task<UserItem> EditPWAsync(String oldPW, String newPW)
        {
            var uri = new Uri(string.Format(url + "me/password", string.Empty));

            Console.WriteLine("Dev_ECA:");

            UpdatePasswordRequest upr = new UpdatePasswordRequest();
            upr.OldPassword = oldPW;
            upr.NewPassword = newPW;
            var jsonRequest = JsonConvert.SerializeObject(upr);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
            //var response = client.PostAsync(uri, content).Result;

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), uri);
            request.Headers.Authorization = new AuthenticationHeaderValue(LoginResultSingleton.SingletonLR.TokenType, LoginResultSingleton.SingletonLR.AccessToken);
            request.Content = content;
            HttpResponseMessage response = await client.SendAsync(request);

            string result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Dev_ECAesponse:" + result);
            Console.WriteLine("Dev_ECAtatusCode:" + response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Dev_ECAResponse:" + result);
                Response<UserItem> r = JsonConvert.DeserializeObject<Response<UserItem>>(result);
                Console.WriteLine("Dev_is_sucess:" + r.IsSuccess);
                Console.WriteLine("Dev_error_code:" + r.ErrorCode);
                Console.WriteLine("Dev_error_message:" + r.ErrorMessage);

                if (r.IsSuccess)
                {

                    return r.Data;
                }
            }
            else
            {
                Debugger.Break();
            }
            return null;
        }

        public async Task<ImageItem> UploadImage(byte[] imageData)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://td-api.julienmialon.com/images");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", LoginResultSingleton.SingletonLR.AccessToken);

            MultipartFormDataContent requestContent = new MultipartFormDataContent();

            var imageContent = new ByteArrayContent(imageData);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            // Le deuxième paramètre doit absolument être "file" ici sinon ça ne fonctionnera pas
            requestContent.Add(imageContent, "file", "file.jpg");

            request.Content = requestContent;

            HttpResponseMessage response = await client.SendAsync(request);

            string result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Dev_RDResponse:" + result);
            Console.WriteLine("Dev_RDStatusCode:" + response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Image uploaded!");
                Response<ImageItem> r = JsonConvert.DeserializeObject<Response<ImageItem>>(result);
                Console.WriteLine("Dev_is_sucess:" + r.IsSuccess);
                Console.WriteLine("Dev_error_code:" + r.ErrorCode);
                Console.WriteLine("Dev_error_message:" + r.ErrorMessage);

                return r.Data;
            }
            else
            {
                Debugger.Break();
                return null;
            }
        }

        /*public async Task<byte[]> SendPicture(bool camera)
        {
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                //Supply media options for saving our photo after it's taken.
                var pictureMediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Receipts",
                    Name = $"{DateTime.UtcNow}.jpg",
                    PhotoSize = PhotoSize.Small

                };

                var galleryMediaOptions = new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small

                };

                // Take a photo of the business receipt.
                MediaFile file;
                if (camera)
                {
                    file = await CrossMedia.Current.TakePhotoAsync(pictureMediaOptions);

                }
                else
                {
                    file = await CrossMedia.Current.PickPhotoAsync(galleryMediaOptions);

                }

                //if (file == null) MODIFY CODE HERE ELSE NULL IF QUIT PICTURE
                //{
                //    file = "profileDef.png";
                //}
                Console.WriteLine("picture:");
                Console.WriteLine("picture:" + file);

                var stream = file.GetStream();
                file.Dispose();
                byte[] imageData = GetImageStreamAsBytes(stream);
                return imageData;

                //return file;
            }

            return null;
        }*/

        public byte[] GetImageStreamAsBytes(Stream input) //Put in private later
        {
            var buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
