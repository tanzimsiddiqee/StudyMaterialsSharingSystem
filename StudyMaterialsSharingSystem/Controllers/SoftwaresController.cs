using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyMaterialsSharingSystem.Areas.Identity.Data;
using StudyMaterialsSharingSystem.Authorization;
using StudyMaterialsSharingSystem.Data;
using StudyMaterialsSharingSystem.Models;
using StudyMaterialsSharingSystem.Models.SoftwareViewModels;

namespace StudyMaterialsSharingSystem.Controllers
{
    public class SoftwaresController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;

        public SoftwaresController(
            UserManager<ApplicationUser> userManager, 
            IAuthorizationService authorizationService, 
            ApplicationDbContext context, IHostingEnvironment env)
        {
            _userManager = userManager;
            _authorizationService = authorizationService;
            _context = context;
            _env = env;
        }

        [AllowAnonymous]
        // GET: Softwares
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["TypeSortParm"] = sortOrder == "Type" ? "Type_desc" : "Type";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var softwares = _context.Softwares
                .Include(s => s.SoftwareType)
                .Where(s => s.Status == SoftStatus.Approved);

            if (!String.IsNullOrEmpty(searchString))
            {
                softwares = softwares.Where(s => s.SoftwareName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.SoftwareType.TypeName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    softwares = softwares.OrderBy(s => s.SoftwareName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    break;
                case "Name_desc":
                    softwares = softwares.OrderByDescending(s => s.SoftwareName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    break;
                case "Type":
                    softwares = softwares.OrderBy(s => s.SoftwareTypeID);
                    ViewData["SortStatus"] = ": Category (A-Z)";
                    ViewData["ChangeSort2"] = "(Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Type_desc":
                    softwares = softwares.OrderByDescending(s => s.SoftwareTypeID);
                    ViewData["SortStatus"] = ": Category (Z-A)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    softwares = softwares.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<Software>.CreateAsync(softwares.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> MySoftwares(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["TypeSortParm"] = sortOrder == "Type" ? "Type_desc" : "Type";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var currentUserId = _userManager.GetUserId(User);
            var softwares = _context.Softwares.Include(s => s.SoftwareType).Where(s => s.OwnerID == currentUserId);

            if (!String.IsNullOrEmpty(searchString))
            {
                softwares = softwares.Where(s => s.SoftwareName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.SoftwareType.TypeName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    softwares = softwares.OrderBy(s => s.SoftwareName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    break;
                case "Name_desc":
                    softwares = softwares.OrderByDescending(s => s.SoftwareName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    break;
                case "Type":
                    softwares = softwares.OrderBy(s => s.SoftwareTypeID);
                    ViewData["SortStatus"] = ": Category (A-Z)";
                    ViewData["ChangeSort2"] = "(Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Type_desc":
                    softwares = softwares.OrderByDescending(s => s.SoftwareTypeID);
                    ViewData["SortStatus"] = ": Category (Z-A)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    softwares = softwares.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<Software>.CreateAsync(softwares.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Manager, Administrator")]
        // GET: Submitted Books
        public async Task<IActionResult> Submitted(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["TypeSortParm"] = sortOrder == "Type" ? "Type_desc" : "Type";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var softwares = _context.Softwares
                .Include(s => s.SoftwareType)
                .Where(s => s.Status == SoftStatus.Submitted);

            if (!String.IsNullOrEmpty(searchString))
            {
                softwares = softwares.Where(s => s.SoftwareName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.SoftwareType.TypeName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    softwares = softwares.OrderBy(s => s.SoftwareName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    break;
                case "Name_desc":
                    softwares = softwares.OrderByDescending(s => s.SoftwareName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    break;
                case "Type":
                    softwares = softwares.OrderBy(s => s.SoftwareTypeID);
                    ViewData["SortStatus"] = ": Category (A-Z)";
                    ViewData["ChangeSort2"] = "(Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Type_desc":
                    softwares = softwares.OrderByDescending(s => s.SoftwareTypeID);
                    ViewData["SortStatus"] = ": Category (Z-A)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    softwares = softwares.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<Software>.CreateAsync(softwares.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Manager, Administrator")]
        // GET: Rejected Books
        public async Task<IActionResult> Rejected(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["TypeSortParm"] = sortOrder == "Type" ? "Type_desc" : "Type";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var softwares = _context.Softwares
                .Include(s => s.SoftwareType)
                .Where(s => s.Status == SoftStatus.Rejected);

            if (!String.IsNullOrEmpty(searchString))
            {
                softwares = softwares.Where(s => s.SoftwareName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.SoftwareType.TypeName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    softwares = softwares.OrderBy(s => s.SoftwareName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    break;
                case "Name_desc":
                    softwares = softwares.OrderByDescending(s => s.SoftwareName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    break;
                case "Type":
                    softwares = softwares.OrderBy(s => s.SoftwareTypeID);
                    ViewData["SortStatus"] = ": Category (A-Z)";
                    ViewData["ChangeSort2"] = "(Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Type_desc":
                    softwares = softwares.OrderByDescending(s => s.SoftwareTypeID);
                    ViewData["SortStatus"] = ": Category (Z-A)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    softwares = softwares.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(A-Z)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<Software>.CreateAsync(softwares.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Softwares/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await _context.Softwares
                .Include(s => s.SoftwareType)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.SoftwareID == id);
            if (software == null)
            {
                return NotFound();
            }

            var isAuthorized = User.IsInRole(Constants.UserRole) ||
                           User.IsInRole(Constants.ManagerRole) ||
                           User.IsInRole(Constants.AdministratorRole);
            var currentUserId = _userManager.GetUserId(User);
            if (!isAuthorized
            && software.Status != SoftStatus.Approved)
            {
                return new ChallengeResult();
            }

            return View(software);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(string id, SoftStatus status)
        {
            var software = await _context.Softwares
                .Include(d => d.SoftwareType)
                .FirstOrDefaultAsync(m => m.SoftwareID == id);
            if (software == null)
            {
                return NotFound();
            }
            var softwareOperation = (status == SoftStatus.Approved)
                                                   ? Operations.Approve
                                                   : Operations.Reject;

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, software,
                                     softwareOperation);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            software.Status = status;
            _context.Update(software);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Submitted));
        }

         [Authorize(Roles = "Manager, Administrator")]
        // GET: Softwares/Create
        public IActionResult Add()
        {
            ViewData["SoftwareTypeID"] = new SelectList(_context.SoftwareTypes, "TypeID", "TypeName");
            return View();
        }

        // POST: Softwares/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(SoftwareInfoViewModel software)
        {
            if (ModelState.IsValid)
            {
                software.SoftwareID = "Soft" + DateTime.Now.ToString("yyMMddhhmmssffffff");
                var model = new SoftwareInfoViewModel2()
                {
                    SoftwareInfo = software
                };
                return View("AddConfirmed", model);
            }
            return View(software);
        }

        // POST: Softwares/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddConfirmed(SoftwareInfoViewModel2 model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                if (model.ImageUoload != null)
                {
                    string extension = Path.GetExtension(model.ImageUoload.FileName);
                    string filename = model.SoftwareInfo.SoftwareID + extension;
                    Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "images", "softwares", model.SoftwareInfo.SoftwareID));
                    string filePath = Path.Combine(_env.WebRootPath, "images", "softwares", model.SoftwareInfo.SoftwareID, filename);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await (model.ImageUoload.CopyToAsync(stream));
                    }
                    model.ImagePath = "~/images/" + "softwares/" + model.SoftwareInfo.SoftwareID + "/" + filename;
                }
                else
                {
                    model.ImagePath = "~/images/softwares/software.png";
                }
                try
                {
                    var software = new Software()
                    {
                        SoftwareID = model.SoftwareInfo.SoftwareID,
                        SoftwareName = model.SoftwareInfo.SoftwareName,
                        Version = model.SoftwareInfo.Version,
                        Description = model.SoftwareInfo.Description,
                        ImagePath = model.ImagePath,
                        OwnerID = _userManager.GetUserId(User),
                        DownloadAddress = model.DownloadAddress,
                        DownloadingProcess = model.DownloadingProcess,
                        InstallingProcess = model.InstallingProcess,
                        AdTime = DateTime.Now,
                        SoftwareTypeID = model.SoftwareInfo.SoftwareTypeID
                    };

                    var isAuthorized = await _authorizationService.AuthorizeAsync(User, software, Operations.Create);
                    if (!isAuthorized.Succeeded)
                    {
                        return new ChallengeResult();
                    }
                    _context.Add(software);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MySoftwares));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator.");
                }              
            }
            return View(model);                 
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ob = await _context.Softwares.FindAsync(id);
            if (ob == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                  User, ob,
                                                  Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            var software = new SoftwareInfoViewModel()
            {
                SoftwareID = id,
                SoftwareName = ob.SoftwareName,
                SoftwareTypeID = ob.SoftwareTypeID,
                Version = ob.Version,
                Description = ob.Description
            };
            ViewData["SoftwareTypeID"] = new SelectList(_context.SoftwareTypes, "TypeID", "TypeName", ob.SoftwareTypeID);
            return View(software);
        }

        // POST: Softwares/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SoftwareInfoViewModel software)
        {
            var ob = await _context.Softwares.FindAsync(software.SoftwareID);

            if (ob == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                 User, ob,
                                                 Operations.Update);
                if (!isAuthorized.Succeeded)
                {
                    return new ChallengeResult();
                }

                var model = new SoftwareInfoViewModel2()
                {
                    ImagePath = ob.ImagePath,
                    OwnerID = ob.OwnerID,
                    DownloadAddress = ob.DownloadAddress,
                    DownloadingProcess = ob.DownloadingProcess,
                    InstallingProcess = ob.InstallingProcess,
                    SoftwareInfo = software
                };
                return View("EditConfirmed", model);
            }
            return View(software);
        }


        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(SoftwareInfoViewModel2 model)
        {
            var software = await _context.Softwares.FindAsync(model.SoftwareInfo.SoftwareID);

            if (software == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var isAuthorized = await _authorizationService.AuthorizeAsync(
                                 User, software,
                                 Operations.Update);
                if (!isAuthorized.Succeeded)
                {
                    return new ChallengeResult();
                }
                if (model.ImageUoload != null)
                {
                    string extension = Path.GetExtension(model.ImageUoload.FileName);
                    string filename = model.SoftwareInfo.SoftwareID + extension;
                    Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "images", "softwares", model.SoftwareInfo.SoftwareID));
                    string filePath = Path.Combine(_env.WebRootPath, "images", "softwares", model.SoftwareInfo.SoftwareID, filename);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await (model.ImageUoload.CopyToAsync(stream));
                    }
                    model.ImagePath = "~/images/" + "softwares/" + model.SoftwareInfo.SoftwareID + "/" + filename;
                }
                try
                {
                    software.SoftwareID = model.SoftwareInfo.SoftwareID;
                    software.SoftwareName = model.SoftwareInfo.SoftwareName;
                    software.Version = model.SoftwareInfo.Version;
                    software.Description = model.SoftwareInfo.Description;
                    software.ImagePath = model.ImagePath;
                    software.OwnerID = model.OwnerID;
                    software.DownloadAddress = model.DownloadAddress;
                    software.DownloadingProcess = model.DownloadingProcess;
                    software.InstallingProcess = model.InstallingProcess;
                    software.AdTime = DateTime.Now;

                    if (software.Status == SoftStatus.Approved)
                    {
                        // If the contact is updated after approval, 
                        // and the user cannot approve,
                        // set the status back to submitted so the update can be
                        // checked and approved.
                        var canApprove = await _authorizationService.AuthorizeAsync(User,
                                                software,
                                                Operations.Approve);

                        if (!canApprove.Succeeded)
                        {
                            software.Status = SoftStatus.Submitted;
                        }
                    }
                    _context.Update(software);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoftwareExists(model.SoftwareInfo.SoftwareID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MySoftwares));
            }
            return View(model);
        }


        // GET: Softwares/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await _context.Softwares
                .AsNoTracking()
                .Include(s => s.SoftwareType)
                .FirstOrDefaultAsync(m => m.SoftwareID == id);
            if (software == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, software, Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            return View(software);
        }

        // POST: Softwares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var software = await _context.Softwares.FindAsync(id);

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, software, Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }
            string path = Path.Combine(_env.WebRootPath, "images", "softwares", software.SoftwareID);
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    _context.Softwares.Remove(software);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MySoftwares));
                }
                _context.Softwares.Remove(software);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MySoftwares));

            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool SoftwareExists(string id)
        {
            return _context.Softwares.Any(e => e.SoftwareID == id);
        }
    }
}
