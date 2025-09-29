using Automotive_Project.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Automotive_Project.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilde)
        {
            modelBuilde.Entity<Product>().HasData(

                new Product
                {
                    Id = 1,
                    Name = "Oil Filter DENCKERMANN A210734",
                    Brand = "Audi",
                    Category = "Filters",
                    Price = 123,
                    Description = "One of the most used filters on market",
                    ImageFileName = "OilFIlter.jpg",
                    CreatedAt = new DateTime(2025, 9, 28)
                });
        }
    }
}
