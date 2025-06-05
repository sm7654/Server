using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    static class RsaEncryption
    {
        private static RSA Service;
        private static string publicKey;
        private static string privateKey;

        public static byte[] GenerateKeys()
        {
            using (RSA service = RSA.Create())
            {
                publicKey = service.ToXmlString(false);
                MessageBox.Show(Encoding.UTF8.GetBytes(publicKey).Length.ToString());
                privateKey = service.ToXmlString(true);
                return Encoding.UTF8.GetBytes(publicKey);
            }
        }

        public static byte[] Encrypt(string data, string key)
        {
            // rsa  יצירת אובייקט זמני של 
            using (RSA service = RSA.Create())
            {
                //יבוא 
                service.FromXmlString(key);
                return service.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.OaepSHA1);
            }
        }

        public static byte[] EncryptBytes(byte[] data, string key)
        {
            using (RSA service = RSA.Create())
            {
                service.FromXmlString(key);
                return service.Encrypt(data, RSAEncryptionPadding.OaepSHA1);
            }
        }

        public static string Decrypt(byte[] data)
        {
            using (RSA service = RSA.Create())
            {
                service.FromXmlString(privateKey);
                byte[] decryptedData = service.Decrypt(data, RSAEncryptionPadding.OaepSHA1);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        public static byte[] DecryptToBytes(byte[] data)
        {
            using (RSA service = RSA.Create())
            {
                service.FromXmlString(privateKey);
                return service.Decrypt(data, RSAEncryptionPadding.OaepSHA1);
            }
        }

        public static byte[] getpublickey()
        {
            return Encoding.UTF8.GetBytes(publicKey);
        }


    }
}
