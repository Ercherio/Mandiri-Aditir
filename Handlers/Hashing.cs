using System.Security.Cryptography;

using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.Extensions.Options;

namespace MerchantService.Handlers
{
    public class Hashing
    {
        private const int SaltLength = 16;
        private readonly ILogger<Hashing> _logger;

        public Hashing(ILogger<Hashing> logger)
        {
            _logger = logger;
        }

        public string HashString(string input, string salt)
        {
            string hashedString = "";

            try
            {
                using (var digest = SHA256.Create())
                {
                    var saltedString = salt + input;
                    var hash = digest.ComputeHash(Encoding.UTF8.GetBytes(saltedString));
                    hashedString = BytesToHex(hash);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error hashing string");
            }

            return salt + "bniqrismerchant" + hashedString;
        }

        public bool VerifyHash(string input, string storedHash)
        {
            var parts = storedHash.Split(new[] { "bniqrismerchant" }, StringSplitOptions.None);
            var salt = parts[0];
            _logger.LogInformation("SALT: {0}", salt);

            var hash = parts[1];
            _logger.LogInformation("Hasil SALT Password: {0}", hash);
            _logger.LogInformation("Password di Database: {0}", storedHash);

            var newHash = HashString(input, salt);
            _logger.LogInformation("New Hash: {0}", newHash);
            return newHash == storedHash;
        }

        public string GetSalt()
        {
            var random = new RNGCryptoServiceProvider();
            var salt = new byte[SaltLength];
            random.GetBytes(salt);
            return BytesToHex(salt);
        }

        private static string BytesToHex(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

    }
}
