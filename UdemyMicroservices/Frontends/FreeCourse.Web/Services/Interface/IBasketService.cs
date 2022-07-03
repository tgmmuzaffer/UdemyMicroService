using FreeCourse.Web.Models.Baskets;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Interface
{
    public interface IBasketService
    {
        Task<bool> SaveOrUpdate(BasketViewModel basketViewModel);
        Task<BasketViewModel> GetBasket();
        Task<bool> DeleteBasket();
        Task AddBasketItem(BasketItemViewModel basketItemViewModel);
        Task<bool> RemoveBasketItem(string courseId);
        Task<bool> ApplyDiscount(string discuntcode);
        Task<bool> CancelDiscount();

    }
}
