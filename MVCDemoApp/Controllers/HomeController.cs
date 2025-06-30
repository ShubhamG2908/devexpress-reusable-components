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
		public IActionResult CustomFilter()
		{
			return View();
		}

		public IActionResult CustomPageDataTable()
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

		[HttpPost]
		public async Task<IActionResult> UploadImage()
		{
			var file = Request.Form.Files[0]; // CA1826 fix: Use indexer instead of LINQ method
			if (file == null || file.Length == 0)
				return BadRequest("No file uploaded.");

			var allowedTypes = new[] { "image/jpg", "image/jpeg", "image/png", "image/gif" };
			const long maxSize = 2 * 1024 * 1024;

			if (!allowedTypes.Contains(file.ContentType))
				return BadRequest("Invalid file type.");

			if (file.Length > maxSize)
				return BadRequest("File too large (max 2MB).");

			var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
			Directory.CreateDirectory(uploadsPath);

			//var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) +
			var fileName = Path.GetFileNameWithoutExtension(Path.GetFileName(file.FileName)) +
Path.GetExtension(file.FileName);
			var filePath = Path.Combine(uploadsPath, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			var imageUrl = Url.Content("~/uploads/" + fileName);
			return Json(new { url = imageUrl });
		}
	}
}
