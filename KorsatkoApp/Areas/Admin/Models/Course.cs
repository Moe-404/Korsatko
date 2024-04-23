using System.ComponentModel.DataAnnotations;

namespace KorsatkoApp.Areas.Admin.Models {
    public class Course {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "اسم الكورس")]
        public string Name { get; set; }
        
        [Display(Name = "تفاصيل الكورس")]
        public string? Description { get; set; }
        
        [Display(Name = "متطلبات الكورس")]
        public string? Prerequisites { get; set; }
        
        [Display(Name = "صورة الكورس")]
        public string? Picture { get; set; }
        
        [Display(Name = "سعر الكورس")]
        public double Price { get; set; }
        
        public List<Session> Sessions { get; } = new(); 
        
        [Display(Name = "تاريخ الإضافة")]
        public DateTime AddedOn { get; set; } = DateTime.Now;
    }
}
