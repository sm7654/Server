using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    static class AesEncryption
    {
        private static byte[] AESkey;
        private static byte[] AESiv;

        /*public static void Addkeys(byte[] key, byte[] Iv)
        {
            AESkey = RsaEncryption.DecryptToBytes(key);
            AESiv = RsaEncryption.DecryptToBytes(Iv);
        }
*/

        public static int GetRandomDelay()
        {
            Random _random = new Random();
            return _random.Next(10000, 11000); // Random number between 60000 and 180000 (inclusive)
        }

        public static void ChengeIv()
        {
            //while (true)
            //{
                int delay = GetRandomDelay();
                Thread.Sleep(delay);
                byte[] AESTemp;
                using (Aes aesServise = Aes.Create())
                {
                    aesServise.KeySize = 192;
                    AESTemp = aesServise.IV;
                }
            MessageBox.Show(Convert.ToBase64String(AESTemp));
                ServerServices.ChangeIvToSessions(AESTemp);
                AESiv = AESTemp;
            //}
        }
        public static void GenarateKeys()
        {

            using (Aes aesServise = Aes.Create())
            {
                aesServise.KeySize = 192;
                AESkey = aesServise.Key;
                AESiv = aesServise.IV;
            }
            //new Thread(ChengeIv).Start();
        }
        public static (byte[], byte[]) GetAesEncryptedKeys(string RsaKey)
        {
            byte[] EncryptedKey = RsaEncryption.EncryptBytes(AESkey, RsaKey);
            byte[] EncryptedIV = RsaEncryption.EncryptBytes(AESiv, RsaKey);

            return (EncryptedKey, EncryptedIV);
        }
        public static byte[] EncryptedData(byte[] data)
        {
            if (AESkey == null || AESiv == null)
                return null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = AESkey;
                    aes.IV = AESiv;

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
            if (AESkey == null || AESiv == null)
                return null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = AESkey;
                    aes.IV = AESiv;

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
        public static string DecryptDataToString(byte[] data)
        {
            return Encoding.UTF8.GetString(DecryptData(data));
        }


        
    }
}
