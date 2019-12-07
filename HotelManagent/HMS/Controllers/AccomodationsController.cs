using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HMS.Data;
using HMS.Entities;
using HMS.Services;
using HMS.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace HMS.Controllers
{
    [Authorize]
    public class AccomodationsController : Controller
    {
        private readonly HMSContext _context;
        private readonly AccomodationPackagesService accomodationPackagesService;
        private readonly AccomodationService accomodationService;

        public AccomodationsController(HMSContext context, AccomodationPackagesService accomodationPackagesService,
                                       AccomodationService accomodationService)
        {
            _context = context;
            this.accomodationPackagesService = accomodationPackagesService;
            this.accomodationService = accomodationService;
        }

        public IActionResult Index(string searchTerm, int? accomodationPackageID, int? page)
        {
            int recordSize = 5;
            page = page ?? 1;

            AccomodationsListingModel model = new AccomodationsListingModel();

            model.SearchTerm = searchTerm;
            model.AccomodationPackageID = accomodationPackageID;
            model.AccomodationPackages = accomodationPackagesService.GetAllAccomodationPackages();

            model.Accomodations = accomodationService.SearchAccomodations(searchTerm, accomodationPackageID, page.Value, recordSize);
            var totalRecords = accomodationService.SearchAccomodationsCount(searchTerm, accomodationPackageID);

            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewData["AccomodationPackageID"] = new SelectList(_context.AccomodationPackages, "ID", "Name");
            return View();
        }

        // POST: Accomodations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AccomodationPackageID,Name,Description")] Accomodation accomodation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accomodation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccomodationPackageID"] = new SelectList(_context.AccomodationPackages, "ID", "Name", accomodation.AccomodationPackageID);
            return View(accomodation);
        }

        // GET: Accomodations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accomodation = await _context.Accomodations.FindAsync(id);
            if (accomodation == null)
            {
                return NotFound();
            }
            ViewData["AccomodationPackageID"] = new SelectList(_context.AccomodationPackages, "ID", "Name", accomodation.AccomodationPackageID);
            return View(accomodation);
        }

        // POST: Accomodations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AccomodationPackageID,Name,Description")] Accomodation accomodation)
        {
            if (id != accomodation.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accomodation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccomodationExists(accomodation.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccomodationPackageID"] = new SelectList(_context.AccomodationPackages, "ID", "Name", accomodation.AccomodationPackageID);
            return View(accomodation);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var accomodation = await _context.Accomodations.FindAsync(id);
            _context.Accomodations.Remove(accomodation);
            if (accomodation == null)
            {
                return NotFound();
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public ActionResult Details(int ID)
        {
            AccomodationPackageDetailsViewModel model = new AccomodationPackageDetailsViewModel();

            model.AccomodationPackage = accomodationPackagesService.GetAccomodationPackageByID(ID);

            return View(model);
        }

        private bool AccomodationExists(int id)
        {
            return _context.Accomodations.Any(e => e.ID == id);
        }

    }
}
