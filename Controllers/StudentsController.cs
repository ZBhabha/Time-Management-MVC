using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModulesClassLibrary;
using TimeWebApp.Model;

namespace TimeWebApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly UniversityContext _context;
        UniversityContext db = new UniversityContext();

        public StudentsController(UniversityContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            

            return View(await _context.Students.ToListAsync());
        }

        //Shows the Login form
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        //Recieves data from the login form
        [HttpPost]
        public ActionResult Login(Student s)
        {
            //Code below to hash password 
            //Learnt and adapted from :
            //Author : Afzaal Ahmad Zeeshan
            //Link : https://www.c-sharpcorner.com/article/hashing-passwords-in-net-core-with-tips/
            using (var sha256 = SHA256.Create())
            {
                
                HttpContext.Session.SetInt32("CurrentUser", s.StuId);
                //Variables declared to take in student number and password from login 
                int stuNum = (int)HttpContext.Session.GetInt32("CurrentUser");
                string pass = s.Password;

               
                // Sending the password to be hashed
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));
                // Get the hashed string verison of the password
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                //Student is matched to the student table in the database in order to allow for login
                Student user = db.Students.Where(us => us.StuId == s.StuId && us.Password.Equals(hash)).FirstOrDefault();

                if (user != null)
                {
                    //The user can now enter and will be taken to the main menu page
                    return RedirectToAction("New", "Home");
                }
                else
                {

                    //wrong credentials,diplay error message
                    ViewBag.Login = "INVALID USERNAME OR PASSWORD";
                    return View();

                }
            }

           
        }
        //Shows the form
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        //recieves data
        [HttpPost]
        public ActionResult Register(Student s)
        {
            HttpContext.Session.SetInt32("CurrentUser", s.StuId);
            int stuNum = (int)HttpContext.Session.GetInt32("CurrentUser");
            string pass = s.Password;
            using (var sha256 = SHA256.Create())
            {
                // Sending the password to be hashed
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));
                // Get the hashed string version of the password
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                //Create a new student object which will be stored in the database
                Student newS = new Student();
                newS.StuId = stuNum;
                newS.Password = hash;
                //Validation to check if user is in database already
                var UserAlreadyExists = db.Students.Any(x => x.StuId == newS.StuId);
                if (UserAlreadyExists)
                {

                    ViewBag.Fail = "USER WITH THIS STUDENT NUMBER ALREADY EXISTS";
                }
                else
                {
                    //New student added to the database
                    db.Students.Add(newS);
                    //Code to save user in database using Async method
                    db.SaveChangesAsync();
                  //User is taken to semester page 
                    return RedirectToAction("Create", "Semesters", new { StudentLogin = stuNum });
                }
            }
            return View();
        }


        
      

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StuId == id);
        }
    }
}
