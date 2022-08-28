using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModulesClassLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TimeWebApp.Model;
using TimeWebApp.Models;

namespace TimeWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
      
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.User = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"));
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult New()
        {
            UniversityContext db = new UniversityContext();
           
            DateModule dm = db.DateModules.Where(x => x.StuId == Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"))).FirstOrDefault();
           int StudentLogin = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"));
           

            
            //Code to manage notification feature
            
            if (StudentLogin != 0 && db.DateModules.Where(x => x.StuId == Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"))).Any())
            {

                ViewBag.Day = dm.ModDate;
                ViewBag.Module = "*Study " + dm.ModId + " Today*";
                
                //If the user is logged in and the dateModule table contains the required data the userwill recieve the alert

            }
            else if (StudentLogin != 0 && !db.DateModules.Where(x => x.StuId == Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"))).Any())
            {
                ViewBag.Enter = " ";
                RedirectToAction("New", "Home");
                //If the user is logged in and there isnt anything in the datModule table for that user then there will be no alert

            }
            else
            {
               ViewBag.Enter = "Please Login First";
              
                //If the user is not logged in,the system will tell them to so
            }


            return View();
        } 





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
