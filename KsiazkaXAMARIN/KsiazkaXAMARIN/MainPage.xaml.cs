using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KsiazkaXAMARIN
{
    public partial class MainPage : ContentPage
    {
        public int page = 1;
        public int pageSize = 5;
        public string url = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "starting.txt");
        public ObservableCollection<Person> persons = new ObservableCollection<Person>();
        public ObservableCollection<Person> accPersons = new ObservableCollection<Person>();
        int counter = 0;

        public MainPage()
        {
            InitializeComponent();

            LoadData(url);
            Load();
        }

        private void Load()
        {
            counter = persons.Count;

            accPersons.Clear();

            for (int i = (page - 1) * pageSize; i < page * pageSize; i++)
            {
                if (i < counter)
                {
                    accPersons.Add(persons[i]);
                }
            }
            listView.ItemsSource = accPersons;
            pageCount.Text = page.ToString();
        }

        private void LoadData(string path)
        {
            persons.Clear();
            accPersons.Clear();

            if (File.Exists(path))
            {
                foreach (var line in File.ReadAllLines(path))
                {
                    var person = Person.Deserialize(line);
                    persons.Add(new Person { Name = person.Name, Surname = person.Surname, Number = person.Number, Email = person.Email });
                }

                page = 1;
                Load();
            }
            else
            {
                DisplayAlert("", "Plik nie został odczytany", "OK");
                persons.Clear();
                Load();
            }
        }

        private void SaveData(string path)
        {
            if (persons != null || persons.Count != 0)
            {
                string data = "";

                foreach (var person in persons)
                {
                    string line = person.Serialize();
                    data += line;
                }

                File.WriteAllText(path, data);
            }
        }

        //dodawanie nowej listy
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayPromptAsync("", "Wpisz nazwę pliku", "OK", "Anuluj");

            if (result != null)
            {
                if (!Regex.IsMatch(result, @"[\w _-]+"))
                {
                    await DisplayAlert("", "Nazwa posiada niedozwolone znaki", "OK");
                    return;
                }
                else
                {
                    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), result+".txt");

                    if (File.Exists(path))
                    {
                        await DisplayAlert("", "Podana nazwa już istnieje", "OK");
                        return;
                    }

                    FileStream fs = File.Create(path);
                    fs.Close();

                    url = path;
                    LoadData(url);
                }
            }
        }

        //wczytywanie listy
        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var filesPath = Directory.GetFiles(folderPath).Select(Path.GetFileName);

            string ActionSheet = await DisplayActionSheet("", "Anuluj", null, filesPath.ToArray());

            if(ActionSheet != "Anuluj")
            {
                url = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ActionSheet);
                
                LoadData(url);
            }
        }

        //zapisywanie listy
        private void ToolbarItem_Clicked_2(object sender, EventArgs e)
        {
            SaveData(url);
        }

        //dodawanie do listy
        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            OptionsPage page = new OptionsPage();

            await Navigation.PushAsync(page);

            //funkcja strzałkowa (anonimowa), strzałka oddziela definicje od implementacji
            page.Disappearing += (object Psender, EventArgs Pe) =>
            {
                var person = page.person;

                if (!string.IsNullOrEmpty(person.Name) || !string.IsNullOrEmpty(person.Surname) || !string.IsNullOrEmpty(person.Number) || !string.IsNullOrEmpty(person.Email))
                {
                    persons.Add(new Person { Name = person.Name, Surname = person.Surname, Number = person.Number, Email = person.Email });
                }

                Load();
            };
        }

        //edycja po przytrzymaniu
        private async void MenuItem_Clicked_1(object sender, EventArgs e)
        {
            var selected = (Person)((MenuItem)sender).BindingContext;

            int id = persons.IndexOf(selected);

            OptionsPage optionsPage = new OptionsPage(persons[id].Name, persons[id].Surname, persons[id].Number, persons[id].Email);

            await Navigation.PushAsync(optionsPage);

            //funkcja strzałkowa (anonimowa), strzałka oddziela definicje od implementacji
            optionsPage.Disappearing += (object Psender, EventArgs Pe) =>
            {
                var pers = optionsPage.person;

                //sprawdzanie czy dane zostały zmienione i, jesli tak, podmiana ich w liście, oraz załadowanie ponownie listy
                if (!string.IsNullOrEmpty(pers.Name) || !string.IsNullOrEmpty(pers.Surname) || !string.IsNullOrEmpty(pers.Number) || !string.IsNullOrEmpty(pers.Email))
                {
                    persons[id] = new Person { Name = pers.Name, Surname = pers.Surname, Number = pers.Number, Email = pers.Email };
                    Load();
                }
            };
        }

        //usuwanie po przytrzymaniu
        private void MenuItem_Clicked_2(object sender, EventArgs e)
        {
            var selected = (Person)((MenuItem)sender).BindingContext;
            persons.Remove(selected);

            if(accPersons.Count > 0)
            {
                back_Clicked(sender, e);
            }

            Load();
        }

        //stronnicowanie przód
        private void next_Clicked(object sender, EventArgs e)
        {
            if (persons.Count > page * pageSize)
            {
                page++;
            }

            Load();
        }

        //stronnicowanie tył
        private void back_Clicked(object sender, EventArgs e)
        {
            if (page - 1 >= 1)
            {
                page--;
            }

            Load();
        }
    }
}
