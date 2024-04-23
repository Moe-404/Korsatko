using KorsatkoApp.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace KorsatkoApp.Areas.Admin.Controllers {

    [Area("Admin")]
    public class HomeController : Controller {
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<Student> _userManager;

        public HomeController(IToastNotification toastNotification, UserManager<Student> userManager) {
            _toastNotification = toastNotification;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index() {
            var student = await _userManager.FindByEmailAsync(User.Identity?.Name!);

            //Success
            _toastNotification.AddInfoToastMessage("مرحبا بك , "+ student.FullName);
            return View();
        }

    }
}
