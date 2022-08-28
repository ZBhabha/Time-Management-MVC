using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModulesClassLibrary;
using TimeWebApp.Model;

namespace TimeWebApp.Controllers
{
    public class SemestersController : Controller
    {
        private readonly UniversityContext _context;

        public SemestersController(UniversityContext context)
        {
            _context = context;
        }

        // GET: Semesters
        public IActionResult Index()
        {
          
            return View();

        }

        // GET: Semesters/Create
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: Semesters/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SemesterId,NumWeeks,StartDate,StuId")] Semester semester)
        {
            if (ModelState.IsValid)
            {
                Helper.weeks = semester.NumWeeks;
                Helper.startDate = (DateTime)semester.StartDate;
                semester.StuId = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"));
                //Semester constructor called
                Semesters ss = new Semesters(Helper.weeks, Helper.startDate);
             //Semester added to the db
                _context.Add(semester);
                await _context.SaveChangesAsync();
             
           

                //User is redirected to the login page
                return RedirectToAction("Login", "Students");
          


            }
        

            return View(semester);
        }

      
       
       
       

        private bool SemesterExists(int id)
        {
            return _context.Semesters.Any(e => e.SemesterId == id);
        }
    }
}
