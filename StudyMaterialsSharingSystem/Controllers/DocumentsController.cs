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
using StudyMaterialsSharingSystem.Authorization;
using StudyMaterialsSharingSystem.Data;
using StudyMaterialsSharingSystem.Models;
using StudyMaterialsSharingSystem.Models.DocViewModels;

namespace StudyMaterialsSharingSystem.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly ApplicationDbContext _context;

        public DocumentsController(
            UserManager<ApplicationUser> userManager, 
            IAuthorizationService authorizationService, 
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _authorizationService = authorizationService;
            _context = context;
        }

        [AllowAnonymous]
        // GET: Documents
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["FormatSortParm"] = sortOrder == "Format" ? "Format_desc" : "Format";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var documents = _context.Documents
                .Where(s => s.Status == DocumentStatus.Approved);

            if (!String.IsNullOrEmpty(searchString))
            {
                documents = documents.Where(s => s.DocumentName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.DocumentFormat.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    documents = documents.OrderBy(s => s.DocumentName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Name_desc":
                    documents = documents.OrderByDescending(s => s.DocumentName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Format":
                    documents = documents.OrderBy(s => s.DocumentFormat);
                    ViewData["SortStatus"] = ": Format (Low-High)";
                    ViewData["ChangeSort2"] = "(High-Low)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Format_desc":
                    documents = documents.OrderByDescending(s => s.DocumentFormat);
                    ViewData["SortStatus"] = ": Format (High-Low)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    documents = documents.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<Document>.CreateAsync(documents.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> MyDocuments(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["FormatSortParm"] = sortOrder == "Format" ? "Format_desc" : "Format";
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
            var documents = _context.Documents
                .Where(s => s.OwnerID == currentUserId);

            if (!String.IsNullOrEmpty(searchString))
            {
                documents = documents.Where(s => s.DocumentName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.DocumentFormat.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    documents = documents.OrderBy(s => s.DocumentName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Name_desc":
                    documents = documents.OrderByDescending(s => s.DocumentName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Format":
                    documents = documents.OrderBy(s => s.DocumentFormat);
                    ViewData["SortStatus"] = ": Format (Low-High)";
                    ViewData["ChangeSort2"] = "(High-Low)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Format_desc":
                    documents = documents.OrderByDescending(s => s.DocumentFormat);
                    ViewData["SortStatus"] = ": Format (High-Low)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    documents = documents.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<Document>.CreateAsync(documents.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Manager, Administrator")]
        // GET: Submitted Books
        public async Task<IActionResult> Submitted(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["FormatSortParm"] = sortOrder == "Format" ? "Format_desc" : "Format";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var documents = _context.Documents
                .Where(s => s.Status == DocumentStatus.Submitted);

            if (!String.IsNullOrEmpty(searchString))
            {
                documents = documents.Where(s => s.DocumentName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.DocumentFormat.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    documents = documents.OrderBy(s => s.DocumentName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Name_desc":
                    documents = documents.OrderByDescending(s => s.DocumentName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Format":
                    documents = documents.OrderBy(s => s.DocumentFormat);
                    ViewData["SortStatus"] = ": Format (Low-High)";
                    ViewData["ChangeSort2"] = "(High-Low)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Format_desc":
                    documents = documents.OrderByDescending(s => s.DocumentFormat);
                    ViewData["SortStatus"] = ": Format (High-Low)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    documents = documents.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<Document>.CreateAsync(documents.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Manager, Administrator")]
        // GET: Rejected Books
        public async Task<IActionResult> Rejected(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["FormatSortParm"] = sortOrder == "Format" ? "Format_desc" : "Format";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var documents = _context.Documents
                .Where(s => s.Status == DocumentStatus.Rejected);

            if (!String.IsNullOrEmpty(searchString))
            {
                documents = documents.Where(s => s.DocumentName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.DocumentFormat.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    documents = documents.OrderBy(s => s.DocumentName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Name_desc":
                    documents = documents.OrderByDescending(s => s.DocumentName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Format":
                    documents = documents.OrderBy(s => s.DocumentFormat);
                    ViewData["SortStatus"] = ": Format (Low-High)";
                    ViewData["ChangeSort2"] = "(High-Low)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Format_desc":
                    documents = documents.OrderByDescending(s => s.DocumentFormat);
                    ViewData["SortStatus"] = ": Format (High-Low)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    documents = documents.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<Document>.CreateAsync(documents.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DocumentID == id);
            if (document == null)
            {
                return NotFound();
            }

            var isAuthorized = User.IsInRole(Constants.UserRole) || 
                           User.IsInRole(Constants.ManagerRole) ||
                           User.IsInRole(Constants.AdministratorRole);
            var currentUserId = _userManager.GetUserId(User);
            if (!isAuthorized
            && document.Status != DocumentStatus.Approved)
            {
                return new ChallengeResult();
            }

            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(string id, DocumentStatus status)
        {
            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.DocumentID == id);
            if (document == null)
            {
                return NotFound();
            }
            var documentOperation = (status == DocumentStatus.Approved)
                                                   ? Operations.Approve
                                                   : Operations.Reject;

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, document,
                                     documentOperation);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            document.Status = status;
            _context.Update(document);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Documents/Create
        public IActionResult Add()
        {
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(DocInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
               if (model.DocumentFormat == "PDF")
                {
                    model.ImagePath = "~/images/documents/pdf.png";
                }
                else if (model.DocumentFormat == "DOC")
                {
                    model.ImagePath = "~/images/documents/doc.png";
                }
               else if (model.DocumentFormat == "PPT/PPTX")
                {
                    model.ImagePath = "~/images/documents/presentation.jpg";
                }
                else
                {
                    model.ImagePath = "~/images/documents/document.png";
                }
                try
                {
                    var document = new Document()
                    {
                        DocumentID = "Document" + DateTime.Now.ToString("yyMMddhhmmssffffff"),
                        DocumentName = model.DocumentName,
                        DocumentFormat = model.DocumentFormat,
                        Description = model.Description,
                        DownloadAddress = model.DownloadAddress,
                        OwnerID = _userManager.GetUserId(User),
                        ImagePath = model.ImagePath,
                        AdTime = DateTime.Now,
                    };
                    var isAuthorized = await _authorizationService.AuthorizeAsync(User, document, Operations.Create);
                    if (!isAuthorized.Succeeded)
                    {
                        return new ChallengeResult();
                    }
                    _context.Add(document);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MyDocuments));

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

            var ob = await _context.Documents.FindAsync(id);
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

            var model = new DocInfoViewModel()
            {
                DocumentID = id,
                DocumentName = ob.DocumentName,
                DocumentFormat = ob.DocumentFormat,
                Description = ob.Description,
                DownloadAddress = ob.DownloadAddress,
                OwnerID = ob.OwnerID
            };

            return View(model);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, DocInfoViewModel model)
        {
            if (id != model.DocumentID)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(model.DocumentID);

            if (document == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var isAuthorized = await _authorizationService.AuthorizeAsync(
                                 User, document,
                                 Operations.Update);
                if (!isAuthorized.Succeeded)
                {
                    return new ChallengeResult();
                }


                if (model.DocumentFormat == "PDF")
                {
                    model.ImagePath = "~/images/documents/pdf.png";
                }
                else if (model.DocumentFormat == "DOC")
                {
                    model.ImagePath = "~/images/documents/doc.png";
                }
                else if (model.DocumentFormat == "PPT/PPTX")
                {
                    model.ImagePath = "~/images/documents/presentation.jpg";
                }
                else
                {
                    model.ImagePath = "~/images/documents/document.png";
                }
                try
                {
                    document.DocumentID = model.DocumentID;
                    document.DocumentName = model.DocumentName;
                    document.DocumentFormat = model.DocumentFormat;
                    document.Description = model.Description;
                    document.DownloadAddress = model.DownloadAddress;
                    document.ImagePath = model.ImagePath;
                    document.OwnerID = model.OwnerID;                    
                    document.AdTime = DateTime.Now;

                    if (document.Status == DocumentStatus.Approved)
                    {
                        // If the contact is updated after approval, 
                        // and the user cannot approve,
                        // set the status back to submitted so the update can be
                        // checked and approved.
                        var canApprove = await _authorizationService.AuthorizeAsync(User,
                                                document,
                                                Operations.Approve);

                        if (!canApprove.Succeeded)
                        {
                            document.Status = DocumentStatus.Submitted;
                        }
                    }
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(model.DocumentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyDocuments));
            }
            return View(model);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DocumentID == id);
            if (document == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, document, Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var document = await _context.Documents.FindAsync(id);
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, document, Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }
            try
            {
                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyDocuments));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool DocumentExists(string id)
        {
            return _context.Documents.Any(e => e.DocumentID == id);
        }
    }
}
