using Automotive_Project.Data;
using Automotive_Project.Extensions;
using Automotive_Project.Models;
using Automotive_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Automotive_Project.Controllers
{
    
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly int pageSize = 5;
        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [Authorize(Roles = "Admin")]
        [Route("/Admin/[controller]/{action=Index}/{id?}")]
        public IActionResult Index(int pageIndex, string? search, string? column, string? orderBy)
        {
            IQueryable<Product> query = _context.Products;

            // search functionality
            if (search != null)
            {
                query = query.Where(p => p.Name.Contains(search) || p.Brand.Contains(search));
            }

            // sort functionality
            string[] validColumns = { "Id", "Name", "Brand", "Category", "Price", "CreatedAt" };
            string[] validOrderBy = { "desc", "asc" };

            if (!validColumns.Contains(column))
            {
                column = "Id";
            }

            if (!validOrderBy.Contains(orderBy))
            {
                orderBy = "desc";
            }

            if (column == "Name")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Name);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Name);
                }
            }
            else if (column == "Brand")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Brand);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Brand);
                }
            }
            else if (column == "Category")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Category);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Category);
                }
            }
            else if (column == "Price")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Price);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Price);
                }
            }
            else if (column == "CreatedAt")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.CreatedAt);
                }
                else
                {
                    query = query.OrderByDescending(p => p.CreatedAt);
                }
            }
            else
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Id);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Id);
                }
            }



            //pagination functionality
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var products = query.ToList();

            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = totalPages;

            ViewData["Search"] = search ?? "";

            ViewData["Column"] = column;
            ViewData["OrderBy"] = orderBy;

            return View(products);
        }

        [Authorize(Roles = "Admin")]
        [Route("/Admin/[controller]/{action=Index}/{id?}")]
        public IActionResult Create()
        {
            return View(new ProductViewModel());
        }

        [Authorize(Roles = "Admin")]
        [Route("/Admin/[controller]/{action=Index}/{id?}")]
        [HttpPost]
        public IActionResult Create(ProductViewModel viewModel)
        {
            // Ensure an image file was uploaded
            if (viewModel.ImageFileName == null || viewModel.ImageFileName.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "The image file is required!");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                // Create a unique filename
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") +
                                     Path.GetExtension(viewModel.ImageFileName.FileName);

                // Ensure /images directory exists
                string imagesPath = Path.Combine(_environment.WebRootPath, "images");
                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                }

                // Full path for saving the file
                string imageFullPath = Path.Combine(imagesPath, newFileName);

                // Save the image file
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    viewModel.ImageFileName.CopyTo(stream);
                }

                // Map ViewModel to Product entity
                var product = new Product()
                {
                    Name = viewModel.Name,
                    Brand = viewModel.Brand,
                    Category = viewModel.Category,
                    ProductWarehouses = viewModel.ProductWarehouses,
                    Quantity = viewModel.Quantity,
                    Price = viewModel.Price,
                    Description = viewModel.Description,
                    ImageFileName = newFileName,
                    CreatedAt = DateTime.Now
                };

                // Save to database
                _context.Products.Add(product);
                _context.SaveChanges();

                return RedirectToAction("Index", "Products");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error saving product: " + ex.Message);
                return View(viewModel);
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("/Admin/[controller]/{action=Index}/{id?}")]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            // create productDto from product
            var productDto = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                ProductWarehouses = product.ProductWarehouses,
                Quantity = product.Quantity,
                Price = product.Price,
                Description = product.Description
                
            };

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

            return View(productDto);
        }

        [Authorize(Roles = "Admin")]
        [Route("/Admin/[controller]/{action=Index}/{id?}")]
        [HttpPost]
        public IActionResult Edit(int id, ProductViewModel productViewModel)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

                return View(productViewModel);
            }

            //update the image file if we have a new image file
            string newFileName = product.ImageFileName;
            if (productViewModel.ImageFileName != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productViewModel.ImageFileName.FileName);

                string imageFullPath = _environment.WebRootPath + "/images/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productViewModel.ImageFileName.CopyTo(stream);
                }

                //delete the old image
                string oldImageFullPath = _environment.WebRootPath + "/images/" + product.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            //update the product int the database
            product.Id = productViewModel.Id;
            product.Name = productViewModel.Name;
            product.Brand = productViewModel.Brand;
            product.Category = productViewModel.Category;
            product.ProductWarehouses = productViewModel.ProductWarehouses;
            product.Quantity = productViewModel.Quantity;
            product.Price = productViewModel.Price;
            product.Description = productViewModel.Description;
            product.ImageFileName = newFileName;

            _context.SaveChanges();

            return RedirectToAction("Index", "Products");

        }

        [Authorize(Roles = "Admin")]
        [Route("/Admin/[controller]/{action=Index}/{id?}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            string imageFullPath = _environment.WebRootPath + "/images/" + product.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            _context.Products.Remove(product);
            _context.SaveChanges(true);

            return RedirectToAction("Index", "Products");
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart([FromBody] int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null || product.Quantity <= 0)
            {
                return Json(new { success = false, message = "Product not available." });
            }

           
            product.Quantity--;
            _context.SaveChanges();

           
            var cart = CartHelper.GetCartDictionary(Request, Response);

            if (cart.ContainsKey(productId))
                cart[productId]++;
            else
                cart[productId] = 1;

            SaveCartToCookie(cart);

            return Json(new { success = true, newQuantity = product.Quantity, cartSize = CartHelper.GetCartSize(Request, Response) });
        }

        private void SaveCartToCookie(Dictionary<int, int> cart)
        {
            var json = JsonSerializer.Serialize(cart);
            var cookieValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            Response.Cookies.Append("shopping_cart", cookieValue, new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) });
        }
    }
}
