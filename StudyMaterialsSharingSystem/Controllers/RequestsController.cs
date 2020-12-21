using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyMaterialsSharingSystem.Areas.Identity.Data;
using StudyMaterialsSharingSystem.Data;
using StudyMaterialsSharingSystem.Models;

namespace StudyMaterialsSharingSystem.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public RequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: Sent Request
        public async Task<IActionResult> Index()
        {
            var user = userManager.GetUserName(User);
            var request = _context.Replies.Where(s => s.Sender == user || s.Request.Receiver == user || s.Request.Sender == user)
              .OrderByDescending(s => s.dateTime).Include(i => i.Request)
              .GroupBy(x => x.RequestID).Select(y => y.First());
            return View(await request.AsNoTracking().OrderByDescending(s => s.dateTime).ToListAsync());
        }

        public async Task<IActionResult> Replies(string id)
        {
            var user = userManager.GetUserName(User);
            if (id != null)
            {
               var request = await _context.Requests
                        .Include(i => i.Replies)
                        .FirstOrDefaultAsync(m => m.ID == id);
                if (request.Replies.OrderByDescending(i => i.dateTime).First().Sender != user)
                {
                    request.Read = true;
                    _context.Requests.Update(request);
                    await _context.SaveChangesAsync();
                    return View(request);
                }
                return View(request);
            }          
           return NotFound();
        }

        [HttpPost, ActionName("Replies")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RepliesSave(string id, string message)
        {
            var user = userManager.GetUserName(User);
            if (id != null)
            {
                var reply = new Reply()
                {
                    RequestID = id,
                    Sender = user,
                    Message = message,
                    dateTime = DateTime.Now
                };
                var request = await _context.Requests
                 .FirstOrDefaultAsync(m => m.ID == id);
                request.dateTime = DateTime.Now;
                request.Read = false;
                _context.Replies.Add(reply);
                _context.Requests.Update(request);
                await _context.SaveChangesAsync();
   
                return RedirectToAction(nameof(Replies), new { id = id});
            }
            return NotFound();
        }
    }
}
 