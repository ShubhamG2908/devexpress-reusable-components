using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ModelsProject;
using MVCDemoApp.Data;
using MVCDemoApp.Models;
using MVCDemoApp.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        #region Components

        public IActionResult Buttons()
        {
            return View();
        }
        public IActionResult DataGrid()
        {
            return View();
        }

        public IActionResult Tabs()
        {
            return View();
        }

        public IActionResult NavMenu()
        {
            var model = MenuData.CustomMenus;
            return View(model);
        }
        public IActionResult InnerMenu()
        {
            var model = MenuData.CustomMenus;
            return View(model);
        }
        #endregion

        public IActionResult ButtonComponents()
        {
            return PartialView("Button/_ButtonComponents.cshtml");
        }

        #region Partial View DataGrid call

        private List<Schools> GetSchoolListFromCache(DataSourceLoadOptions loadOptions)
        {

            //if (_memoryCache.TryGetValue("schools", out schools))
            //{
            //    return schools;
            //}

            var schools = SchoolData.SchoolsList.AsQueryable();
            // Apply filters using a helper function
            if (loadOptions.Filter != null)
            {
                schools = ReflectionHelper.ApplyFilter<Schools>(schools, loadOptions.Filter);
            }

            _memoryCache.Set("schools", schools, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1000)
            });

            return schools.ToList();
        }

        [HttpGet]
        public IActionResult GetSchools(DataSourceLoadOptions loadOptions, string searchQuery = "", string filterText = "")
        {
            var result = DataSourceLoader.Load(GetSchoolListFromCache(loadOptions), loadOptions);
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
        [HttpGet]
        public object GetTabPanelList(DataSourceLoadOptions options)
        {
            var tabPanelItems = TabPanelData.TabPanelItems.ToList();
            return DataSourceLoader.Load(tabPanelItems, options);
        }


        [HttpGet]
        public object GetMenus(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(MenuData.CustomMenus, loadOptions);
        }

        #region Custom DataGrid

        // Provides data to the grid with server-side processing
        public object GetSchoolDataWithSearchText(string searchQuery = "")
        {
            var schools = SchoolData.SchoolsList;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                schools = schools
                    .Where(p => p.Name.Contains(searchQuery))
                .ToList();
            }

            return Ok(schools);
        }

        #endregion

        #endregion

        #region Binding methods or fucntions

        public List<string> GetSchoolColumnNames()
        {

            var data = typeof(Schools).GetModelColName();
            var cols = ReflectionHelper.GetModelColumns(typeof(Schools));
            return ReflectionHelper.BindDropDownBoxOnlyColumnNameForDynamicModel(SchoolData.SchoolsList);
        }

        #endregion


    }
}
