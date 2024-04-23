using System.ComponentModel.DataAnnotations;

namespace KorsatkoApp.Areas.Admin.Models
{
    public class Session
    {
        [Key]

        public int Id { get; set; }
		[Display(Name = "الكورس")]
		public Course? course { get; set; } = null!;//Reference Navigation property
		[Display(Name = "المدرب")]
		public Instructor? instructor { set; get; } = null!; //Reference Navigation property


		[Display(Name ="تاريخ البداية")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime StartDate { get; set; }
        
        [Display(Name ="تاريخ النهاية")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime EndDate { get; set; }

		[Display(Name = "المكان")]
		public string Location { get; set; }
		
        [Display(Name = "الحد الأقصى")]
		public int Limit { get; set; }
        
        [Display(Name ="وقت البداية")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime startTime { get; set; }
        
        [Display(Name ="وقت النهاية")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime EndTime { get; set; }

        [Display(Name ="متاح")]
        public bool IsAvailable { get; set; }
        
        [Display(Name ="معدل السعر")]
        public float PriceRate { get; set; }
        
        [Display(Name ="الكورس")]
        public int CourseId { get; set; }
        
        [Display(Name ="المدرب")]
        public int InstructorId { get; set; }
        
        [Display(Name ="تاريخ الإضافة")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm}")]

        public DateTime? AddedOn { get; set; } = DateTime.Now;
        
        public List<Enrollment> Enrollments { get; set; } = new(); //Reference Navigation property

    }
}