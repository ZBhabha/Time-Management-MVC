using System;
using System.Collections.Generic;

#nullable disable

namespace TimeWebApp.Model
{
    public partial class Student
    {
        public Student()
        {
            DateModules = new HashSet<DateModule>();
            Modules = new HashSet<Module>();
            Semesters = new HashSet<Semester>();
            StudentModules = new HashSet<StudentModule>();
        }

        public int StuId { get; set; }
        public string Password { get; set; }

        public virtual ICollection<DateModule> DateModules { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
        public virtual ICollection<Semester> Semesters { get; set; }
        public virtual ICollection<StudentModule> StudentModules { get; set; }
    }
}
