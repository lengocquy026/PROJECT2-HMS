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
using HMS.Email;
using MimeKit;

namespace HMS.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly HMSContext _context;
        private readonly AccomodationPackagesService _accomodationPackagesService;
        private readonly BookingService _bookingService;

        public BookingsController(HMSContext context, AccomodationPackagesService accomodationPackagesService
                                 ,BookingService bookingService)
        {
            _context = context;
            _accomodationPackagesService = accomodationPackagesService;
            _bookingService = bookingService;
        }

        public IActionResult Index(string searchTerm, int? accomodationPackageID, int? page)
        {
            int recordSize = 5;
            page = page ?? 1;

            BookingPackagesListingModel model = new BookingPackagesListingModel();

            model.SearchTerm = searchTerm;
            model.AccomodationPackageID = accomodationPackageID;
            model.AccomodationPackages = _accomodationPackagesService.GetAllAccomodationPackages();

            model.Bookings = _bookingService.SearchBoookings(searchTerm, accomodationPackageID, page.Value, recordSize);
            var totalRecords = _bookingService.SearchBoookingsCount(searchTerm, accomodationPackageID);

            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.AccomodationPackage)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        public IActionResult Create()
        {
            ViewData["AccomodationPackageID"] = new SelectList(_context.AccomodationPackages, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AccomodationPackageID,FromDate,Duration,NoOfAdults,NoOfChildren,Name,Email,Notes")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccomodationPackageID"] = new SelectList(_context.AccomodationPackages, "ID", "Name", booking.AccomodationPackageID);
            return View(booking);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["AccomodationPackageID"] = new SelectList(_context.AccomodationPackages, "ID", "Name", booking.AccomodationPackageID);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AccomodationPackageID,FromDate,Duration,NoOfAdults,NoOfChildren,Name,Email,Notes")] Booking booking)
        {
            if (id != booking.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.ID))
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
            ViewData["AccomodationPackageID"] = new SelectList(_context.AccomodationPackages, "ID", "Name", booking.AccomodationPackageID);
            return View(booking);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.ID == id);
        }


        [AllowAnonymous]
        public IActionResult CheckBooking()
        {
            CheckAccomodationAvailabilityViewModel objCheck = new CheckAccomodationAvailabilityViewModel();

            objCheck.FromDate = DateTime.Now;
            objCheck.ListOfPackage = _context.AccomodationPackages.Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.Name }).ToList();
            return View(objCheck);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CheckBooking(CheckAccomodationAvailabilityViewModel model)
        {
            if (ModelState.IsValid)
            {
                string sendemail = $"THÔNG TIN ĐẶT PHÒNG <br> <br>";

                Booking booking = new Booking
                {
                    AccomodationPackageID = model.AccomodationPackageID,
                    FromDate = model.FromDate,
                    Duration = model.Duration,
                    NoOfAdults = model.NoOfAdults,
                    NoOfChildren = model.NoOfChildren,
                    Notes = model.Notes,
                    Email = model.Email,
                    Name = model.Name
                };

                _context.Booking.Add(booking);
                _context.SaveChanges();

                var sendEmail = EmailService.Send(new SendEmailRequest()
                {
                    Template = "",
                    Body = $" :  <br> " +
                          sendemail +
                        $"+ GIÁ GÓI PHÒNG: {booking.AccomodationPackage.FeePerNight} <br>",
                    Subject = "Hẹn gặp lại bạn vào ngày đặt phòng!",
                    ToEmail = model.Email
                });

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public IActionResult BookingComplete()
        {
            ViewBag.CheckoutCompleteMessage = "CẢM ƠN CÁC BẠN ĐÃ ĐẶT HÀNG!";
            return View();
        }
    }
}
