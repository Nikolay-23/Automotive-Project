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
                    Price = 153,
                    Description = "One of the most used filters on market",
                    ImageFileName = "OilFIlter.jpg",
                    CreatedAt = new DateTime(2025, 9, 28)
                },

                new Product
                {
                    Id = 2,
                    Name = "K&N High Performance Air Filter",
                    Brand = "Audi",
                    Category = "Filters",
                    Price = 267,
                    Description = "Filter with high efficiency",
                    ImageFileName = "K&N High Performance Air Filter.jpg",
                    CreatedAt = new DateTime(2025, 10, 28)

                },

                new Product
                {
                    Id = 3,
                    Name = "Mahle Fuel Filter",
                    Brand = "Audi",
                    Category = "Filters",
                    Price = 300,
                    Description = "Fuel filter",
                    ImageFileName = "Mahle Fuel Filter.jpg",
                    CreatedAt = new DateTime(2025, 10, 28)
                },

                new Product
                {
                    Id = 4,
                    Name = "WIX Oil Filter",
                    Brand = "Audi",
                    Category = "Filters",
                    Price = 290,
                    Description = "Fuel filter",
                    ImageFileName = "Fuel filter.jpg",
                    CreatedAt = new DateTime(2025, 10, 28)
                });

        }
    }
}
