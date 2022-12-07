using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfPruebaTecnica
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button.IsEnabled = false;
            string texto = "Sin Datos.";

            try
            {
                var t = Task.Run(() => consulta(new Uri("https://api.github.com/users")));
                t.Wait();
                List<Modelo> modelos = t.Result;

                Modelo m = modelos.Where(x => x.id == 1).FirstOrDefault();

                if (m != null)
                {
                    texto = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", m.id, m.login, m.node_id, m.avatar_url, m.gravatar_id, m.url, m.html_url, m.followers_url, m.following_url, m.gists_url);
                }
            }
            catch(Exception ex)
            {
                texto = "Ocurrio un error: " + ex.Message;
            }

            txb.Text = texto;
            Button.IsEnabled = true;
        }

        public static async Task<List<Modelo>> consulta(Uri uri)
        {
            List<Modelo> model = new List<Modelo>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "http://developer.github.com/v3/#user-agent-required");
                    HttpResponseMessage result = await client.GetAsync(uri);
                    if (result.IsSuccessStatusCode)
                    {
                        string response = await result.Content.ReadAsStringAsync();
                        model = JsonConvert.DeserializeObject<List<Modelo>>(response);
                    }
                }
            }
            catch (Exception ex)
            {
                model = new List<Modelo>();
            }

            return model;
        }

    }
}
