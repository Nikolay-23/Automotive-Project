using System.ComponentModel.DataAnnotations;
using static Automotive_Project.Common.EntityValidationConstants;

namespace Automotive_Project.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(EmailAddressMaxLength)]
        public string Email { get; set; }

    }
}
