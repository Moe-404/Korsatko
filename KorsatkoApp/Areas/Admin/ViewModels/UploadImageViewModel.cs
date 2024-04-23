using System.ComponentModel.DataAnnotations;

namespace KorsatkoApp.Areas.Admin.ViewModels {
    public class UploadImageViewModel {

        [Display(Name = "الصورة المصغرة للكورس")]
        public IFormFile? CoursePicture { get; set; }
    }
}
