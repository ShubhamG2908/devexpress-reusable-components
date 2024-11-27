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
		public static ButtonBuilder SetDefaults(this ButtonBuilder button)
		{
			button.Type(ButtonType.Success);
			button.Width(300);
			return button;
		}
	}
}
