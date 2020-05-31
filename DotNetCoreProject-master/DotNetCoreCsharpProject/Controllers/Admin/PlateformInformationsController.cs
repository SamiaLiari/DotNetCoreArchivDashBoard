using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreCsharpProject.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreCsharpProject.Controllers.Admin
{
    public class PlateformInformationsController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PlateformInformationsController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            PlanningDates planning = _dataContext.PlanningDates.FirstOrDefault();
            if (planning == null)
            {
                planning = new PlanningDates();
                planning.PlanningSoutenanceFile = "";
                _dataContext.PlanningDates.Add(planning);
                _dataContext.SaveChanges();
            }
           
            return View(planning);
        }

        [HttpPost]
        public async Task<ActionResult> uploadPlanning(IFormFile file)
        {
            PlanningDates planning = _dataContext.PlanningDates.FirstOrDefault();

            var plannningDir = Path.Combine(_webHostEnvironment.WebRootPath, "Files/planning");
            var filePath = Path.Combine(plannningDir, planning.PlanningSoutenanceFile);
            if (System.IO.File.Exists(@filePath))
            {
                System.IO.File.Delete(@filePath);
            }

            if (file != null && file.Length > 0)
                try
                {
                    
                    filePath = Path.Combine(plannningDir, Path.GetFileName(file.FileName));
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                   planning.PlanningSoutenanceFile = Path.GetFileName(file.FileName);

                    _dataContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            ViewBag.planning = Path.GetFileName(file.FileName);
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult planningDates(DateTime DateAttributionEnc,
            DateTime DateOuvertureEnrGrp,
            DateTime DateFinEnrGrp,
            DateTime DateLimitInsSujet,
            DateTime DateLimitRapportAvt1,
            DateTime DateLimitRapportAvt2,
            DateTime DateLimitRapportAvt3,
            DateTime DateLimitRapportAvt4,
            DateTime DateLimitRapportFinal
            )
        {
            PlanningDates planning = _dataContext.PlanningDates.FirstOrDefault();


         planning.DateAttributionEnc = DateAttributionEnc;
        planning.DateOuvertureEnrGrp = DateOuvertureEnrGrp;
            planning.DateFinEnrGrp = DateFinEnrGrp;


                    planning.DateLimitInsSujet = DateLimitInsSujet;
                    planning.DateLimitRapportAvt1 = DateLimitRapportAvt1;
                    planning.DateLimitRapportAvt2 = DateLimitRapportAvt2;
                    planning.DateLimitRapportAvt3 = DateLimitRapportAvt3;
                    planning.DateLimitRapportAvt4 = DateLimitRapportAvt4;
                    planning.DateLimitRapportFinal = DateLimitRapportFinal;
                    


            _dataContext.SaveChanges();



            return View("Index", planning);

        }
    }
}