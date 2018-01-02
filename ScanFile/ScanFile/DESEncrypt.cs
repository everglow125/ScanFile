namespace ScanFile
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;

    public static class DESEncrypt
    {
        private static string Dencrypt(EncryptInfo model)
        {
            try
            {
                string str = "";
                byte[] bytes = model.Encode.GetBytes(model.Key);
                byte[] rgbIV = model.Encode.GetBytes(model.Iv);
                byte[] buffer = Convert.FromBase64String(model.Source);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                using (MemoryStream stream = new MemoryStream(buffer))
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(stream2))
                        {
                            str = reader.ReadToEnd();
                        }
                    }
                }
                return str;
            }
            catch
            {
                return "";
            }
        }

        public static string Dencrypt(this string source)
        {
            EncryptInfo model = new EncryptInfo(source)
            {
                Iv = "liaojing",
                Key = "liaojing"
            };
            return Dencrypt(model);
        }

        private static string Encrypt(EncryptInfo model)
        {
            string str = "";
            try
            {
                byte[] bytes = model.Encode.GetBytes(model.Key);
                byte[] rgbIV = model.Encode.GetBytes(model.Iv);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                int keySize = provider.KeySize;
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(stream2))
                        {
                            writer.Write(model.Source);
                            writer.Flush();
                            stream2.FlushFinalBlock();
                            writer.Flush();
                            str = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length);
                        }
                    }
                    return str;
                }
            }
            catch
            {
                return "";
            }
            return str;
        }

        public static string Encrypt(this string source)
        {
            EncryptInfo model = new EncryptInfo(source)
            {
                Iv = "liaojing",
                Key = "liaojing"
            };
            return Encrypt(model);
        }
    }
}

