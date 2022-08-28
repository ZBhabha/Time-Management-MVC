using System;
using System.Collections.Generic;

#nullable disable

namespace TimeWebApp.Model
{
    public partial class DateModule
    {
        public int DateModuleId { get; set; }
        public DateTime ModDate { get; set; }
        public string ModId { get; set; }
        public int? StuId { get; set; }

        public virtual Module Mod { get; set; }
        public virtual Student Stu { get; set; }
    }
}
