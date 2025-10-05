using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Automotive_Project.Common.EntityValidationConstants;
namespace Automotive_Project.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(BrandMaxLength)]
        public string Brand { get; set; } = null!;

        [MaxLength(CategoryMaxLength)]
        public string Category { get; set; } = null!;

        [Precision(16, 2)]
        public decimal Price { get; set; }

        public string Description { get; set; } = null!;

        public string ImageFileName { get; set; } 

        public DateTime CreatedAt { get; set; }

        public string ProductWarehouses { get; set; } 

        public int Quantity { get; set; }
    }
}
