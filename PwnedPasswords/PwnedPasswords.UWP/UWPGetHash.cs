using System.Security.Cryptography;
using System.Text;

namespace PwnedPasswords.UWP
{
    public class UWPGetHash : IHash
    {
        public string GetHash(string input)
        {
            using (var sha1 = SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha1.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
