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
    public class DateModulesController : Controller
    {
        private readonly UniversityContext _context;

        public DateModulesController(UniversityContext context)
        {
            _context = context;
        }

        // GET: DateModules
        public async Task<IActionResult> Index()
        {
            int StudentLogin = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"));
            //Displays user specific data
            var universityContext = _context.DateModules.Where(x => x.StuId == StudentLogin);
           
           
         
            return View(await universityContext.ToListAsync());
        }

        

        // GET: DateModules/Create
        public IActionResult Create()
        {   //Populate the drop down list with user specific module codes
            List<Module> mods = _context.Modules.Where(x => x.StuId == Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"))).ToList();
            ViewData["ModId"] = new SelectList(mods.Select(s => s.ModId), "ModId");
           
         
            return View();
        }

        // POST: DateModules/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DateModuleId,ModDate,ModId,StuId")] DateModule dateModule)
        {
            if (ModelState.IsValid)
            {
                
                dateModule.StuId = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"));
             
                
                _context.Add(dateModule);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            List<Module> mods = _context.Modules.Where(x => x.StuId == Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUser"))).ToList();
            ViewData["ModId"] = new SelectList(mods.Select(s => s.ModId), "ModId");
            
            return View(dateModule);
        }

       

        // GET: DateModules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dateModule = await _context.DateModules
                .Include(d => d.Mod)
                .Include(d => d.Stu)
                .FirstOrDefaultAsync(m => m.DateModuleId == id);
            if (dateModule == null)
            {
                return NotFound();
            }

            return View(dateModule);
        }

        // POST: DateModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dateModule = await _context.DateModules.FindAsync(id);
            _context.DateModules.Remove(dateModule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DateModuleExists(int id)
        {
            return _context.DateModules.Any(e => e.DateModuleId == id);
        }
    }
}
