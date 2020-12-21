using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyMaterialsSharingSystem.Areas.Identity.Data;
using StudyMaterialsSharingSystem.Authorization;
using StudyMaterialsSharingSystem.Data;
using StudyMaterialsSharingSystem.Models;
using StudyMaterialsSharingSystem.Models.AdminViewModels;

namespace StudyMaterialsSharingSystem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Books = _context.Books.Count();
            ViewBag.approvedBooks = _context.Books.Where(s => s.Status == BookStatus.Approved).Count();
            ViewBag.rejectedBooks = _context.Books.Where(s => s.Status == BookStatus.Rejected).Count();
            ViewBag.submittedBooks = _context.Books.Where(s => s.Status == BookStatus.Submitted).Count();
            ViewBag.BAPercent = (ViewBag.approvedBooks *100) / ViewBag.Books;
            ViewBag.BRPercent = (ViewBag.rejectedBooks *100) / ViewBag.Books;
            ViewBag.BSPercent = (ViewBag.submittedBooks *100) / ViewBag.Books;

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

            ViewBag.appUsers = userManager.Users.Count();
            return View();
        }

        #region Application User (Creat, Details, Update, Delete)
         
        public async Task<IActionResult> AppUser()
        {
            var admins = await userManager.GetUsersInRoleAsync(Constants.AdministratorRole);
            var managers = await userManager.GetUsersInRoleAsync(Constants.ManagerRole);
            var users = await userManager.GetUsersInRoleAsync(Constants.UserRole);
            var appUser = new ApplicationUserViewModel()
            {
                Administrators = admins,
                Managers = managers,
                Users = users
            };
           return View(appUser);
        }

        public IActionResult AddUser()
        {
            var model = new UserViewModel();
            model.Roles = roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name = model.Name,
                    DOB = model.DOB,
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = model.IsEmailConfirmed,
                    PhoneNumber = model.Phone,
                };
                 var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var applicationRole = await roleManager.FindByIdAsync(model.RoleId);
                    if (applicationRole != null)
                    {
                        IdentityResult roleResult = await userManager.AddToRoleAsync(user, applicationRole.Name);
                        if (roleResult.Succeeded)
                        {
                            return RedirectToAction("AppUser");
                        }
                    }
                }
           
            }
            return View(model);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            UserViewModel model = new UserViewModel();
            model.Roles = roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            if (!String.IsNullOrEmpty(id))
            {
                var user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                    model.Name = user.Name;
                    model.DOB = user.DOB;
                    model.UserName = user.UserName;
                    model.Email = user.Email;
                    model.IsEmailConfirmed = user.EmailConfirmed;
                    model.Phone = user.PhoneNumber;
                    model.RoleId = roleManager.Roles.Single(r => r.Name == userManager.GetRolesAsync(user).Result.Single()).Id;
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.Name = model.Name;
                    user.DOB = model.DOB;
                    user.UserName = model.UserName;
                    user.UserName = model.Email;
                    user.EmailConfirmed = model.IsEmailConfirmed;
                    user.PhoneNumber = model.Phone;
                    var existingRole = userManager.GetRolesAsync(user).Result.Single();
                    string existingRoleId = roleManager.Roles.Single(r => r.Name == existingRole).Id;
                    var modelRole = await roleManager.FindByIdAsync(model.RoleId);
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {

                            var roleResult = await userManager.RemoveFromRoleAsync(user, existingRole);
                            var record = userManager.GetRolesAsync(user);
                            if (roleResult.Succeeded && record != null)
                            {
                                var Result = await userManager.AddToRoleAsync(user, modelRole.Name);
                                return RedirectToAction("AppUser");
                            }
                    }
                }
            }
            return View(model);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationUser applicationUser = await userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(applicationUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("AppUser");
                    }
                }
            }
            return RedirectToAction("AppUser");
        }
        #endregion

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

        // POST: Feedbacks/Delete/
        [HttpPost, ActionName("DeleteFeedBack")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFeedBack(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var feedback = await _context.Feedbacks.FindAsync(id);
            _context.Remove(feedback);
            await _context.SaveChangesAsync();
             return RedirectToAction(nameof(Feedbacks));
            }
            return RedirectToAction(nameof(Feedbacks));
        }
    }
}