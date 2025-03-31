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

        // inditaskor nullazza az ertekeket, nem tudom pontosan, hogy szukseg van-e ra,  megduma majd, egyenlore mukodik //
        AktivKulcs = string.Empty;
        AktivKulcsId = string.Empty;
    }



    public void ShowClient() // Kliens felület megjelenítő //
    {
        kliens_felulet.Visibility = Visibility.Visible;
        mesterkulcs_felulet.Visibility = Visibility.Collapsed;
    }



    private void enter_gomb_Click(object sender, RoutedEventArgs e)
    {
        // Mesterkulcs táblázat metódus //
        List<MesterKulcs> mester_kulcs = new AdatbazisMetodusok().MesterkulcsTablazatLekeres();

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
                AktivKulcsId = new AdatbazisMetodusok().MesterkulcsIDLekerdezes(AktivKulcs); // Mesterkulcs id-t jegyzi meg //

                MessageBox.Show($"Mesterkulcs: \t{AktivKulcs} \nId: \t\t{AktivKulcsId}", "Bejelentkezés sikeres!", MessageBoxButton.OK, MessageBoxImage.Information);
                // Ha helyes a kulcs akkor megjeleníti a kliens felületet //
                aktiv_kulcs.Content = $"[Key]:\t{mesterkulcs_mezo.Password}";
                ShowClient();
               
                // Felhasználó táblázat metódus //
                List<KliensLista> kliens_lista = new AdatbazisMetodusok().KliensListaLekeres(AktivKulcsId);

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
    } // Bejelentkezes gomb esemeny - Login //



    private void uj_kulcs_Click(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Sikeresen hozzáadva a mesterkulcs az adatbázishoz.", "Mesterkulcs hozzáadva!", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    } // Új mesterkulcs hozzáadása gomb esemény - Login//



    private void felhasznalo_rogzites_gomb_Click(object sender, RoutedEventArgs e)
    {

        string erosseg = "placeholder";

        AdatbazisMetodusok metodus = new AdatbazisMetodusok();

        try
        {
            // uj jelszo hozzaadasa //
            // AktivKulcsId = mesterkulcs_id ami hasznalatban van //
            metodus.JelszoTablazatIras(jelszo_mezo.Password.ToString(), erosseg, AktivKulcsId);

            // Az utolso beszurt jelszo_id lekerese //
            int jelszoId;
            using (MySqlConnection db_csatlakozas = metodus.AdatbazisCsatlakozas())
            {
                jelszoId = metodus.JelszoIdUtolso(db_csatlakozas);
            }

            // Beszur egy uj felhasznalo rekordot a lekerdezett jelszo_id-vel //
            metodus.FelhasznaloTablazatIras(nev_mezo.Text, eMail_mezo.Text, url_mezo.Text, hozzafuzes_mezo.Text, jelszoId);

            // Felhasznalok lista uritese //
            felhasznalok_lista.ItemsSource = null;

            // Felhasznalok lista frissitese //
            List<KliensLista> kliens_lista = new AdatbazisMetodusok().KliensListaLekeres(AktivKulcsId);
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


    } // Felhasználó rögzítés gomb esemény - Main //



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
    }  // Jelszó mező változás esemény - Main//



    private void kijelentkezes_gomb_Click(object sender, RoutedEventArgs e) // Kijelentkezés gomb esemény - Main //
    {
        // Mezők ürítése //
        mesterkulcs_mezo.Password = null;

        // Aktív kulcs mező ürítése //
        aktiv_kulcs.Content = null;

        // Aktív kulcs és id nullázása //
        AktivKulcs = string.Empty;
        AktivKulcsId = string.Empty;

        // Felhasznalok lista uritese //
        felhasznalok_lista.ItemsSource = null;

        // Login felulet mutatasa //
        kliens_felulet.Visibility = Visibility.Collapsed;
        mesterkulcs_felulet.Visibility = Visibility.Visible;

    }
}