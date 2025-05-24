using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ServerSide
{
    static class AesEncryption
    {
        
        private static byte[] AESkeyTemp;
        private static byte[] AESivTemp;

        private static byte[] AESkeyCredentialVar;
        private static byte[] AESivCredentialVar;



        public static void GenerateTempKeys()
        {
            using (Aes aesServise = Aes.Create())
            {
                aesServise.KeySize = 256;
                AESkeyTemp = aesServise.Key;
                AESivTemp = aesServise.IV;
            }
        }

        public static void SetAESENVRkeys(string key, string iv)
        {
            AESkeyCredentialVar = Convert.FromBase64String(key);
            AESivCredentialVar = Convert.FromBase64String(iv);
        }
        public static (string, string) generate()
        {
            using (Aes aesServise = Aes.Create())
            {
                aesServise.KeySize = 256;
                AESkeyCredentialVar = aesServise.Key;
                AESivCredentialVar = aesServise.IV;
            }



            return (Convert.ToBase64String(AESkeyCredentialVar), Convert.ToBase64String(AESivCredentialVar));
        }



        public static (byte[], byte[]) GetAesEncryptedTempKeys(string RsaKey)
        {
            byte[] EncryptedKey = RsaEncryption.EncryptBytes(AESkeyTemp, RsaKey);
            byte[] EncryptedIV = RsaEncryption.EncryptBytes(AESivTemp, RsaKey);

            return (EncryptedKey, EncryptedIV);
        }
        public static (byte[], byte[]) GetAesEncryptedKeys(string RsaKey)
        {
            byte[] EncryptedKey = RsaEncryption.EncryptBytes(AESkeyCredentialVar, RsaKey);
            byte[] EncryptedIV = RsaEncryption.EncryptBytes(AESivCredentialVar, RsaKey);

            return (EncryptedKey, EncryptedIV);
        }





        public static byte[] EncryptDataToSql(byte[] data)
        {
            if (AESkeyCredentialVar == null || AESivCredentialVar == null)
                return null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = AESkeyCredentialVar;
                    aes.IV = AESivCredentialVar;

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

        public static byte[] DecryptDataToSql(byte[] encryptedData)
        {
            if (AESkeyCredentialVar == null || AESivCredentialVar == null)
                return null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = AESkeyCredentialVar;
                    aes.IV = AESivCredentialVar;

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


        public static string DecryptDataToSqlToString(byte[] data)
        {
            return Encoding.UTF8.GetString(DecryptDataToSql(data));
        }


















        public static byte[] EncryptedDataWithTempKeys(byte[] data)
        {
            if (AESkeyTemp == null || AESivTemp == null)
                return null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = AESkeyTemp;
                    aes.IV = AESivTemp;

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

        public static byte[] DecryptDataWithTempKeys(byte[] encryptedData)
        {
            if (AESkeyTemp == null || AESivTemp == null)
                return null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = AESkeyTemp;
                    aes.IV = AESivTemp;

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

        
        public static string DecryptDataToStringWithTempKeys(byte[] data)
        {
            return Encoding.UTF8.GetString(DecryptDataWithTempKeys(data));
        }




















        /*public static void GenarateKeys()
        {

            using (Aes aesServise = Aes.Create())
            {
                aesServise.KeySize = 256;
                AESkey = aesServise.Key;
                AESiv = aesServise.IV;
            }
           
            //new Thread(ChengeIvAndKey).Start();
        }*/


        /* public static byte[] EncryptedData(byte[] data)
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
        }*/
        /*public static int GetRandomDelay()
        {
            Random _random = new Random();
            return _random.Next(10000, 11000); // Random number between 60000 and 180000 (inclusive)
        }

        public static void ChengeIvAndKey()
        {
            //while (true)
            //{
                int delay = GetRandomDelay();
                delay = 0;
                Thread.Sleep(delay);
                byte[] AESTempIV;
                byte[] AESTempIKey;
                using (Aes aesServise = Aes.Create())
                {
                    aesServise.KeySize = 256;
                    AESTempIV = aesServise.IV;
                    AESTempIKey = aesServise.Key;
                }
            
                ServerServices.ChengeIvAndKeyToSessions(AESTempIV, AESTempIKey);
                AESiv = AESTempIV;
            AESkey = AESTempIKey;

            //}
        }*/
    }
}
