using Automotive_Project.Data;
using Automotive_Project.Extensions;
using Automotive_Project.Models;
using Automotive_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Automotive_Project.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CustomUserManager<UserAccount> _userManager;
        private readonly decimal shippingFee;

        public CartController(ApplicationDbContext context, IConfiguration configuration, CustomUserManager<UserAccount> userManager)
        {
            _context = context;
            _userManager = userManager;
            shippingFee = configuration.GetValue<decimal>("CartSettings:ShippingFee");
        }

        public IActionResult Index()
        {
            List<OrderItem> cartItems = CartHelper.GetCartItems(Request, Response, _context);
            decimal subtotal = CartHelper.GetSubTotal(cartItems);


            ViewBag.CartItems = cartItems;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.Subtotal = subtotal;
            ViewBag.Total = subtotal + shippingFee;

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index(CheckOutViewModel checkOutViewModel)
        {
            List<OrderItem> cartItems = CartHelper.GetCartItems(Request, Response, _context);
            decimal subtotal = CartHelper.GetSubTotal(cartItems);


            ViewBag.CartItems = cartItems;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.Subtotal = subtotal;
            ViewBag.Total = subtotal + shippingFee;

            if (!ModelState.IsValid)
            {
                return View(checkOutViewModel);
            }

            //Check if shopping cart is empty or not
            if (cartItems.Count == 0)
            {
                ViewBag.ErrorMessage = "Your cart is empty";
                return View(checkOutViewModel);
            }

            TempData["DeliveryAddress"] = checkOutViewModel.DeliveryAddress;

            return RedirectToAction("Confirm");
        }

        public IActionResult Confirm()
        {
            List<OrderItem> cartItems = CartHelper.GetCartItems(Request, Response, _context);
            decimal total = CartHelper.GetSubTotal(cartItems) + shippingFee;
            int cartSize = 0;

            foreach (var item in cartItems)
            {
                cartSize += item.Quantity;
            }

            string deliveryAddress = TempData["DeliveryAddress"] as string ?? "";
            string paymentMethod = TempData["PaymentMethod"] as string ?? "";
            TempData.Keep();

            if (cartSize == 0 || deliveryAddress.Length == 0 || paymentMethod.Length == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.DeliveryAddress = deliveryAddress;
            ViewBag.PaymentMethod = paymentMethod;
            ViewBag.Total = total;
            ViewBag.CartSize = cartSize;

            return View();
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Confirm(int any)
        {
            var cartItems = CartHelper.GetCartItems(Request, Response, _context);

            string deliveryAddress = TempData["DeliveryAddress"] as string ?? "";
            string paymenthMethod = TempData["PaymentMethod"] as string ?? "";
            TempData.Keep();

            if (cartItems.Count == 0 || deliveryAddress.Length == 0 || paymenthMethod.Length == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var userWithClaims = await _userManager.GetUserAsync(User);
            if (userWithClaims == null)
                return RedirectToAction("Index", "Home");

            var appUser = userWithClaims.User; 
            var claimsPrincipal = userWithClaims.ClaimsPrincipal; 

            var order = new Order()
            {
                ClientId = appUser.Id.ToString(), 
                Items = cartItems,
                ShippingFee = shippingFee,
                DeliveryAddress = deliveryAddress,
                PaymentMethod = paymenthMethod,
                PaymentStatus = "pending",
                PaymentDetails = "",
                OrderStatus = "created",
                CreatedAt = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            //delete the shopping cart cookie
            Response.Cookies.Delete("shopping_cart");

            ViewBag.SuccessMessage = "Order created successfully";

            return View();
        }
    }
}
