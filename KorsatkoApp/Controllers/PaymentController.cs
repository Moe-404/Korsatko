using Microsoft.AspNetCore.Mvc;

namespace KorsatkoApp.Controllers {
	public class PaymentController : Controller {
		public IActionResult Index() {
			return View();
		}
	}
}
