using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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

        [MaxLength(ImageFileMaxLength)]
        public string ImageFileName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
