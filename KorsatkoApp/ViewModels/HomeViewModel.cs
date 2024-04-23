using KorsatkoApp.Areas.Admin.Models;

namespace KorsatkoApp.ViewModels {
    public class HomeViewModel {
        public IEnumerable<Course> courses { get; set; }
		public IEnumerable<Instructor> instructors { get; set; }
    }
}
