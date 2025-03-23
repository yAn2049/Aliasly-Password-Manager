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

/*
 * változók: snake_case
 * osztályok és tartozékai: PascalCase
 * metódusok: PascalCase
 */



public partial class MainWindow : Window
{   
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



    private void enter_gomb_Click(object sender, RoutedEventArgs e)
    {
        
        // Mesterkulcs táblázat metódus //
        List<MesterKulcs> mester_kulcs = new AdatbazisMetodusok().MesterkulcsTablazatLekeres();

        // Végigfut a mesterkulcs listán és lecsekkolja hogy létezik-e már az írni kívánt adat //
        bool van_mar_ilyen_kulcs = false;
        foreach (var m in mester_kulcs)
        {
            if (mesterkulcs_mezo.Password == m.KulcsString)
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
            // Ha helyes a kulcs akkor megjeleníti a kliens felületet //
            ShowClient();

            // Felhasználó táblázat metódus //
            List<Felhasznalo> felhasznalok = new AdatbazisMetodusok().FelhasznaloTablazatLekeres();

            // Sql soronkénti betöltése a listbox elembe //
            foreach (var f in felhasznalok)
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
        string mesterkulcs = mesterkulcs_mezo.Password;

        TitkositasMetodusok titkositasMetodusok = new TitkositasMetodusok();
        List<string> hashResults = titkositasMetodusok.HashMasterKey(mesterkulcs);

        string fullHash = hashResults[0];
        string salt = hashResults[1];
        string hashWithoutSalt = hashResults[2];

        AdatbazisMetodusok adatbazisMetodusok = new AdatbazisMetodusok();

        // Mesterkulcs táblázat metódus //
        List<MesterKulcs> mester_kulcs = adatbazisMetodusok.MesterkulcsTablazatLekeres();

        // Végigfut a mesterkulcs listán és lecsekkolja hogy létezik-e már az írni kívánt adat //
        bool van_mar_ilyen_kulcs = mester_kulcs.Any(m => m.KulcsString == fullHash);

        // Ha létezik már ilyen adat, hibát dob fel. //
        if (van_mar_ilyen_kulcs)
        {
            MessageBox.Show("Ez a kulcs már létezik az adatbázisodban!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        // Ha nem létezik ilyen adat, akkor feltölti az adatbázis táblázatába //
        else
        {
            try
            {
                adatbazisMetodusok.MesterkulcsTablazatIras(fullHash, salt, hashWithoutSalt);
                MessageBox.Show("Új mesterkulcs sikeresen hozzáadva!", "Siker!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }



    private void felhasznalo_rogzites_gomb_Click(object sender, RoutedEventArgs e)
    {
        // ezt majd krafting kesobb
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