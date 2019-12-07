using HMS.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Components
{
    public class AccomodationTypesMenu : ViewComponent
    {
        private readonly AccomodationTypesService _accomodationTypesService;

        public AccomodationTypesMenu(AccomodationTypesService accomodationTypesService)
        {
            _accomodationTypesService = accomodationTypesService;
        }

        public IViewComponentResult Invoke()
        {
            var accomodation = _accomodationTypesService.GetAllAccomodationTypes();

            return View(accomodation);
        }
    }
}
