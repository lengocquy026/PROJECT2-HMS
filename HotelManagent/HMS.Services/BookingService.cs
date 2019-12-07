using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMS.Services
{
    public class BookingService
    {
        private readonly HMSContext _context;

        public BookingService(HMSContext context)
        {
            _context = context;
        }

        public IEnumerable<Booking> GetAllAccomodations()
        {
            return _context.Booking.ToList();
        }

        public IEnumerable<Booking> GetAllAccomodationsByAccomodationPackage(int accomodationPackageID)
        {
            return _context.Booking.Where(x => x.AccomodationPackageID == accomodationPackageID).ToList();
        }

        public IEnumerable<Booking> SearchBoookings(string searchTerm, int? accomodationPackageID, int page, int recordSize)
        {
            var bookings = _context.Booking.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                bookings = bookings.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (accomodationPackageID.HasValue && accomodationPackageID.Value > 0)
            {
                bookings = bookings.Where(a => a.AccomodationPackageID == accomodationPackageID.Value);
            }

            var skip = (page - 1) * recordSize;

            return bookings.OrderBy(x => x.AccomodationPackageID).Skip(skip).Take(recordSize).ToList();
        }

        public int SearchBoookingsCount(string searchTerm, int? accomodationPackageID)
        {
            var bookings = _context.Booking.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                bookings = bookings.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (accomodationPackageID.HasValue && accomodationPackageID.Value > 0)
            {
                bookings = bookings.Where(a => a.AccomodationPackageID == accomodationPackageID.Value);
            }

            return bookings.Count();
        }

        public Booking GetAccomodationByID(int ID)
        {
            return _context.Booking.Find(ID);
        }

        public bool SaveAccomodation(Booking booking)
        {
            _context.Booking.Add(booking);

            return _context.SaveChanges() > 0;
        }
    }
}
