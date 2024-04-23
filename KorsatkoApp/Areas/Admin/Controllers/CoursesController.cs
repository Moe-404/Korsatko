using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KorsatkoApp.Data;
using KorsatkoApp.Areas.Admin.Models;
using NToastNotify;
using KorsatkoApp.Areas.Admin.ViewModels;

namespace KorsatkoApp.Areas.Admin.Controllers {
	[Area("Admin")]
	public class CoursesController : Controller {
		private readonly ApplicationDbContext _context;
		private readonly IToastNotification toastNotification;
		private readonly IWebHostEnvironment webHostEnvironment;

		public CoursesController(ApplicationDbContext context, IToastNotification toastNotification, IWebHostEnvironment hostEnvironment) {
			_context = context;
			webHostEnvironment = hostEnvironment;
			this.toastNotification = toastNotification; //Marly: injecting toastNotification
		}

		// GET: Admin/Courses
		public async Task<IActionResult> Index() {
			return _context.Courses != null ?
						View(await _context.Courses.ToListAsync()) :
						Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
		}

		// GET: Admin/Courses/Details/5
		public async Task<IActionResult> Details(int? id) {
			if (id == null || _context.Courses == null) {
				return NotFound();
			}

			var course = await _context.Courses
				.FirstOrDefaultAsync(m => m.Id == id);


			if (course == null) {
				return NotFound();
			}

			//the same as course
			var model = new CourseViewModel() {
				Id = course.Id,
				Name = course.Name,
				Description = course.Description,
				Prerequisites = course.Prerequisites,
				Price = course.Price,
				ExistingImage = course.Picture,
				AddedOn = course.AddedOn
			};


			return View(model);
		}

		// GET: Admin/Courses/Create
		public IActionResult Create() {
			return View();
		}

		// POST: Admin/Courses/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CourseViewModel model) {
			if (ModelState.IsValid) {
				string uniqueFileName = ProcessUploadedFile(model);

				Course course = new Course {
					Id = model.Id,
					Name = model.Name,
					Description = model.Description,
					Prerequisites = model.Prerequisites,
					Price = model.Price,
					Picture = uniqueFileName,
					AddedOn = DateTime.Now
				};

				_context.Add(course);
				await _context.SaveChangesAsync();
				toastNotification.AddSuccessToastMessage("تمت اضافة الكورس بنجاح"); //Marly: notification
				return RedirectToAction(nameof(Index));
			}
			toastNotification.AddErrorToastMessage("برجاء التأكد من صحة البيانات");
			return View(model);
		}

		// GET: Admin/Courses/Edit/5
		public async Task<IActionResult> Edit(int? id) {
			if (id == null || _context.Courses == null) {
				return NotFound();
			}

			var course = await _context.Courses.FindAsync(id);

			var courseViewModel = new CourseViewModel() {
				Id = course.Id,
				Name = course.Name,
				Description = course.Description,
				Prerequisites = course.Prerequisites,
				Price = course.Price,
				ExistingImage = course.Picture
			};

			if (course == null) {
				return NotFound();
			}

			return View(courseViewModel);
		}

		// POST: Admin/Courses/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, CourseViewModel model) {
			if (ModelState.IsValid) {
				var course = await _context.Courses.FindAsync(model.Id);
				course.Name = model.Name;
				course.Description = model.Description;
				course.Prerequisites = model.Prerequisites;
				course.Price = model.Price;

				if (model.CoursePicture != null) {
					course.Picture = ProcessUploadedFile(model);
					if (model.ExistingImage != null) {
						string filePath = Path.Combine(webHostEnvironment.WebRootPath, "Uploads", model.ExistingImage);
						System.IO.File.Delete(filePath);
					}
				}
				_context.Update(course);
				await _context.SaveChangesAsync();
				toastNotification.AddSuccessToastMessage("تم تعديل الكورس بنجاح");//Marly: notification
				return RedirectToAction(nameof(Index));
			}
			return View();
		}

		// GET: Admin/Courses/Delete/5
		public async Task<IActionResult> Delete(int? id) {
			if (id == null || _context.Courses == null) {
				return NotFound();
			}

			var course = await _context.Courses
				.FirstOrDefaultAsync(m => m.Id == id);

			var courseViewModel = new CourseViewModel() {
				Id = course.Id,
				Name = course.Name,
				Description = course.Description,
				Prerequisites = course.Prerequisites,
				Price = course.Price,
				ExistingImage = course.Picture,
				AddedOn = course.AddedOn
			};

			if (course == null) {
				return NotFound();
			}

			return View(courseViewModel);
		}

		// POST: Admin/Courses/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id) {
			var course = await _context.Courses.FindAsync(id);
			var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", course.Picture);
			_context.Courses.Remove(course);
			toastNotification.AddSuccessToastMessage("تم حذف  الكورس بنجاح");// Basmalla & Rewan : notification
			if (await _context.SaveChangesAsync() > 0) {
				if (System.IO.File.Exists(CurrentImage)) {
					System.IO.File.Delete(CurrentImage);
				}
			}
			return RedirectToAction(nameof(Index));
		}

		private bool CourseExists(int id) {
			return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
		}

		private string ProcessUploadedFile(CourseViewModel model) {

			string uniqueFileName = null;
			if (model.CoursePicture != null) {
				string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
				uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CoursePicture.FileName;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create)) {
					model.CoursePicture.CopyTo(fileStream);
				}
			}

			return uniqueFileName;
		}
	}
}
