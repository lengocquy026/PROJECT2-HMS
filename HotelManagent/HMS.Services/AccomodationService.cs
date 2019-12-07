using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMS.Services
{
    public class AccomodationService
    {
        private readonly HMSContext _context;

        public AccomodationService(HMSContext context)
        {
            _context = context;
        }
        public IEnumerable<Accomodation> GetAllAccomodations()
        {
            return _context.Accomodations.ToList();
        }

        public IEnumerable<Accomodation> GetAllAccomodationsByAccomodationPackage(int accomodationPackageID)
        {
            return _context.Accomodations.Where(x => x.AccomodationPackageID == accomodationPackageID).ToList();
        }

        public IEnumerable<Accomodation> SearchAccomodations(string searchTerm, int? accomodationPackageID, int page, int recordSize)
        {
            var accomodations = _context.Accomodations.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodations = accomodations.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (accomodationPackageID.HasValue && accomodationPackageID.Value > 0)
            {
                accomodations = accomodations.Where(a => a.AccomodationPackageID == accomodationPackageID.Value);
            }

            var skip = (page - 1) * recordSize;

            return accomodations.OrderBy(x => x.AccomodationPackageID).Skip(skip).Take(recordSize).ToList();
        }

        public int SearchAccomodationsCount(string searchTerm, int? accomodationPackageID)
        {
            var accomodations = _context.Accomodations.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodations = accomodations.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (accomodationPackageID.HasValue && accomodationPackageID.Value > 0)
            {
                accomodations = accomodations.Where(a => a.AccomodationPackageID == accomodationPackageID.Value);
            }

            return accomodations.Count();
        }

        public Accomodation GetAccomodationByID(int ID)
        {
            return _context.Accomodations.Find(ID);
        }

        public bool SaveAccomodation(Accomodation accomodation)
        {
            _context.Accomodations.Add(accomodation);

            return _context.SaveChanges() > 0;
        }
    }
}
