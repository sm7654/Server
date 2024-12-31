using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    static class Encryption
    {
        private static RSA Service;
        private static string publicKey;
        private static string privateKey;

        public static byte[] GenerateKeys()
        {
            Service = RSA.Create();
            Service.KeySize = 1024;
            publicKey = Service.ToXmlString(false);
            privateKey = Service.ToXmlString(true);
            return Encoding.UTF8.GetBytes(publicKey);
        }

        public static byte[] getpublickey()
        {
            return Encoding.UTF8.GetBytes(publicKey);
        }





        public static byte[] Encrypt(string data, string Key)
        {
            Service = RSA.Create();
            Service.FromXmlString(Key);

            return Service.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.Pkcs1);
        }
        public static string Decrypt(byte[] data)//
        {
            Service = RSA.Create();
            Service.FromXmlString(privateKey);

            return Encoding.UTF8.GetString(Service.Decrypt(data, RSAEncryptionPadding.Pkcs1));
        }
    }
}
