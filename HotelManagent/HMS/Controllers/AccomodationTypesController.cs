using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HMS.Data;
using HMS.Entities;
using HMS.ViewModels;
using HMS.Services;
using Microsoft.AspNetCore.Authorization;

namespace HMS.Controllers
{
    [Authorize]
    public class AccomodationTypesController : Controller
    {
        private readonly HMSContext _context;
        private readonly AccomodationTypesService accomodationTypesService;

        public AccomodationTypesController(HMSContext context, AccomodationTypesService accomodationTypesService)
        {
            _context = context;
            this.accomodationTypesService = accomodationTypesService;
        }

        public ActionResult Index(string searchTerm)
        {
            AccomodationTypesListingModel model = new AccomodationTypesListingModel();

            model.SearchTerm = searchTerm;

            model.AccomodationTypes = accomodationTypesService.SearchAccomodationTypes(searchTerm);

            return View(model);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description")] AccomodationType accomodationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accomodationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accomodationType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accomodationType = await _context.AccomodationTypes.FindAsync(id);
            if (accomodationType == null)
            {
                return NotFound();
            }
            return View(accomodationType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description")] AccomodationType accomodationType)
        {
            if (id != accomodationType.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accomodationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccomodationTypeExists(accomodationType.ID))
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
            return View(accomodationType);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accomodationType = _context.AccomodationTypes.SingleOrDefault(m => m.ID == id);
            accomodationType.IsStatus = false;
            _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool AccomodationTypeExists(int id)
        {
            return _context.AccomodationTypes.Any(e => e.ID == id);
        }

        [AllowAnonymous]
        public IActionResult AccomodationType(int Id)
        {
            var accomodationType = _context.AccomodationTypes.Where(ac => ac.IsStatus == true).SingleOrDefault(ac => ac.ID == Id);
            if (accomodationType == null)
            {
                return NotFound();
            }
            var accomodationPackages = _context.AccomodationPackages.Where(ac => ac.AccomodationTypeID == Id && ac.IsStatus == true).OrderBy(ac => ac.FeePerNight).ToList();
            if (accomodationPackages.Count == 0)
            {
                ViewBag.Product = "Zero!";
            }
            return View(accomodationPackages);
        }
    }
}
