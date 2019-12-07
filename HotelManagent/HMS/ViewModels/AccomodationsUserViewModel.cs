using HMS.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels
{
    public class AccomodationsUserViewModel
    {
        public AccomodationType AccomodationType { get; set; }
        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }
        public IEnumerable<Accomodation> Accomodations { get; set; }
        public int SelectedAccomodationPackageID { get; internal set; }
    }

    public class AccomodationPackageDetailsViewModel
    {
        public AccomodationPackage AccomodationPackage { get; set; }
    }

    public class CheckAccomodationAvailabilityViewModel
    {
        [Required]
        public int AccomodationPackageID { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessage = "date is not a correct format")]
        public DateTime FromDate { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int NoOfAdults { get; set; }

        [Required]
        public int NoOfChildren { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Notes { get; set; }

        public List<SelectListItem> ListOfPackage { get; set; }
    }
}
