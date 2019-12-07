using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels
{
    public class AccomodationPackagesListingModel
    {
        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }
        public int? AccomodationTypeID { get; set; }
        public IEnumerable<AccomodationType> AccomodationTypes { get; set; }
        public string SearchTerm { get; set; }
        public string CurrentPackage { get; set; }

        public Pager Pager { get; set; }
    }
}
