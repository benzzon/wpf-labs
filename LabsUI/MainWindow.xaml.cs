using LabsUI.ViewModels;
using System.Windows;
using System.Windows.Controls;
//using YourApp.ViewModels;

namespace LabsUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem != null)
            {
                if (comboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    // Update the SelectedPerson's Gender property
                    // ViewModel.SelectedPerson.Gender = selectedItem.Content.ToString();
                }
            }
        }
    }
}
