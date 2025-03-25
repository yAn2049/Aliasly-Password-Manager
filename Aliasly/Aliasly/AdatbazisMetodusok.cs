﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Aliasly
{
    public class AdatbazisMetodusok
    {
        // Osztályok 
        private List<MesterKulcs> mester_kulcs = new List<MesterKulcs>();
        private List<Jelszo> jelszavak = new List<Jelszo>();
        private List<Felhasznalo> felhasznalok = new List<Felhasznalo>();
        private List<HozzaferesLog> hozzaferes_log = new List<HozzaferesLog>();
        private List<KliensLista> kliens_lista = new List<KliensLista>();

        // Sql csatlakozási paraméterek, xampp és mampp
        private string xampp_conn_params = "server=localhost;user=root;database=aliasly;port=3306";
        private string mampp_conn_params = "server=localhost;user=root;database=aliasly;port=3306;password=root";
        


        public MySqlConnection AdatbazisCsatlakozas() // Adatbázis kapcsolat létesítése
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



        public List<MesterKulcs> MesterkulcsTablazatLekeres()
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();

            try
            {
                // Tábla SELECT 
                string sql_mesterkulcs_select = "SELECT mester_id, salt_string, hashed_kulcs FROM mesterkulcs";
                MySqlCommand sql_command_mesterkulcs = new MySqlCommand(sql_mesterkulcs_select, db_csatlakozas);
                MySqlDataReader sql_reader = sql_command_mesterkulcs.ExecuteReader();

                // Sql mesterkulcs beolvasás
                while (sql_reader.Read())
                {
                    // Sql Mesterkulcs táblázat betöltése egy konstruktorba
                    MesterKulcs temp_mk = new MesterKulcs()
                    {
                        MesterId = int.Parse(sql_reader["mester_id"].ToString()),
                        SaltString = sql_reader["salt_string"].ToString(),
                        HashedKulcs = sql_reader["hashed_kulcs"].ToString()
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



        public List<Jelszo> JelszoTablazatLekeres()
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();

            try
            {
                // Tábla SELECT 
                string sql_jelszo_select = "SELECT jelszo_id, jelszo_string, erosseg, titkositas, mester_id FROM jelszo";
                MySqlCommand sql_command_jelszo = new MySqlCommand(sql_jelszo_select, db_csatlakozas);
                MySqlDataReader sql_reader = sql_command_jelszo.ExecuteReader();

                // Sql mesterkulcs beolvasás
                while (sql_reader.Read())
                {
                    // Sql Mesterkulcs táblázat betöltése egy konstruktorba
                    Jelszo temp_j = new Jelszo()
                    {
                        JelszoId = int.Parse(sql_reader["jelszo_id"].ToString()),
                        JelszoString = sql_reader["jelszo_string"].ToString(),
                        Erosseg = sql_reader["erosseg"].ToString(),
                        Titkositas = sql_reader["titkositas"].ToString(),
                        MesterId = int.Parse(sql_reader["mester_id"].ToString())
                    };
                    jelszavak.Add(temp_j);
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

            return jelszavak;
        }



        public List<Felhasznalo> FelhasznaloTablazatLekeres()
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();

            try
            {
                // Tábla SELECT 
                string sql_felhasznalo_select = "SELECT felhasznalo_id, nev, email, url, hozzafuzes, jelszo_id FROM felhasznalo";
                MySqlCommand sql_command_felhasznalok = new MySqlCommand(sql_felhasznalo_select, db_csatlakozas);
                MySqlDataReader sql_reader = sql_command_felhasznalok.ExecuteReader();

                // Sql felhasználók beolvasás
                while (sql_reader.Read())
                {
                    // Sql felhasználó táblázat betöltése egy konstruktorba
                    Felhasznalo temp_fh = new Felhasznalo()
                    {
                        FelhasznaloId = int.Parse(sql_reader["felhasznalo_id"].ToString()),
                        Nev = sql_reader["nev"].ToString(),
                        EMail = sql_reader["email"].ToString(),
                        Url = sql_reader["url"].ToString(),
                        Hozzafuzes = sql_reader["hozzafuzes"].ToString(),
                        JelszoId = int.Parse(sql_reader["jelszo_id"].ToString())
                    };
                    felhasznalok.Add(temp_fh);
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

            return felhasznalok;
        }



        public List<HozzaferesLog> HozzaferesLogTablazatLekeres()
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();

            try
            {
                // Tábla SELECT 
                string sql_hozzaferes_log_select = "SELECT log_id, datum_ido, leiras, jelszo_id, jelszo_id, felhasznalo_id FROM hozzafereslog";
                MySqlCommand sql_command_log = new MySqlCommand(sql_hozzaferes_log_select, db_csatlakozas);
                MySqlDataReader sql_reader = sql_command_log.ExecuteReader();

                // Sql felhasználók beolvasás
                while (sql_reader.Read())
                {
                    // Sql felhasználó táblázat betöltése egy konstruktorba
                    HozzaferesLog temp_l = new HozzaferesLog()
                    {
                        LogId = int.Parse(sql_reader["log_id"].ToString()),
                        DatumIdo = DateTime.Parse(sql_reader["datum_ido"].ToString()),
                        Leiras = sql_reader["leiras"].ToString(),
                        JelszoId = int.Parse(sql_reader["jelszo_id"].ToString()),
                        FelhasznaloId = int.Parse(sql_reader["felhasznalo_id"].ToString())
                    };
                    hozzaferes_log.Add(temp_l);
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

            return hozzaferes_log;
        }



        public List<KliensLista> KliensListaLekeres()
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();

            try
            {
                // Adatok SELECT 
                string sql_kliensek_select = $"SELECT j.jelszo_id, j.jelszo_string, f.nev, f.email, f.url, f.hozzafuzes FROM Jelszo j JOIN Felhasznalo f ON j.jelszo_id = f.jelszo_id";
                MySqlCommand sql_command_kliensek = new MySqlCommand(sql_kliensek_select, db_csatlakozas);
                MySqlDataReader sql_reader = sql_command_kliensek.ExecuteReader();

                while (sql_reader.Read())
                {
                    KliensLista temp_k = new KliensLista()
                    {
                        JelszoId = int.Parse(sql_reader["jelszo_id"].ToString()),
                        JelszoString = sql_reader["jelszo_string"].ToString(),
                        Nev = sql_reader["nev"].ToString(),
                        EMail = sql_reader["email"].ToString(),
                        Url = sql_reader["url"].ToString(),
                        Hozzafuzes = sql_reader["hozzafuzes"].ToString(),
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



        public void MesterkulcsTablazatIras(string salt_string, string hashed_kulcs)
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();
            try
            {
                // Tábla INSERT
                string sql_kulcs_iras = $"INSERT INTO mesterkulcs (salt_string, hashed_kulcs) VALUES ('{salt_string}', '{hashed_kulcs}')";
                MySqlCommand sql_command_kulcs_iras = new MySqlCommand(sql_kulcs_iras, db_csatlakozas);
                //db_csatlakozas.Open();
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



        public void JelszoTablazatIras(string jelszo_string, string erosseg, int mester_id)
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();

            try
            {
                // Tábla INSERT
                string sql_jelszo_iras = $"INSERT INTO jelszo (jelszo_string, erosseg, titkositas, mester_id) VALUES ('{jelszo_string}', '{erosseg}', 'AES-256', '{mester_id}')";
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



        public void FelhasznaloTablazatIras(string nev, string email, string url, string hozzafuzes)
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();

            try
            {
                // Tábla INSERT
                string sql_felhasznalo_iras = $"INSERT INTO felhasznalo (nev, email, url, hozzafuzes, jelszo_id) VALUES ('{nev}', '{email}', '{url}', '{hozzafuzes}', LAST_INSERT_ID())";
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



        public void LogTablazatIras(DateTime datum_ido, string leiras, int jelszo_id, int felhasznalo_id)
        {
            // Adatbázis kapcsolat
            MySqlConnection db_csatlakozas = new AdatbazisMetodusok().AdatbazisCsatlakozas();

            try
            {
                // Tábla INSERT
                string sql_log_iras = $"INSERT INTO hozzafereslog (datum_ido, leiras, jelszo_id, felhasznalo_id) VALUES ('{datum_ido}', '{leiras}', '{jelszo_id}', '{felhasznalo_id}')";
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
    }
}