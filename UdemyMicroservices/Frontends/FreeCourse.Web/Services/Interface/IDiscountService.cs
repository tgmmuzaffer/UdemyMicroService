using FreeCourse.Web.Models.Discounts;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Interface
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscount(string discount);
    }
}
