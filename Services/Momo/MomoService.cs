using FinalProject.Models;
using FinalProject.Models.Momo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FinalProject.Services.Momo;

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
            var orderId = model.OrderId;
            var requestId = Guid.NewGuid().ToString();
            var extraData = "";

            var orderInfo = "Thanh_toan"; // 🔥 không dấu, không space

            var rawData =
             $"accessKey={_options.Value.AccessKey}" +
             $"&amount={model.Amount}" +
             $"&extraData=" +
             $"&ipnUrl={_options.Value.NotifyUrl}" +
             $"&orderId={model.OrderId}" +
             $"&orderInfo={model.OrderInfo}" +
             $"&partnerCode={_options.Value.PartnerCode}" +
             $"&redirectUrl={_options.Value.ReturnUrl}" +
             $"&requestId={model.OrderId}" +
             $"&requestType={_options.Value.RequestType}";

            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest("", Method.Post);

            request.AddHeader("Content-Type", "application/json");

            request.AddHeader("ngrok-skip-browser-warning", "true");

            // Create an object representing the request data
            var requestData = new
            {
                partnerCode = _options.Value.PartnerCode,
                accessKey = _options.Value.AccessKey,
                requestId = requestId,
                amount = model.Amount.ToString(),
                orderId = orderId,
                orderInfo = orderInfo,
                redirectUrl = _options.Value.ReturnUrl,
                ipnUrl = _options.Value.NotifyUrl,
                requestType = _options.Value.RequestType,
                extraData = "",
                signature = signature,
                lang = "en"
            };

            request.AddJsonBody(requestData);

            var response = await client.ExecuteAsync(request);

            Console.WriteLine("MOMO RESPONSE: " + response.Content);

            return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
        }
        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.First(s => s.Key == "amount").Value;
            var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
            var orderId = collection.First(s => s.Key == "orderId").Value;

            return new MomoExecuteResponseModel()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo

            };
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }
    }

}
