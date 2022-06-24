using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.PhotoStocks;
using FreeCourse.Web.Services.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeletePhoto(string photoUrl)
        {
            var response = await _httpClient.DeleteAsync($"photos?photoUrl={photoUrl}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoStockViewModel> UploadPhoto(IFormFile photo)
        {
            if (photo == null || photo.Length <= 0)
            {
                return null;
            }

            var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";
            using var memorystream = new MemoryStream();
            await photo.CopyToAsync(memorystream);
            var multpartContent = new MultipartFormDataContent();
            multpartContent.Add(new ByteArrayContent(memorystream.ToArray()), "photo", randomFileName);
            /*
             * BIR UST SATIRDADAKI KODDA "photo" OLARAK VERDIGIMIZ ISIM PHOTOSTOCK MICRO SERVICE IMMIZIN CREATE METOTUNDAKI IFORMFILE PARAMETRESININ ISMI ILEAYNI OLMALI
             * BU DEGISKEN ISMINI FARKLI OLMASINDAN KAYNAKLI HATA ALINDI
             * BU TARZ DURUMA AZAMI DIKKAT
             */

            var response = await _httpClient.PostAsync("photos", multpartContent);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responsesuccess = await response.Content.ReadFromJsonAsync<Response<PhotoStockViewModel>>();
            return responsesuccess.Data;
        }
    }
}
