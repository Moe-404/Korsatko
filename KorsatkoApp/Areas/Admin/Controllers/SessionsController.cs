using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KorsatkoApp.Data;
using KorsatkoApp.Models;
using NToastNotify;
using NToastNotify;
using KorsatkoApp.Areas.Admin.Models;

namespace KorsatkoApp.Areas.Admin.Controllers {
    [Area("Admin")]
    public class SessionsController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;

        public SessionsController(ApplicationDbContext context, IToastNotification toastNotification) {
            _toastNotification = toastNotification;
            _context = context;
        }

        // GET: Admin/Sessions
        public async Task<IActionResult> Index() {
            var applicationDbContext = _context.Sessions.Include(s => s.course).Include(s => s.instructor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Sessions/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null || _context.Sessions == null) {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.course)
                .Include(s => s.instructor)
                .Include(s => s.Enrollments)
               .ThenInclude(s => s.student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (session == null) return NotFound();

            return View(session);
        }

        // GET: Admin/Sessions/Create
        public IActionResult Create() {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "FullName");
            return View();
        }

        // POST: Admin/Sessions/Create
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,EndDate,startTime,EndTime,Location,Limit,IsAvailable,PriceRate,CourseId,InstructorId,AddedOn")] Session session) {

            if (ModelState.IsValid) {
                _context.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", session.CourseId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "FullName", session.InstructorId);
            return View(session);
        }

        // GET: Admin/Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null || _context.Sessions == null) {
                return NotFound();
            }

            var session = await _context.Sessions.FindAsync(id);
            if (session == null) {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", session.CourseId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "FullName", session.InstructorId);
            return View(session);
        }

        // POST: Admin/Sessions/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,EndDate,startTime,EndTime,Location,Limit,IsAvailable,PriceRate,CourseId,InstructorId,AddedOn")] Session session) {
            if (id != session.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(session);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!SessionExists(session.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", session.CourseId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "Email", session.InstructorId);
            return View(session);
        }

        // GET: Admin/Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null || _context.Sessions == null) {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.course)
                .Include(s => s.instructor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null) {
                return NotFound();
            }

            return View(session);
        }

        // POST: Admin/Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            if (_context.Sessions == null) {
                return Problem("Entity set 'ApplicationDbContext.Sessions'  is null.");
            }
            var session = await _context.Sessions.FindAsync(id);
            if (session != null) {
                _context.Sessions.Remove(session);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(int id) {
            return (_context.Sessions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
