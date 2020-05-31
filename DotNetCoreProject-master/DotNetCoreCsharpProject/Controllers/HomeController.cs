using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetCoreCsharpProject.Models;
using DotNetCoreCsharpProject.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DotNetCoreCsharpProject.Controllers
{

    public class HomeController : Controller
    {
        readonly DataContext db;
        AspNetUsers user;
        readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            db = context;
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if(userId != null)
            {
                user = db.AspNetUsers.Find(userId.Value);

            }

        }

     


        public IActionResult Index()
        {
            if(user != null)
            {
                string role = db.AspNetUserRoles.Where(rl => rl.UserId == user.Id).Select(rl => rl.Role.Name).FirstOrDefault();
                if (role.Equals("STUDENT"))
                {
                    return RedirectToAction("Index", "Student");

                }
                else if (role.Equals("PROFESSOR"))
                {
                    return RedirectToAction("Index", "PlateformInformations");
                }
                else if (role.Equals("ADMIN"))
                {
                    return RedirectToAction("Index", "Admin");

                }
            }
            
            return View();

        }

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
