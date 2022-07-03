using FreeCourse.Web.Models.FakePayments;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Interface
{
    public interface IPaymentService
    {
        Task<bool> RecievePayment(PaymentInfoInput paymentInfoInput);
    }
}
