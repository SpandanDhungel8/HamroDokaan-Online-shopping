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
        public IActionResult Categories()
        {
            if (HttpContext.Session.GetString("Role") != "Customer")
            {
                return RedirectToAction("Login", "Account");
            }
            var category = context.Categories.ToList();
            return View(category);
        }
        public IActionResult Products(int? id)
        {
            List<Product> products;

            if (id == null)
            {
            
                products = context.Products.ToList();
            }
            else
            {
                
                products = context.Products
                                  .Where(p => p.Category_Id == id)
                                  .ToList();
            }

            return View(products);
        }
        public IActionResult AddToCart(int id)
        {
            
            var existingItem = context.Carts
                                      .FirstOrDefault(c => c.Product_Id == id);

            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {
                Cart cart = new Cart()
                {
                    Product_Id = id,
                    Quantity = 1
                };

                context.Carts.Add(cart);
            }

            context.SaveChanges();
            TempData["Added"] = "Product added to cart!";
            return RedirectToAction("Products");

            
        }
        public IActionResult Cart()
        {
            var cartItems = context.Carts
                                   .Include(c => c.Product)
                                   .ToList();

            return View(cartItems);
        }
        public IActionResult RemoveFromCart(int id)
        {
            var item = context.Carts.Find(id);

            if (item != null)
            {
                context.Carts.Remove(item);
                context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }
        public IActionResult IncreaseQty(int id)
        {
            var item = context.Carts.Find(id);

            if (item != null)
            {
                item.Quantity += 1;
                context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

        public IActionResult DecreaseQty(int id)
        {
            var item = context.Carts.Find(id);

            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity -= 1;
                }
                else
                {
                    context.Carts.Remove(item);
                }

                context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");

        }
    }

}
