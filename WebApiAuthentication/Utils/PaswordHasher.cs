using System.Security.Cryptography;
using System.Text;

namespace WebApiAuthentication.Utils
{
    public static class PaswordHasher
    {
        public static string ToSha256(this string pass)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));
            StringBuilder builder = new StringBuilder();

            foreach (var i in bytes)
            {
                builder.Append(i.ToString("x2"));
            }
            return builder.ToString();
        }

        public static async Task<string> ToSha256Async(this string pass)
        {
            return await Task.Run(() =>
            {
                using SHA256 sha256 = SHA256.Create();
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));
                StringBuilder builder = new StringBuilder();

                foreach (var i in bytes)
                {
                    builder.Append(i.ToString("x2"));
                }
                return builder.ToString();
            });
        }
    }
}
