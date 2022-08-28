using System;
using System.Collections.Generic;

#nullable disable

namespace TimeWebApp.Model
{
    public partial class Semester
    {
        public int SemesterId { get; set; }
        public int NumWeeks { get; set; }
        public DateTime? StartDate { get; set; }
        public int? StuId { get; set; }

        public virtual Student Stu { get; set; }
    }
}
