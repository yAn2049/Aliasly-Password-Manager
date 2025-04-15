using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Aliasly
{
    public class TitkositasMetodusok
    {
        public string EncryptText(string masterKey, string text) // Szöveg titkosítás
        {
            using (var sha256 = SHA256.Create())
            {
                // A mesterkulcsból 256 bites kulcsot generál
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(masterKey));

                
                byte[] iv = new byte[16]; // ennek mindig 16 bytenak kell lennie
                Array.Copy(key, iv, iv.Length); // A kulcs elso 16 bytejat hasznaljuk IV-nek

                // AES algoritmus elinditasa
                using (var aes = Aes.Create())
                {
                    // AES beallitasok
                    aes.Key = key; 
                    aes.IV = iv; 
                    aes.Mode = CipherMode.CBC; 
                    aes.Padding = PaddingMode.PKCS7;

                    // Encrypter letrehozasa
                    using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    using (var ms = new MemoryStream())
                    {
                        // titkosisott adatok memory streambe irasa
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(text); // A titkosítandó szöveg írása.
                        }

                        // bytek tömbbé alakítja és Base64 stringge alakítja
                        byte[] encryptedBytes = ms.ToArray();
                        return Convert.ToBase64String(encryptedBytes);
                    }
                }
            }
        }



        public string DecryptText(string masterKey, string encryptedText) // Szöveg visszafejtés
        {
            // SHA256 algoritmus elinditasa
            using (var sha256 = SHA256.Create())
            {
                // A mesterkulcsból 256 bites kulcsot generál
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(masterKey));
              
                byte[] iv = new byte[16];       // mindig 16 byte hosszu
                Array.Copy(key, iv, iv.Length); // a kulcs elso 16 bytejat hasznaljuk IV-nek

                // AES algoritmus elinditasa
                using (var aes = Aes.Create())
                {

                    // AES beallitasok
                    aes.Key = key; 
                    aes.Mode = CipherMode.CBC; 
                    aes.Padding = PaddingMode.PKCS7;

                    // Decrypter letrehozasa
                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var sr = new StreamReader(cs))
                    {
                        // A visszafejtett visszaadása
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}