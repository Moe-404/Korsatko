using System.ComponentModel.DataAnnotations;

namespace KorsatkoApp.Areas.Admin.Models
{
    public class Enrollment
    {
        public string? EnrollmentNumber { get; set; }
        [Display(Name ="الطالب")]
        public string StudentId { get; set; }
        [Display(Name ="الميعاد")]
        public int SessionId { get; set; }
        [Display(Name ="حالة الدفع")]
        public string PaymentStatus { get; set; }
        [Display(Name ="الميعاد")]
        public Session? session { get; set; } = null!;  //  Reference navigation property
        [Display(Name ="الطالب")]
        public Student? student { get; set; } = null!; // Reference navigation property
        [Display(Name ="تاريخ الإضافة")]
        public DateTime AddedOn { get; set; } = DateTime.Now;

    }
}
