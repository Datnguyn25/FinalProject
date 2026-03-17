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
            var rawData =
                $"partnerCode={_options.Value.PartnerCode}" +
                $"&accessKey={_options.Value.AccessKey}" +
                $"&requestId={model.OrderID}" +
                $"&amount={model.Amount}" +
                $"&orderId={model.OrderID}" +
                $"&orderInfo={model.OrderInfo}" +
                $"&returnUrl={_options.Value.ReturnUrl}" +
                $"&notifyUrl={_options.Value.NotifyUrl}" +
                $"&extraData=";
            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);
            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest()  { Method = Method.Post };
            request.AddHeader("Content-Type", "application/json; charset UTF = 8");

            var requestData = new
            {
                accessKey = _options.Value.AccessKey,
                partnerCode = _options.Value.PartnerCode,
                requestId = model.OrderID,
                amount = model.Amount.ToString(),
                orderId = model.OrderID,
                orderInfo = model.OrderInfo,
                returnUrl = _options.Value.ReturnUrl,
                notifyUrl = _options.Value.NotifyUrl,
                extraData = "",
                requestType = _options.Value.RequestType,
                signature = signature
            };
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
        }

        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.First(s => s.Key == "amount").Value;
            var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
            var orderId = collection.First(s => s.Key == "orderId").Value;
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
