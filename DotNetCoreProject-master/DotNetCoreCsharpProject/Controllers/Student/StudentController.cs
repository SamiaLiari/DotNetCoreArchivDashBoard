using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetCoreCsharpProject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MimeKit;

namespace DotNetCoreCsharpProject.Controllers.Student
{
   // [Authorize(Roles = "STUDENT")]
    public class StudentController : Controller
    {
        readonly DataContext db;
        readonly UserManager<IdentityUser> _userManager;
        Students student;
        IHttpContextAccessor _httpContextAccessor;
        public StudentController(DataContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            db = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            ClaimsPrincipal currentuser = this.User;
            //.Identity.IsAuthenticated
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var userId = _userManager.GetUserId(currentuser);
            student = db.Students.Where(std => std.IdUser == userId).FirstOrDefault();
        }


        public IActionResult Index()
        {
            List<Students> students = db.Students.Include(std => std.IdUserNavigation).Where(std => std.GroupId == student.GroupId && std.GroupId != null).ToList();
            if (students.Count() == 0)
            {
                students.Add(student);
            }

            ViewBag.FullName = student.IdUserNavigation.FirstName + ' ' + student.IdUserNavigation.LastName;

            return View(students);
        }

        public IActionResult addNewGroup()
        {
            List<Students> students = db.Students.Include(std => std.IdUserNavigation).Where(std => std.IdFil.Equals(student.IdFil) && std.IdNiv.Equals(student.IdNiv) && std.GroupId == null).ToList();
            students.Remove(student);
            return View("newGroup", students);
        }

        [HttpPost]
        public IActionResult addNewGroup(string[] stdid, string sujet, string sujetDesc, string nomSociete, string tele, string ville, string nomEnc, string mailEnc, string teleEnc)
        {
            Societes societe = new Societes
            {
                Nom = nomSociete,
                Tele = tele,
                Ville = ville,
                NomEncadrant = nomEnc,
                MailEncadrant = mailEnc,
                TeleEncadrant = teleEnc
            };

            db.Societes.Add(societe);
            db.SaveChanges();
            Groupes groupes = new Groupes();
            groupes.Sujet = sujet;
            groupes.DesciptionSujet = sujetDesc;
            groupes.IdSociete = societe.Id;
            db.Groupes.Add(groupes);
            db.SaveChanges();

            student.GroupId = groupes.Id;
            List<Students> students = db.Students.Where(std => std.IdFil.Equals(student.IdFil) && std.IdNiv.Equals(student.IdNiv) && std.GroupId == null).ToList();
            foreach (Students std in students)
            {
                string stringid = "std" + std.Id;
                if (Array.Exists(stdid, elt => elt.Equals(stringid)))
                {
                    Students etudiant = db.Students.Find(std.Id);
                    etudiant.GroupId = groupes.Id;
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult addReport(string id)
        {
            if (student.GroupId == null)
            {
                return RedirectToAction("Index");
            }
            string msg = "0";
            string remainingTime = "";
            string filePath = "";
            switch (id)
            {
                case "1":
                    msg = "Rapport d\'avancement N°1";
                    remainingTime = checkDateReport("1");
                    filePath = student.Group.RapportAvt1;
                    break;
                case "2":
                    msg = "Rapport d\'avancement N°2";
                    remainingTime = checkDateReport("2");
                    filePath = student.Group.RapportAvt2;

                    break;
                case "3":
                    msg = "Rapport d\'avancement N°3";
                    remainingTime = checkDateReport("3");
                    filePath = student.Group.RapportAvt3;

                    break;
                case "4":
                    msg = "Rapport d\'avancement N°4";
                    remainingTime = checkDateReport("4");
                    filePath = student.Group.RapportAvt4;
                    break;
                case "5":
                    msg = "Rapport final";
                    remainingTime = checkDateReport("5");
                    ViewData["IdType"] = new SelectList(db.TypeReports.ToList(), "IdType", "Type");
                    Reports repo = db.Reports.Where(rp => rp.IdGroup == student.GroupId).FirstOrDefault();
                    if (repo != null)
                        filePath = repo.PathReport;
                    else
                        filePath = "";
                    break;
            }
            ViewBag.msg = msg;
            ViewBag.idReport = id;
            ViewBag.remainingTime = remainingTime;
            ViewBag.path = filePath;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string idReport, string id_type)
        {
            string errMsg = "";
            if (file == null || file.Length == 0)
                errMsg = "file not selected";
            if (file.Length > 2000000)
                errMsg = "file size is too big: " + file.Length / 1000000 + "Mb";
            string fileName = file.FileName;
            bool extValid = (Path.GetExtension(fileName).Equals(".pdf")) || (Path.GetExtension(fileName).Equals(".docx"));
            if (!extValid)
                errMsg = "file format not accepted: " + Path.GetExtension(fileName);

            if (extValid)
            {
                string dir = "";
                if (idReport == "5")
                {
                    dir = "RapportsFinaux";
                }
                else
                {
                    dir = "RapportsAvt" + idReport;
                }
                var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", dir,
                        "group" + student.GroupId + Path.GetExtension(fileName));

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                saveReport(idReport, path, id_type);

                return RedirectToAction("Index");
            }

            ViewBag.errMsg = errMsg;
            ViewBag.idReport = idReport;

            return View("addReport");


        }

        private void saveReport(string idReport, string path, string idType)
        {
            int idGrp = (int) student.GroupId;
            Groupes groupes = db.Groupes.Find(idGrp);
            switch (idReport)
            {
                case "1":
                    groupes.RapportAvt1 = path;
                    groupes.DateDepotRapport1 = DateTime.Now;
                    break;
                case "2":
                    groupes.RapportAvt2 = path;
                    groupes.DateDepotRapport1 = DateTime.Now;
                    break;
                case "3":
                    groupes.RapportAvt3 = path;
                    groupes.DateDepotRapport1 = DateTime.Now;
                    break;
                case "4":
                    groupes.RapportAvt4 = path;
                    groupes.DateDepotRapport1 = DateTime.Now;
                    break;
                case "5":
                    Reports reports = new Reports
                    {
                        DateDepot = DateTime.Now,
                        IdGroup = (int)student.GroupId,
                        IdType = Convert.ToInt32(idType),
                        PathReport = path

                    };

                    db.Reports.Add(reports);
                    break;

            }
            db.SaveChanges();

        }

        private string checkDateReport(string id)
        {
            PlanningDates dates = db.PlanningDates.FirstOrDefault();
            if (dates == null)
                return "Date non spécifiée";
            DateTime dateTime;
            DateTime now = DateTime.Now;
            string msg = "";
            int daysDiff = 0;
            try
            {
                switch (id)
                {
                    case "1":
                        dateTime = dates.DateLimitRapportAvt1.Value;
                        daysDiff = -((TimeSpan)(now - dateTime)).Days;
                        break;
                    case "2":
                        dateTime = dates.DateLimitRapportAvt2.Value;
                        daysDiff = - ((TimeSpan)(now - dateTime)).Days;
                        break;
                    case "3":
                        dateTime = dates.DateLimitRapportAvt3.Value;
                        daysDiff = - ((TimeSpan)(now - dateTime)).Days;
                        break;
                    case "4":
                        dateTime = dates.DateLimitRapportAvt4.Value;
                        daysDiff = - ((TimeSpan)(now - dateTime)).Days;
                        break;
                    case "5":
                        dateTime = dates.DateLimitRapportFinal.Value;
                        daysDiff = - ((TimeSpan)(now - dateTime)).Days;
                        break;
                }

                if (daysDiff > 0)
                {
                    msg = "Il vous reste " +  daysDiff + " jours pour rendre ce travail";
                }
                else if (daysDiff < 0)
                {
                    msg = "Ce travail est en retard de: " +  - daysDiff + " jours";
                }
                else if (daysDiff == 0)
                {
                    msg = "Aujourdh\'hui est le dernier délai pour rendre ce travail";

                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;

            }
            
            return msg;
        }

        public IActionResult DownloadFile(string filePath)
        {

            return PhysicalFile(filePath, MimeTypes.GetMimeType(filePath), Path.GetFileName(filePath));
        }

    }

  
}