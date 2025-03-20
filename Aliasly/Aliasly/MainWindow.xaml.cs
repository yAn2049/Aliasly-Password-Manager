using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    static List<MesterKulcs> mesterkulcs = new List<MesterKulcs>();
    static List<Jelszo> jelszavak = new List<Jelszo>();
    static List<Felhasznalo> felhasznalok = new List<Felhasznalo>();

    static List<KliensLista> klienslista = new List<KliensLista>();
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



    public List<MesterKulcs> MesterkulcsTablazatLekeres()
    {
        // Sql kapcsolat //
        MySqlConnection connection = new MySqlConnection(connParam);

        try
        {
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
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return mesterkulcs;
    }



    public List<Jelszo> JelszoTablazatLekeres() 
    {
        // Sql kapcsolat //
        MySqlConnection connection = new MySqlConnection(connParam);

        try
        {
            // Sql kapcsolat nyitás //
            connection.Open();

            // Sql Adatok táblázat lekérése //
            string sqlJelszo = "SELECT jelszo_id, jelszo_string, erosseg, titkositas, mester_id FROM jelszo";
            MySqlCommand sqlCommand_jelszo = new MySqlCommand(sqlJelszo, connection);
            MySqlDataReader sqlReader = sqlCommand_jelszo.ExecuteReader();

            // Sql mesterkulcs beolvasás //
            while (sqlReader.Read())
            {
                // Sql Mesterkulcs táblázat betöltése egy konstruktorba //
                Jelszo j = new Jelszo()
                {
                    JelszoId = int.Parse(sqlReader["jelszo_id"].ToString()),
                    JelszoString = sqlReader["jelszo_string"].ToString(),
                    Erosseg = sqlReader["erosseg"].ToString(),
                    Titkositas = sqlReader["titkositas"].ToString(),
                    MesterId = int.Parse(sqlReader["mester_id"].ToString())
                };
                jelszavak.Add(j);
            }
            sqlReader.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return jelszavak;
    }



    public List<Felhasznalo> FelhasznaloTablazatLekeres()
    {
        // Sql kapcsolat //
        MySqlConnection connection = new MySqlConnection(connParam);

        try
        {
            // Sql kapcsolat nyitás //
            connection.Open();

            // Sql felhasználó táblázat lekérése //
            string sqlFelhasznalo = "SELECT felhasznalo_id, nev, email, url, hozzafuzes, jelszo_id FROM felhasznalo";
            MySqlCommand sqlCommand_felhasznalok = new MySqlCommand(sqlFelhasznalo, connection);
            MySqlDataReader sqlReader = sqlCommand_felhasznalok.ExecuteReader();

            // Sql felhasználók beolvasás //
            while (sqlReader.Read())
            {
                // Sql felhasználó táblázat betöltése egy konstruktorba //
                Felhasznalo a = new Felhasznalo()
                {
                    FelhasznaloId = int.Parse(sqlReader["felhasznalo_id"].ToString()),
                    Nev = sqlReader["nev"].ToString(),
                    EMail = sqlReader["email"].ToString(),
                    Url = sqlReader["url"].ToString(),
                    Hozzafuzes = sqlReader["hozzafuzes"].ToString(),
                    JelszoId = int.Parse(sqlReader["jelszo_id"].ToString())
                };
                felhasznalok.Add(a);
            }
            sqlReader.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return felhasznalok;
    }



    
    // public List<Log> LogTablazatLekeres() {}
    


    private void enter_gomb_Click(object sender, RoutedEventArgs e)
    {
        // Mesterkulcs táblázat metódus //
        MesterkulcsTablazatLekeres();

        // Végigfut a mesterkulcs listán és lecsekkolja hogy létezik-e már az írni kívánt adat //
        bool vanMarIlyenKulcs = false;
        foreach (var m in mesterkulcs)
        {
            if (mesterkulcs_mezo.Password == m.KulcsString)
            {
                vanMarIlyenKulcs = true;
                break;
            }
            else
            {
                vanMarIlyenKulcs = false;
            }
        }

        if (vanMarIlyenKulcs)
        {
            // Ha helyes a kulcs akkor megjeleníti a kliens felületet //
            ShowClient();

            // Felhasználó táblázat metódus //
            JelszoTablazatLekeres();

            // Sql soronkénti betöltése a listbox elembe //
            foreach (var a in jelszavak)
            {
                this.felhasznalok_lista.ItemsSource = jelszavak;
            }
        }
        else
        {
            MessageBox.Show("Ez a mesterkulcs nem található az adatbázisban.", "Mesterkulcs hiba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }



    private void uj_kulcs_Click(object sender, RoutedEventArgs e)
    {
        // Sql kapcsolat //
        MySqlConnection connection = new MySqlConnection(connParam);

        connection.Open();

        try
        {
            // Mesterkulcs táblázat metódus //
            MesterkulcsTablazatLekeres();

            // Végigfut a mesterkulcs listán és lecsekkolja hogy létezik-e már az írni kívánt adat //
            bool vanMarIlyenKulcs = false;
            foreach (var m in mesterkulcs)
            {
                if (mesterkulcs_mezo.Password == m.KulcsString)
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
                string sqlKulcsIras = $"INSERT INTO mesterkulcs (kulcs_string, salt_string, hashed_kulcs) VALUES ('{mesterkulcs_mezo.Password}', '-', '-')";
                MySqlCommand sqlCommand_kulcsIras = new MySqlCommand(sqlKulcsIras, connection);
                sqlCommand_kulcsIras.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }



    private void felhasznalo_rogzites_gomb_Click(object sender, RoutedEventArgs e)
    {
        /*
         * 
         * Ezt kurvara meg kell fixalni
         * kiegtem :skull:
         * 
         */

        // Sql kapcsolat //
        MySqlConnection connection = new MySqlConnection(connParam);
        connection.Open();

        mesterkulcs = new List<MesterKulcs>(MesterkulcsTablazatLekeres());
        jelszavak = new List<Jelszo>(JelszoTablazatLekeres());

        string sqlJelszo_iras = $"INSERT INTO jelszo (jelszo, erosseg, titkositas, mester_id) VALUES ('{jelszo_mezo.Password}', '-', '-', '{mesterkulcs[0].MesterId}')";

        string sqlFelhasznalo_iras = $"INSERT INTO felhasznalo (nev, email, url, hozzafuzes, jelszo_id) VALUES  ('{nev_mezo.Text}', '{eMail_mezo.Text}', '{url_mezo.Text}', '{hozzafuzes_mezo.Text}', '{jelszavak[0].JelszoId}')";

        MySqlCommand sqlCommand_jelszoIras = new MySqlCommand(sqlJelszo_iras, connection);
        MySqlCommand sqlCommand_felhasznaloIras = new MySqlCommand(sqlFelhasznalo_iras, connection);

        sqlCommand_jelszoIras.ExecuteNonQuery();
        sqlCommand_felhasznaloIras.ExecuteNonQuery();
    }



    private void jelszo_mezo_PasswordChanged(object sender, RoutedEventArgs e)
    {
        // Ha a jelszó mező üres akkor a rögzítés gombot kikapcsolja //
        if (jelszo_mezo.Password != string.Empty)
        {
            felhasznalo_rogzites_gomb.IsEnabled = true;
        }
        else
        {
            felhasznalo_rogzites_gomb.IsEnabled = false;
        }
    }
}