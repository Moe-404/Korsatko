using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KorsatkoApp.Areas.Admin.Models
{
    public class Student : IdentityUser
    {
        [Display(Name = "الاسم بالكامل")]
        public string FullName { get; set; }
        
        [Display(Name = "النوع")]
        public char Gender { get; set; }
		
		[Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }
        
        [Display(Name = "الرقم القومي")]
        public string NationalId { get; set; }
        
        [Display(Name = "تاريخ الإضافة")]
        public DateTime AddedOn { get; set; } = DateTime.Now;
        
        public List<Enrollment> Enrollments { get; set; } = new();
    }
}
