using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ScanFile
{
    public static class DESEncrypt
    {
        public static string Encrypt(this string source)
        {
            EncryptInfo model = new EncryptInfo(source);
            model.Iv = "liaojing";
            model.Key = "liaojing";
            return Encrypt(model);
        }
        public static string Dencrypt(this string source)
        {
            EncryptInfo model = new EncryptInfo(source);
            model.Iv = "liaojing";
            model.Key = "liaojing";
            return Dencrypt(model);
        }
        private static string Encrypt(EncryptInfo model)
        {
            string result = "";
            try
            {
                byte[] byKey = model.Encode.GetBytes(model.Key);
                byte[] byIV = model.Encode.GetBytes(model.Iv);
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                int i = cryptoProvider.KeySize;
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cst))
                {
                    sw.Write(model.Source);
                    sw.Flush();
                    cst.FlushFinalBlock();
                    sw.Flush();
                    result = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                }
            }
            catch
            {
                return "";
            }
            return result;
        }

        private static string Dencrypt(EncryptInfo model)
        {
            try
            {
                string result = "";
                byte[] byKey = model.Encode.GetBytes(model.Key);
                byte[] byIV = model.Encode.GetBytes(model.Iv);
                byte[] byEnc = Convert.FromBase64String(model.Source);
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                using (MemoryStream ms = new MemoryStream(byEnc))
                using (CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cst))
                    result = sr.ReadToEnd();
                return result;
            }
            catch
            {
                return "";
            }
        }
    }
    public class EncryptInfo
    {
        public EncryptInfo(string source)
        {
            this.Source = source;
            this.Encode = Encoding.Default;
        }

        public EncryptInfo(string source, Encoding encode)
        {
            this.Source = source;
            this.Encode = encode;
        }
        public string Source { get; set; }
        public string Key { get; set; }
        public string Iv { get; set; }
        public Encoding Encode { get; set; }
    }
}
