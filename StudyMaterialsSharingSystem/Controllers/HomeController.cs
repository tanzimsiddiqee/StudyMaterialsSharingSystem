using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyMaterialsSharingSystem.Data;
using StudyMaterialsSharingSystem.Models;
using StudyMaterialsSharingSystem.Models.HomeViewModels;

namespace StudyMaterialsSharingSystem.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.books = _context.Books.Where(s => s.Status == BookStatus.Approved).Count();
            ViewBag.documents = _context.Documents.Where(s => s.Status == DocumentStatus.Approved).Count();
            ViewBag.houses = _context.Houses.Where(s => s.Status == HouseStatus.Approved).Count();
            ViewBag.softwares = _context.Softwares.Where(s => s.Status == SoftStatus.Approved).Count();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Search(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var search = new SearchViewModel();
            var books = _context.Books
                .Include(b => b.SubCategory)
                .Include(b => b.SubCategory.Category)
                .Where(s => s.Status == BookStatus.Approved);

            var houses = _context.Houses
               .Where(s => s.Status == HouseStatus.Approved);

            var documents = _context.Documents
               .Where(s => s.Status == DocumentStatus.Approved);

            var softwares = _context.Softwares
               .Include(b => b.SoftwareType)
               .Where(s => s.Status == SoftStatus.Approved);

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.BookName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.Location.Contains(searchString)
                || s.SubCategory.SubCategoryName.Contains(searchString));

                ViewBag.bCount = books.Count();

                houses = houses.Where(s => s.HouseName.Contains(searchString)
               || s.Description.Contains(searchString)
               || s.Location.Contains(searchString)
               || s.Category.Contains(searchString)
               || s.HouseType.Contains(searchString));

                ViewBag.hCount = houses.Count();

                softwares = softwares.Where(s => s.SoftwareName.Contains(searchString)
               || s.SoftwareType.TypeName.Contains(searchString)
               || s.Version.Contains(searchString));

                ViewBag.sCount = softwares.Count();

                documents = documents.Where(s => s.DocumentName.Contains(searchString)
               || s.DocumentFormat.Contains(searchString)
               || s.Description.Contains(searchString));

                ViewBag.dCount = documents.Count();
            }

            ViewBag.tCount = ViewBag.bCount+ ViewBag.hCount+ ViewBag.sCount+ ViewBag.dCount;

            var model = new SearchViewModel()
            {
                Books = books,
                Documents = documents,
                Softwares = softwares,
                Houses = houses
            };
            return View(model);
        }

        // Post: Fbackeed   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FeedBack(string email, string message, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var feedback = new Feedback()
            {
                User = email,
                Comment = message,
                dateTime = DateTime.Now
            };
            _context.Add(feedback);
            await _context.SaveChangesAsync();
            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
