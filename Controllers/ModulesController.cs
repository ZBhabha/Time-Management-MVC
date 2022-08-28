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
    public class ModulesController : Controller
    {
        private readonly UniversityContext _context;
       
        
        


        public ModulesController(UniversityContext context)
        {
            _context = context;
        }

        // GET: Modules
        public async Task<IActionResult> Index(int StudentLogin)
        {
    


            StudentLogin = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"));
            //Displays list of user specific modules
            var universityContext = _context.Modules.Where(x => x.StuId == StudentLogin);
            return View(await universityContext.ToListAsync());


        }

      

        // GET: Modules/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Modules/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModId,ModName,NumCredits,ClassHours,SelfStudy,StuId")] Module module)
        {
            if (ModelState.IsValid)
            {
                Helper.code = module.ModId;
                Helper.name = module.ModName;
                Helper.credits = module.NumCredits;
                Helper.classHours = module.ClassHours;
                Semester s = _context.Semesters.Where(x => x.StuId == Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"))).FirstOrDefault();
                //Semester values are passed to the Helper variable so it can be used in the calculation
                Helper.startDate = (DateTime)s.StartDate;
                Helper.weeks = s.NumWeeks;
                module.StuId = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"));
                //Constructor called and values are passed into it
                Modules m = new Modules(Helper.code, Helper.name, Helper.credits, Helper.classHours);
                m.calcSelfStudy();
                Helper.selfStudy = m.SelfStudy;
                module.SelfStudy = m.SelfStudy;
               

              
                _context.Add(@module);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
          
            return View(module);
        }

         // GET: Modules/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Modules
                .Include(s => s.Stu)
                .FirstOrDefaultAsync(m => m.ModId == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var @module = await _context.Modules.FindAsync(id);
            _context.Modules.Remove(@module);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

     






        private bool ModuleExists(string id)
        {
            return _context.Modules.Any(e => e.ModId == id);
        }
    }
}
