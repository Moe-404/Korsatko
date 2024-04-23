using KorsatkoApp.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KorsatkoApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<Student> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Session>()
                .HasOne(s => s.course).WithMany(s => s.Sessions).HasForeignKey(s => s.CourseId);// one to many relation between Sessions Tbl and Courses tbl.
            modelBuilder.Entity<Session>()
                .HasOne(s => s.instructor).WithMany(s => s.Sessions).HasForeignKey(s => s.InstructorId);// one to many relation between Sessions Tbl and Instructors tbl. 

            //build relation between Enrolments Table and Sessions table
            modelBuilder.Entity<Enrollment>()
                .HasKey(t => new { t.StudentId , t.SessionId });// build composite key 

            modelBuilder.Entity<Enrollment>()
                .HasOne(s => s.session).WithMany(s => s.Enrollments).HasForeignKey(s => s.SessionId);// one to many relation between Enrolments Tbl and Session tbl

            modelBuilder.Entity<Enrollment>()
                .HasOne(s => s.student).WithMany(s => s.Enrollments).HasForeignKey(s => s.StudentId);// one to many relation between Enrolments Tbl and Students tbl

            //add temp data

            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 123, Name = "Advanced C++", Description = "C++ Programming Language for Advanced Programmers", Prerequisites = "C Course", Price = 1500, AddedOn = DateTime.Now },
                new Course { Id = 124, Name = "C For Beginners", Description = "C Programming Language", Prerequisites = "None", Price = 1200, AddedOn = DateTime.Now }
               );
            //modelBuilder.Entity<Instructor>().HasData(
            //    new Instructor { Id = 567, FullName = "Abanob", Email = "Aba@gmail.com", Gender = 'M', PhoneNumber = "0123456789", NationalId = "321857012", ExperienceYears = 8, Qualifications = "Doctor" }
            //    );
            //modelBuilder.Entity<Session>().HasData(
            //    new Session { Id = 456, StartDate = DateTime.UtcNow, Location = "Online", Limit = 30, IsAvailable = true, PriceRate = 1.0f, CourseId = 123, InstructorId = 567 }
            //    );

            //modelBuilder.Entity<Student>().HasData(
            //    new Student {Id = 789, FullName="Adele", Email="Adele@gmail.com",Gender='F',PhoneNumber="012987654",NationalId="32185776582",UserName="CoolAdele",UserPassword="a123"}
            //    );
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

    }
}