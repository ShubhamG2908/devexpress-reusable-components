using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using MVCDemoApp.Data;
using System.Diagnostics;
using ModelsProject;


namespace MVCDemoApp.Controllers
{
	//[Route("api/[controller]")]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			var model = SchoolData.SchoolsList;
			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		public IActionResult Get(DataSourceLoadOptions loadOptions)
		{
			return Ok(DataSourceLoader.Load(SchoolData.SchoolsList, loadOptions));
		}
	}
}
