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

        public static byte[] Encrypt(string data, string Key)
        {
            Service = RSA.Create();
            Service.FromXmlString(Key);

            return Service.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.Pkcs1);
        }
        public static byte[] Decrypt(string data, string Key)
        {
            Service = RSA.Create();
            Service.FromXmlString(Key);

            return Service.Decrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.Pkcs1);
        }
    }
}
