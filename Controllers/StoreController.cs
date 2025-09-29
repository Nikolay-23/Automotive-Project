using Automotive_Project.Data;
using Automotive_Project.Models;
using Automotive_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Automotive_Project.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int pageSize = 8;
        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int pageIndex, string? search, string? brand, string? category, string? sort)
        {
            IQueryable<Product> query = _context.Products;

            // search functionality
            if (search != null && search.Length > 0)
            {
                query = query.Where(p => p.Name.Contains(search));
            }


            // filter functionality
            if (brand != null && brand.Length > 0)
            {
                query = query.Where(p => p.Brand.Contains(brand));
            }

            if (category != null && category.Length > 0)
            {
                query = query.Where(p => p.Category.Contains(category));
            }

            // sort functionality
            if (sort == "price_asc")
            {
                query = query.OrderBy(p => p.Price);
            }
            else if (sort == "price_desc")
            {
                query = query.OrderByDescending(p => p.Price);
            }
            else
            {
                // newest products first
                query = query.OrderByDescending(p => p.Id);
            }



            // pagination functionality
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);


            var products = query.ToList();

            ViewBag.Products = products;
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = totalPages;

            var storeSearchModel = new StoreSearchViewModel()
            {
                Search = search,
                Brand = brand,
                Category = category,
                Sort = sort
            };
            return View(storeSearchModel);
        }

        public IActionResult Details(int id)
        {
            var products = _context.Products.Find(id);
            if (products == null)
            {
                return RedirectToAction("Index", "Store");
            }

            return View(products);
        }
    }
}
