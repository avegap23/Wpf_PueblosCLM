using System.ComponentModel;
using System.Collections.ObjectModel;
using Wpf_PueblosCLM.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Linq;

namespace Wpf_PueblosCLM.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MainViewModel()
        {
            listaPueblos = new ObservableCollection<PuebloAux>();
            LeerJson();
        }

        private ObservableCollection<PuebloAux> _listaPueblos;
        public ObservableCollection<PuebloAux> listaPueblos
        {
            get { return _listaPueblos; }
            set
            {
                _listaPueblos = value;
                OnPropertyChanged("listaPueblos");
            }
        }

        private async void LeerJson()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024;

            // URL al fichero csv
            string URLData = "https://docs.google.com/spreadsheets/d/1G0YM-YztE0hQBA6vQ0LpBU93x4OEO6LlNUqWpvIZVbM/gviz/tq?tqx=out:json&gid=482057707";

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Un await para que responseMessage no trabaje antes de que llegue el JSON
            HttpResponseMessage responseMessage = await client.GetAsync(URLData);

            if (responseMessage.IsSuccessStatusCode)
            {
                var data1 = responseMessage.Content.ReadAsStringAsync();

                // El JSON tiene caracteres que sobran. Vamos a eliminarlos

                // Averiguar en qué posición comienza el JSON (donde está el "{")
                int indice = (data1.Result.ToString()).IndexOf("{");
                // Eliminar todo el principio
                string cadena = (data1.Result.ToString()).Remove(0, indice);
                // Eliminamos los dos últimos caracteres
                string cadenaResultado = cadena.Substring(0, cadena.Length -2);

                Root? pueblosData = JsonConvert.DeserializeObject<Root>(cadenaResultado);

                int c = pueblosData.table.rows.Count();

                // Metemos el nombre de cada pueblo en listaPueblos
                for (int i = 0; i < c; i++)
                {
                    PuebloAux puebloA = new PuebloAux();
                    
                    puebloA.pueblo = pueblosData.table.rows[i].c[1].v.ToString();
                    listaPueblos.Add(puebloA);
                }
            }
        }
    }
}