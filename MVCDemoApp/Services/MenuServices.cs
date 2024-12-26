using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ModelsProject;
using MVCDemoApp.Repository;
using System.Data;

namespace MVCDemoApp.Services
{
    public class MenuServices
    {
        public readonly MenuRepository _menuRepository;
        internal MenuServices() 
        {
            _menuRepository = new MenuRepository();
        }

        internal IEnumerable<MenuItem> GetAll()
        {
            return _menuRepository.GetAll();
        }
		internal IEnumerable<MenuItem> GetDataByFilter(object filter)
		{
			return _menuRepository.GetDataByFilter(filter);
		}

		internal MenuItem GetById(int id)
        {
            return _menuRepository.GetById(id);
        }
		internal async Task<MenuItem> GetByIdAsync(int id)
		{
			return await _menuRepository.GetByIdAsync(id);
		}

		internal int Add(MenuItem menuItem)
        {
            return _menuRepository.Add(menuItem);
        }

		internal async Task<int> AddAsync(MenuItem menuItem)
		{
			return await _menuRepository.AddAsync(menuItem);
		}
		internal int Update(MenuItem menuItem)
        {
			var ExistingMenu = GetById(menuItem.Id);

			if (ExistingMenu == null)
			{
				return -1;
			}
			menuItem.Text = !string.IsNullOrWhiteSpace(menuItem.Text) ? menuItem.Text : ExistingMenu.Text;
			menuItem.Icon = !string.IsNullOrWhiteSpace(menuItem.Icon) ? menuItem.Icon : ExistingMenu.Icon;
			menuItem.Url = !string.IsNullOrWhiteSpace(menuItem.Url) ? menuItem.Url : ExistingMenu.Url;

			return _menuRepository.Update(menuItem);
        }

		internal async Task<int> UpdateAsync(MenuItem menuItem)
		{
			var ExistingMenu = await GetByIdAsync(menuItem.Id);

			if (ExistingMenu == null)
			{
				return -1;
			}
			menuItem.Text = !string.IsNullOrWhiteSpace(menuItem.Text) ? menuItem.Text : ExistingMenu.Text;
			menuItem.Icon = !string.IsNullOrWhiteSpace(menuItem.Icon) ? menuItem.Icon : ExistingMenu.Icon;
			menuItem.Url = !string.IsNullOrWhiteSpace(menuItem.Url) ? menuItem.Url : ExistingMenu.Url;

			return await _menuRepository.UpdateAsync(menuItem);
		}

		internal bool Delete(int id)
        {
            return _menuRepository.Delete(id);
        }


    }
}
