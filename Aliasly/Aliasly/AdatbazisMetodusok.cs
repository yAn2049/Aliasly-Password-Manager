using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Aliasly
{
    public class AdatbazisMetodusok
    {
        // Táblázat listák //
        private List<MesterKulcs> mester_kulcs = new List<MesterKulcs>();
        private List<KliensLista> kliens_lista = new List<KliensLista>();
       


        // Sql csatlakozási paraméterek, xampp és mampp //
        private string xampp_conn_params = "server=localhost;user=root;database=aliasly;port=3306";
        private string mampp_conn_params = "server=localhost;user=root;database=aliasly;port=3306;password=root";



        // Adatbázis kapcsolat paraméteradása //
        public MySqlConnection AdatbazisCsatlakozas() // Adatbázis csatlakozás 
        {
            MySqlConnection db_csatlakozas;
            try
            {
                db_csatlakozas = new MySqlConnection(xampp_conn_params);               
                db_csatlakozas.Open();
                
            }
            catch
            {
                db_csatlakozas = new MySqlConnection(mampp_conn_params);
                db_csatlakozas.Open();
            }
            return db_csatlakozas;
        }



        public List<MesterKulcs> MesterkulcsTablazatLekeres() // Mesterkulcs tábla lekérdezés
        {

            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();

            try
            {
                // Tábla SELECT 
                string sql_mesterkulcs_select = "SELECT mester_id, encrypted_kulcs FROM mesterkulcs";
                MySqlCommand sql_command_mesterkulcs = new MySqlCommand(sql_mesterkulcs_select, db_csatlakozas);
                MySqlDataReader sql_reader = sql_command_mesterkulcs.ExecuteReader();

                // Sql mesterkulcs beolvasás
                while (sql_reader.Read())
                {
                    // Sql Mesterkulcs táblázat betöltése egy konstruktorba
                    MesterKulcs temp_mk = new MesterKulcs()
                    {
                        MesterId = int.Parse(sql_reader["mester_id"].ToString()),
                        EncryptedKulcs = sql_reader["encrypted_kulcs"].ToString()
                    };
                    mester_kulcs.Add(temp_mk);
                }
                sql_reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (db_csatlakozas.State == System.Data.ConnectionState.Open)
                {
                    db_csatlakozas.Close();
                }
            }
            return mester_kulcs;
        }



        public List<KliensLista> KliensListaLekeres(string mester_id, string mester_kulcs) // Kliens lista lekérdezés
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();

            // vissza titkositas
            TitkositasMetodusok szuper_titkos = new TitkositasMetodusok();

            try
            {
                // Adatok SELECT 
                string sql_kliensek_select = $"SELECT j.jelszo_id, j.jelszo_string, f.nev, f.email, f.url, f.hozzafuzes FROM Jelszo j JOIN Felhasznalo f ON j.jelszo_id = f.jelszo_id WHERE j.mester_id = '{mester_id}'";
                MySqlCommand sql_command_kliensek = new MySqlCommand(sql_kliensek_select, db_csatlakozas);
                MySqlDataReader sql_reader = sql_command_kliensek.ExecuteReader();

                while (sql_reader.Read())
                {
                    KliensLista temp_k = new KliensLista()
                    {
                        JelszoId = int.Parse(sql_reader["jelszo_id"].ToString()),
                        JelszoString = szuper_titkos.DecryptText(mester_kulcs, sql_reader["jelszo_string"].ToString()),
                        Nev = szuper_titkos.DecryptText(mester_kulcs, sql_reader["nev"].ToString()),
                        EMail = szuper_titkos.DecryptText(mester_kulcs, sql_reader["email"].ToString()),
                        Url = szuper_titkos.DecryptText(mester_kulcs, sql_reader["url"].ToString()),
                        Hozzafuzes = szuper_titkos.DecryptText(mester_kulcs, sql_reader["hozzafuzes"].ToString()),
                    };
                    kliens_lista.Add(temp_k);
                }
                sql_reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (db_csatlakozas.State == System.Data.ConnectionState.Open)
                {
                    db_csatlakozas.Close();
                }
            }
            return kliens_lista;
        }



        public void MesterkulcsTablazatIras(string encrypted_key) // Mesterkulcs tábla írás
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();
            try
            {
                // Mesterkulcs tábla INSERT
                string sql_kulcs_iras = $"INSERT INTO mesterkulcs (encrypted_kulcs) VALUES ('{encrypted_key}')";
                MySqlCommand sql_command_kulcs_iras = new MySqlCommand(sql_kulcs_iras, db_csatlakozas);
                sql_command_kulcs_iras.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (db_csatlakozas.State == System.Data.ConnectionState.Open)
                {
                    db_csatlakozas.Close();
                }
            }
        }



        public void JelszoTablazatIras(string jelszo_string, string erosseg, string mester_id) // Jelszó tábla írás
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = AdatbazisCsatlakozas();

            try
            {
                // Jelszo tábla INSERT
                string sql_jelszo_iras = $"INSERT INTO jelszo (jelszo_string, erosseg, titkositas, mester_id) VALUES ('{jelszo_string}', '{erosseg}', 'AES-256', {mester_id})";
                MySqlCommand sql_command_jelszo_iras = new MySqlCommand(sql_jelszo_iras, db_csatlakozas);
                sql_command_jelszo_iras.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (db_csatlakozas.State == System.Data.ConnectionState.Open)
                {
                    db_csatlakozas.Close();
                }
            }
           
        }



        public void FelhasznaloTablazatIras(string nev, string email, string url, string hozzafuzes, int jelszo_id) // Felhasználó tábla írás
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = AdatbazisCsatlakozas();

            try
            {
                // Felhasználo tábla INSERT
                string sql_felhasznalo_iras = $"INSERT INTO felhasznalo (nev, email, url, hozzafuzes, jelszo_id) VALUES ('{nev}', '{email}', '{url}', '{hozzafuzes}', {jelszo_id})";
                MySqlCommand sql_command_felhasznalo_iras = new MySqlCommand(sql_felhasznalo_iras, db_csatlakozas);
                sql_command_felhasznalo_iras.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (db_csatlakozas.State == System.Data.ConnectionState.Open)
                {
                    db_csatlakozas.Close();
                }
            }

        }



        public void LogTablazatIras(string leiras, string jelszo_id, string felhasznalo_id, string mester_id) // Hozzáférés log tábla írás
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();

            try
            {
                // Hozzáférés log tábla INSERT
                string sql_log_iras = $"INSERT INTO hozzafereslog ( leiras, jelszo_id, felhasznalo_id, mester_id) VALUES ('{leiras}', '{jelszo_id}', '{felhasznalo_id}', '{mester_id}')"; 
                MySqlCommand sql_command_log_iras = new MySqlCommand(sql_log_iras, db_csatlakozas);
                sql_command_log_iras.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (db_csatlakozas.State == System.Data.ConnectionState.Open)
                {
                    db_csatlakozas.Close();
                }
            }
        }



        public string MesterkulcsIDLekerdezes(string encrypted_key) // Mesterkulcs ID lekérdezés
        {
            string mesterkulcs_id = "";
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();
            try
            {
                // Tábla SELECT 
                string sql_mesterkulcs_id_select = $"SELECT mester_id FROM mesterkulcs WHERE encrypted_kulcs = '{encrypted_key}'";
                MySqlCommand sql_command_mesterkulcs_id = new MySqlCommand(sql_mesterkulcs_id_select, db_csatlakozas);
                MySqlDataReader sql_reader = sql_command_mesterkulcs_id.ExecuteReader();
                // Sql mesterkulcs beolvasás
                while (sql_reader.Read())
                {
                    mesterkulcs_id = sql_reader["mester_id"].ToString();
                }
                sql_reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (db_csatlakozas.State == System.Data.ConnectionState.Open)
                {
                    db_csatlakozas.Close();
                }
            }
            return mesterkulcs_id;
        }
 


        public int UtolsoBeszurtId(MySqlConnection db_csatlakozas) // Utolsó beszúrt ID lekérdezés
        {
            int lastInsertedId = 0;
            try
            {
                string sql_last_insert_id = "SELECT LAST_INSERT_ID()";
                MySqlCommand sql_command_last_insert_id = new MySqlCommand(sql_last_insert_id, db_csatlakozas);
                lastInsertedId = Convert.ToInt32(sql_command_last_insert_id.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lastInsertedId;
        }

 

        public void FelhasznaloSorTorles(int jelszo_id) // Felhasználó sor törlés
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();
            try
            {
                // Megerősítés
                MessageBoxResult result = MessageBox.Show("Biztosan törölni szeretné a kiválasztott felhasználót?", "Felhasználó törlése", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Felhasználó sor törlés parancsok
                    string sql_felhasznalo_torles = $"DELETE FROM felhasznalo WHERE jelszo_id = {jelszo_id}";
                    string sql_jelszo_torles = $"DELETE FROM jelszo WHERE jelszo_id = {jelszo_id}";
                    MySqlCommand sql_command_felhasznalo_torles = new MySqlCommand(sql_felhasznalo_torles, db_csatlakozas);
                    MySqlCommand sql_command_jelszo_torles = new MySqlCommand(sql_jelszo_torles, db_csatlakozas);


                    // Felhasználó sor törlés vegrehajtás
                    sql_command_felhasznalo_torles.ExecuteNonQuery();
                    sql_command_jelszo_torles.ExecuteNonQuery();

                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (db_csatlakozas.State == System.Data.ConnectionState.Open)
                {
                    db_csatlakozas.Close();
                }
            }
        }
    }
}