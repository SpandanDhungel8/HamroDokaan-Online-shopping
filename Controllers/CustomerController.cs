using HamroDokaan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HamroDokaan.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HamroDokaanDbContext context;

        public CustomerController(HamroDokaanDbContext context)
        {
            this.context = context;
        }

        private int? GetUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        public IActionResult Categories()
        {
            if (HttpContext.Session.GetString("Role") != "Customer")
                return RedirectToAction("Login", "Account");

            var category = context.Categories.ToList();
            return View(category);
        }

        
        public IActionResult Products(int? id)
        {
            List<Product> products;

            if (id == null)
                products = context.Products.ToList();
            else
                products = context.Products
                                  .Where(p => p.Category_Id == id)
                                  .ToList();

            return View(products);
        }

        public IActionResult AddToCart(int id)
        {
            int? userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var existingItem = context.Carts
                .FirstOrDefault(c => c.Product_Id == id && c.User_Id == userId);

            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {
                Cart cart = new Cart()
                {
                    Product_Id = id,
                    Quantity = 1,
                    User_Id = userId.Value
                };

                context.Carts.Add(cart);
            }

            context.SaveChanges();

            TempData["Added"] = "Product added to cart!";
            return RedirectToAction("Products");
        }

        public IActionResult Cart()
        {
            int? userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cartItems = context.Carts
                .Where(c => c.User_Id == userId)
                .Include(c => c.Product)
                .ToList();

            return View(cartItems);
        }

        public IActionResult RemoveFromCart(int id)
        {
            int? userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var item = context.Carts
                .FirstOrDefault(c => c.Cart_Id == id && c.User_Id == userId);

            if (item != null)
            {
                context.Carts.Remove(item);
                context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

        public IActionResult IncreaseQty(int id)
        {
            int? userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var item = context.Carts
                .FirstOrDefault(c => c.Cart_Id == id && c.User_Id == userId);

            if (item != null)
            {
                item.Quantity += 1;
                context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }
    
        public IActionResult DecreaseQty(int id)
        {
            int? userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var item = context.Carts
                .FirstOrDefault(c => c.Cart_Id == id && c.User_Id == userId);

            if (item != null)
            {
                if (item.Quantity > 1)
                    item.Quantity -= 1;
                else
                    context.Carts.Remove(item);

                context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }
        public IActionResult Checkout()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cartItems = context.Carts
                .Where(c => c.User_Id == userId)
                .Include(c => c.Product)
                .ToList();

            return View(cartItems);
        }
        [HttpPost]

        public IActionResult PlaceOrder(string paymentMethod, string address, string phone)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null || !context.Users.Any(u => u.User_Id == userId))
                return RedirectToAction("Login", "Account");

            var cartItems = context.Carts
                .Where(c => c.User_Id == userId)
                .Include(c => c.Product)
                .ToList();

            if (!cartItems.Any())
                return RedirectToAction("Cart");

            decimal total = 0;

            Order order = new Order()
            {
                User_Id = userId.Value,
                OrderDate = DateTime.Now,
                PaymentMethod = paymentMethod,
                Status = "Pending",

                
                Address = address,
                Phone = phone,

                OrderItems = new List<OrderItem>()
            };

            foreach (var item in cartItems)
            {
                decimal price = Convert.ToDecimal(item.Product.Product_Price);
                total += price * item.Quantity;

                order.OrderItems.Add(new OrderItem()
                {
                    Product_Id = item.Product_Id,
                    Quantity = item.Quantity,
                    Price = price
                });
            }

            order.TotalAmount = total;

            context.Orders.Add(order);
            context.Carts.RemoveRange(cartItems);

            context.SaveChanges();

            return RedirectToAction("Orders");
        }
        public IActionResult Orders()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            var orders = context.Orders
                .Where(o => o.User_Id == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToList();

            return View(orders);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}