using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LabsUI.Models
{
    [ObservableObject]
    public partial class PersonModel : IDataErrorInfo
    {
        [Required(ErrorMessage = "You must provide a name.")]
        [StringLength(200, MinimumLength = 3,
        ErrorMessage = "The name must be at least 3 characters long")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "The name must only contain letters (a-z, A-Z).")]
        public string PersonName { get; set; }

        [Required(ErrorMessage = "You must provide a mail-address.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }


        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                var context = new ValidationContext(this, null, null) { MemberName = columnName };
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateProperty(GetType().GetProperty(columnName).GetValue(this, null), context, results))
                {
                    return results[0].ErrorMessage;
                }
                return null;
            }
        }
    }
}
