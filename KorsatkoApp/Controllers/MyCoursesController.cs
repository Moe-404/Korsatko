using KorsatkoApp.Areas.Admin.Models;
using KorsatkoApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KorsatkoApp.Controllers {
	[Authorize]
	public class MyCoursesController : Controller {
		private readonly ApplicationDbContext _context;

		public MyCoursesController(ApplicationDbContext context) {
			_context = context;
		}
		public IActionResult Index() {
			var enrolls = _context.Enrollments
				.Include(e => e.student)
				.Include(e => e.session)
				.ThenInclude(e => e.course)
				//.Include(e => e.session)
				//.ThenInclude(e => e.instructor)
				.Where(e => e.student.Email == User.Identity.Name).ToList();
			return View(enrolls);
		}

		public async Task<IActionResult> Sessions()
		{
			IEnumerable<Enrollment> enrolls = await _context.Enrollments
				.Include(e => e.student)
				.Include(e => e.session)
				.ThenInclude(e => e.course)
				.Include(e => e.session)
				.ThenInclude(e => e.instructor)
				.Where(e => e.student.Email == User.Identity.Name).ToListAsync();

			return View(enrolls);
		}
	}
}
