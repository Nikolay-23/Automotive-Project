using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Automotive_Project.Common.EntityValidationConstants;

namespace Automotive_Project.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name is required!")]
        [MaxLength(NameMaxLength, ErrorMessage = "Product Name is too long!")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Product Brand is required!")]
        [MaxLength(BrandMaxLength, ErrorMessage = "Product Brand is too long!")]
        public string Brand { get; set; } = null!;

        [Required(ErrorMessage = "Category Name is required!")]
        [MaxLength(CategoryMaxLength, ErrorMessage = "Product Category is too long!")]
        public string Category { get; set; } = null!;

        [Required(ErrorMessage = "Product Price is required!")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Product Description is required!")]
        public string Description { get; set; } = null!;
        [JsonIgnore]
        public IFormFile? ImageFileName { get; set; } 
        public int Quantity { get; set; }

        [Required(ErrorMessage = "ProductWarehouse is required")]
        [MaxLength(30, ErrorMessage = "ProductWarehouse is too long!")]
        public string ProductWarehouses { get; set; }
    }
}
