using System.Text;

namespace Imagio.Helpdesk.ViewModel.Helper
{
    internal static class Hash
    {
        public static byte[] Calc(string password)
        {
            byte[] hash = System.Security.Cryptography.MD5.Create().ComputeHash(
                Encoding.ASCII.GetBytes(password));
            return hash;
        }

        public static string ToString(byte[] hash)
        {
            var sb = new StringBuilder("*");
            foreach (var b in hash)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString().ToUpper();
        }

        public static string ToString(string password)
        {
            var hash = Calc(password);
            var sb = new StringBuilder("*");
            foreach (var b in hash)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString().ToUpper();
        }
    }
}
