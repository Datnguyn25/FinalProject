using FinalProject.Models.Momo;
using FinalProject.Models;

namespace FinalProject.Services.Momo
{
    public interface IMomoService 
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
