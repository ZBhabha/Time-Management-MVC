using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TimeWebApp.Model
{
    public partial class UniversityContext : DbContext
    {
        public UniversityContext()
        {
        }

        public UniversityContext(DbContextOptions<UniversityContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DateModule> DateModules { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentModule> StudentModules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-N6L95NC\\ZAHEERA;Initial Catalog=University;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<DateModule>(entity =>
            {
                entity.ToTable("DATE_MODULE");

                entity.Property(e => e.DateModuleId).HasColumnName("DATE_MODULE_ID");

                entity.Property(e => e.ModDate)
                    .HasColumnType("date")
                    .HasColumnName("MOD_DATE");

                entity.Property(e => e.ModId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MOD_ID");

                entity.Property(e => e.StuId).HasColumnName("STU_ID");

                entity.HasOne(d => d.Mod)
                    .WithMany(p => p.DateModules)
                    .HasForeignKey(d => d.ModId)
                    .HasConstraintName("FK__DATE_MODU__MOD_I__49C3F6B7");

                entity.HasOne(d => d.Stu)
                    .WithMany(p => p.DateModules)
                    .HasForeignKey(d => d.StuId)
                    .HasConstraintName("FK__DATE_MODU__STU_I__4AB81AF0");
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.HasKey(e => e.ModId)
                    .HasName("PK__Modules__AE553E6B2C733F80");

                entity.Property(e => e.ModId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MOD_ID");

                entity.Property(e => e.ClassHours).HasColumnName("CLASS_HOURS");

                entity.Property(e => e.ModName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MOD_NAME");

                entity.Property(e => e.NumCredits).HasColumnName("NUM_CREDITS");

                entity.Property(e => e.SelfStudy).HasColumnName("SELF_STUDY");

                entity.Property(e => e.StuId).HasColumnName("STU_ID");

                entity.HasOne(d => d.Stu)
                    .WithMany(p => p.Modules)
                    .HasForeignKey(d => d.StuId)
                    .HasConstraintName("FK__Modules__STU_ID__38996AB5");
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.ToTable("SEMESTER");

                entity.Property(e => e.SemesterId).HasColumnName("SEMESTER_ID");

                entity.Property(e => e.NumWeeks).HasColumnName("NUM_WEEKS");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("START_DATE");

                entity.Property(e => e.StuId).HasColumnName("STU_ID");

                entity.HasOne(d => d.Stu)
                    .WithMany(p => p.Semesters)
                    .HasForeignKey(d => d.StuId)
                    .HasConstraintName("FK__SEMESTER__STU_ID__3B75D760");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StuId)
                    .HasName("PK__Student__C97EC9644A280DB2");

                entity.ToTable("Student");

                entity.Property(e => e.StuId)
                    .ValueGeneratedNever()
                    .HasColumnName("STU_ID");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");
            });

            modelBuilder.Entity<StudentModule>(entity =>
            {
                entity.ToTable("STUDENT_MODULE");

                entity.Property(e => e.StudentModuleId).HasColumnName("STUDENT_MODULE_ID");

                entity.Property(e => e.HoursRemaining).HasColumnName("HOURS_REMAINING");

                entity.Property(e => e.ModId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MOD_ID");

                entity.Property(e => e.StuId).HasColumnName("STU_ID");

                entity.Property(e => e.WorkDate)
                    .HasColumnType("date")
                    .HasColumnName("WORK_DATE");

                entity.Property(e => e.WorkedHours).HasColumnName("WORKED_HOURS");

                entity.HasOne(d => d.Mod)
                    .WithMany(p => p.StudentModules)
                    .HasForeignKey(d => d.ModId)
                    .HasConstraintName("FK__STUDENT_M__MOD_I__4222D4EF");

                entity.HasOne(d => d.Stu)
                    .WithMany(p => p.StudentModules)
                    .HasForeignKey(d => d.StuId)
                    .HasConstraintName("FK__STUDENT_M__STU_I__4316F928");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
