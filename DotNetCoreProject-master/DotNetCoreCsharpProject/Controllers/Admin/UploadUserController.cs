using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DotNetCoreCsharpProject.Areas.Identity.Pages.Account;
using DotNetCoreCsharpProject.Data;
using DotNetCoreCsharpProject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetCoreCsharpProject.Controllers.Admin
{
    public class UploadUserController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DataContext _dataContext;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public UploadUserController(UserManager<IdentityUser> userManager,
                                                    SignInManager<IdentityUser> signInManager,
                                                    ILogger<RegisterModel> logger,
                                                    IEmailSender emailSender,
                                                    DataContext dataContext,
                                                    ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _dataContext = dataContext;
            _applicationDbContext =applicationDbContext;
        }

        public ActionResult Index()
        {

            
            ViewBag.filieres = _dataContext.Filieres;
            ViewBag.niveaux = _dataContext.Levels;
            return View();
        }


        public ActionResult uploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> uploadStudent(IFormFile file, String niv, String fil)
        {
            List<AspNetUsers> users = new List<AspNetUsers>();

            ViewBag.filieres = _dataContext.Filieres;
            ViewBag.niveaux = _dataContext.Levels;

            int addedStudent = 0;

            if (file != null && file.Length > 0)
                try
                {
                    // string path = Path.Combine(Server.MapPath("~/Content/Files"),
                    //        Path.GetFileName(file.FileName));
                    //  file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                    ViewBag.path = Path.Combine("/Files", Path.GetFileName(file.FileName));
                    ViewBag.fileMime = file.ContentType;
                    ViewBag.fileSize = _userManager.ToString();

                    Stream fileStream = file.OpenReadStream();
                    var stream = file.OpenReadStream();
                    if (stream.Length != 0)
                    {
                        //handle the stream here
                        using (XLWorkbook excelWorkbook = new XLWorkbook(stream))
                        {
                            var name = excelWorkbook.Worksheet(1).Name;
                            //do more things whatever you like as you now have a handle to the entire workbook.
                            var row = excelWorkbook.Worksheet(1).Row(2);

                            while (!row.Cell(1).IsEmpty())
                            {
                                IdentityUser user = new IdentityUser();
                                string FirstName = row.Cell(1).GetString();
                                string LastName = row.Cell(2).GetString();
                                string PhoneNumber = row.Cell(4).GetString();
                                string Email = row.Cell(3).GetString();
                                string cne = row.Cell(5).GetString();
                                string cin = row.Cell(6).GetString();
                                user.Email = Email;
                                user.UserName = Email;
                                user.PhoneNumber = PhoneNumber;
                                var exist = from u in _dataContext.AspNetUsers where u.UserName == user.UserName select u;

                                if (exist.FirstOrDefault() == null)
                                {
                                    var result = await _userManager.CreateAsync(user, LastName +  "En123@");

                                    if (result.Succeeded)
                                    {
                                       
                                        Students student = new Students();
                                        AspNetUsers aspNetUser = _dataContext.AspNetUsers.Find(user.Id);
                                        aspNetUser.FirstName = FirstName;
                                        aspNetUser.LastName = LastName;
                                        aspNetUser.EmailConfirmed = true;
                    
                                
                                        student.IdUser = aspNetUser.Id;
                                        student.IdNiv = Int32.Parse(niv);
                                        student.IdFil = Int32.Parse(fil);
                                        student.Cne = cne;
                                        student.Cin = cin;
                                        _dataContext.Students.Add(student);

                                        AspNetRoles role = _dataContext.AspNetRoles.Where(rl => rl.Name.Equals("STUDENT")).FirstOrDefault();
                                        AspNetUserRoles userRole = new AspNetUserRoles
                                        {
                                            Role = role,
                                            User = aspNetUser
                                        };

                                        _dataContext.AspNetUserRoles.Add(userRole);

                                        _dataContext.SaveChanges();
                                        ++addedStudent;
                                    }
                                    ViewBag.fileSize = result.ToString();
                                }

                                row = row.RowBelow();
                            }

                            _dataContext.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.StudentMessage = "ERROR:" + ex.ToString();
                }
            else
            {
                ViewBag.StudentMessage = "You have not specified a file.";
            }
            ViewBag.AddedStudent= "Added students : " + addedStudent;
            return View("index",users);
        }


        [HttpPost]
        public async Task<ActionResult> uploadProf(IFormFile file, String fil)
        {
            List<AspNetUsers> users = new List<AspNetUsers>();

            ViewBag.filieres = _dataContext.Filieres;
            ViewBag.niveaux = _dataContext.Levels;

            int addedProfessor = 0;

            if (file != null && file.Length > 0)
               try
                {
                    // string path = Path.Combine(Server.MapPath("~/Content/Files"),
                    //        Path.GetFileName(file.FileName));
                    //  file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                    ViewBag.path = Path.Combine("/Files", Path.GetFileName(file.FileName));
                    ViewBag.fileMime = file.ContentType;
                    ViewBag.fileSize = _userManager.ToString();

                    Stream fileStream = file.OpenReadStream();
                    var stream = file.OpenReadStream();
                    if (stream.Length != 0)
                    {
                        //handle the stream here
                        using (XLWorkbook excelWorkbook = new XLWorkbook(stream))
                        {
                            var name = excelWorkbook.Worksheet(1).Name;
                            //do more things whatever you like as you now have a handle to the entire workbook.
                            var row = excelWorkbook.Worksheet(1).Row(1);

                            while (!row.Cell(1).IsEmpty())
                            {
                                IdentityUser user = new IdentityUser();
                                string FirstName = row.Cell(1).GetString();
                                string LastName = row.Cell(2).GetString();
                                string PhoneNumber = row.Cell(4).GetString();
                                string Email = row.Cell(3).GetString();

                                user.Email = Email;
                                user.UserName = Email;
                                user.PhoneNumber = PhoneNumber;
                                var exist = from u in _dataContext.AspNetUsers where u.UserName == user.UserName select u;

                                if (exist.FirstOrDefault() == null)
                                {
                                    var result = await _userManager.CreateAsync(user, LastName + "En123@");
                                    if (result.Succeeded)
                                    {

                                        Professors prof = new Professors();
                                        AspNetUsers aspNetUser = _dataContext.AspNetUsers.Find(user.Id);
                                        aspNetUser.FirstName = FirstName;
                                        aspNetUser.LastName = LastName;
                                        aspNetUser.EmailConfirmed = true;


                                        prof.IdUser = aspNetUser.Id;
                                        prof.IdFil = Int32.Parse(fil);

                                        _dataContext.Professors.Add(prof);

                                        AspNetRoles role = _dataContext.AspNetRoles.Where(rl => rl.Name.Equals("PROFESSOR")).FirstOrDefault();
                                        AspNetUserRoles userRole = new AspNetUserRoles
                                        {
                                            Role = role,
                                            User = aspNetUser
                                        };

                                        _dataContext.AspNetUserRoles.Add(userRole);
                                        
                                        
                                        _dataContext.SaveChanges();
                                        ++addedProfessor;
                                    }
                                    ViewBag.fileSize = result.ToString();
                                }

                                row = row.RowBelow();
                            }

                            _dataContext.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ProfessorMessage = "ERROR:" + ex.ToString();
               }
            else
            {
                ViewBag.ProfessorMessage = "You have not specified a file.";
            }
            ViewBag.AddedProfessor = "Added professors : " + addedProfessor;
            return View("index", users);
        }











    }
}