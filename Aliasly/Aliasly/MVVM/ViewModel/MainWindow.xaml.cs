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
using Aliasly.MVVM.Model;
using MySql.Data.MySqlClient;

namespace Aliasly;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    List<Adatok> adatok = new List<Adatok>();
    string connParams = "server=localhost;user=root;database=aliasly;port=3306";
    public MainWindow()
    {
        InitializeComponent();
        StartUp();

        MySqlConnection connection = new MySqlConnection(connParams);
        try
        {
            connection.Open();

            string sqlAdatok = "SELECT id, jelszo, email, nev, url, hozzafuzes, mester_id FROM adatok";
            MySqlCommand sqlCommand = new MySqlCommand(sqlAdatok, connection);
            MySqlDataReader sqlReader = sqlCommand.ExecuteReader();

            while (sqlReader.Read())
            {
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
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Adatbázis csatlakozás error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        foreach (var a in adatok)
        {
            this.lista.ItemsSource = adatok;
        }

    }
    public void StartUp()
    {

    }
}