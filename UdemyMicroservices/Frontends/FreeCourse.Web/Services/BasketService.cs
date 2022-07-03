using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.Baskets;
using FreeCourse.Web.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly IDiscountService _discountService;
        public BasketService(HttpClient httpClient, IDiscountService discountService)
        {
            _httpClient = httpClient;
            _discountService = discountService;
        }

        public async Task AddBasketItem(BasketItemViewModel basketItemViewModel)
        {
            var basket = await GetBasket();
            if (basket != null)
            {
                if(!basket.BasketItems.Any(x=>x.CourseId == basketItemViewModel.CourseId))
                {
                    basket.BasketItems.Add(basketItemViewModel);
                }
            }
            else
            {
                basket = new BasketViewModel();
                basket.BasketItems.Add(basketItemViewModel);
            }

            await SaveOrUpdate(basket);
        }

        public async Task<bool> ApplyDiscount(string discuntcode)
        {
            await CancelDiscount();
            var basket = await GetBasket();
            if(basket==null)
            {
                return false;
            }

            var hasDisCount = await _discountService.GetDiscount(discuntcode);
            if (hasDisCount == null)
                return false;

            basket.ApplyDiscount(hasDisCount.Code, hasDisCount.Rate);
            return await SaveOrUpdate(basket);
        }

        public async Task<bool> CancelDiscount()
        {
            var basket = await GetBasket();
            if(basket ==null || basket.DiscountCode == null)
            { return false; }

            basket.CancelDiscount();
           return await SaveOrUpdate(basket);

        }

        public async Task<bool> DeleteBasket()
        {
            var result = await _httpClient.DeleteAsync("baskets");
            return result.IsSuccessStatusCode;
        }

        public async Task<BasketViewModel> GetBasket()
        {
            var response = await _httpClient.GetAsync("baskets");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var basketViewModel = await response.Content.ReadFromJsonAsync<Response<BasketViewModel>>();
            return basketViewModel.Data;
        }

        public async Task<bool> RemoveBasketItem(string courseId)
        {
            var basket = await GetBasket();
            if(basket == null)
            {
                return false;
            }
            var deleteBasketItem = basket.BasketItems.FirstOrDefault(x => x.CourseId == courseId);
            if(deleteBasketItem == null)
            {
                return false;
            }
            var deleteresult = basket.BasketItems.Remove(deleteBasketItem);
            if (!deleteresult)
            {
                return false;
            }

            if (!basket.BasketItems.Any())
            {
                basket.DiscountCode = string.Empty;
            }

            return await SaveOrUpdate(basket);
                
        }

        public async Task<bool> SaveOrUpdate(BasketViewModel basketViewModel)
        {
            var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("baskets", basketViewModel);
            return response.IsSuccessStatusCode;
        }
    }
}
