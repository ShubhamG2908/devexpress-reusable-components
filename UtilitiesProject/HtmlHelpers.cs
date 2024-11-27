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
		/// <param name="buttonText"></param>
		/// <returns></returns>
		internal static ButtonBuilder IPSButton(this IHtmlHelper Html, string buttonText,string buttonId)
		{
			ButtonBuilder btn = Html.DevExtreme().Button().ID(buttonId).Text(buttonText).Type(ButtonType.Success);
			return btn;
		}
		internal static IHtmlContent IPSDateBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, EditorApplyValueMode ApplyValueMode)
		{
			var res = htmlHelper.DevExtreme().DateBoxFor(expression).ApplyValueMode(ApplyValueMode).ToString();
			return new HtmlString(res);
		}

		internal static IHtmlContent IPSDateBox<TModel>(this IHtmlHelper<TModel> htmlHelper, EditorApplyValueMode ApplyValueMode)
		{
			var res = htmlHelper.DevExtreme().DateBox().ApplyValueMode(ApplyValueMode).ToString();
			return new HtmlString(res);
		}

	}
}
