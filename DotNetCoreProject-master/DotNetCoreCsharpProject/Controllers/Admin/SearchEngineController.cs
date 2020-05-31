using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreCsharpProject.Entities;
using DotNetCoreCsharpProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNetCoreCsharpProject.Controllers.Admin
{
    public class SearchEngineController : Controller
    {
        private readonly DataContext dbContext;

        public SearchEngineController(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {


            ViewBag.filieres = dbContext.Filieres;
            ViewBag.levels = dbContext.Levels;
            ViewBag.professors = (from p in dbContext.Professors
                                          join u in dbContext.AspNetUsers
             on p.IdUser equals u.Id
                                          select u).ToList();

            ViewBag.Societes = dbContext.Societes;


            //dbContext.Filieres.Find(0).id_iliere++;

            return View();
        }

        public JsonResult GetStudents(Int32? FilId, Int32? Id_niveau)
        {

            List<AspNetUsers> students = (from s in dbContext.Students
                                          join u in dbContext.AspNetUsers
             on s.IdUser equals u.Id
                                          where ( (s.IdFil == FilId || FilId == 0  )&& (s.IdNiv == Id_niveau || Id_niveau == 0 ) )
                                          select u).ToList();

            AspNetUsers usr = new AspNetUsers();
            usr.FirstName = "bader";

            students.Add(usr);
            return Json(students);
        }

            public JsonResult GetReports(Int32? FilId, Int32? Id_niveau,String selectedStudents, Int32? Id_Societe,  String SearchedText, Int32? profId)
        {
          //  dbContext.Configuration.ProxyCreationEnabled = false;

            List<Groupes> grps = dbContext.Groupes.Where(g =>
           (!FilId.HasValue || FilId == 0 || g.Students.FirstOrDefault().IdFil == FilId)
            &&
            (!Id_niveau.HasValue || Id_niveau == 0 || g.Students.FirstOrDefault().IdNiv == Id_niveau)
            &&
           (!profId.HasValue || profId == 0 || g.Idprof == profId)
            &&
             (selectedStudents == null || selectedStudents == "0" || g.Students.Where(s => s.IdUser == selectedStudents).FirstOrDefault() != null)
            &&
             (!Id_Societe.HasValue || Id_Societe == 0 || g.IdSociete == Id_Societe)
            &&
           (SearchedText == null  || g.Sujet.Contains(SearchedText))
           ).ToList();

            
            List<RepportDetails> repportsDetails = new List<RepportDetails>();

            RepportDetails repportDetails = new RepportDetails(); ;
            foreach (Groupes grp in grps)
            {
                Students stdd= grp.Students.FirstOrDefault();
                repportDetails = new RepportDetails();

                //archivedReport.Filiere.Nom_filiere;
                Students std = dbContext.Students.Where(s => s.GroupId == grp.Id).FirstOrDefault();
                int idNiv = std.IdNiv;
                int idFil = std.IdFil;
                String profUserId = dbContext.Professors.Where(p => p.Id == grp.Idprof).FirstOrDefault().IdUser;
                repportDetails.Filiere = dbContext.Filieres.Where(f => f.IdFiliere == idFil).FirstOrDefault().NomFiliere;
                repportDetails.niveau = dbContext.Levels.Where(n => n.IdNiveau == idNiv).FirstOrDefault().NomNiveau;
                repportDetails.Encadrant = dbContext.AspNetUsers.Where(u => u.Id == profUserId).FirstOrDefault().LastName;
                Reports reports = dbContext.Reports.Where(r => r.IdGroup == grp.Id).FirstOrDefault();
                repportDetails.path = reports == null ? "" : reports.PathReport;
                repportDetails.remarque = grp.DesciptionSujet;
                repportDetails.sujet = grp.Sujet;
                repportDetails.Au = dbContext.Societes.Where(s => s.Id == grp.IdSociete).FirstOrDefault().Nom;
                repportsDetails.Add(repportDetails);
            }
            return Json(repportsDetails);
        }
    }
}