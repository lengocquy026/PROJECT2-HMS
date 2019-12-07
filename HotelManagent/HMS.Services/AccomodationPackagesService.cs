using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public class AccomodationPackagesService
    {
        private readonly HMSContext context;

        public AccomodationPackagesService(HMSContext context)
        {
            this.context = context;
        }
        public IEnumerable<AccomodationPackage> GetAllAccomodationPackages()
        {
            return context.AccomodationPackages.Where(x => x.IsStatus == true).OrderBy(pk => pk.AccomodationTypeID);
        }

        public IEnumerable<AccomodationPackage> GetAllAccomodationPackagesByAccomodationType(int accomodationTypeID)
        {
            return context.AccomodationPackages.Where(x => x.AccomodationTypeID == accomodationTypeID).ToList();
        }

        public IEnumerable<AccomodationPackage> SearchAccomodationPackages(string searchTerm, int? accomodationTypeID, int page, int recordSize)
        {

            var accomodationPackages = context.AccomodationPackages.Where(pk => pk.IsStatus == true).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationPackages = accomodationPackages.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (accomodationTypeID.HasValue && accomodationTypeID.Value > 0)
            {
                accomodationPackages = accomodationPackages.Where(a => a.AccomodationTypeID == accomodationTypeID.Value);
            }

            var skip = (page - 1) * recordSize;

            return accomodationPackages.OrderBy(x => x.AccomodationTypeID).Skip(skip).Take(recordSize).ToList();
        }

        public int SearchAccomodationPackagesCount(string searchTerm, int? accomodationTypeID)
        {
            var accomodationPackages = context.AccomodationPackages.Where(x => x.IsStatus == true).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationPackages = accomodationPackages.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (accomodationTypeID.HasValue && accomodationTypeID.Value > 0)
            {
                accomodationPackages = accomodationPackages.Where(a => a.AccomodationTypeID == accomodationTypeID.Value);
            }

            return accomodationPackages.Count();
        }

        public AccomodationPackage GetAccomodationPackageByID(int ID)
        {
            return context.AccomodationPackages.Include(r => r.AccomodationType).SingleOrDefault(pr => pr.ID == ID);
        }

        public bool SaveAccomodationPackage(AccomodationPackage accomodationPackage)
        {
            context.AccomodationPackages.Add(accomodationPackage);

            return context.SaveChanges() > 0;
        }

        public AccomodationPackage Update(AccomodationPackage roomChanges)
        {
            var room = context.AccomodationPackages.Attach(roomChanges);
            room.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return roomChanges;
        }
    }
}
