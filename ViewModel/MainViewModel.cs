using System.ComponentModel;
using System.Collections.ObjectModel;
using Wpf_PueblosCLM.Models;
using System.Net.Http;
using System.Net.Http.Headers;

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

            string URLData = "https://docs.google.com/spreadsheets/d/1G0YM-YztE0hQBA6vQ0LpBU93x4OEO6LlNUqWpvIZVbM/gviz/tq?tqx=out:json&gid=482057707";

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(URLData);
        }
    }
}