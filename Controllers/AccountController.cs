using HamroDokaan.Models;
using Microsoft.AspNetCore.Mvc;

namespace HamroDokaan.Controllers
{
    public class AccountController : Controller
    {
        private readonly HamroDokaanDbContext context;
        public AccountController(HamroDokaanDbContext context) {
            this.context = context;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
      public IActionResult Register(User user) {
            if (ModelState.IsValid)
            {
                user.IsApproved = false;
                user.User_Role = "Customer";
                context.Users.Add(user);
                context.SaveChanges();
                TempData["Message"] = "Registration completed,Wait for approval";
                return RedirectToAction("Register");
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            if (user.User_Email == "spandandhungel8@gmail.com" &&
                user.User_Password == "StrongPass123!")
            {
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetString("Email", user.User_Email);
                return RedirectToAction("Dashboard", "Admin");
            }

            var usr = context.Users
                .FirstOrDefault(u => u.User_Email == user.User_Email &&
                                     u.User_Password == user.User_Password);

            if (usr == null)
            {
                ViewBag.Error = "Invalid Credentials";
                return View();
            }

            if (!usr.IsApproved)
            {
                ViewBag.Error = "Your account is not approved yet";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", usr.User_Id);

            HttpContext.Session.SetString("Role", usr.User_Role);
            HttpContext.Session.SetString("Email", usr.User_Email);

            return RedirectToAction("Products", "Customer");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");

        }
    }
}
