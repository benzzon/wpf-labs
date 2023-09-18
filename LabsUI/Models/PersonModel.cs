using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace LabsUI.Models
{
    [ObservableObject]
    public partial class PersonModel
    {
        public string PersonName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
    }
}
