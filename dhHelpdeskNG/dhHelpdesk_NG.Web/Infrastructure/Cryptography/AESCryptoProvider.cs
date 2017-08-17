
using System;
using System.Security.Cryptography;
using System.Text;

namespace DH.Helpdesk.Web.Infrastructure.Cryptography
{
    public static class AESCryptoProvider
    {
        private const string _IV256 = @"SA%Z2LPE}KC4R!FM";       

        public static string Encrypt256(string valueToEncrypt, string key)
        {
            try
            {
                var aes = new AesCryptoServiceProvider();
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.IV = Encoding.UTF8.GetBytes(_IV256);
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;                
                byte[] src = Encoding.Unicode.GetBytes(valueToEncrypt);
                
                using (var encrypt = aes.CreateEncryptor())
                {
                    byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);
                    return Convert.ToBase64String(dest);
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string Decrypt256(string valueToDecrypt, string key)
        {            
            try
            {
                var aes = new AesCryptoServiceProvider();
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.IV = Encoding.UTF8.GetBytes(_IV256);
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;             
                byte[] src = Convert.FromBase64String(valueToDecrypt);
                
                using (var decrypt = aes.CreateDecryptor())
                {
                    byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                    return Encoding.Unicode.GetString(dest);
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}