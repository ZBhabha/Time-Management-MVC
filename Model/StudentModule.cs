using System;
using System.Collections.Generic;

#nullable disable

namespace TimeWebApp.Model
{
    public partial class StudentModule
    {
        public int StudentModuleId { get; set; }
        public DateTime? WorkDate { get; set; }
        public int WorkedHours { get; set; }
        public int? HoursRemaining { get; set; }
        public string ModId { get; set; }
        public int? StuId { get; set; }

        public virtual Module Mod { get; set; }
        public virtual Student Stu { get; set; }
    }
}
