using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.GetBasket();
            ViewBag.basket = basket;

            return View(new CheckoutInfoInput());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInfoInput checkOutInfoInput)
        {
            //Senkron iletişim sipariş işlemleri gateway'den orderMişcroservice e direk gider 
            //var ordrStatus = await _orderService.CreateOrder(checkOutInfoInput);
            //if (!ordrStatus.IsSuccessful)
            //{
            //    var basket = await _basketService.GetBasket();
            //    ViewBag.basket = basket;
            //    ViewBag.error = ordrStatus.Error;
            //    return View();
            //}
            //return RedirectToAction(nameof(SuccessfulCheckOut), new { orderId = ordrStatus.OrderId});
            ////////////////////////////////////////////////////////////////
            // 2. yol asenkron iletişim rabbitMQ ile kuyruk yapısı ile
            var ordrStatus = await _orderService.SuspendOrder(checkOutInfoInput);
            if (!ordrStatus.IsSuccessful)
            {
                var basket = await _basketService.GetBasket();
                ViewBag.basket = basket;
                ViewBag.error = ordrStatus.Error;
                return View();
            }
            Random random = new();
            return RedirectToAction(nameof(SuccessfulCheckOut), new { orderId = random.Next(1,1000) });
        }

        public IActionResult SuccessfulCheckOut(int orderId)
        {
            ViewBag.orderId = orderId;

            return View();
        }

        public async Task<IActionResult> CheckOutHistory()
        {
            return View(await _orderService.GetOrder());
        }
    }
}
