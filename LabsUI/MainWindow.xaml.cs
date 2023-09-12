﻿using LabsUI.ViewModels;
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LabsUI.Properties.Settings.Default.WinLeft = Application.Current.MainWindow.Left;
            LabsUI.Properties.Settings.Default.WinTop = Application.Current.MainWindow.Top;
            LabsUI.Properties.Settings.Default.WinHeight = Application.Current.MainWindow.Height;
            LabsUI.Properties.Settings.Default.WinWidth = Application.Current.MainWindow.Width;

            LabsUI.Properties.Settings.Default.Save();
        }
    }
}
