using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels
{
    public class AccomodationPackageViewModel
    {
        public int AccomodationTypeID { get; set; }

        [Required(ErrorMessage = "Accomodation Name is required")]
        [Display(Name = "Accomodation Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "No of room is required")]
        [Display(Name = "No of room")]
        public int NoOfRoom { get; set; }

        [Required(ErrorMessage = "Room Price is required")]
        [Display(Name = "Room Price")]
        [Range(500, 1000, ErrorMessage = "Room Price should be qual and greater than {1}")]
        public decimal FeePerNight { get; set; }

        [Required(ErrorMessage = "Room Image is required"), StringLength(550)]
        [Display(Name = "Room Image")]
        public string Image { get; set; }

        public List<SelectListItem> ListOfAccomodationType { get; set; }

    }
}
