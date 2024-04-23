using System.ComponentModel.DataAnnotations;

namespace KorsatkoApp.Areas.Admin.ViewModels {
    public class CourseViewModel : UploadImageViewModel {
        public int Id { get; set; }

        [Required]
        [Display(Name = "اسم الكورس")]
        public string Name { get; set; }
        
        [Display(Name = "تفاصيل الكورس")]
        public string? Description { get; set; }
        
        [Display(Name = "متطلبات الكورس")]
        public string? Prerequisites { get; set; }
        
        [Display(Name = "سعر الكورس")]
        public double Price { get; set; }
        
        [Display(Name = "تاريخ الإضافة")]
        public DateTime AddedOn { get; set; } = DateTime.Now;
        
        public string? ExistingImage { get; set; }
    }
}
