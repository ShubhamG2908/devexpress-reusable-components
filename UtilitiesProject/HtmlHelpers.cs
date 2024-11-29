using DevExtreme.AspNet.Mvc.Builders;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DevExtreme.AspNet.Mvc.Factories;
using ModelsProject;
[assembly: InternalsVisibleTo("MVCDemoApp")]

namespace UtilitiesProject
{
	//Extend html components for generic builders or contents (e.g., a strongly-typed DataGrid)
	internal static class HtmlHelpers
	{
		#region Button html

		/// <summary>
		/// Html rendering with specific html and return with ButtonBuilder of Devexpress. 
		/// </summary>
		/// <param name="Html"></param>
		/// <param name="buttonText"></param>
		/// <returns></returns>
		internal static ButtonBuilder IPSButton(this IHtmlHelper html, string buttonText, string buttonId)
		{
			ButtonBuilder btn = html.DevExtreme().Button().ID(buttonId).Text(buttonText).Type(ButtonType.Success);
			return btn;
		}

		#endregion

		#region Datagrid html
		/// <summary>
		/// Html rendering with specific html and return with datagridbuilder
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="grid"></param>
		/// <param name="model"></param>
		/// <param name="datasourceCollection"></param>
		/// <param name="listColumns"></param>
		/// <param name="controllerName"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		internal static DataGridBuilder<T> IPSDataGrid<T>(this IHtmlHelper grid, object model, List<T> datasourceCollection = null, List<ColumnSettings> listColumns = null, string controllerName = "", string key = "")
		{
			DataGridBuilder<T> dgb = grid.DevExtreme().DataGrid<T>()
				.DataSource(datasourceCollection)
				.IPSDataGridSetDefaults(datasourceCollection, listColumns, controllerName, key);
			return dgb;
		}
		#endregion

		#region Date Box with model pass

		/// <summary>
		/// Date with model pass
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="htmlHelper"></param>
		/// <param name="expression"></param>
		/// <param name="ApplyValueMode"></param>
		/// <returns></returns>
		internal static IHtmlContent IPSDateBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, EditorApplyValueMode ApplyValueMode)
		{
			var res = htmlHelper.DevExtreme().DateBoxFor(expression).ApplyValueMode(ApplyValueMode).ToString();
			return new HtmlString(res);
		}

		#endregion

		#region Date without model pass
		
		/// <summary>
		/// Date without model pass
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <param name="htmlHelper"></param>
		/// <param name="applyValueMode"></param>
		/// <returns></returns>
		internal static IHtmlContent IPSDateBox<TModel>(this IHtmlHelper<TModel> htmlHelper, EditorApplyValueMode applyValueMode)
		{
			var res = htmlHelper.DevExtreme().DateBox().ApplyValueMode(applyValueMode).ToString();
			return new HtmlString(res);
		}

		#endregion

		#region Synchronized date boxes

		/// <summary>                   
		/// Synchronized Date Boxes
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		internal static IHtmlContent SynchronizedDateBoxes(this IHtmlHelper html)
		{
			string startDateBox = html.DevExtreme().DateBox().ID("startDate")
				.Value(DateTime.Now)
				.OnValueChanged("function(e){  var endDate= $('#endDate').dxDateBox('instance'); endDate.option('min', e.value);}")
				.ToString();
			string endDateBox = html.DevExtreme().DateBox().ID("endDate")
				 .Value(DateTime.Now)

				 .OnValueChanged("function(e){  var startDate= $('#startDate').dxDateBox('instance'); endDate.option('max', e.value);}")
				.ToString();
			string res = startDateBox + endDateBox;
			return new HtmlString(res);
		}

		#endregion

		
	}
}
