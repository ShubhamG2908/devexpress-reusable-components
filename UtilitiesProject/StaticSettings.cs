using DevExtreme.AspNet.Mvc.Builders;
using DevExtreme.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DevExtreme.AspNet.Mvc.Factories;
using ModelsProject;
using Microsoft.AspNetCore.Mvc.Rendering;
[assembly: InternalsVisibleTo("MVCDemoApp")]

namespace UtilitiesProject
{
	/// <summary>
	/// All static component setting with default configuration
	/// </summary>
	internal static class StaticSettings
	{
		#region Components Settings

		#region Button component Settings

		/// <summary>
		/// Default setting set for button
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		internal static ButtonBuilder IPSButtonSetDefaults(this ButtonBuilder button)
		{
			button.Type(ButtonType.Success);
			button.Width(300);
			return button;
		}

		#endregion

		#region Datagrid component Settings

		/// <summary>
		/// Default setting set for Data grid
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="grid"></param>
		/// <param name="datasourceCollection"></param>
		/// <param name="listColumns"></param>
		/// <param name="controllerName"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		internal static DataGridBuilder<T> IPSDataGridSetDefaults<T>(this DataGridBuilder<T> grid, List<T> datasourceCollection = null, List<ColumnSettings> listColumns = null, string controllerName = "", string key = "")
		{
			//this will use while calling from controller methods and key.
			if (!string.IsNullOrWhiteSpace(controllerName) && !string.IsNullOrWhiteSpace(key))
			{
				grid.DataSource(d => d.Mvc().Controller(controllerName).LoadAction("Get").Key(key));
			}
			//this will use while calling from list of collection that time passed that directly to datasource.
			if (datasourceCollection != null && datasourceCollection.Count > 0) { grid.DataSource(datasourceCollection); }

			//default configuration. If need any changes then pass param and call it whenever use.
			grid.FocusedRowEnabled(true);
			grid.FocusedRowIndex(0);
			grid.GroupPanel(g => g.Visible(true));
			grid.SearchPanel(s => s.Visible(true));
			grid.ColumnAutoWidth(true);
			grid.ElementAttr(new { @class = "dx-card wide-card" });
			grid.ShowBorders(false);
			grid.FilterRow(f => f.Visible(true));
			grid.ColumnHidingEnabled(true);
			if (listColumns != null && listColumns.Count > 0) { grid.Columns(columns => { IPSConfigureColumns<T>(columns, listColumns); }); }
			grid.Pager(IPSDataGridDefaultPagerConfig);
			return grid;
		}

		/// <summary>
		/// Configure columns each columns with provided list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="columns"></param>
		/// <param name="myColumns"></param>
		internal static void IPSConfigureColumns<T>(CollectionFactory<DataGridColumnBuilder<T>> columns, List<ColumnSettings> myColumns)
		{
			foreach (var def in myColumns)
			{
				columns.Add()
					.DataType(def.DataType)
					.DataField(def.Field)
					.Format(def.Format)
					.Caption(def.Caption);
			}
		}

		/// <summary>
		/// Default seting to call from controller methods and pass key value in datagrid
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="column"></param>
		/// <param name="controllerName"></param>
		/// <param name="key"></param>
		/// <param name="displayExpr"></param>
		/// <returns></returns>
		internal static DataGridColumnBuilder<T> IPSsAddLookupConfig<T>(this DataGridColumnBuilder<T> column, string controllerName, string key, string displayExpr)
		{
			column.Lookup((lookup) =>
			{
				lookup.DataSource(d => d.Mvc().Controller(controllerName).LoadAction("Get").Key(key));
				lookup.DisplayExpr(displayExpr);
				lookup.ValueExpr(key);
			});
			column.HeaderFilter(hf => hf.AllowSearch(true));
			column.Width(200);
			return column;
		}

		/// <summary>
		/// Used for datagrid to set default pager configuration
		/// </summary>
		internal static Action<DataGridPagerBuilder> IPSDataGridDefaultPagerConfig = (builder) => {
			builder.ShowPageSizeSelector(true);
			builder.AllowedPageSizes(new[] { 5, 10, 20 });
			builder.ShowInfo(true);
		};

		/// <summary>
		/// Need to add some action on button that time used this settings
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="column"></param>
		/// <param name="html"></param>
		/// <returns></returns>
		internal static DataGridColumnBuilder<T> IPSAddActionColumn<T>(this DataGridColumnBuilder<T> column, IHtmlHelper html)
		{
			var button = html.DevExtreme().Button().StylingMode(ButtonStylingMode.Text).Text(new JS("value"));
			column.CellTemplate(String.Format("<text> <div><b>Row key is: <%= row.key %> </b> </div>{0}</text>", button.ToTemplate()));
			return column;
		}

		#endregion

		#region SelectBox or Dropdown or Toolbar components Settings

		/// <summary>
		/// Look up editor for drop down box or select box or toolbar item
		/// </summary>
		/// <param name="toolbar"></param>
		/// <param name="html"></param>
		/// <param name="controllerName"></param>
		/// <param name="key"></param>
		/// <param name="displayExpr"></param>
		/// <returns></returns>
		internal static SelectBoxBuilder IPSLookupEditor(this ToolbarItemFactory toolbar, IHtmlHelper html, string controllerName, string key, string displayExpr)
		{
			SelectBoxBuilder lookup = html.DevExtreme().SelectBox();
			lookup.DataSource(d => d.Mvc().Controller(controllerName).LoadAction("Get").Key(key));
			lookup.DisplayExpr(displayExpr);
			lookup.ValueExpr(key);
			return lookup;
		}

		#endregion

		#endregion










	}
}
