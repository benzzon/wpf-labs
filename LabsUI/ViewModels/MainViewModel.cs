using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabsUI.Models;
//using Microsoft.Toolkit.Mvvm.ComponentModel;
//using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Serialization;
//using YourApp.Models;

namespace LabsUI.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<PersonModel> People { get; set; } = new ObservableCollection<PersonModel>();

        private PersonModel selectedPerson;
        public PersonModel SelectedPerson
        {
            get => selectedPerson;
            set => SetProperty(ref selectedPerson, value);
        }

        public IRelayCommand SaveCommand { get; }
        public IRelayCommand AddPersonCommand { get; }

        public MainViewModel()
        {
            // Initialize the ObservableCollection
            People = new ObservableCollection<PersonModel>();

            // Initialize the SelectedPerson
            SelectedPerson = new PersonModel();

            AddPersonCommand = new RelayCommand(AddPerson);
            SaveCommand = new RelayCommand(SaveData);
        }

        private void AddPerson()
        {
            // Create a new PersonModel and add it to the ObservableCollection
            var newPerson = new PersonModel
            {
                FirstName = SelectedPerson.FirstName,
                LastName = SelectedPerson.LastName,
                Gender = SelectedPerson.Gender,
                PhoneNumber = SelectedPerson.PhoneNumber
            };

            People.Add(newPerson);

            // Clear the input fields
            SelectedPerson = new PersonModel();
        }

        private void SaveData()
        {
            // Check if SelectedPerson is not null and has valid data.
            if (SelectedPerson != null && !string.IsNullOrEmpty(SelectedPerson.FirstName))
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
    }
}
