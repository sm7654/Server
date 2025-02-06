using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    static class AesEncryption
    {
        private static byte[] aesKey;
        private static byte[] aesIV;

        public static void Addkeys(byte[] key, byte[] Iv)
        {
            aesKey = RsaEncryption.DecryptToBytes(key);
            aesIV = RsaEncryption.DecryptToBytes(Iv);
        }


        public static byte[] EncryptedData(byte[] data)
        {
            if (aesKey == null || aesIV == null)
                return null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = aesKey;
                    aes.IV = aesIV;

                    ICryptoTransform encryptor = aes.CreateEncryptor();
                    using (MemoryStream ms = new MemoryStream())
                    using (CryptoStream Cry = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        Cry.Write(data, 0, data.Length);

                        Cry.FlushFinalBlock();
                        return ms.ToArray();
                    }

                }
            }
            catch (Exception e) { }

            return null;
        }

        public static byte[] DecryptData(byte[] encryptedData)
        {
            if (aesKey == null || aesIV == null)
                return null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = aesKey;
                    aes.IV = aesIV;

                    ICryptoTransform decryptor = aes.CreateDecryptor();
                    using (MemoryStream ms = new MemoryStream(encryptedData))
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (MemoryStream decryptedStream = new MemoryStream())
                    {
                        cs.CopyTo(decryptedStream);
                        return decryptedStream.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                // Handle the exception appropriately
            }

            return null;
        }

       
    }
}
