using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ModelsProject;
using MVCDemoApp.Data;
using MVCDemoApp.Models;
using Newtonsoft.Json;

namespace MVCDemoApp.Controllers
{
	public class ComponentsController : Controller
	{
        private readonly IMemoryCache _memoryCache;

        public ComponentsController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
		{
			return View();
		}

        public IActionResult Buttons()
        {
            return View();
        }
        public IActionResult DataGrid()
        {
            return View();
        }

        public IActionResult ButtonComponents()
		{
			return PartialView("Button/_ButtonComponents.cshtml");
		}

        #region Partial View DataGrid call

        private List<Schools> GetSchoolListFromCache()
        {
            List<Schools> schools;

            if (_memoryCache.TryGetValue("schools", out schools))
            {
                return schools;
            }


            schools = SchoolData.SchoolsList;

            _memoryCache.Set("schools", schools, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

            return schools;
        }

        [HttpGet]
        public IActionResult GetSchools(DataSourceLoadOptions loadOptions)
        {
            var result = DataSourceLoader.Load(GetSchoolListFromCache(), loadOptions);
            return Ok(result);
        }

        [HttpGet]
        public object GetClassroomsBySchool(int schoolId, DataSourceLoadOptions options)
        {
            var classRooms = ClassroomData.ClassroomsList.Where(w => w.SchoolId == schoolId).ToList();
            return DataSourceLoader.Load(classRooms, options);
        }

        [HttpGet]
        public object GetTeachersByClassroom(int classRoomId, DataSourceLoadOptions options)
        {
            var teachers = TeacherData.TeachersList.Where(w => w.ClassroomId == classRoomId).ToList();
            return DataSourceLoader.Load(teachers, options);
        }

        #endregion
    }
}
