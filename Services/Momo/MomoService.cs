using FinalProject.Models;
using FinalProject.Models.Momo;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using RestSharp;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace FinalProject.Services.Momo
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;
        public MomoService(IOptions<MomoOptionModel> options)
        {
            _options = options;
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfoModel model)
        {
            model.OrderID = DateTime.UtcNow.Ticks.ToString();
            model.OrderInfo = "Customer: " + model.FullName + ". Context: " + model.OrderInfo;

            long amountLong = Convert.ToInt64(model.Amount);

            var rawData =
                $"accessKey={_options.Value.AccessKey}" +
                $"&amount={amountLong}" +
                $"&extraData=" +
                $"&ipnUrl={_options.Value.NotifyUrl}" + // Dùng ipnUrl
                $"&orderId={model.OrderID}" +
                $"&orderInfo={model.OrderInfo}" +
                $"&partnerCode={_options.Value.PartnerCode}" +
                $"&redirectUrl={_options.Value.ReturnUrl}" + // Dùng redirectUrl
                $"&requestId={model.OrderID}" +
                $"&requestType={_options.Value.RequestType}";
            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);
            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest()  { Method = Method.Post };
            

            var requestData = new
            {
                partnerCode = _options.Value.PartnerCode,
                partnerName = "Test",
                storeId = "MomoStore",
                requestId = model.OrderID,
                amount = amountLong,
                orderId = model.OrderID,
                orderInfo = model.OrderInfo,
                redirectUrl = _options.Value.ReturnUrl, // Khớp với rawData
                ipnUrl = _options.Value.NotifyUrl,    // Khớp với rawData
                lang = "en",
                extraData = "",
                requestType = _options.Value.RequestType,
                signature = signature
            };
            request.AddJsonBody(requestData);

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                // Nếu lỗi, log nội dung lỗi ra để xem (nó chính là cái đống HTML gây lỗi đấy)
                var errorContent = response.Content;
                // Bạn có thể đặt breakpoint tại đây để kiểm tra biến errorContent
                return new MomoCreatePaymentResponseModel { Message = "Momo API Error: " + response.StatusCode };
            }

            
                return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
            
            
            
        }

        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.FirstOrDefault(s => s.Key == "amount").Value;
            var orderInfo = collection.FirstOrDefault(s => s.Key == "orderInfo").Value;
            var orderId = collection.FirstOrDefault(s => s.Key == "orderId").Value;
            return new MomoExecuteResponseModel
            {
                Amount = amount,
                OrderInfo = orderInfo,
                OrderId = orderId
            };
        }


        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = System.Text.Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = System.Text.Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var HashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return HashString;
        }
    }
}
