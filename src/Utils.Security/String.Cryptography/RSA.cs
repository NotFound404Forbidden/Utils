using System;
using System.IO;
using System.Security.Cryptography;

namespace Utils.Security.String.Cryptography
{
    public class RSA
    {
        public int RsaKeySize = 2048;
        public string PublicKeyFileName = "RSA.Pub";
        public string PrivateKeyFileName = "RSA.PRIVATE";

        /// <summary>
        /// 在给定路径中生成XML格式的私钥和公钥
        /// </summary>
        /// <param name="path">生成文件路径</param>
        public void GenerateKeys(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"path:{path} not fount!");

            using (var rsa = new RSACryptoServiceProvider(RsaKeySize))
            {
                try
                {
                    // 获取私钥和公钥。
                    var publicKey = rsa.ToXmlString(false);
                    var privateKey = rsa.ToXmlString(true);

                    // 保存到磁盘
                    File.WriteAllText(Path.Combine(path, PublicKeyFileName), publicKey);
                    File.WriteAllText(Path.Combine(path, PrivateKeyFileName), privateKey);
                }
                catch (Exception e)
                {
                    throw new Exception("Create pub or pri File failed!", e);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <param name="publicKeyFilePath">公钥路径</param>
        /// <returns>密文</returns>
        public string Encrypt(string plaintext, string publicKeyFilePath)
        {
            if (string.IsNullOrEmpty(plaintext))
                throw new ArgumentNullException($"{nameof(plaintext)} must not be null or empty!");

            if (!File.Exists(publicKeyFilePath))
                throw new FileNotFoundException("Pub file is not founded!");

            using (var rsa = new RSACryptoServiceProvider(RsaKeySize))
            {
                try
                {
                    //加载公钥
                    var publicXmlKey = File.ReadAllText(publicKeyFilePath);
                    rsa.FromXmlString(publicXmlKey);
                    var bytesToEncrypt = System.Text.Encoding.Unicode.GetBytes(plaintext);
                    var bytesEncrypted = rsa.Encrypt(bytesToEncrypt, false);
                    return Convert.ToBase64String(bytesEncrypted);
                }
                catch (Exception e)
                {
                    throw new Exception("Encrypt failed", e);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="privateKeyFilePath">私钥路径</param>
        /// <returns>明文</returns>
        public string Decrypt(string ciphertext, string privateKeyFilePath)
        {
            if (string.IsNullOrEmpty(ciphertext))
                throw new ArgumentNullException($"{nameof(ciphertext)} must not be null or empty!");

            if (!File.Exists(privateKeyFilePath))
                throw new FileNotFoundException("Pub file is not founded!");

            using (var rsa = new RSACryptoServiceProvider(RsaKeySize))
            {
                try
                {
                    var privateXmlKey = File.ReadAllText(privateKeyFilePath);
                    rsa.FromXmlString(privateXmlKey);
                    var bytesEncrypted = Convert.FromBase64String(ciphertext);
                    var bytesPlainText = rsa.Decrypt(bytesEncrypted, false);
                    return System.Text.Encoding.Unicode.GetString(bytesPlainText);
                }
                catch (Exception e)
                {
                    throw new Exception("Decrypt failed", e);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
    }
}
