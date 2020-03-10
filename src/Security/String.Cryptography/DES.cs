using System;
using System.IO;
using System.Security.Cryptography;

namespace Security.String.Cryptography
{
    /// <summary>
    /// 使用DES加密
    /// </summary>
    public class DES
    {
        //密钥
        public static byte[] KEY = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
        //向量
        public static byte[] IV = new byte[] { 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01 };

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
                throw new ArgumentNullException($"{nameof(plaintext)} must not be null or empty!");

            try
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                int i = cryptoProvider.KeySize;
                MemoryStream ms = new MemoryStream();
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(KEY, IV), CryptoStreamMode.Write);

                using (StreamWriter sw = new StreamWriter(cst))
                {
                    sw.Write(plaintext);
                    sw.Flush();
                    cst.FlushFinalBlock();
                    sw.Flush();
                }

                string strRet = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                return strRet;
            }
            catch (Exception ex)
            {
                throw new Exception("DES encrypt falied", ex);
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext))
                throw new ArgumentNullException($"{nameof(ciphertext)} must not be null or empty!");

            byte[] byEnc;
            try
            {
                ciphertext.Replace("_%_", "/");
                ciphertext.Replace("-%-", "#");
                byEnc = Convert.FromBase64String(ciphertext);

                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream(byEnc);
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY, IV), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cst);
                return sr.ReadToEnd();
            }
            catch(Exception ex)
            {
                throw new Exception("DES decrypt falied!", ex);
            }
        }
    }
}
