using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HMS.Models;
using Microsoft.AspNetCore.Authorization;
using HMS.Services;
using HMS.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace HMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AccomodationTypesService accomodationTypesService;
        private readonly AccomodationPackagesService accomodationPackagesService;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(AccomodationTypesService accomodationTypesService, AccomodationPackagesService accomodationPackagesService
                              ,IHostingEnvironment hostingEnvironment)
        {
            this.accomodationTypesService = accomodationTypesService;
            this.accomodationPackagesService = accomodationPackagesService;
            this.hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();

            model.AccomodationTypes = accomodationTypesService.GetAllAccomodationTypes();
            model.AccomodationPackages = accomodationPackagesService.GetAllAccomodationPackages();

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
