using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HMS.Data;
using HMS.Entities;
using HMS.Services;
using HMS.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Collections.Generic;

namespace HMS.Controllers
{
    public class AccomodationPackagesController : Controller
    {
        private readonly HMSContext _context;
        private readonly AccomodationPackagesService accomodationPackagesService;
        private readonly AccomodationTypesService accomodationTypesService;
        private readonly IHostingEnvironment hostingEnvironment;

        public AccomodationPackagesController(HMSContext context,AccomodationPackagesService accomodationPackagesService,
                                              AccomodationTypesService accomodationTypesService, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            this.accomodationPackagesService = accomodationPackagesService;
            this.accomodationTypesService = accomodationTypesService;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult List(string PackageName)
        {
            IEnumerable<AccomodationPackage> accomodationPackages;
            string CurrentPackage;

            if (string.IsNullOrEmpty(PackageName))
            {
                accomodationPackages = accomodationPackagesService.GetAllAccomodationPackages();
                CurrentPackage = "All Package";
            }
            else
            {
                accomodationPackages = accomodationPackagesService.GetAllAccomodationPackages().Where(g => g.AccomodationType.Name == PackageName)
                    .OrderBy(s => s.ID);
                CurrentPackage = _context.AccomodationPackages.FirstOrDefault(g => g.AccomodationType.Name == PackageName)?.Name;
            }

            return View(new AccomodationPackagesListingModel
            {
                AccomodationPackages = accomodationPackages,
                CurrentPackage = CurrentPackage
            });
        }

        public ActionResult Index(string searchTerm, int? accomodationTypeID, int? page)
        {
            int recordSize = 5;
            page = page ?? 1;

            AccomodationPackagesListingModel model = new AccomodationPackagesListingModel();

            model.SearchTerm = searchTerm;
            model.AccomodationTypeID = accomodationTypeID;

            model.AccomodationTypes = accomodationTypesService.GetAllAccomodationTypes();

            model.AccomodationPackages = accomodationPackagesService.SearchAccomodationPackages(searchTerm, accomodationTypeID, page.Value, recordSize);
            var totalRecords = accomodationPackagesService.SearchAccomodationPackagesCount(searchTerm, accomodationTypeID);

            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }

        //public IActionResult Create()
        //{
        //    ViewData["AccomodationTypeID"] = new SelectList(_context.AccomodationTypes, "ID", "Name");
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID,AccomodationTypeID,Name,NoOfRoom,FeePerNight,IsStatus")] AccomodationPackage accomodationPackage)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(accomodationPackage);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AccomodationTypeID"] = new SelectList(_context.AccomodationTypes, "ID", "Name", accomodationPackage.AccomodationTypeID);
        //    return View(accomodationPackage);
        //}

        [HttpGet]
        public IActionResult Create()
        {
            AccomodationPackageViewModel objAccomodationPackage = new AccomodationPackageViewModel();
            objAccomodationPackage.ListOfAccomodationType = _context.AccomodationTypes.Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Name }).ToList();

            return View(objAccomodationPackage);
        }

        [HttpPost]
        public IActionResult Create(AccomodationPackageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                AccomodationPackage newAccomo = new AccomodationPackage
                {
                    AccomodationTypeID = model.AccomodationTypeID,
                    IMGPackage = ProcessUploadedFile(model),
                    FeePerNight = model.FeePerNight,
                    NoOfRoom = model.NoOfRoom,
                    Name = model.Name,
                };
                _context.AccomodationPackages.Add(newAccomo);
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var accomodationPackage = await _context.AccomodationPackages.FindAsync(id);
        //    if (accomodationPackage == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["AccomodationTypeID"] = new SelectList(_context.AccomodationTypes, "ID", "Name", accomodationPackage.AccomodationTypeID);
        //    return View(accomodationPackage);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,AccomodationTypeID,Name,NoOfRoom,FeePerNight,IsStatus")] AccomodationPackage accomodationPackage)
        //{
        //    if (id != accomodationPackage.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(accomodationPackage);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!AccomodationPackageExists(accomodationPackage.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AccomodationTypeID"] = new SelectList(_context.AccomodationTypes, "ID", "Name", accomodationPackage.AccomodationTypeID);
        //    return View(accomodationPackage);
        //}

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var accomodation = accomodationPackagesService.GetAccomodationPackageByID(id);
            AccomodationPackageEditViewModel objaccomodationEditViewModel = new AccomodationPackageEditViewModel
            {
                Id = accomodation.ID,
                AccomodationTypeID = accomodation.AccomodationTypeID,
                FeePerNight = accomodation.FeePerNight,
                ExistingPhotoPath = accomodation.IMGPackage,
                Name = accomodation.Name,
                NoOfRoom = accomodation.NoOfRoom,
            };
            objaccomodationEditViewModel.ListOfAccomodationType = _context.AccomodationTypes.Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Name }).ToList();

            return View(objaccomodationEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AccomodationPackageEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                AccomodationPackage accomodation = _context.AccomodationPackages.SingleOrDefault(r => r.ID == model.Id);
                accomodation.AccomodationTypeID = model.AccomodationTypeID;
                accomodation.FeePerNight = model.FeePerNight;
                accomodation.NoOfRoom = model.NoOfRoom;
                accomodation.Name = model.Name;

                if (model.Image == null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                            "img", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    accomodation.IMGPackage = ProcessUploadedFile(model);
                }

                AccomodationPackage updatedAccomodation = accomodationPackagesService.Update(accomodation);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accomodationPackage = _context.AccomodationPackages
                .Include(a => a.AccomodationType)
                .SingleOrDefault(m => m.ID == id);
            accomodationPackage.IsStatus = false;
            _context.SaveChanges();
            if (accomodationPackage == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        private bool AccomodationPackageExists(int id)
        {
            return _context.AccomodationPackages.Any(e => e.ID == id);
        }

        private string ProcessUploadedFile(AccomodationPackageViewModel model)
        {
            var files = HttpContext.Request.Form.Files;
            string uniqueFileName = null;
            foreach (var Image in files)
            {
                if (Image != null && Image.Length > 0)
                {
                    var file = Image;
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "img");
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            uniqueFileName = $"{fileName}";
                        }
                    }
                }
            }
            return uniqueFileName;
        }
    }
}
