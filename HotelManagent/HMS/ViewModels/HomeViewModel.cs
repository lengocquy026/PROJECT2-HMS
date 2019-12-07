using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<AccomodationType> AccomodationTypes { get; set; }
        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }
    }
}
