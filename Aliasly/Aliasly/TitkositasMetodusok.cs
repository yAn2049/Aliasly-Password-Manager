using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aliasly
{
    public class TitkositasMetodusok
    {
        private List<MesterKulcs> mester_kulcs = new List<MesterKulcs>();
        private List<Jelszo> jelszavak = new List<Jelszo>();
        private List<Felhasznalo> felhasznalok = new List<Felhasznalo>();
        private List<HozzaferesLog> hozzaferes_log = new List<HozzaferesLog>();

        public List<string> HashMasterKey(string masterKey)
        {
            // Generate a salt
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Hash the master key with the salt using PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(masterKey, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Combine the salt and hash
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Convert to base64 strings
            string saltString = Convert.ToBase64String(salt);
            string hashString = Convert.ToBase64String(hash);
            string fullHashString = Convert.ToBase64String(hashBytes);

            // Return the list containing the full hash, the salt, and the hash without the salt
            return new List<string> { fullHashString, saltString, hashString };
        }
    }

    

}