using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace DevTrack.Infrastructure.Services.Encryption
{
    public class SymmetricEncryptionService : ISymmetricEncryptionService
    {
        private readonly IConfiguration _configuration;

        public SymmetricEncryptionService(IConfiguration config)
        {
            _configuration = config;
        }

        public async Task<string> Encrypt(string data)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                var kkey = _configuration["Encryption:symmetricKey"];
                aes.Key = Encoding.UTF8.GetBytes(kkey);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            await streamWriter.WriteAsync(data);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public async Task<string> Decrypt(string encryptedData)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(encryptedData.Replace(' ', '+'));

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_configuration["Encryption:symmetricKey"]);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return await streamReader.ReadToEndAsync();
                        }
                    }
                }
            }
        }
    }
}
