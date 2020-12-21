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
using StudyMaterialsSharingSystem.Models.BookViewModels;

namespace StudyMaterialsSharingSystem.Controllers
{
    public class BooksController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;

        public BooksController(
            UserManager<ApplicationUser> userManager, 
            IAuthorizationService authorizationService, 
            ApplicationDbContext context, IHostingEnvironment env)
        {
            _userManager = userManager;
            _authorizationService = authorizationService;
            _context = context;
            _env = env;
        }

        //GET: Cascading DropDownList JsonResult
        public JsonResult getSubCategory(int id)
        {
            IList<SubCategory> subCategorylist = _context.SubCategories.Where(p => p.CategoryID == id).ToList();
            subCategorylist.Insert(0, new SubCategory { SubCategoryID = 0, SubCategoryName = "Select" });
            return Json(new SelectList(subCategorylist, "SubCategoryID", "SubCategoryName"));
        }

        #region Book Infromation (Create, Read, Update & Delete)
        // GET: Books
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var books = _context.Books
                .Include(b => b.SubCategory)
                .Include(b => b.SubCategory.Category)
                .Where(s => s.Status == BookStatus.Approved);

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.BookName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.Location.Contains(searchString)
                || s.SubCategory.SubCategoryName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    books = books.OrderBy(s => s.BookName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Name_desc":
                    books = books.OrderByDescending(s => s.BookName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Price":
                    books = books.OrderBy(s => s.Price);
                    ViewData["SortStatus"] = ": Price (Low-High)";
                    ViewData["ChangeSort2"] = "(High-Low)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Price_desc":
                    books = books.OrderByDescending(s => s.Price);
                    ViewData["SortStatus"] = ": Price (High-Low)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    ViewData["ChangeSort1"] = "(A-Z)"; 
                    break;
                default:
                    books = books.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<Book>.CreateAsync(books.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: User Own Add Books
        public async Task<IActionResult> MyBooks(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";
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
            var books = _context.Books
                .Include(b => b.SubCategory)
                .Include(b => b.SubCategory.Category)
                .Where(s => s.OwnerID == currentUserId);

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.BookName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.Location.Contains(searchString)
                || s.SubCategory.SubCategoryName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    books = books.OrderBy(s => s.BookName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Name_desc":
                    books = books.OrderByDescending(s => s.BookName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Price":
                    books = books.OrderBy(s => s.Price);
                    ViewData["SortStatus"] = ": Price (Low-High)";
                    ViewData["ChangeSort2"] = "(High-Low)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Price_desc":
                    books = books.OrderByDescending(s => s.Price);
                    ViewData["SortStatus"] = ": Price (High-Low)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    books = books.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<Book>.CreateAsync(books.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Manager, Administrator")]
        // GET: Submitted Books
        public async Task<IActionResult> Submitted(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var books = _context.Books
                .Include(b => b.SubCategory)
                .Include(b => b.SubCategory.Category)
                .Where(s => s.Status == BookStatus.Submitted);

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.BookName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.Location.Contains(searchString)
                || s.SubCategory.SubCategoryName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    books = books.OrderBy(s => s.BookName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Name_desc":
                    books = books.OrderByDescending(s => s.BookName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Price":
                    books = books.OrderBy(s => s.Price);
                    ViewData["SortStatus"] = ": Price (Low-High)";
                    ViewData["ChangeSort2"] = "(High-Low)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Price_desc":
                    books = books.OrderByDescending(s => s.Price);
                    ViewData["SortStatus"] = ": Price (High-Low)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    books = books.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<Book>.CreateAsync(books.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Manager, Administrator")]
        // GET: Rejected Books
        public async Task<IActionResult> Rejected(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var books = _context.Books
                .Include(b => b.SubCategory)
                .Include(b => b.SubCategory.Category)
                .Where(s => s.Status == BookStatus.Rejected);

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.BookName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.Location.Contains(searchString)
                || s.SubCategory.SubCategoryName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    books = books.OrderBy(s => s.BookName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Name_desc":
                    books = books.OrderByDescending(s => s.BookName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Price":
                    books = books.OrderBy(s => s.Price);
                    ViewData["SortStatus"] = ": Price (Low-High)";
                    ViewData["ChangeSort2"] = "(High-Low)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Price_desc":
                    books = books.OrderByDescending(s => s.Price);
                    ViewData["SortStatus"] = ": Price (High-Low)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    books = books.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<Book>.CreateAsync(books.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.SubCategory)
                .Include(b => b.SubCategory.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            var isAuthorized = User.IsInRole(Constants.UserRole) ||
                           User.IsInRole(Constants.ManagerRole) ||
                           User.IsInRole(Constants.AdministratorRole);
            var currentUserId = _userManager.GetUserId(User);
            if (!isAuthorized
            && book.Status != BookStatus.Approved)
            {
                return new ChallengeResult();
            }

            return View(book);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(string id, BookStatus status)
        {
            var book = await _context.Books
                .Include(d => d.SubCategory)
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }
            var bookOperation = (status == BookStatus.Approved)
                                                   ? Operations.Approve
                                                   : Operations.Reject;

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, book,
                                     bookOperation);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            book.Status = status;
            _context.Update(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Submitted));
        }

        // GET: Books/Create
        public IActionResult Add()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            ViewData["SubCategoryID"] = new SelectList(string.Empty,"SubCategoryID", "SubCategoryName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(BookInfoViewModel book)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                book.BookID = "Book" + DateTime.Now.ToString("yyMMddhhmmssffffff");
                var model = new BookInfoViewModel2()
                {
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    BookInfo = book
                };
                return View("AddConfirmed", model);
            }
            return View(book);
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddConfirmed(BookInfoViewModel2 model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageUoload != null)
                {
                    string extension = Path.GetExtension(model.ImageUoload.FileName);
                    string filename = model.BookInfo.BookID + extension;
                    Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "images", "books", model.BookInfo.BookID));
                    string filePath = Path.Combine(_env.WebRootPath, "images", "books", model.BookInfo.BookID, filename);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await (model.ImageUoload.CopyToAsync(stream));
                    }
                    model.ImagePath = "~/images/" + "books/" + model.BookInfo.BookID + "/" + filename;                    
                }
                else
                {
                    model.ImagePath = "~/images/books/book.jpg";
                }
                try
                {
                    var book = new Book
                    {
                        BookID = model.BookInfo.BookID,
                        BookName = model.BookInfo.BookName,
                        Condition = model.BookInfo.Condition,
                        Price = model.BookInfo.Price,
                        PriceType = model.BookInfo.PriceType,
                        Description = model.BookInfo.Description,
                        OwnerID = _userManager.GetUserId(User),
                        Email = model.Email,
                        Phone = model.Phone,
                        Location = model.Location,
                        AdTime = DateTime.Now,
                        SubCategoryID = model.BookInfo.SubCategoryID,
                        ImagePath = model.ImagePath
                    };
                    var isAuthorized = await _authorizationService.AuthorizeAsync(User, book, Operations.Create);
                    if (!isAuthorized.Succeeded)
                    {
                        return new ChallengeResult();
                    }

                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MyBooks));

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

        //GET: Books/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ob = await _context.Books.FindAsync(id);
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
            var book = new BookInfoViewModel()
            {
                BookID = id,
                BookName = ob.BookName,
                SubCategoryID = ob.SubCategoryID,
                Condition = ob.Condition,
                Price = ob.Price,
                Description = ob.Description,
            };
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            ViewData["SubCategoryID"] = new SelectList(string.Empty, "SubCategoryID", "SubCategoryName", ob.SubCategoryID);
            return View(book);
        }

        //// POST: Books/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookInfoViewModel book)
        {
            var ob = await _context.Books.FindAsync(book.BookID);
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
                var model = new BookInfoViewModel2()
                {
                    ImagePath = ob.ImagePath,
                    OwnerID = ob.OwnerID,
                    Email = ob.Email,
                    Phone = ob.Phone,
                    Location = ob.Location,
                    BookInfo = book
                };
                return View("EditConfirmed", model);
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(BookInfoViewModel2 model)
        {
            var book = await _context.Books.FindAsync(model.BookInfo.BookID);
            if (book == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var isAuthorized = await _authorizationService.AuthorizeAsync(
                                 User, book,
                                 Operations.Update);
                if (!isAuthorized.Succeeded)
                {
                    return new ChallengeResult();
                }
                if (model.ImageUoload != null)
                {
                    string extension = Path.GetExtension(model.ImageUoload.FileName);
                    string filename = model.BookInfo.BookID + extension;
                    Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "images", "books", model.BookInfo.BookID));
                    string filePath = Path.Combine(_env.WebRootPath, "images", "books", model.BookInfo.BookID, filename);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await (model.ImageUoload.CopyToAsync(stream));
                    }
                    model.ImagePath = "~/images/" + "books/" + model.BookInfo.BookID + "/" + filename;
                }
                try
                {
                    book.BookID = model.BookInfo.BookID;
                    book.BookName = model.BookInfo.BookName;
                    book.Condition = model.BookInfo.Condition;
                    book.OwnerID = model.OwnerID;
                    book.Email = model.Email;
                    book.PriceType = model.BookInfo.PriceType;
                    book.Price = model.BookInfo.Price;
                    book.Phone = model.Phone;
                    book.Location = model.Location;
                    book.AdTime = DateTime.Now;
                    book.SubCategoryID = model.BookInfo.SubCategoryID;
                    book.ImagePath = model.ImagePath;

                    if (book.Status == BookStatus.Approved)
                    {
                        // If the contact is updated after approval, 
                        // and the user cannot approve,
                        // set the status back to submitted so the update can be
                        // checked and approved.
                        var canApprove = await _authorizationService.AuthorizeAsync(User,
                                                book,
                                                Operations.Approve);

                        if (!canApprove.Succeeded)
                        {
                            book.Status = BookStatus.Submitted;
                        }
                    }

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(model.BookInfo.BookID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyBooks));
            }
            return View(model);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.SubCategory)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, book, Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var book = await _context.Books.FindAsync(id);

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, book, Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }
            string path = Path.Combine(_env.WebRootPath, "images", "books", book.BookID);
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MyBooks));
                }
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyBooks));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }      
        }

        private bool BookExists(string id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }
        #endregion

        // Post: Requests/Sent    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sent(string id, string receiver, string message)
        {
            var book = await _context.Books.FindAsync(id);
            var user = await _userManager.FindByIdAsync(receiver);
            if (ModelState.IsValid)
            {
                var request = new Request()
                {
                    MaterialID = book.BookID,
                    Material = book.BookName,
                    Sender = _userManager.GetUserName(User),
                    Receiver = user.UserName,
                    Read = false,
                    dateTime = DateTime.Now
                };
                _context.Requests.Add(request);

                var reply = new Reply()
                {
                    RequestID = request.ID,
                    Sender = _userManager.GetUserName(User),
                    Message = message,
                    dateTime = DateTime.Now
                };
                _context.Replies.Add(reply);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = book.BookID });
            }
            return RedirectToAction(nameof(Details), new { id = book.BookID });
        }
    }
}
