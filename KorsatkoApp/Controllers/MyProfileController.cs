using KorsatkoApp.Data;
using KorsatkoApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KorsatkoApp.Controllers
{
    [Authorize]
    public class MyProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyProfileController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: MyProfileController
        public ActionResult Index()
        {
            var student = _context.Users
                .Where(e => e.Email == User.Identity.Name)
                .First();

            return View(student);
        }
    }
}
