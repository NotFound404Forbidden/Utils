using System;
using System.Security.Cryptography;
using System.Text;

namespace Security.String.Cryptography
{
    public class AES
    {
        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <param name="keys">keys</param>
        /// <returns>密文</returns>
        public static string Encrypt(string plaintext, string key)
        {
            if (string.IsNullOrEmpty(plaintext))
                throw new ArgumentNullException($"{nameof(plaintext)} must not be null or empty!");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException($"{nameof(key)} must not be null or empty!");

            if (key.Length < 16)
                key = (key + new string('0', 16)).Substring(0, 16);
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(plaintext);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray);
        }

        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="keys">keys</param>
        /// <returns>明文</returns>
        public static string Decrypt(string ciphertext, string key)
        {
            if (string.IsNullOrEmpty(ciphertext))
                throw new ArgumentNullException($"{nameof(ciphertext)} must not be null or empty!");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException($"{nameof(key)} must not be null or empty!");

            if (key.Length < 16)
                key = (key + new string('0', 16)).Substring(0, 16);
            Byte[] toEncryptArray = Convert.FromBase64String(ciphertext);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
    }
}
