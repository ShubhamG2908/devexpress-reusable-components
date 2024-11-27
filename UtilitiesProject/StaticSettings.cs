using DevExtreme.AspNet.Mvc.Builders;
using DevExtreme.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("MVCDemoApp")]

namespace UtilitiesProject
{
	internal static class StaticSettings
	{
		#region SetDefault for button, Datagrid, grid pagination

		public static ButtonBuilder SetDefaults(this ButtonBuilder button)
		{
			button.Type(ButtonType.Success);
			button.Width(300);
			return button;
		}

		public static DataGridBuilder<T> SetDefaults<T>(this DataGridBuilder<T> grid)
		{
			grid.GroupPanel(g => g.Visible(true));
			return grid;
		} 
		#endregion
		public static Action<DataGridPagerBuilder> DefaultPagerConfig = (builder) => {
			builder.ShowPageSizeSelector(true);
			builder.AllowedPageSizes(new[] { 5, 10, 20 });
			builder.ShowInfo(true);
		};
	}
}
