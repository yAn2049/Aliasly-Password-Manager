using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aliasly
{
    public class TitkositasMetodusok
    {
        public string EncryptText(string masterKey, string text)
        {
            // Generate a key and IV from the master key using SHA256
            using (var sha256 = SHA256.Create())
            {
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(masterKey));
                byte[] iv = new byte[16]; // AES block size is 16 bytes
                Array.Copy(key, iv, iv.Length);

                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(text);
                        }

                        byte[] encryptedBytes = ms.ToArray();
                        return Convert.ToBase64String(encryptedBytes);
                    }
                }
            }
        }



        public string DecryptText(string masterKey, string encryptedText)
        {
            // Felhasznaloi adatokhoz pl.

            // Generate a key and IV from the master key using SHA256
            using (var sha256 = SHA256.Create())
            {
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(masterKey));
                byte[] iv = new byte[16]; // AES block size is 16 bytes
                Array.Copy(key, iv, iv.Length);

                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}