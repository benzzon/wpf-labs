﻿using CommunityToolkit.Mvvm.ComponentModel; //Previous name "Microsoft.Toolkit.Mvvm.ComponentModel"
using CommunityToolkit.Mvvm.Input; // Previous name "Microsoft.Toolkit.Mvvm.Input"
using LabsUI.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
//using YourApp.Models;

namespace LabsUI.ViewModels
{
    public partial class MainWindowViewModel : ObservableRecipient
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

//        public IRelayCommand LoadCommand { get; }
//        public IRelayCommand SaveCommand { get; }
//        public IRelayCommand AddPersonCommand { get; }

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

//            AddPersonCommand = new RelayCommand(DoAdd);
//            LoadCommand = new RelayCommand(DoLoad);
//            SaveCommand = new RelayCommand(DoSave);

            genders = new string[] { "Male", "Female" };
        }

        [RelayCommand]
        private void DoAdd()
        {
            if (selectedPerson.PersonName == null) // Name is required..
            {
                return;
            }

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

        [RelayCommand]
        private void DoDelete()
        {
            People.Remove(SelectedPerson);
            SelectedPerson = new PersonModel(); // Clear the input fields
        }

        [RelayCommand]
        private void DoLoad()
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

        [RelayCommand]
        private void DoSave()
        {
            if (People == null) 
            {
                return; 
            }

            // Serialize and save the data to an XML file.
            var serializer = new XmlSerializer(typeof(ObservableCollection<PersonModel>));
            using (var writer = new System.IO.StreamWriter("people.xml"))
            {
                serializer.Serialize(writer, People);
            }

            CollectionViewSource.GetDefaultView(People).Refresh(); // Force refresh of grid to show any changes to person-fields..
        }

        [RelayCommand]
        private void DoSearch(string textToSearch)
        {
            var coll = CollectionViewSource.GetDefaultView(People);
            if (!string.IsNullOrWhiteSpace(textToSearch))
                coll.Filter = c => ((PersonModel)c).PersonName.ToLower().Contains(textToSearch.ToLower());
            else
                coll.Filter = null;
        }

        [RelayCommand]
        private void DoClose()
        {
            var result = MessageBox.Show("Close application?", "Close", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var w = Application.Current.Windows[0];
                w.Close();
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
