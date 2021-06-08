using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace QiuYudengMathematics.Comm
{
    public class AESComm
    {
        /// <summary>
        /// 加解密
        /// </summary>
        /// <param name="SourceStr"></param>
        /// <param name="Encryption">True:加密;False:解密</param>
        /// <returns></returns>
        public string AES(string SourceStr, bool Encryption)
        {
            try
            {
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider
                {
                    Key = sha256.ComputeHash(Encoding.UTF8.GetBytes("QiuYudengMathematics.AESKey")),
                    IV = md5.ComputeHash(Encoding.UTF8.GetBytes("QiuYudengMathematics.AESIv"))
                };
                var dataByteArray = Encryption ? Encoding.UTF8.GetBytes(SourceStr) : Convert.FromBase64String(SourceStr);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, Encryption ? aes.CreateEncryptor() : aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        return Encryption ? Convert.ToBase64String(ms.ToArray()) : Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}