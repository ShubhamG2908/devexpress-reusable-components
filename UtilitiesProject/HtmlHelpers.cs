using DevExtreme.AspNet.Mvc.Builders;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("MVCDemoApp")]

namespace UtilitiesProject
{
	//Extend html components for generic builders or contents (e.g., a strongly-typed DataGrid)
	internal static class HtmlHelpers
	{
		/// <summary>
		/// Html rendering with specific html and return with ButtonBuilder of Devexpress. 
		/// </summary>
		/// <param name="Html"></param>
		/// <param name="ButtonText"></param>
		/// <returns></returns>
		internal static ButtonBuilder IPSButton(this IHtmlHelper Html, string ButtonText)
		{
			ButtonBuilder btn = Html.DevExtreme().Button().Text(ButtonText).Type(ButtonType.Success);
			return btn;
		}
		

	}
}
