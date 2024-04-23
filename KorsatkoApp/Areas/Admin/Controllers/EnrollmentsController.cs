using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KorsatkoApp.Areas.Admin.Models;
using KorsatkoApp.Data;
using NToastNotify;

namespace KorsatkoApp.Areas.Admin.Controllers {
	[Area("Admin")]
	public class EnrollmentsController : Controller {
		private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;

        public EnrollmentsController(ApplicationDbContext context, IToastNotification toastNotification) {
			_context = context;
			_toastNotification = toastNotification;
		}

		// GET: Admin/Enrollments
		public async Task<IActionResult> Index() {
			var applicationDbContext = _context.Enrollments
				.Include(e => e.student)
				.Include(e => e.session)
				.ThenInclude(e => e.course)
				.Include(e => e.session)
				.ThenInclude(e => e.instructor);
			var eList = await applicationDbContext.ToListAsync();
			return View(eList);
		}

		// GET: Admin/Enrollments/Details/5
		public async Task<IActionResult> Details(string id) {
			if (id == null || _context.Enrollments == null) {
				return NotFound();
			}

			var enrollment = await _context.Enrollments
				.Include(e => e.session)
				.Include(e => e.student)
				.FirstOrDefaultAsync(m => m.EnrollmentNumber == id);
			if (enrollment == null) {
				return NotFound();
			}

			return View(enrollment);
		}

		// GET: Admin/Enrollments/Create
		public IActionResult Create() {
			var customValues = new List<SelectListItem>();
			var sessions = _context.Sessions.Include(e => e.course).Include(e => e.instructor).ToList();

			foreach (var s in sessions) {
				string sessionName = s.course.Name + " - " + s.instructor.FullName + " - " + s.Location;
				customValues.Add(new SelectListItem { Value = s.Id.ToString(), Text = sessionName });
			}


			var SessionselectList = new SelectList(customValues, "Value", "Text");
			ViewData["SessionId"] = SessionselectList;
			ViewData["StudentId"] = new SelectList(_context.Users, "Id", "FullName");
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("EnrollmentNumber,StudentId,SessionId,PaymentStatus,AddedOn")] Enrollment enrollment) {
			
			//check for dup in enrollments table
			bool alreadyexist = false;
			var eList = _context.Enrollments.ToList();
			foreach(var e in eList) {
				if(e.SessionId==enrollment.SessionId && e.StudentId == enrollment.StudentId) {
					alreadyexist = true;
					break;
				}
			}
			if (alreadyexist) {
				_toastNotification.AddErrorToastMessage("الطالب مشترك في الميعاد بالفعل");
			}
            if (ModelState.IsValid && !alreadyexist) {
				enrollment.EnrollmentNumber = Guid.NewGuid().ToString();
				enrollment.AddedOn = DateTime.Now;
				_context.Add(enrollment);
				await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("تمت اضافة اللإشتراك بنجاح");
                return RedirectToAction(nameof(Index));
			}
            _toastNotification.AddErrorToastMessage("برجاء التأكد من صحة البيانات");
            var customValues = new List<SelectListItem>();
            var sessions = _context.Sessions.Include(e => e.course).Include(e => e.instructor).ToList();

            foreach (var s in sessions) {
                string sessionName = s.course.Name + " - " + s.instructor.FullName + " - " + s.Location;
                customValues.Add(new SelectListItem { Value = s.Id.ToString(), Text = sessionName });
            }

			ViewData["SessionId"] = customValues;

            ViewData["StudentId"] = new SelectList(_context.Users, "Id", "FullName", enrollment.StudentId);
			return View(enrollment);
		}

		// GET: Admin/Enrollments/Edit/5
		public async Task<IActionResult> Edit(string id) {
			if (id == null || _context.Enrollments == null) {
				return NotFound();
			}

			var enrollment = await _context.Enrollments.FirstOrDefaultAsync(e => e.EnrollmentNumber == id);
			if (enrollment == null) {
				return NotFound();
			}
			ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", enrollment.SessionId);
			ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Id", enrollment.StudentId);
			return View(enrollment);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("EnrollmentNumber,StudentId,SessionId,PaymentStatus,AddedOn")] Enrollment enrollment) {
			if (id != enrollment.EnrollmentNumber) {
				return NotFound();
			}

			if (ModelState.IsValid) {
				try {
					_context.Update(enrollment);
					await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("تم تعديل الإشتراك بنجاح");
                } catch (DbUpdateConcurrencyException) {
					if (!EnrollmentExists(enrollment.EnrollmentNumber)) {
						return NotFound();
					} else {
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Id", enrollment.SessionId);
			ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Id", enrollment.StudentId);
			return View(enrollment);
		}

		// GET: Admin/Enrollments/Delete/5
		public async Task<IActionResult> Delete(string id) {
			if (id == null || _context.Enrollments == null) {
				return NotFound();
			}

			var enrollment = await _context.Enrollments
				.Include(e => e.session)
				.Include(e => e.student)
				.FirstOrDefaultAsync(m => m.EnrollmentNumber == id);
			if (enrollment == null) {
				return NotFound();
			}
            
            return View(enrollment);
		}

		// POST: Admin/Enrollments/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id) {
			if (_context.Enrollments == null) {
				return Problem("Entity set 'ApplicationDbContext.Enrollments'  is null.");
			}
			var enrollment = await _context.Enrollments.FirstOrDefaultAsync(e => e.EnrollmentNumber == id);
			if (enrollment != null) {
				_context.Enrollments.Remove(enrollment);
			}

			await _context.SaveChangesAsync();
            _toastNotification.AddSuccessToastMessage("تم حذف الإشتراك بنجاح");
            return RedirectToAction(nameof(Index));
		}

		private bool EnrollmentExists(string id) {
			return (_context.Enrollments?.Any(e => e.EnrollmentNumber == id)).GetValueOrDefault();
		}
	}
}
