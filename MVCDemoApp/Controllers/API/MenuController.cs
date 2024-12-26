using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ModelsProject;
using MVCDemoApp.Services;
using Newtonsoft.Json;

namespace MVCDemoApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly MenuServices _menuServices;

        public MenuController(IMemoryCache memoryCache, MenuServices menuServices)
        {
            _memoryCache = memoryCache;
            _menuServices = menuServices;
        }

        private List<MenuItem> GetMenuItemsListFromCache()
        {
			List<MenuItem> menus;

			//if (_memoryCache.TryGetValue("menus", out menus))
			//{
			//	return menus;
			//}

            menus = _menuServices.GetAll().ToList();

			_memoryCache.Set("menus", menus, new MemoryCacheEntryOptions()
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1)
			});

			return menus;
		}
		
        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            var result = DataSourceLoader.Load(GetMenuItemsListFromCache(), loadOptions);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var menuItem = JsonConvert.DeserializeObject<MenuItem>(values);
            var menus = GetMenuItemsListFromCache();
            menuItem.Id = menus.Count + 1;
            try
            {
                _menuServices.Add(menuItem);
            }
            catch (Exception ex)
            {
            }

            _memoryCache.Set("menus", menus, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

            return CreatedAtAction(nameof(Get), new { id = menuItem.Id }, menuItem);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm] int key, [FromForm] string values)
        {
            var menuItem = JsonConvert.DeserializeObject<MenuItem>(values);
            var ExistingMenu = _menuServices.GetById(key);

            if (ExistingMenu == null)
            {
                return NotFound();
            }
            menuItem.Id = ExistingMenu.Id;
            menuItem.Text = !string.IsNullOrWhiteSpace(menuItem.Text) ? menuItem.Text : ExistingMenu.Text;
            menuItem.Icon = !string.IsNullOrWhiteSpace(menuItem.Icon) ? menuItem.Icon : ExistingMenu.Icon;
            menuItem.Url = !string.IsNullOrWhiteSpace(menuItem.Url) ? menuItem.Url : ExistingMenu.Url;
            
            _menuServices.Update(menuItem);
            
            
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] int key)
        {
            _menuServices.Delete(key);
            var menus = GetMenuItemsListFromCache();
            _memoryCache.Set("menus", menus, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });
            return NoContent();
        }
    }
}
