using System;
using System.Collections.Generic;

#nullable disable

namespace TimeWebApp.Model
{
    public partial class Module
    {
        public Module()
        {
            DateModules = new HashSet<DateModule>();
            StudentModules = new HashSet<StudentModule>();
        }

        public string ModId { get; set; }
        public string ModName { get; set; }
        public int NumCredits { get; set; }
        public int ClassHours { get; set; }
        public int? SelfStudy { get; set; }
        public int? StuId { get; set; }

        public virtual Student Stu { get; set; }
        public virtual ICollection<DateModule> DateModules { get; set; }
        public virtual ICollection<StudentModule> StudentModules { get; set; }
    }
}
