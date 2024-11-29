using Microsoft.AspNetCore.Mvc;

namespace MVCDemoApp.Controllers
{
	public class ComponentsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult ButtonComponents()
		{
			return PartialView("Button/_ButtonComponents.cshtml");
		}
	}
}
