using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DotNetCoreCsharpProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace WebApplicationProjectPartial.Controllers
{
    public class AdminController : Controller
    {
        private readonly DataContext _context;

        public AdminController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Filieres> filieres = _context.Filieres.ToList();
            filieres.Insert(0, new Filieres { IdFiliere = 0, NomFiliere = "Choisissez une filière" });

            ViewData["Idfil"] = new SelectList(filieres, "IdFiliere", "NomFiliere");

            List<Levels> levels = _context.Levels.ToList();
            levels.Insert(0, new Levels { IdNiveau = 0, NomNiveau = "Choisissez un niveau" });
            ViewData["Idlev"] = new SelectList(levels, "IdNiveau", "NomNiveau");

            return View();
        }

        public IActionResult Notes()
        {
            List<Filieres> filieres = _context.Filieres.ToList();
            filieres.Insert(0, new Filieres { IdFiliere = 0, NomFiliere = "Choisissez une filière" });

            ViewData["Idfil"] = new SelectList(filieres, "IdFiliere", "NomFiliere");

            List<Levels> levels = _context.Levels.ToList();
            levels.Insert(0, new Levels { IdNiveau = 0, NomNiveau = "Choisissez un niveau" });
            ViewData["Idlev"] = new SelectList(levels, "IdNiveau", "NomNiveau");
            return View();
        }

        [HttpPost]
        public IActionResult addEncadrant(string encadrant, string group)
        {
            int id_grp;
            int id_prof;
            try
            {
                id_grp = Int32.Parse(group);
                id_prof = Int32.Parse(encadrant);
                Groupes grp = _context.Groupes.Find(id_grp);
                grp.Idprof = id_prof;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                id_grp = 0;
                id_prof = 0;
            }

            return RedirectToAction("Index");

        }

        public JsonResult showProfs(string id_f)
        {
            int id_fil;

            try
            {
                id_fil = Int32.Parse(id_f);
                var profs = _context.Professors.Where(std => std.IdFil.Equals(id_fil))
           .Select(std => new stdModel
           {
               Id = std.Id,
               LastName = std.IdUserNavigation.FirstName + ' ' + std.IdUserNavigation.LastName

           }).ToList();

                return new JsonResult(profs, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }
            catch (Exception ex)
            {
                id_fil = 0;
            }
            return null;


        }

        public JsonResult getNotes(string id_f, string id_l)
        {
            int id_fil;
            int id_lev;
            try
            {
                id_fil = Int32.Parse(id_f);
                id_lev = Int32.Parse(id_l);
                var stds = _context.Students.Include(s => s.IdUserNavigation).Where(std => std.IdFil.Equals(id_fil) && std.IdNiv.Equals(id_lev))
           .Select(std => new stdModel
           {
               Id = std.Id,
               Cne = std.Cne,
               LastName = std.IdUserNavigation.FirstName + ' ' + std.IdUserNavigation.LastName,
               Note = (Double)std.Group.Note

           }).ToList();

                return new JsonResult(stds, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }
            catch (Exception ex)
            {
                id_fil = 0;
            }
            return null;


        }

        public JsonResult getStudents(string id_g)
        {
            int id_grp;
            try
            {
                id_grp = Int32.Parse(id_g);
                var students = _context.Students.Include(s => s.IdUserNavigation).Where(std => std.GroupId.Equals(id_grp))
            .Select(std => new stdModel
            {
                Id = std.Id,
                LastName = std.IdUserNavigation.FirstName + ' ' + std.IdUserNavigation.LastName,
                Cne = std.Cne, 
                Email = std.IdUserNavigation.Email,
                Number = std.IdUserNavigation.PhoneNumber

            }).ToList();

                return new JsonResult(students, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }
            catch (Exception ex)
            {
                id_grp = 0;
            }
            return null;
        }

        public JsonResult showGroups(string id_f, string id_l)
        {
            int id_fil;
            int id_lev;
            try
            {
                id_fil = Int32.Parse(id_f);
                id_lev = Int32.Parse(id_l);
                var grps = _context.Groupes.Where(grp => grp.Students.FirstOrDefault().IdFil.Equals(id_fil) && grp.Students.FirstOrDefault().IdNiv.Equals(id_lev)).Select(grp => new stdModel
                {
                    Id = grp.Id,
                    LastName = "Groupe N°"
                }).ToList();


                return new JsonResult(grps, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }
            catch (Exception ex)
            {
                id_fil = 0;
                id_lev = 0;
            }
            return null;

        }

        public IActionResult DownloadExcel(string id_f, string id_l)
        {
            int id_fil;
            int id_lev;

            id_fil = Int32.Parse(id_f);
            id_lev = Int32.Parse(id_l);

            var stds = _context.Students.Include(s => s.IdUserNavigation).Include(s => s.Group).Where(std => std.IdFil.Equals(id_fil) && std.IdNiv.Equals(id_lev) && std.GroupId != null);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "Cne";

            Sheet.Cells["B1"].Value = "Nom";
            Sheet.Cells["C1"].Value = "Note";

            int row = 2;
            foreach (var item in stds)
            {

                Sheet.Cells[string.Format("A{0}", row)].Value = item.Cne;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.IdUserNavigation.LastName + ' ' + item.IdUserNavigation.FirstName;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Group.Note == null ? 0 : item.Group.Note;

                row++;
            }

            byte[] fileContents = Ep.GetAsByteArray();
            string excelName = $"Notes-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

        }

        internal class stdModel
        {
            public string LastName { get; set; }
            public string Email { get; set; }
            public int Id { get; internal set; }
            public string Cne { get; set; }
            public string Number { get; set; }
            public double Note { get; set; }
        }
    }
}