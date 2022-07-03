using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.Discounts;
using FreeCourse.Web.Services.Interface;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DiscountViewModel> GetDiscount(string discount)
        {
           var response = await _httpClient.GetAsync($"discounts/getbycode/{discount}");
            if (!response.IsSuccessStatusCode) { return null; }

            var discountResponse = await response.Content.ReadFromJsonAsync<Response<DiscountViewModel>>();
            return discountResponse.Data;
        }
    }
}
