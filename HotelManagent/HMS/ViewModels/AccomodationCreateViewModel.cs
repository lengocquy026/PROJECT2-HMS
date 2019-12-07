using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels
{
    public class AccomodationCreateViewModel
    {
        [Required(ErrorMessage = "Accomodation Package is required")]
        [Display(Name = "Accomodation Packag")]
        public int AccomodationPackageID { get; set; }

        public string IMGAccomodation { get; set; }

        [Required(ErrorMessage = "Accomodation Name is required")]
        [Display(Name = "Accomodation Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
