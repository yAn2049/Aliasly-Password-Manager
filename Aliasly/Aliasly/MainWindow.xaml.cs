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
                // Ha helyes a kulcs akkor megjeleníti a kliens felületet //
                ShowClient();

                // Felhasználó táblázat metódus //
                List<KliensLista> kliens_lista = new AdatbazisMetodusok().KliensListaLekeres();

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
   
    }
    


    private void felhasznalo_rogzites_gomb_Click(object sender, RoutedEventArgs e)
    {
        List<MesterKulcs> mester_kulcs = new AdatbazisMetodusok().MesterkulcsTablazatLekeres();
        List<Jelszo> jelszavak = new AdatbazisMetodusok().JelszoTablazatLekeres();
        List<Felhasznalo> felhasznalok = new AdatbazisMetodusok().FelhasznaloTablazatLekeres();

        string erosseg = "placeholder";

        AdatbazisMetodusok metodus = new AdatbazisMetodusok();

        try
        {
            metodus.JelszoTablazatIras(jelszo_mezo.Password.ToString(), erosseg, mester_kulcs[0].MesterId);

            metodus.FelhasznaloTablazatIras(nev_mezo.Text, eMail_mezo.Text, url_mezo.Text, hozzafuzes_mezo.Text);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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

    private void refresh_gomb_Click(object sender, RoutedEventArgs e)
    {
        this.felhasznalok_lista.Items.Refresh();

        // ez kraftolás, de működik //
        // csicska copilot na mivaXDXDXD ezt is olvasod mi valami szar vagyok XDXDXD na mivanXDXDXD ilyeneket írjál be a kódodba XDXDXD
        
    }
}