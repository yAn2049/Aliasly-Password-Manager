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
                // A mesterkulcsból 256 bites kulcsot generálunk.
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(masterKey));

                // Az IV (Initialization Vector) az AES blokkméretének megfelelően 16 bájt hosszú.
                byte[] iv = new byte[16];
                Array.Copy(key, iv, iv.Length); // Az IV-t a kulcs első 16 bájtjából állítjuk elő.

                // AES algoritmus inicializálása.
                using (var aes = Aes.Create())
                {
                    aes.Key = key; // A generált kulcs beállítása.
                    aes.IV = iv; // A generált IV beállítása.
                    aes.Mode = CipherMode.CBC; // CBC (Cipher Block Chaining) mód használata.
                    aes.Padding = PaddingMode.PKCS7; // PKCS7 padding a szöveg hosszának igazításához.

                    // Titkosító létrehozása.
                    using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    using (var ms = new MemoryStream())
                    {
                        // A titkosított adatokat egy memóriastreambe írjuk.
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(text); // A titkosítandó szöveg írása.
                        }

                        // A titkosított bájtokat Base64 formátumba konvertáljuk.
                        byte[] encryptedBytes = ms.ToArray();
                        return Convert.ToBase64String(encryptedBytes);
                    }
                }
            }
        }



        public string DecryptText(string masterKey, string encryptedText) // Szöveg visszafejtés
        {
            // SHA256 algoritmus használata a mesterkulcsból kulcs és IV generálásához.
            using (var sha256 = SHA256.Create())
            {
                // A mesterkulcsból 256 bites kulcsot generálunk.
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(masterKey));

                // Az IV (Initialization Vector) az AES blokkméretének megfelelően 16 bájt hosszú.
                byte[] iv = new byte[16];
                Array.Copy(key, iv, iv.Length); // Az IV-t a kulcs első 16 bájtjából állítjuk elő.

                // AES algoritmus inicializálása.
                using (var aes = Aes.Create())
                {
                    aes.Key = key; // A generált kulcs beállítása.
                    aes.IV = iv; // A generált IV beállítása.
                    aes.Mode = CipherMode.CBC; // CBC (Cipher Block Chaining) mód használata.
                    aes.Padding = PaddingMode.PKCS7; // PKCS7 padding a szöveg hosszának igazításához.

                    // Visszafejtő létrehozása.
                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var sr = new StreamReader(cs))
                    {
                        // A visszafejtett szöveg olvasása és visszaadása.
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}