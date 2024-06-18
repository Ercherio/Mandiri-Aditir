using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace MerchantService.Handlers
{
    public class HmacHelper
    {
        private readonly string _secretKey;

        public HmacHelper(IOptions<AppSettings> appSettings) { 
            _secretKey = appSettings.Value.SecretKey;
        }


        public string GenerateHmac(string message)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                return Convert.ToBase64String(hash);
            }
        }

        public bool VerifyHmac(string message, string hmacToVerify)
        {
            var generatedHmac = GenerateHmac(message);
            return generatedHmac == hmacToVerify;
        }
    }
}
