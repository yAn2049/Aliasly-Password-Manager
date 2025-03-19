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
    static List<Felhasznalo> felhasznalok = new List<Felhasznalo>();
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



    public List<Felhasznalo> FelhasznaloTablazatLekeres()
    {
        // Sql kapcsolat //
        MySqlConnection connection = new MySqlConnection(connParam);

        try
        {
            // Sql kapcsolat nyitás //
            connection.Open();

            // Sql felhasználó táblázat lekérése //
            string sqlFelhasznalo = "SELECT id, jelszo, email, nev, url, hozzafuzes, mester_id FROM adatok";
            MySqlCommand sqlCommand_felhasznalok = new MySqlCommand(sqlFelhasznalo, connection);
            MySqlDataReader sqlReader = sqlCommand_felhasznalok.ExecuteReader();

            // Sql felhasználók beolvasás //
            while (sqlReader.Read())
            {
                // Sql felhasználó táblázat betöltése egy konstruktorba //
                Felhasznalo a = new Felhasznalo()
                {
                    Id = int.Parse(sqlReader["id"].ToString()),
                    Jelszo = sqlReader["jelszo"].ToString(),
                    EMail = sqlReader["email"].ToString(),
                    Nev = sqlReader["nev"].ToString(),
                    Url = sqlReader["url"].ToString(),
                    Hozzafuzes = sqlReader["hozzafuzes"].ToString(),
                    MesterId = int.Parse(sqlReader["mester_id"].ToString())
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
            FelhasznaloTablazatLekeres();

            // Sql soronkénti betöltése a listbox elembe //
            foreach (var a in felhasznalok)
            {
                this.felhasznalok_lista.ItemsSource = felhasznalok;
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
                string sqlKulcsIras = $"INSERT INTO mesterkulcs ( kulcs_string, salt_string, hashed_kulcs) VALUES ('{mesterkulcs_mezo.Password}', '-', '-')";
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
        string sqlFelhasznalo_iras = $"INSERT INTO adatok(jelszo, email, nev, url, hozzafuzes, mester_id) VALUES ()";

    }



    private void jelszo_mezo_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Ha a jelszó mező üres akkor a rögzítés gombot kikapcsolja //
        if (jelszo_mezo.Text != string.Empty)
        {
            felhasznalo_rogzites_gomb.IsEnabled = true;
        }
        else
        {
            felhasznalo_rogzites_gomb.IsEnabled = false;
        }
    }



}