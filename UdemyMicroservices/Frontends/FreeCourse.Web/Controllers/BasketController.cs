using FreeCourse.Web.Models.Baskets;
using FreeCourse.Web.Models.Discounts;
using FreeCourse.Web.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly  ICatalogService _catalogService;

        public BasketController(IBasketService basketService, ICatalogService catalogService)
        {
            _basketService = basketService;
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index()
        {          
            return View(await _basketService.GetBasket());
        }
        
        public async Task<IActionResult> AddBasketItem(string courseId)
        {          
            var course = await _catalogService.GetByCourseIdAsync(courseId);
            var basketItem = new BasketItemViewModel
            {
                CourseId = courseId,
                CourseName = course.Name,
                Price = course.Price
            };
            await _basketService.AddBasketItem(basketItem);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveBasketItem(string courseId)
        {
            await _basketService.RemoveBasketItem(courseId);
            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        public async Task<IActionResult> ApplyDisCount(DiscountApplyInput discountApplyInput)
        {
            if (!ModelState.IsValid)
            {
                TempData["discountError"]= ModelState.Values.SelectMany(x=>x.Errors).Select(c=>c.ErrorMessage).First();
                return RedirectToAction(nameof(Index));
            }
            var discountStatus = await _basketService.ApplyDiscount(discountApplyInput.Code);
            TempData["discountStatus"] = discountStatus;
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> CancelApplyDisCount()
        {
            await _basketService.CancelDiscount();
            return RedirectToAction(nameof(Index));

        }
    }
}
