namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/system.security.cryptography.md5(v=vs.110).aspx
    /// </summary>
    public static class EncryptionHelper
    {
        public static string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return GetMd5Hash(md5Hash, input);
            }
        }

        public static bool VerifyMd5Hash(string input, string hash)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return VerifyMd5Hash(md5Hash, input, hash);
            }
        }

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            var sb = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sb.ToString();
        }

        // Verify a hash against a string. 
        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input. 
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            var comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }
    }
}