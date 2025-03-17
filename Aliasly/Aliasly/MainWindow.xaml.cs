﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Aliasly;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static List<Adatok> adatok = new List<Adatok>();
    static List<MesterKulcs> mesterkulcs = new List<MesterKulcs>();
    static string connParam = "server=localhost;user=root;database=aliasly;port=3306"; // Sql csatlakozási paraméterek //

    
    public MainWindow()
    {
        InitializeComponent();
        StartUp();

        

    }



    public void StartUp() // Indítási paraméterek //
    {
        mesterkulcs_felulet.Visibility = Visibility.Visible;
        kliens_felulet.Visibility = Visibility.Collapsed;
    }



    public void ShowClient() // Kliens felület megjelenítő //
    {
        kliens_felulet.Visibility = Visibility.Visible;
        mesterkulcs_felulet.Visibility = Visibility.Collapsed;
    }



    public List<Adatok> AdatokTablazatLekeres()
    {
        // Sql kapcsolat //
        MySqlConnection connection = new MySqlConnection(connParam);

        // Sql kapcsolat nyitás //
        connection.Open();

        // Sql Adatok táblázat lekérése //
        string sqlAdatok = "SELECT id, jelszo, email, nev, url, hozzafuzes, mester_id FROM adatok";
        MySqlCommand sqlCommand_adatok = new MySqlCommand(sqlAdatok, connection);
        MySqlDataReader sqlReader = sqlCommand_adatok.ExecuteReader();

        // Sql adatok beolvasás //
        while (sqlReader.Read())
        {
            // Sql Adatok táblázat betöltése egy konstruktorba //
            Adatok a = new Adatok()
            {
                Id = int.Parse(sqlReader["id"].ToString()),
                Jelszo = sqlReader["jelszo"].ToString(),
                EMail = sqlReader["email"].ToString(),
                Nev = sqlReader["nev"].ToString(),
                Url = sqlReader["url"].ToString(),
                Hozzafuzes = sqlReader["hozzafuzes"].ToString(),
                MesterId = int.Parse(sqlReader["mester_id"].ToString())
            };
            adatok.Add(a);
        }
        sqlReader.Close();

        return adatok;
    }



    public List<MesterKulcs> MesterkulcsTablazatLekeres()
    {
        // Sql kapcsolat //
        MySqlConnection connection = new MySqlConnection(connParam);

        // Sql kapcsolat nyitás //
        connection.Open();

        // Sql Adatok táblázat lekérése //
        string sqlMesterkulcs = "SELECT mester_id, kulcs_string, salt_string, hashed_kulcs FROM mesterkulcs";
        MySqlCommand sqlCommand_mesterkulcs = new MySqlCommand(sqlMesterkulcs, connection);
        MySqlDataReader sqlReader = sqlCommand_mesterkulcs.ExecuteReader();

        // Sql mesterkulcs beolvasás //
        while (sqlReader.Read())
        {
            // Sql Mesterkulcs táblázat betöltése egy konstruktorba //
            MesterKulcs m = new MesterKulcs()
            {
                MesterId = int.Parse(sqlReader["mester_id"].ToString()),
                KulcsString = sqlReader["kulcs_string"].ToString(),
                SaltString = sqlReader["salt_string"].ToString(),
                HashedKulcs = sqlReader["hashed_kulcs"].ToString()
            };
            mesterkulcs.Add(m);
        }
        sqlReader.Close();

        return mesterkulcs;
    }



    private void enter_gomb_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            /*
             * Mesterkulcs belépés ide
             */

            try
            {
                // Adatok táblázat metódus //
                AdatokTablazatLekeres();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Ha helyes a kulcs akkor megjeleníti a kliens felületet //
            ShowClient();

            // Sql soronkénti betöltése egy listbox elembe //
            foreach (var a in adatok)
            {
                this.adat_lista.ItemsSource = adatok;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ez a mesterkulcs nem található az adatbázisban.", "Mesterkulcs hiba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }



    private void uj_kulcs_Click(object sender, RoutedEventArgs e)
    {
        // Sql kapcsolat //
        MySqlConnection connection = new MySqlConnection(connParam);

        try
        {
            // Mesterkulcs táblázat metódus //
            MesterkulcsTablazatLekeres();

            // Végigfut a mesterkulcs listán és lecsekkolja hogy létezik-e már az írni kívánt adat //
            bool vanMarIlyenKulcs = false;
            foreach (var m in mesterkulcs)
            {
                if (masterkulcs_mezo.Password == m.KulcsString)
                {
                    vanMarIlyenKulcs = true;
                    break;
                }
                else
                {
                    vanMarIlyenKulcs = false;
                }
            }

            // Ha létezik már ilyen adat, hibát dob fel. //
            if (vanMarIlyenKulcs)
            {
                MessageBox.Show("Ez a kulcs már létezik az adatbázisodban!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            // Ha nem létezik ilyen adat, akkor feltölti az adatbázis táblázatába //
            else
            {
                string sqlKulcsIras = $"INSERT INTO mesterkulcs ( kulcs_string, salt_string, hashed_kulcs) VALUES ('{masterkulcs_mezo.Password}', '-', '-')";
                MySqlCommand sqlCommand_kulcsIras = new MySqlCommand(sqlKulcsIras, connection);
                sqlCommand_kulcsIras.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }



    private void adat_rogzites_gomb_Click(object sender, RoutedEventArgs e)
    {
        string sqlAdatok_iras = $"INSERT INTO adatok(jelszo, email, nev, url, hozzafuzes, mester_id) VALUES ()";

    }



    private void jelszo_mezo_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Ha a jelszó mező üres akkor a rögzítés gombot kikapcsolja //
        if (jelszo_mezo.Text != string.Empty)
        {
            adat_rogzites_gomb.IsEnabled = true;
        }
        else
        {
            adat_rogzites_gomb.IsEnabled = false;
        }
    }



}