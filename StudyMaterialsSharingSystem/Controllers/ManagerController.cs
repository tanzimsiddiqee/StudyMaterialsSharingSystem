using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyMaterialsSharingSystem.Data;
using StudyMaterialsSharingSystem.Models;

namespace StudyMaterialsSharingSystem.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Books = _context.Books.Count();
            ViewBag.approvedBooks = _context.Books.Where(s => s.Status == BookStatus.Approved).Count();
            ViewBag.rejectedBooks = _context.Books.Where(s => s.Status == BookStatus.Rejected).Count();
            ViewBag.submittedBooks = _context.Books.Where(s => s.Status == BookStatus.Submitted).Count();
            ViewBag.BAPercent = (ViewBag.approvedBooks * 100) / ViewBag.Books;
            ViewBag.BRPercent = (ViewBag.rejectedBooks * 100) / ViewBag.Books;
            ViewBag.BSPercent = (ViewBag.submittedBooks * 100) / ViewBag.Books;

            ViewBag.Documents = _context.Documents.Count();
            ViewBag.approvedDocuments = _context.Documents.Where(s => s.Status == DocumentStatus.Approved).Count();
            ViewBag.rejectedDocuments = _context.Documents.Where(s => s.Status == DocumentStatus.Rejected).Count();
            ViewBag.submittedDocuments = _context.Documents.Where(s => s.Status == DocumentStatus.Submitted).Count();
            ViewBag.DAPercent = (ViewBag.approvedDocuments * 100) / ViewBag.Documents;
            ViewBag.DRPercent = (ViewBag.rejectedDocuments * 100) / ViewBag.Documents;
            ViewBag.DSPercent = (ViewBag.submittedDocuments * 100) / ViewBag.Documents;

            ViewBag.Softwares = _context.Softwares.Count();
            ViewBag.approvedSoftwares = _context.Softwares.Where(s => s.Status == SoftStatus.Approved).Count();
            ViewBag.rejectedSoftwares = _context.Softwares.Where(s => s.Status == SoftStatus.Rejected).Count();
            ViewBag.submittedSoftwares = _context.Softwares.Where(s => s.Status == SoftStatus.Submitted).Count();
            ViewBag.SAPercent = (ViewBag.approvedSoftwares * 100) / ViewBag.Softwares;
            ViewBag.SRPercent = (ViewBag.rejectedSoftwares * 100) / ViewBag.Softwares;
            ViewBag.SSPercent = (ViewBag.submittedSoftwares * 100) / ViewBag.Softwares;

            ViewBag.Houses = _context.Houses.Count();
            ViewBag.approvedHouses = _context.Houses.Where(s => s.Status == HouseStatus.Approved).Count();
            ViewBag.rejectedHouses = _context.Houses.Where(s => s.Status == HouseStatus.Rejected).Count();
            ViewBag.submittedHouses = _context.Houses.Where(s => s.Status == HouseStatus.Submitted).Count();
            ViewBag.HAPercent = (ViewBag.approvedHouses * 100) / ViewBag.Houses;
            ViewBag.HRPercent = (ViewBag.rejectedHouses * 100) / ViewBag.Houses;
            ViewBag.HSPercent = (ViewBag.submittedHouses * 100) / ViewBag.Houses;

            ViewBag.total = ViewBag.Books + ViewBag.Documents + ViewBag.Softwares + ViewBag.Houses;
            ViewBag.BPercent = (ViewBag.Books * 100) / ViewBag.total;
            ViewBag.DPercent = (ViewBag.Documents * 100) / ViewBag.total;
            ViewBag.SPercent = (ViewBag.Softwares * 100) / ViewBag.total;
            ViewBag.HPercent = (ViewBag.Houses * 100) / ViewBag.total;
            return View();
        }

        // GET: Feedbacks
        public async Task<IActionResult> Feedbacks()
        {
            var feedbacks = _context.Feedbacks.OrderByDescending(s => s.dateTime);

            return View(await feedbacks.ToListAsync());
        }

        // Post: Feedback/ReadStatus    
        [HttpPost]
        public async Task<IActionResult> ReadFeedBack(string id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                feedback.Read = true;
                _context.Update(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Feedbacks));
            }
            return RedirectToAction(nameof(Feedbacks));
        }
    }
}
