using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabsUI.Models;
using System.Collections.Generic;
//using Microsoft.Toolkit.Mvvm.ComponentModel;
//using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
//using YourApp.Models;

namespace LabsUI.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public ObservableCollection<PersonModel> People { get; set; } = new ObservableCollection<PersonModel>();

        private PersonModel selectedPerson;
        const double defaultWinHeight = 300;
        const double defaultWinWidth = 600;

        public PersonModel SelectedPerson
        {
            get => selectedPerson;
            set => SetProperty(ref selectedPerson, value);
        }

        public IRelayCommand LoadCommand { get; }
        public IRelayCommand SaveCommand { get; }
        public IRelayCommand AddPersonCommand { get; }

        public MainWindowViewModel()
        {
            Application.Current.MainWindow.Left = LabsUI.Properties.Settings.Default.WinLeft;
            Application.Current.MainWindow.Top = LabsUI.Properties.Settings.Default.WinTop;
            Application.Current.MainWindow.Height = LabsUI.Properties.Settings.Default.WinHeight == 0 ? defaultWinHeight : LabsUI.Properties.Settings.Default.WinHeight;
            Application.Current.MainWindow.Width = LabsUI.Properties.Settings.Default.WinWidth == 0 ? defaultWinWidth : LabsUI.Properties.Settings.Default.WinWidth;

            // Initialize the ObservableCollection
            People = new ObservableCollection<PersonModel>();

            // Initialize the SelectedPerson
            SelectedPerson = new PersonModel();

            AddPersonCommand = new RelayCommand(AddPerson);
            LoadCommand = new RelayCommand(LoadData);
            SaveCommand = new RelayCommand(SaveData);

            genders = new string[] { "Male", "Female" };
        }

        private void AddPerson()
        {
            // Create a new PersonModel and add it to the ObservableCollection
            var newPerson = new PersonModel
            {
                PersonName = SelectedPerson.PersonName,
                Email = SelectedPerson.Email,
                Gender = SelectedPerson.Gender,
                PhoneNumber = SelectedPerson.PhoneNumber
            };

            People.Add(newPerson);

            // Clear the input fields
            SelectedPerson = new PersonModel();
        }

        private void LoadData()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<PersonModel>));

                List<PersonModel> temp = new List<PersonModel>();

                using (XmlReader reader = XmlReader.Create("people.xml"))
                {
                    temp = (List<PersonModel>)ser.Deserialize(reader);
                }

                People.Clear();
                temp.ForEach(x => People.Add(x));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occurred..");
            }
        }
        private void SaveData()
        {
            // Check if SelectedPerson is not null and has valid data.
            if (SelectedPerson != null && !string.IsNullOrEmpty(SelectedPerson.PersonName))
            {
                People.Add(SelectedPerson);
                SelectedPerson = new PersonModel(); // Clear the input fields
            }

            // Serialize and save the data to an XML file.
            var serializer = new XmlSerializer(typeof(ObservableCollection<PersonModel>));
            using (var writer = new System.IO.StreamWriter("people.xml"))
            {
                serializer.Serialize(writer, People);
            }
        }

        private IEnumerable<string> genders;
        public IEnumerable<string> Genders
        {
            get { return genders; }
            set
            {
                genders = value;
                //OnPropertyChanged("Currencies");
            }
        }

    }
}
