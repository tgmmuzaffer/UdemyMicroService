using FreeCourse.Web.Models.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Interface
{
    public interface IOrderService
    {
        Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkOutInfoInput);//senkron akış için kullandığımız metot
        Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkOutInfoInput); //Asnkron için kullandıgız metot.RabbitMQ
        Task<List<OrderViewModel>> GetOrder();
    }
}
