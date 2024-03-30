using Auth.Auth.Interfaces;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Auth.Services
{
    public class Sha256Encryptor : IEncryptionHandler
    {
        public string Encrypt(byte[] dataToEncrypt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(dataToEncrypt);
                StringBuilder builder = new StringBuilder();
                for(int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
