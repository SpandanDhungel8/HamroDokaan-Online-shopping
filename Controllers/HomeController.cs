using HamroDokaan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using System.Diagnostics;

namespace HamroDokaan.Controllers
{
    public class HomeController : Controller
    {
        private readonly HamroDokaanDbContext context;
        public HomeController(HamroDokaanDbContext context) {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs() {
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(ContactMessage contactmessage)
        {
            if (ModelState.IsValid)
            {
                context.ContactMessages.Add(contactmessage);
                context.SaveChanges();
                TempData["Message"] = "Message Sent Successfully";
                return RedirectToAction("ContactUs");
            }
            return View(contactmessage);
          
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
