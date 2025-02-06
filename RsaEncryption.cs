using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
                service.KeySize = 4096;
                publicKey = service.ToXmlString(false);
                privateKey = service.ToXmlString(true);
                return Encoding.UTF8.GetBytes(publicKey);
            }
        }

        public static byte[] Encrypt(string data, string key)
        {
            using (RSA service = RSA.Create())
            {
                service.FromXmlString(key);
                return service.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.Pkcs1);
            }
        }

        public static byte[] EncryptBytes(byte[] data, string key)
        {
            using (RSA service = RSA.Create())
            {
                service.FromXmlString(key);
                return service.Encrypt(data, RSAEncryptionPadding.Pkcs1);
            }
        }

        public static string Decrypt(byte[] data)
        {
            using (RSA service = RSA.Create())
            {
                service.FromXmlString(privateKey);
                byte[] decryptedData = service.Decrypt(data, RSAEncryptionPadding.Pkcs1);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        public static byte[] DecryptToBytes(byte[] data)
        {
            using (RSA service = RSA.Create())
            {
                service.FromXmlString(privateKey);
                return service.Decrypt(data, RSAEncryptionPadding.Pkcs1);
            }
        }

        public static byte[] getpublickey()
        {
            return Encoding.UTF8.GetBytes(publicKey);
        }


    }
}
