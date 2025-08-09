using System.Security.Cryptography;
using System.Text;

namespace Automotive_Project.Extensions
{
    public class PasswordHasher
    {
        public static string GenerateSalt(int size = 16)
        {
            byte[] saltBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }


        public static string Hasher(string password, string salt)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                string saltedPassword = password + salt;
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

                var sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                    sb.Append(b.ToString("X2"));

                return sb.ToString();
            }
        }
    }
}
