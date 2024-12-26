using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ModelsProject;
using MVCDemoApp.Services;
using MVCDemoApp.Utilities;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;

namespace MVCDemoApp.Controllers
{
	public class MenuItemController : Controller
	{
		public readonly MenuServices _menuServices;

		public MenuItemController()
		{
			_menuServices = new MenuServices();
		}

		// GET: MenuItemController
		public ActionResult Index()
		{
			return View(_menuServices.GetAll());
		}

		// GET: MenuItemController/Details/5
		public ActionResult Details(int id)
		{
			return View(_menuServices.GetById(id));
		}

		// GET: MenuItemController/Create
		public ActionResult Create()
		{
			if (Request.Method.ToUpper() == "POST")
			{
				var modelAdd = new MenuItem
				{
					Text = Request.Form["Text"].ToString(),
					Icon = Request.Form["Icon"].ToString(),
					Url = Request.Form["Url"].ToString(),
				};

				if (ModelState.IsValid)
				{
					_menuServices.Add(modelAdd);
					return RedirectToAction("Index");
				}
				return View(modelAdd);
			}
			else
			{
				var model = new MenuItem();
				return View(model);
			}
		}

		// GET: MenuItemController/Edit/5
		public ActionResult Edit(int id)
		{
			if (Request.Method.ToUpper() == "POST")
			{
				var existingValue = _menuServices.GetById(id);

				existingValue.Text = Request.Form["Text"].ToString();
				existingValue.Icon = Request.Form["Icon"].ToString();
				existingValue.Url = Request.Form["Url"].ToString();
				existingValue.Disabled = Convert.ToBoolean(Request.Form["Disabled"][0].ToString());


				if (ModelState.IsValid)
				{
					_menuServices.Update(existingValue);
					return RedirectToAction("Index");
				}
				return View(existingValue);
			}
			else
			{
				return View(_menuServices.GetById(id));
			}
		}


		// GET: MenuItemController/Delete/5
		[HttpGet]
		public ActionResult Delete(int id)
		{
			if (Request.Method.ToUpper() == "POST")
			{
				if (ModelState.IsValid)
				{
					_menuServices.Delete(id);
					return RedirectToAction("Index");
				}
				return View();
			}
			else
			{
				return View(_menuServices.GetById(id));
			}
		}

		#region My CRUD opertation with API or normal way.

		private List<MenuItem> GetMenuItemsList(DataSourceLoadOptions loadOptions = null)
		{
			List<MenuItem> menus;

			if (loadOptions != null && loadOptions.Filter != null && loadOptions.Filter.Count > 0)
			{
				menus = _menuServices.GetDataByFilter(loadOptions.Filter).ToList();
			}
			else
			{
				menus = _menuServices.GetAll().ToList();
			}

			return menus;
		}

		[HttpGet]
		public IActionResult Get(DataSourceLoadOptions loadOptions)
		{
			var result = DataSourceLoader.Load(GetMenuItemsList(loadOptions), loadOptions);
			return Ok(result);
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromForm] string values)
		{
			var menuItem = JsonConvert.DeserializeObject<MenuItem>(values);
			try
			{
				await _menuServices.AddAsync(menuItem);
			}
			catch (Exception ex)
			{
			}
			return CreatedAtAction(nameof(Get), new { id = menuItem.Id }, menuItem); 
		}

		[HttpPut]
		public async Task<IActionResult> Put([FromForm] int key, [FromForm] string values)
		{
			var menuItem = JsonConvert.DeserializeObject<MenuItem>(values);
			try
			{
				menuItem.Id = key;
				var returnValue = _menuServices.UpdateAsync(menuItem).Result;
				if (returnValue < 0)
				{
					return NotFound();
				}
			}
			catch (Exception ex)
			{
			}

			return NoContent();
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteRow([FromForm] int key)
		{
			_menuServices.Delete(key);
			var menus = GetMenuItemsList();
			
			return NoContent();
		}


		#endregion




	}
}
