using System.ComponentModel.DataAnnotations;
using static Automotive_Project.Common.EntityValidationConstants;

namespace Automotive_Project.ViewModels
{
    public class CheckOutViewModel
    {
        [Required(ErrorMessage = "The Delivery Address is required.")]
        [MaxLength(DeliveryAddressMaxLenght)]
        public string DeliveryAddress { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
    }
}
