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
using StudyMaterialsSharingSystem.Models.HouseViewModels;

namespace StudyMaterialsSharingSystem.Controllers
{
    public class HousesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;

        public HousesController(
            UserManager<ApplicationUser> userManager, 
            IAuthorizationService authorizationService, 
            ApplicationDbContext context, IHostingEnvironment env)
        {
            _userManager = userManager;
            _authorizationService = authorizationService;
            _context = context;
            _env = env;
        }

        #region House Information (Create, Read, Update & Delete)
        // GET: Houses
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["RentSortParm"] = sortOrder == "Rent" ? "Rent_desc" : "Rent";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var houses = _context.Houses
                .Where(s => s.Status == HouseStatus.Approved && s.dateTime >= DateTime.Now);

            if (!String.IsNullOrEmpty(searchString))
            {
                houses = houses.Where(s => s.HouseName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.Location.Contains(searchString)
                || s.Category.Contains(searchString)
                || s.HouseType.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    houses = houses.OrderBy(s => s.HouseName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Name_desc":
                    houses = houses.OrderByDescending(s => s.HouseName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Rent":
                    houses = houses.OrderBy(s => s.RentPerSeat);
                    ViewData["SortStatus"] = ": Rent (Low-High)";
                    ViewData["ChangeSort2"] = "(High-Low)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Rent_desc":
                    houses = houses.OrderByDescending(s => s.RentPerSeat);
                    ViewData["SortStatus"] = ": Rent (High-Low)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    houses = houses.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<House>.CreateAsync(houses.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> MyHouses(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["RentSortParm"] = sortOrder == "Rent" ? "Rent_desc" : "Rent";
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
            var houses = _context.Houses.Where(s => s.OwnerID == currentUserId);

            if (!String.IsNullOrEmpty(searchString))
            {
                houses = houses.Where(s => s.HouseName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.Location.Contains(searchString)
                || s.Category.Contains(searchString)
                || s.HouseType.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    houses = houses.OrderBy(s => s.HouseName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Name_desc":
                    houses = houses.OrderByDescending(s => s.HouseName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Rent":
                    houses = houses.OrderBy(s => s.RentPerSeat);
                    ViewData["SortStatus"] = ": Rent (Low-High)";
                    ViewData["ChangeSort2"] = "(High-Low)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Rent_desc":
                    houses = houses.OrderByDescending(s => s.RentPerSeat);
                    ViewData["SortStatus"] = ": Rent (High-Low)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    houses = houses.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<House>.CreateAsync(houses.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Manager, Administrator")]
        // GET: Submitted Books
        public async Task<IActionResult> Submitted(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["RentSortParm"] = sortOrder == "Rent" ? "Rent_desc" : "Rent";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var houses = _context.Houses
                .Where(s => s.Status == HouseStatus.Submitted);

            if (!String.IsNullOrEmpty(searchString))
            {
                houses = houses.Where(s => s.HouseName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.Location.Contains(searchString)
                || s.Category.Contains(searchString)
                || s.HouseType.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    houses = houses.OrderBy(s => s.HouseName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Name_desc":
                    houses = houses.OrderByDescending(s => s.HouseName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Rent":
                    houses = houses.OrderBy(s => s.RentPerSeat);
                    ViewData["SortStatus"] = ": Rent (Low-High)";
                    ViewData["ChangeSort2"] = "(High-Low)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Rent_desc":
                    houses = houses.OrderByDescending(s => s.RentPerSeat);
                    ViewData["SortStatus"] = ": Rent (High-Low)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    houses = houses.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<House>.CreateAsync(houses.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Manager, Administrator")]
        // GET: Rejected Books
        public async Task<IActionResult> Rejected(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["RentSortParm"] = sortOrder == "Rent" ? "Rent_desc" : "Rent";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var houses = _context.Houses
                .Where(s => s.Status == HouseStatus.Rejected);

            if (!String.IsNullOrEmpty(searchString))
            {
                houses = houses.Where(s => s.HouseName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.Location.Contains(searchString)
                || s.Category.Contains(searchString)
                || s.HouseType.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    houses = houses.OrderBy(s => s.HouseName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Name_desc":
                    houses = houses.OrderByDescending(s => s.HouseName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Rent":
                    houses = houses.OrderBy(s => s.RentPerSeat);
                    ViewData["SortStatus"] = ": Rent (Low-High)";
                    ViewData["ChangeSort2"] = "(High-Low)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Rent_desc":
                    houses = houses.OrderByDescending(s => s.RentPerSeat);
                    ViewData["SortStatus"] = ": Rent (High-Low)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    houses = houses.OrderBy(s => s.AdTime);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<House>.CreateAsync(houses.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Houses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var house = await _context.Houses
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.HouseID == id);
            if (house == null)
            {
                return NotFound();
            }

            var isAuthorized = User.IsInRole(Constants.UserRole) ||
                           User.IsInRole(Constants.ManagerRole) ||
                           User.IsInRole(Constants.AdministratorRole);
            var currentUserId = _userManager.GetUserId(User);
            if (!isAuthorized
            && house.Status != HouseStatus.Approved)
            {
                return new ChallengeResult();
            }

            return View(house);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(string id, HouseStatus status)
        {
            var house = await _context.Houses
                .FirstOrDefaultAsync(m => m.HouseID == id);
            if (house == null)
            {
                return NotFound();
            }
            var houseOperation = (status == HouseStatus.Approved)
                                                   ? Operations.Approve
                                                   : Operations.Reject;

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, house,
                                     houseOperation);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            house.Status = status;
            _context.Update(house);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Houses/Create
        public IActionResult Add()
        {
            return View();
        }

        // POST: Houses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(HouseInfoViewModel house)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                house.HouseID = "House" + DateTime.Now.ToString("yyMMddhhmmssffffff");
                house.HouseName = "To-let (For " + house.HouseType + ")";
                var model = new HouseInfoViewModel2()
                {
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    HouseInfo = house
                };
                return View("AddConfirmed", model);
            }
            return View(house);
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddConfirmed(HouseInfoViewModel2 model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageUoload != null)
                {
                    string extension = Path.GetExtension(model.ImageUoload.FileName);
                    string filename = model.HouseInfo.HouseID + extension;
                    Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "images", "houses", model.HouseInfo.HouseID));
                    string filePath = Path.Combine(_env.WebRootPath, "images", "houses", model.HouseInfo.HouseID, filename);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await (model.ImageUoload.CopyToAsync(stream));
                    }
                    model.ImagePath = "~/images/" + "houses/" + model.HouseInfo.HouseID + "/" + filename;
                }
                else
                {
                    model.ImagePath = "~/images/houses/house.jpg";
                }
                try
                {
                    var house = new House()
                        {
                           HouseID = model.HouseInfo.HouseID,
                           HouseName = model.HouseInfo.HouseName,
                           Category = model.HouseInfo.Category,
                           HouseType = model.HouseInfo.HouseType,
                           Location = model.HouseInfo.Location,
                           dateTime = model.HouseInfo.dateTime,
                           AvailableSeat = model.HouseInfo.AvailableSeat,
                           AvailableRoom = model.HouseInfo.AvailableRoom,
                           RentPerSeat = model.HouseInfo.RentPerSeat,
                           RentPerRoom = model.HouseInfo.RentPerRoom,
                           ServiceCharge = model.HouseInfo.ServiceCharge,
                           Facilities = model.Facilities,
                           Description = model.Description,
                           ImagePath = model.ImagePath,
                           OwnerID = _userManager.GetUserId(User),
                           Email = model.Email,
                           Phone = model.Phone,
                           AdTime = DateTime.Now,
                        };

                        var isAuthorized = await _authorizationService.AuthorizeAsync(User, house, Operations.Create);
                        if (!isAuthorized.Succeeded)
                        {
                            return new ChallengeResult();
                        }
                        _context.Add(house);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(MyHouses));

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

        // GET: Houses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ob = await _context.Houses.FindAsync(id);
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
            var house = new HouseInfoViewModel()
            {
                HouseID = id,
                Category = ob.Category,
                HouseType = ob.HouseType,
                Location = ob.Location,
                dateTime = ob.dateTime,
                AvailableSeat = ob.AvailableSeat,
                AvailableRoom = ob.AvailableRoom,
                RentPerSeat = ob.RentPerSeat,
                RentPerRoom = ob.RentPerRoom,
                ServiceCharge = ob.ServiceCharge,
            };
            return View(house);
        }
       
        //// POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HouseInfoViewModel house)
        {
            var ob = await _context.Houses.FindAsync(house.HouseID);
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
                var model = new HouseInfoViewModel2()
                {
                    ImagePath = ob.ImagePath,
                    Facilities = ob.Facilities,
                    Description = ob.Description,
                    OwnerID = ob.OwnerID,
                    Email = ob.Email,
                    Phone = ob.Phone,
                    HouseInfo = house,
                };
                return View("EditConfirmed", model);
            }
            return View(house);
        }

        // POST: Houses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(HouseInfoViewModel2 model)
        {
            var house = await _context.Houses.FindAsync(model.HouseInfo.HouseID);
            if (house == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var isAuthorized = await _authorizationService.AuthorizeAsync(
                                 User, house,
                                 Operations.Update);
                if (!isAuthorized.Succeeded)
                {
                    return new ChallengeResult();
                }
                if (model.ImageUoload != null)
                {
                    string extension = Path.GetExtension(model.ImageUoload.FileName);
                    string filename = model.HouseInfo.HouseID + extension;
                    Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "images", "houses", model.HouseInfo.HouseID));
                    string filePath = Path.Combine(_env.WebRootPath, "images", "houses", model.HouseInfo.HouseID, filename);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await (model.ImageUoload.CopyToAsync(stream));
                    }
                    model.ImagePath = "~/images/" + "houses/" + model.HouseInfo.HouseID + "/" + filename;
                }
                try
                {
                    house.HouseID = model.HouseInfo.HouseID;
                    house.HouseName = "To-let (For " + house.HouseType + ")";
                    house.Category = model.HouseInfo.Category;
                    house.HouseType = model.HouseInfo.HouseType;
                    house.Location = model.HouseInfo.Location;
                    house.dateTime = model.HouseInfo.dateTime;
                    house.AvailableSeat = model.HouseInfo.AvailableSeat;
                    house.AvailableRoom = model.HouseInfo.AvailableRoom;
                    house.RentPerSeat = model.HouseInfo.RentPerSeat;
                    house.RentPerRoom = model.HouseInfo.RentPerRoom;
                    house.ServiceCharge = model.HouseInfo.ServiceCharge;
                    house.Facilities = model.Facilities;
                    house.Description = model.Description;
                    house.ImagePath = model.ImagePath;
                    house.OwnerID = model.OwnerID;
                    house.Email = model.Email;
                    house.Phone = model.Phone;
                    house.AdTime = DateTime.Now;

                    if (house.Status == HouseStatus.Approved)
                    {
                        // If the contact is updated after approval, 
                        // and the user cannot approve,
                        // set the status back to submitted so the update can be
                        // checked and approved.
                        var canApprove = await _authorizationService.AuthorizeAsync(User,
                                                house,
                                                Operations.Approve);

                        if (!canApprove.Succeeded)
                        {
                            house.Status = HouseStatus.Submitted;
                        }
                    }

                    _context.Update(house);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HouseExists(house.HouseID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyHouses));
            }
            return View(model);
        }

        // GET: Houses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var house = await _context.Houses
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.HouseID == id);
            if (house == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, house, Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }


            return View(house);
        }

        // POST: Houses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var house = await _context.Houses.FindAsync(id);

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, house, Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            string path = Path.Combine(_env.WebRootPath, "images", "houses", house.HouseID);
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    _context.Houses.Remove(house);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MyHouses));
                }
                _context.Houses.Remove(house);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyHouses));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool HouseExists(string id)
        {
            return _context.Houses.Any(e => e.HouseID == id);
        }

        #endregion

        // Post: Requests/Sent    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sent(string id, string receiver, string message)
        {
            var house = await _context.Houses.FindAsync(id);
            var user = await _userManager.FindByIdAsync(receiver);
            if (ModelState.IsValid)
            {
                var request = new Request()
                {
                    MaterialID = house.HouseID,
                    Material = house.HouseName,
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
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = house.HouseID });
            }
            return RedirectToAction(nameof(Details), new { id = house.HouseID });
        }
    }
}
