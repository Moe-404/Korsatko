using System.Diagnostics;
using KorsatkoApp.Areas.Admin.Models;
using KorsatkoApp.Data;
using KorsatkoApp.Models;
using KorsatkoApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace KorsatkoApp.Controllers {
    public class HomeController : Controller {

		private readonly ApplicationDbContext _context;
		private readonly UserManager<Student> _userManager;
        private readonly IToastNotification _toastNotification;


        public HomeController(ApplicationDbContext context, UserManager<Student> userManager, IToastNotification toastNotification) {
            _context = context;
			_toastNotification= toastNotification;
			_userManager = userManager;
        }
 
        public async Task<IActionResult> Index() {
            IEnumerable<Course> _courses=_context.Courses.ToList();
			IEnumerable<Instructor> _instructors=_context.Instructors.ToList();
            var model = new HomeViewModel() {
                courses = _courses,
                instructors = _instructors
			};
            return View(model);
        }
		[Authorize]
        public async Task<IActionResult> CourseDetails(int? id) {
			if (id == 0 || _context.Instructors == null) {
				return NotFound();
			}

			var course = await _context.Courses
				.Where(e => e.Id == id)
				.Include(e => e.Sessions)
				.ThenInclude(e => e.instructor)
				.FirstAsync();
				
			if (course == null) {
				return NotFound();
			}

			return View(course);
        }

		public async Task<IActionResult> Instructors()
		{
			var instructors = await _context.Instructors.ToListAsync();
			return View(instructors);
		}

        public async Task <IActionResult> InstructorDetails(int? id) {
			if (id == null || _context.Instructors == null) {
				return NotFound();
			}
			var instructor = await _context.Instructors
				.FirstOrDefaultAsync(m => m.Id == id);

			if (instructor == null) {
				return NotFound();
			}
			return View(instructor);
        }

		public async Task<IActionResult> Courses()
		{
			var courses = await _context.Courses
				.Include(e => e.Sessions)
				.ThenInclude(e => e.instructor)
				.ToListAsync();

			return View(courses);
		}


		public IActionResult AboutUs()
		{
			return View();
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}