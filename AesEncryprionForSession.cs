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
    public class AesEncryprionForSession
    {
        private byte[] aesIvClient;
        private byte[] aesKeyClient;

        private byte[] aesIvMicro;
        private byte[] aesKeyMicro;

        private byte[] TempClientKey;
        private byte[] TempClientIv;


        private byte[] TempMicroKey;
        private byte[] TempMicroIv;



        private session currentSession;

        public AesEncryprionForSession()
        {
            using (Aes aesServise = Aes.Create())
            {
                aesServise.KeySize = 256;
                aesIvMicro = aesServise.IV;
                aesKeyMicro = aesServise.Key;
            }
            using (Aes aesServise = Aes.Create())
            {
                aesServise.KeySize = 256;
                aesIvClient = aesServise.IV;
                aesKeyClient = aesServise.Key;
            }
        }

        public void setSession(session s)
        {
            currentSession = s;
        }
        public void regenerateKeysForClient()
        {

            try
            {
                byte[] Key;

                byte[] Iv;

                using (Aes aesServise = Aes.Create())
                {
                    aesServise.KeySize = 256;
                    Iv = aesServise.IV;
                    Key = aesServise.Key;
                }

                string encrypted = Convert.ToBase64String(Key);

                string encryptedIv = Convert.ToBase64String(Iv);
                byte[] serverRole = ServerServices.GetServerRole();
                byte[] messageToSend = Encoding.UTF8.GetBytes($"&CHANGEKEYIV&{encrypted}&{encryptedIv}");
                
                currentSession.SendToClient(serverRole.Concat(messageToSend).ToArray(), false);

                TempClientKey = Key;
                TempClientIv = Iv;
            }
            catch (Exception ex)
            {

            }


        }


        public void regenerateKeysForMicro()
        {

            try
            {
                byte[] Key;

                byte[] Iv;

                using (Aes aesServise = Aes.Create())
                {
                    aesServise.KeySize = 256;
                    Iv = aesServise.IV;
                    Key = aesServise.Key;
                }

                string encrypted = Convert.ToBase64String(Key);

                string encryptedIv = Convert.ToBase64String(Iv);

                byte[] serverRole = ServerServices.GetServerRole();
                
                byte[] messageToSend = Encoding.UTF8.GetBytes($"&CHANGEKEYIV&{encrypted}&{encryptedIv}");
                currentSession.SendToMicro(serverRole.Concat(messageToSend).ToArray(), false);

                TempMicroKey = Key;
                TempMicroIv = Iv;
            }
            catch (Exception ex)
            {

            }


        }

        public byte[] EncryptedDataToClient(byte[] data)
        {
            if (aesKeyClient == null || aesIvClient == null)
                return null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = aesKeyClient;
                    aes.IV = aesIvClient;

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

        public byte[] EncryptedDataToClient(string data)
        {
            return EncryptedDataToClient(Encoding.UTF8.GetBytes(data));
        }
        public byte[] DecryptDataForClient(byte[] encryptedData)
        {
            if (aesKeyClient == null || aesIvClient == null)
            {
                MessageBox.Show("they are null");
                return null;
            }
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = aesKeyClient;
                    aes.IV = aesIvClient;

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
                Console.WriteLine();
            }

            return null;
        }

        public string DecryptDataForClientToString(byte[] encryptedData)
        {
            try
            {
                return Encoding.UTF8.GetString(DecryptDataForClient(encryptedData));
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public byte[] EncryptedDataToMicro(byte[] data)
        {
            if (aesKeyMicro == null || aesIvMicro == null)
                return null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = aesKeyMicro;
                    aes.IV = aesIvMicro;

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

        public byte[] EncryptedDataToMicro(string data)
        {
            return EncryptedDataToMicro(Encoding.UTF8.GetBytes(data));
        }
        public byte[] DecryptDataForMicro(byte[] encryptedData)
        {
            if (aesKeyMicro == null || aesIvMicro == null)
                return null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = aesKeyMicro;
                    aes.IV = aesIvMicro;

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
                Console.WriteLine();
            }

            return null;
        }

        public string DecryptDataForMicroToString(byte[] encryptedData)
        {
            try
            {
                return Encoding.UTF8.GetString(DecryptDataForMicro(encryptedData));
            } catch (Exception e)
            {
                return "";
            }
        }



        public (byte[], byte[]) GetMicroKeys(string publickeyofmicro)
        {
            return (RsaEncryption.EncryptBytes(aesKeyMicro, publickeyofmicro), RsaEncryption.EncryptBytes(aesIvMicro, publickeyofmicro));
        }

        public (byte[], byte[]) GetClientKeys(string publickeyofclient)
        {
            return (RsaEncryption.EncryptBytes(aesKeyClient, publickeyofclient), RsaEncryption.EncryptBytes(aesIvClient, publickeyofclient));
        }


        public void ClientGotKeysSuccessfuly()
        {
            aesIvClient = TempClientIv;
            aesKeyClient = TempClientKey;

            TempClientIv = null;
            TempClientKey = null;
        }
        public void MicroGotKeysSuccessfuly()
        {
            aesIvMicro = TempMicroIv;
            aesKeyMicro = TempMicroKey;

            TempMicroKey = null;
            TempMicroIv = null;
        }



    }
}
