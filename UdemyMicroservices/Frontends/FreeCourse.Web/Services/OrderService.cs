﻿using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.FakePayments;
using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interface;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IPaymentService _paymentService;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderService(HttpClient httpClient, IPaymentService paymentService, IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _httpClient = httpClient;
            _paymentService = paymentService;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.GetBasket();

            var paymentInfoInput = new PaymentInfoInput()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Expiration = checkoutInfoInput.Expiration,
                CVV = checkoutInfoInput.CVV,
                TotalPrice = basket.TotalPrice
            };
            var responsePayment = await _paymentService.RecievePayment(paymentInfoInput);

            if (!responsePayment)
            {
                return new OrderCreatedViewModel() { Error = "Ödeme alınamadı", IsSuccessful = false };
            }

            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput 
                { 
                    Province = checkoutInfoInput.Province,
                    District = checkoutInfoInput.District, 
                    Street = checkoutInfoInput.Street, 
                    Line = checkoutInfoInput.Line, 
                    ZipCode = checkoutInfoInput.ZipCode 
                },
            };

            basket.BasketItems.ForEach(x =>
            {
                var orderItem = new OrderItemCreateInput 
                { 
                    ProductId = x.CourseId, 
                    Price = x.GetCurrentPrice, 
                    PictureUrl = "", 
                    ProductName = x.CourseName 
                };
                orderCreateInput.OrderItems.Add(orderItem);
            });

            var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);

            if (!response.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel() { Error = "Sipariş oluşturulamadı", IsSuccessful = false };
            }

            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();

            orderCreatedViewModel.Data.IsSuccessful = true;
            await _basketService.DeleteBasket();
            return orderCreatedViewModel.Data;
        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");

            return response.Data;
        }

        public async Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.GetBasket();

            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput
                {
                    Province = checkoutInfoInput.Province,
                    District = checkoutInfoInput.District,
                    Street = checkoutInfoInput.Street,
                    Line = checkoutInfoInput.Line,
                    ZipCode = checkoutInfoInput.ZipCode
                },
            };

            basket.BasketItems.ForEach(x =>
            {
                var orderItem = new OrderItemCreateInput
                {
                    ProductId = x.CourseId,
                    Price = x.GetCurrentPrice,
                    PictureUrl = "",
                    ProductName = x.CourseName
                };
                orderCreateInput.OrderItems.Add(orderItem);
            });

            var paymentInfoInput = new PaymentInfoInput()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Expiration = checkoutInfoInput.Expiration,
                CVV = checkoutInfoInput.CVV,
                TotalPrice = basket.TotalPrice,
                Order= orderCreateInput
            };
            var responsePayment = await _paymentService.RecievePayment(paymentInfoInput);

            if (!responsePayment)
            {
                return new OrderSuspendViewModel() { Error = "Ödeme Alınamadı", IsSuccessful = false };
            }

           

            await _basketService.DeleteBasket();
            return new OrderSuspendViewModel() {IsSuccessful = true };

        }
    }

    
}
