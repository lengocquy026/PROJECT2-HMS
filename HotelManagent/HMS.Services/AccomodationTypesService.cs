using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HMS.Services
{
    public class AccomodationTypesService
    {
        private readonly HMSContext _db;

        public AccomodationTypesService(HMSContext db)
        {
            _db = db;
        }
        public IEnumerable<AccomodationType> GetAllAccomodationTypes()
        {
            return _db.AccomodationTypes.Where(x => x.IsStatus == true).ToList();
        }

        public bool SaveAccomodationType(AccomodationType accomodationType)
        {
            _db.AccomodationTypes.Add(accomodationType);

            return _db.SaveChanges() > 0;
        }

        public AccomodationType GetAccomodationTypeByID(int ID)
        {
            return _db.AccomodationTypes.Find(ID);
        }

        public IEnumerable<AccomodationType> SearchAccomodationTypes(string searchTerm)
        {
            var accomodationTypes = _db.AccomodationTypes.Where(tp => tp.IsStatus == true).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationTypes = accomodationTypes.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return accomodationTypes.ToList();
        }
    }
}
