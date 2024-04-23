using System.ComponentModel.DataAnnotations;

namespace KorsatkoApp.Areas.Admin.Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="الاسم بالكامل")]
        public string FullName { get; set; }
        [Required]
        [Display(Name ="البريد الإلكتروني")]
        public string Email { get; set; }
        [Display(Name ="النوع")]
        public char Gender { get; set; }
        [Display(Name ="رقم الهاتف")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "National ID  is required")]
        [Display(Name ="الرقم القومي")]
        public string NationalId { get; set; }
        [Display(Name ="سنوات الخبرة")]
        public int ExperienceYears { get; set; }
        [Display(Name ="المؤهلات")]
        public string? Qualifications { get; set; }
        public List<Session> Sessions { get; set; } = new();

    }
}