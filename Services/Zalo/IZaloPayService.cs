using FinalProject.Models;

namespace FinalProject.Services.Zalo
{
    public interface IZaloPayService
    {
        // Trả về một chuỗi link thanh toán để Controller chuyển hướng người dùng
        Task<string> CreatePaymentUrl(OrderInfoModel model);
    }
}
