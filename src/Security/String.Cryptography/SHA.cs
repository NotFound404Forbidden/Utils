using System.Security.Cryptography;
using System.Text;

namespace Security.String.Cryptography
{
    public class SHA
    {
        public static string SHAmd5Encrypt(string plaintext)
        {
            var bytes = Encoding.Default.GetBytes(plaintext);//求Byte[]数组  
            var Md5 = new MD5CryptoServiceProvider();
            var encryptbytes = Md5.ComputeHash(bytes);//求哈希值  
            return Base64To16(encryptbytes);//将Byte[]数组转为净荷明文(其实就是字符串)  
        }

        public static string SHA1Encrypt(string plaintext)
        {
            var bytes = Encoding.Default.GetBytes(plaintext);
            var SHA = new SHA1CryptoServiceProvider();
            var encryptbytes = SHA.ComputeHash(bytes);
            return Base64To16(encryptbytes);
        }

        public static string SHA256Encrypt(string plaintext)
        {
            var bytes = Encoding.Default.GetBytes(plaintext);
            var SHA256 = new SHA256CryptoServiceProvider();
            var encryptbytes = SHA256.ComputeHash(bytes);
            return Base64To16(encryptbytes);
        }

        public static string SHA384Encrypt(string plaintext)
        {
            var bytes = Encoding.Default.GetBytes(plaintext);
            var SHA384 = new SHA384CryptoServiceProvider();
            var encryptbytes = SHA384.ComputeHash(bytes);
            return Base64To16(encryptbytes);
        }

        public static string SHA512Encrypt(string plaintext)
        {
            var bytes = Encoding.Default.GetBytes(plaintext);
            var SHA512 = new SHA512CryptoServiceProvider();
            var encryptbytes = SHA512.ComputeHash(bytes);
            return Base64To16(encryptbytes);
        }

        private static string Base64To16(byte[] buffer)
        {
            string md_str = string.Empty;
            for (int i = 0; i < buffer.Length; i++)
            {
                md_str += buffer[i].ToString("x2");
            }
            return md_str;
        }
    }
}
