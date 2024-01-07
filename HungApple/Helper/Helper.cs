using System.Globalization;
using System.Text;

namespace HungApple
{
    public static class Helper
    {
        public static string MoneyFormat(decimal m)
        {
            var a = Math.Ceiling(m).ToString("N").Replace(".00", " đ");
            return Math.Ceiling(m).ToString("N").Replace(".00", " đ");
        }
        public static string ToMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
