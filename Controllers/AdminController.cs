using HamroDokaan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HamroDokaan.Controllers
{
    public class AdminController : Controller
    {
        private readonly HamroDokaanDbContext context;

        public AdminController(HamroDokaanDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Admin";
        }
        public IActionResult Dashboard()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }


            ViewBag.TotalUsers = context.Users.Count(u => u.IsApproved == true);


            ViewBag.PendingOrders = context.Orders
                .Count(o => o.Status == "Pending");


            ViewBag.CompletedOrders = context.Orders
                .Count(o => o.Status == "Completed");

            return View();
        }
        public IActionResult Users()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var users = context.Users.ToList();
            return View(users);
        }

        public IActionResult Approve(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var user = context.Users.Find(id);

            if (user != null)
            {
                user.IsApproved = true;
                context.SaveChanges();
            }

            return RedirectToAction("Users");
        }
        public IActionResult Reject(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var user = context.Users.Find(id);

            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }

            return RedirectToAction("Users");
        }
        public IActionResult Revert(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var user = context.Users.Find(id);

            if (user != null)
            {
                user.IsApproved = false;
                context.SaveChanges();
            }

            return RedirectToAction("Users");
        }


        public IActionResult Messages()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var messages = context.ContactMessages
                                  .OrderByDescending(m => m.Id)
                                  .ToList();

            return View(messages);
        }
        [HttpGet]
        public IActionResult DeleteMessages(int id)
        {
            var msg = context.ContactMessages.FirstOrDefault(m => m.Id == id);
            if (msg == null)
            {
                return NotFound();
            }
            return View(msg);
        }
        [HttpPost,ActionName("DeleteMessages")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(ContactMessage model) {
            var msg = context.ContactMessages.Find(model.Id);
            if( msg == null) {
                return NotFound();
            }
            context.ContactMessages.Remove(msg);
            context.SaveChanges();
            return RedirectToAction("Messages");
        }
        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            var prod = context.Products.FirstOrDefault(p => p.Product_Id == id);
            if (prod == null)
            {
                return NotFound();
            }
            return View(prod);

        }
        [HttpPost,ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProductConfirmed(int id)
        {
            var prod = context.Products.Find(id);
            if (prod == null) { return NotFound(); }
            context.Products.Remove(prod);
            context.SaveChanges();
            return RedirectToAction("Products");
        }
        [HttpGet]
        public IActionResult DeleteCategory(int id)
        {
            var cat = context.Categories.FirstOrDefault(p => p.Category_Id == id);
            if (cat == null)
            {
                return NotFound();
            }
            return View(cat);

        }
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategoryConfirmed(int id)
        {
            var cat = context.Categories.Find(id);
            if (cat == null) { return NotFound(); }
            context.Categories.Remove(cat);
            context.SaveChanges();
            return RedirectToAction("Categories");
        }

        public IActionResult Orders()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var orders = context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToList();

            return View(orders);
        }
        public IActionResult UpdateOrderStatus(int id, string status)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var order = context.Orders.Find(id);

            if (order != null)
            {
                order.Status = status;
                context.SaveChanges();
            }

            return RedirectToAction("Orders");
        }
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();

            return RedirectToAction("Categories");
        }
        public IActionResult CreateProduct()
        {
            ViewBag.Categories = context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Category_Id.ToString(),
                    Text = c.Category_Name
                }).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (product.ImageFile != null)
            {
                var fileName = Path.GetFileName(product.ImageFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    product.ImageFile.CopyTo(stream);
                }

                product.Product_img = fileName;
            }
            context.Products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Products");
        }
        public IActionResult Products()
        {
            var prod = context.Products.ToList();
            return View(prod);
        }
        public IActionResult Categories()
        {
            var catgry = context.Categories.ToList();
            return View(catgry);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}