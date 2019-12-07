using HMS.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels
{
    public class BookingViewModel
    {
        public int AccomodationPackageID { get; set; }
        public DateTime FromDate { get; set; }

        public int Duration { get; set; }
        public int NoOfAdults { get; set; }
        public int NoOfChildren { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }

        public List<SelectListItem> ListOfPackage { get; set; }
    }

    public class BookingPackagesListingModel
    {
        public IEnumerable<Booking> Bookings { get; set; }

        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }
        public int? AccomodationPackageID { get; set; }
        public string SearchTerm { get; set; }

        public Pager Pager { get; set; }
    }
}
