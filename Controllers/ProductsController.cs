using Automotive_Project.Data;
using Automotive_Project.Models;
using Automotive_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Automotive_Project.Controllers
{
    [Authorize(Roles = "User")]
    [Route("/Admin/[controller]/{action=Index}/{id?}")]
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

        public IActionResult Create()
        {
            return View(new ProductViewModel());
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel viewModel)
        {
            if (viewModel.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required!");
            }

            //Validate the input data using ModelState
            if (ModelState.IsValid)
            {
                // save the image file
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(viewModel.ImageFile!.FileName);

                string imageFullPath = _environment.WebRootPath + "/images/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    viewModel.ImageFile.CopyTo(stream);
                }

                //Map the view model to the Product entity
                var product = new Product()
                {
                    Name = viewModel.Name,
                    Brand = viewModel.Brand,
                    Category = viewModel.Category,
                    Price = viewModel.Price,
                    Description = viewModel.Description,
                    ImageFileName = newFileName,
                    CreatedAt = DateTime.Now
                };

                _context.Products.Add(product);
                _context.SaveChanges();

                return RedirectToAction("Index", "Products");
            }



            return View(viewModel);
        }

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

                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description
            };

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

            return View(productDto);
        }

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
            if (productViewModel.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productViewModel.ImageFile.FileName);

                string imageFullPath = _environment.WebRootPath + "/images/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productViewModel.ImageFile.CopyTo(stream);
                }

                //delete the old image
                string oldImageFullPath = _environment.WebRootPath + "/images/" + product.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            //update the product int the database
            product.Name = productViewModel.Name;
            product.Brand = productViewModel.Brand;
            product.Category = productViewModel.Category;
            product.Price = productViewModel.Price;
            product.Description = productViewModel.Description;
            product.ImageFileName = newFileName;

            _context.SaveChanges();

            return RedirectToAction("Index", "Products");

        }

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
    }
}
