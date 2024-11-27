using Microsoft.AspNetCore.Mvc;

namespace MVCDemoApp.Controllers
{
    public class DataGridController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
