using FinalProject.Models;
using FinalProject.Services.Zalo;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace FinalProject.Services.ZaloPay
{
    public class ZaloPayService : IZaloPayService
    {
        // Thông tin Sandbox mặc định của ZaloPay để bạn test luôn
        private readonly string _appId = "2553";
        private readonly string _key1 = "9ph6Z6u9pS_gu915N7pPh3psS8u9pS_g";
        private readonly string _endpoint = "https://sb-openapi.zalopay.vn/v2/create";

        public async Task<string> CreatePaymentUrl(OrderInfoModel model)
        {
            var appTransId = DateTime.Now.ToString("yyMMdd") + "_" + Guid.NewGuid().ToString().Substring(0, 8);
            var appTime = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            var amount = Convert.ToInt64(model.Amount);

            var appUser = "FashionStoreUser";

            var embedData = JsonConvert.SerializeObject(new { redirecturl = "https://localhost:7015/Cart/PaymentCallBack" });
            var items = "[]";

            // Bước tạo mã MAC (Signature) của ZaloPay: appId|appTransId|appUser|amount|appTime|embedData|item
            var data = $"{_appId}|{appTransId}|{amount}|{appTime}|{embedData}|{items}";
            var mac = ComputeHmacSha256(data, _key1);

            var values = new Dictionary<string, string>
            {
                { "app_id", _appId },
                { "app_user", appUser },
                { "app_time", appTime },
                { "amount", amount.ToString() },
                { "app_trans_id", appTransId },
                { "embed_data", embedData },
                { "item", items },
                { "description", $"Transaction for FashionStore #{appTransId}" },
                { "mac", mac }
            };

            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(_endpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ZaloPayResponse>(responseString);

            // Nếu thành công (return_code = 1), trả về order_url để đi tới trang thanh toán
            return result?.return_code == 1 ? result.order_url : null;
        }

        private string ComputeHmacSha256(string data, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    // Class phụ để hứng kết quả trả về từ ZaloPay
    public class ZaloPayResponse
    {
        public int return_code { get; set; }
        public string return_message { get; set; }
        public string order_url { get; set; }
        public string zp_trans_token { get; set; }
    }
}