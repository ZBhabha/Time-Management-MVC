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
    public class StudentModulesController : Controller
    {
        private readonly UniversityContext _context;

        public StudentModulesController(UniversityContext context)
        {
            _context = context;
        }

        // GET: StudentModules
        public async Task<IActionResult> Index()

        {
            int StudentLogin = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"));
            //Displays user specific data
            var universityContext = _context.StudentModules.Where(x => x.StuId == StudentLogin);
            return View(await universityContext.ToListAsync());
        }

       
        // GET: StudentModules/Create
        public IActionResult Create()
        {
            //Populate the drop down list with user specific module codes
            List<Module> mods = _context.Modules.Where(x => x.StuId == Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"))).ToList();
            ViewData["ModId"] = new SelectList(mods.Select(s => s.ModId), "ModId");
            
            return View();
        }

        // POST: StudentModules/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentModuleId,WorkDate,WorkedHours,HoursRemaining,ModId,StuId")] StudentModule studentModule)
        {
            if (ModelState.IsValid)
            {
                int workHours = studentModule.WorkedHours;
                //Moduleinformation retrieved from the databse
                Module module = _context.Modules.Where(x => x.StuId == Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"))).FirstOrDefault();
                Helper.code = module.ModId;
                Helper.name = module.ModName;
                Helper.credits = module.NumCredits;
                Helper.classHours = module.ClassHours;
                Helper.selfStudy = (int)module.SelfStudy;
                //Passed to constructor from class library to allow for calculations
                Modules m1 = new Modules(Helper.code, Helper.name, Helper.credits, Helper.classHours, workHours, Helper.selfStudy);
                //Remaining hours are calculated
                m1.calcHoursLeft();
                //Remaining hours passed to the required varibales
                studentModule.WorkedHours = workHours;
                studentModule.HoursRemaining = m1.Hours;
                studentModule.ModId = m1.Code;
                studentModule.StuId = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"));
                //Information added to the database
                _context.Add(studentModule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<Module> mods = _context.Modules.Where(x => x.StuId == Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"))).ToList();
            ViewData["ModId"] = new SelectList(mods.Select(s => s.ModId), "ModId");
            
            return View(studentModule);
        }

       
        // GET: StudentModules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentModule = await _context.StudentModules
                .Include(s => s.Mod)
                .Include(s => s.Stu)
                .FirstOrDefaultAsync(m => m.StudentModuleId == id);
            if (studentModule == null)
            {
                return NotFound();
            }

            return View(studentModule);
        }

        // POST: StudentModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentModule = await _context.StudentModules.FindAsync(id);
            _context.StudentModules.Remove(studentModule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentModuleExists(int id)
        {
            return _context.StudentModules.Any(e => e.StudentModuleId == id);
        }
    }
}
