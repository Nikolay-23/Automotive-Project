using Automotive_Project.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
                    ProductWarehouses = "Sofia",
                    Quantity = 4,
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
                    ProductWarehouses = "Varna",
                    Quantity = 7,
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
                    ProductWarehouses = "Varna",
                    Quantity = 9,
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
                    ProductWarehouses = "Gabrovo",
                    Quantity = 9,
                    Description = "Fuel filter",
                    ImageFileName = "Fuel filter.jpg",
                    CreatedAt = new DateTime(2025, 10, 28)
                },

                new Product
                {
                    Id = 5,
                    Name = "Brake calipers red for Mercedes - AMG 63 ",
                    Brand = "Mercedes-Benz",
                    Category = "Breaks",
                    ProductWarehouses = "Sofia",
                    Quantity = 6,
                    Price = 800,
                    Description = "One of the most used breaks for Mercedes AMG",
                    ImageFileName = "MercedesBreaks.jpg",
                    CreatedAt = new DateTime(2025, 11, 13)
                },

                new Product
                {
                    Id = 6,
                    Name = "Brake discs for Mercedes - ML 63 AMG",
                    Brand = "Mercedes-Benz",
                    Category = "Breaks",
                    ProductWarehouses = "Troqn",
                    Quantity = 4,
                    Price = 490,
                    Description = "Most used brake discs for Mercedes AMG line",
                    ImageFileName = "Mercedes-Benz-Brake discs.jpg",
                    CreatedAt = new DateTime(2025, 11, 16)
                });

        }
    }
}
