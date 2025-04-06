using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

/*
 * változók: snake_case
 * osztályok és tartozékai: PascalCase
 * metódusok: PascalCase
 */


public partial class MainWindow : Window
{
    // Aktív kulcs
    public string AktivKulcs { get; set; }
    public string AktivKulcsId { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        StartUp();
    }



    public void StartUp() // Indítási paraméterek //
    {
        mesterkulcs_felulet.Visibility = Visibility.Visible;
        kliens_felulet.Visibility = Visibility.Collapsed;
        uj_kulcs_felulet.Visibility = Visibility.Collapsed;

        mesterkulcs_mezo.Password = string.Empty;

        // inditaskor nullazza az ertekeket, nem tudom pontosan, hogy szukseg van-e ra,  megduma majd, egyenlore mukodik //
        AktivKulcs = string.Empty;
        AktivKulcsId = string.Empty;
    }



    public void ShowClient() // Kliens felület megjelenítő //
    {
        kliens_felulet.Visibility = Visibility.Visible;
        mesterkulcs_felulet.Visibility = Visibility.Collapsed;
        uj_kulcs_felulet.Visibility = Visibility.Collapsed;
    }



    private void uj_kulcs_link_Click(object sender, RoutedEventArgs e)
    {
        kliens_felulet.Visibility = Visibility.Collapsed;
        mesterkulcs_felulet.Visibility = Visibility.Collapsed;
        uj_kulcs_felulet.Visibility = Visibility.Visible;
    }



    private void uj_kulcs_vissza_Click(object sender, RoutedEventArgs e)
    {
        StartUp();
    }



    private void enter_gomb_Click(object sender, RoutedEventArgs e) // Bejelentkezes gomb esemeny - Login //

    {
        // Adatbazis osztaly peldany //
        AdatbazisMetodusok adatbazis = new AdatbazisMetodusok();

        // Mesterkulcs táblázat metódus //
        List<MesterKulcs> mester_kulcs = adatbazis.MesterkulcsTablazatLekeres();

        // Titkosítás metódus //
        TitkositasMetodusok szuper_titkos = new TitkositasMetodusok();

        // Mesterkulcs titkosítása //
        string titkos_kulcs = szuper_titkos.EncryptText(mesterkulcs_mezo.Password, mesterkulcs_mezo.Password);

        // Végigfut a mesterkulcs listán és lecsekkolja hogy létezik-e már az írni kívánt adat //
        bool van_mar_ilyen_kulcs = false;
        try
        {
            foreach (var m in mester_kulcs)
            {
                if (titkos_kulcs == m.EncryptedKulcs)
                {
                    van_mar_ilyen_kulcs = true;
                    break;
                }
                else
                {
                    van_mar_ilyen_kulcs = false;
                }
            }

            if (van_mar_ilyen_kulcs)
            {
                // Aktuális kulcs //

                AktivKulcs = titkos_kulcs; // Titkositott mesterKulcsot jegyzi meg //
                AktivKulcsId = adatbazis.MesterkulcsIDLekerdezes(AktivKulcs); // Mesterkulcs id-t jegyzi meg //

                // logolas //
                adatbazis.LogTablazatIras("Bejelentkezes", string.Empty, string.Empty, AktivKulcsId);

                // kliens felulet megjelenitese //
                ShowClient();
               
                // Felhasználó táblázat metódus //
                List<KliensLista> kliens_lista = adatbazis.KliensListaLekeres(AktivKulcsId, AktivKulcs);

                // Sql soronkénti betöltése a listbox elembe //
                foreach (var k in kliens_lista)
                {
                    this.felhasznalok_lista.ItemsSource = kliens_lista;
                }

                
            }
            else
            {
                MessageBox.Show("Ez a mesterkulcs nem található az adatbázisban.", "Mesterkulcs hiba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    } 


    private void uj_kulcs_Click(object sender, RoutedEventArgs e) // Új mesterkulcs hozzáadása gomb esemény - Login //
    {

        // Mesterkulcs táblázat metódus //
        AdatbazisMetodusok adatbazis = new AdatbazisMetodusok();

        // Mesterkulcs lista //
        List<MesterKulcs> mester_lista = adatbazis.MesterkulcsTablazatLekeres();

        // Titkosítás metódus //
        TitkositasMetodusok szuper_titkos = new TitkositasMetodusok();

        // Eredeti mesterkulcs //
        string mesterkulcs = mesterkulcs_mezo.Password;

        // Mesterkulcs titkosítása //
        string titkos_kulcs = szuper_titkos.EncryptText(mesterkulcs, mesterkulcs);

        // Végigfut a mesterkulcs listán és lecsekkolja hogy létezik-e már az írni kívánt adat //
        bool van_mar_ilyen_kulcs = false;
        try
        {
            foreach (var a in mester_lista)
            {
                if (a.EncryptedKulcs == titkos_kulcs)
                {
                    van_mar_ilyen_kulcs = true;
                    break;
                }
                else
                {
                    van_mar_ilyen_kulcs = false;
                }
            }

            // Ha a mesterkulcs már létezik akkor hibaüzenet //
            if (van_mar_ilyen_kulcs)
            {
                MessageBox.Show("Ez a mesterkulcs már létezik az adatbázisban.", "Mesterkulcs hiba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                // Ha a mesterkulcs nem létezik akkor hozzáadja az adatbázishoz //
                adatbazis.MesterkulcsTablazatIras(titkos_kulcs);

                // logolás //
                adatbazis.LogTablazatIras("Uj kulcs hozza adva!", string.Empty, string.Empty, adatbazis.MesterkulcsIDLekerdezes(titkos_kulcs));


                // felhasznalo visszajelzes //
                MessageBox.Show("Sikeresen hozzáadva a mesterkulcs az adatbázishoz.", "Mesterkulcs hozzáadva!", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    } 



    private void felhasznalo_rogzites_gomb_Click(object sender, RoutedEventArgs e) // Felhasználó rögzítés gomb esemény - Main //
    {

        string erosseg = "placeholder";

        AdatbazisMetodusok metodus = new AdatbazisMetodusok();
        TitkositasMetodusok szuper_titkos = new TitkositasMetodusok();

        try
        {
            // titkositas //
            string titkos_jelszo = szuper_titkos.EncryptText(AktivKulcs, jelszo_mezo.Password.ToString());

            // uj jelszo hozzaadasa //
            // AktivKulcsId = mesterkulcs_id ami hasznalatban van //
            metodus.JelszoTablazatIras(titkos_jelszo, erosseg, AktivKulcsId);

            // Az utolso beszurt jelszo_id lekerese //
            int utolso_id;
            using (MySqlConnection db_csatlakozas = metodus.AdatbazisCsatlakozas())
            {
                utolso_id = metodus.UtolsoBeszurtId(db_csatlakozas);
            }

            string id = utolso_id.ToString();
            // logolas //

            metodus.LogTablazatIras("Uj jelszo hozza adva!", id, id, AktivKulcsId);

            // titkositas //
            string t_nev = szuper_titkos.EncryptText(AktivKulcs, nev_mezo.Text);
            string t_email = szuper_titkos.EncryptText(AktivKulcs, eMail_mezo.Text);
            string t_url = szuper_titkos.EncryptText(AktivKulcs, url_mezo.Text);
            string t_hozzafuzes = szuper_titkos.EncryptText(AktivKulcs, hozzafuzes_mezo.Text);

            // Beszur egy uj felhasznalo rekordot a lekerdezett jelszo_id-vel //
            metodus.FelhasznaloTablazatIras(t_nev, t_email, t_url, t_hozzafuzes, utolso_id);

            // Felhasznalok lista uritese //
            felhasznalok_lista.ItemsSource = null;

            // Felhasznalok lista frissitese //
            List<KliensLista> kliens_lista = new AdatbazisMetodusok().KliensListaLekeres(AktivKulcsId, AktivKulcs);
            foreach (var k in kliens_lista)
            {
                felhasznalok_lista.ItemsSource = kliens_lista;
            }

            // Mezők ürítése //
            nev_mezo.Text = null;
            eMail_mezo.Text = null;
            url_mezo.Text = null;
            hozzafuzes_mezo.Text = null;
            jelszo_mezo.Password = null;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    } 



    private void jelszo_mezo_PasswordChanged(object sender, RoutedEventArgs e) // Jelszó mező változás esemény - Main//
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



    private void kijelentkezes_gomb_Click(object sender, RoutedEventArgs e) // Kijelentkezés gomb esemény - Main //
    {
        AdatbazisMetodusok metodus = new AdatbazisMetodusok();

        // Mezők ürítése //
        mesterkulcs_mezo.Password = null;

        // logolas //
        metodus.LogTablazatIras("Kijelentkezes", string.Empty, string.Empty, AktivKulcsId);

        // Aktív kulcs és id nullázása //
        AktivKulcs = string.Empty;
        AktivKulcsId = string.Empty;

        // Felhasznalok lista uritese //
        felhasznalok_lista.ItemsSource = null;

        // Login felulet mutatasa //
        kliens_felulet.Visibility = Visibility.Collapsed;
        mesterkulcs_felulet.Visibility = Visibility.Visible;
    }



    private void sor_torles_gomb_Click(object sender, RoutedEventArgs e) // Sor törlés gomb esemény - Main //
    {
        // Megkapja a megnyomott gombot //
        Button button = sender as Button;

        if (button != null)
        {
            // Megkapja a gomb erteket //
            int sor_id = (int)button.CommandParameter;

            string sor_id_string = sor_id.ToString();

            // Adatbazis osztaly peldany //
            AdatbazisMetodusok metodus = new AdatbazisMetodusok();

            try
            {
                // Törli a kiválasztott sort az adatbázisból //
                metodus.FelhasznaloSorTorles(sor_id);

                // logolas //
                metodus.LogTablazatIras("Sor torolve!", sor_id_string, sor_id_string, AktivKulcsId);

                // Kliens lista frissítése //
                felhasznalok_lista.ItemsSource = null;
                List<KliensLista> kliens_lista = metodus.KliensListaLekeres(AktivKulcsId, AktivKulcs);
                foreach (var k in kliens_lista)
                {
                    felhasznalok_lista.ItemsSource = kliens_lista;
                }

                // felhasznalo visszajelzes //
                MessageBox.Show($"A sor {sor_id} torolve lett az adatbazisbol!", "Rekord torolve", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}