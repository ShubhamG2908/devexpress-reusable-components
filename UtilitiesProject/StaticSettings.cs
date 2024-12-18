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
using System.Linq.Expressions;
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
		internal static DataGridBuilder<T> IPSDataGridSetDefaults<T>(this DataGridBuilder<T> grid, List<T> datasourceCollection = null, List<GridColumnSettings> listColumns = null, string controllerName = "", string key = "",string action = "Get",object controllerParameters = null)
		{
			//this will use while calling from controller methods and key.
			if (!string.IsNullOrWhiteSpace(controllerName) && !string.IsNullOrWhiteSpace(key) && controllerParameters != null) //must have controller, key and parameters
            {
                grid.DataSource(d => d.Mvc().Controller(controllerName).LoadAction(action).LoadParams(controllerParameters).Key(key));
            }
			else if (!string.IsNullOrWhiteSpace(controllerName) && string.IsNullOrWhiteSpace(key) && controllerParameters != null) //must have controller and parameters
            {
                grid.DataSource(d => d.Mvc().Controller(controllerName).LoadAction(action).LoadParams(controllerParameters));
            }
            else if (!string.IsNullOrWhiteSpace(controllerName) && !string.IsNullOrWhiteSpace(key)) //must have controller ands key
            {
                grid.DataSource(d => d.Mvc().Controller(controllerName).LoadAction(action).Key(key));
            }
            //this will use while calling from list of collection that time passed that directly to datasource.
            if (datasourceCollection != null && datasourceCollection.Count > 0) { grid.DataSource(datasourceCollection); }

			//default configuration. If need any changes then pass param and call it whenever use.
			grid.RemoteOperations(true);
			grid.FocusedRowEnabled(true);
			grid.FocusedRowIndex(0);			
			grid.GroupPanel(g => g.Visible(true));
			grid.SearchPanel(IPSDataGridDefaultSearchPanelConfig);
			grid.ColumnAutoWidth(true);
			grid.ElementAttr(new { @class = "dx-card wide-card" });
			grid.ShowBorders(false);
			grid.HeaderFilter(h => h.Visible(true));
            grid.FilterSyncEnabled(true);
			grid.FilterRow(f => f.Visible(true));
			grid.ColumnHidingEnabled(true);
			if (listColumns != null && listColumns.Count > 0) { grid.Columns(columns => { IPSDataGridConfigureColumns<T>(columns, listColumns); }); }
			grid.Pager(IPSDataGridDefaultPagerConfig);
			return grid;
		}

		/// <summary>
		/// Configure columns each columns with provided list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="columns"></param>
		/// <param name="myColumns"></param>
		internal static void IPSDataGridConfigureColumns<T>(CollectionFactory<DataGridColumnBuilder<T>> columns, List<GridColumnSettings> myColumns)
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
		internal static DataGridColumnBuilder<T> IPSDataGridAddLookupConfig<T>(this DataGridColumnBuilder<T> column, string controllerName, string key, string displayExpr)
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
			builder.AllowedPageSizes(new[] {5, 10, 20});
			//builder.AllowedPageSizes(new JS("[ 5, 10, 20, 'all' ]"));
			builder.ShowInfo(false);
			builder.DisplayMode(GridPagerDisplayMode.Compact);
            builder.ShowPageSizeSelector(true);

        };

		internal static Action<DataGridSearchPanelBuilder> IPSDataGridDefaultSearchPanelConfig = (builder) =>  {
			builder.SearchVisibleColumnsOnly(true);
			builder.Visible(true);
			builder.HighlightCaseSensitive(true);
			builder.HighlightSearchText(true);
			builder.Width("250");				
			builder.Placeholder("Search specific..");
		};

		/// <summary>
		/// Need to add some action on button that time used this settings
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="column"></param>
		/// <param name="html"></param>
		/// <returns></returns>
		internal static DataGridColumnBuilder<T> IPSDataGridAddActionColumn<T>(this DataGridColumnBuilder<T> column, IHtmlHelper html)
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

        #region TabPanel component Settings

        /// <summary>
        /// This is used for tab section default settings.
        /// </summary>
        /// <param name="tabPanel"></param>
        /// <param name="controlId"></param>
        /// <param name="accessKey"></param>
        /// <param name="tabPanelItemsSettings"></param>
        /// <param name="controllerName"></param>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal static TabPanelBuilder IPSSetTabPanelSetDefaults(this TabPanelBuilder tabPanel,string controlId ="tbPanelId",string accessKey = "", List<TabPanelItemsSettings> tabPanelItemsSettings = null, string controllerName = "", string key = "", string action = "Get", object parameters = null,
			Position position = Position.Left,TabsStyle tabsStyle = TabsStyle.Primary,TabsIconPosition tabsIconPosition = TabsIconPosition.Start)
        {
			if (!string.IsNullOrWhiteSpace(controlId)) { tabPanel.ID(controlId); }
			
            //this will use while calling from controller methods and key.
            if (!string.IsNullOrWhiteSpace(controllerName) && !string.IsNullOrWhiteSpace(key) && parameters != null) //must have controller, key and parameters
            {
                tabPanel.DataSource(d => d.Mvc().Controller(controllerName).LoadAction(action).LoadParams(parameters).Key(key));
            }
            else if (!string.IsNullOrWhiteSpace(controllerName) && string.IsNullOrWhiteSpace(key) && parameters != null) //must have controller and parameters
            {
                tabPanel.DataSource(d => d.Mvc().Controller(controllerName).LoadAction(action).LoadParams(parameters));
            }
            else if (!string.IsNullOrWhiteSpace(controllerName) && !string.IsNullOrWhiteSpace(key)) //must have controller ands key
            {
                tabPanel.DataSource(d => d.Mvc().Controller(controllerName).LoadAction(action).Key(key));
            }
            else if (!string.IsNullOrWhiteSpace(controllerName) ) //must have controller ands key
            {
                tabPanel.DataSource(d => d.Mvc().Controller(controllerName).LoadAction(action));
            }

            //it means that when create one variable in javascript of access key that variable name pass in access key.
            //it will help to see "Razor C#" code - https://docs.devexpress.com/AspNetCore/DevExtreme.AspNet.Mvc.Builders.TabPanelBuilder.AccessKey(System.String)
            if (!string.IsNullOrWhiteSpace(accessKey)) { tabPanel.AccessKey(accessKey); }
            //it will help to see "Razor C#" code - https://docs.devexpress.com/AspNetCore/DevExtreme.AspNet.Mvc.Builders.TabPanelBuilder.AccessKey(DevExtreme.AspNet.Mvc.JS)
            if (!string.IsNullOrWhiteSpace(accessKey)) { tabPanel.AccessKey(new JS(accessKey)); }


            tabPanel.Width("100%");
			tabPanel.Height("90%");
			tabPanel.AnimationEnabled(true);
			tabPanel.SwipeEnabled(true);

            tabPanel.ScrollingEnabled(true);
			tabPanel.ScrollByContent(true);

			tabPanel.TabsPosition(position);
			tabPanel.StylingMode(tabsStyle);
			tabPanel.IconPosition(tabsIconPosition);


			if (tabPanelItemsSettings != null && tabPanelItemsSettings.Count > 0) { tabPanel.AddDynamicTabPanelItems(tabPanelItemsSettings); }

            return tabPanel;
        }

		/// <summary>
		/// this is adding dynamically tab panel items.
		/// </summary>
		/// <param name="tabPanel"></param>
		/// <param name="tabPanelItemsSettings"></param>
		/// <returns></returns>
		internal static TabPanelBuilder AddDynamicTabPanelItems(this TabPanelBuilder tabPanel, List<TabPanelItemsSettings> tabPanelItemsSettings)
		{
            tabPanel.Items(tabItems =>
            {
                foreach (var item in tabPanelItemsSettings)
                {
					var addItem = tabItems.Add().Title(item.Title);
					if (!string.IsNullOrWhiteSpace(item.TemplateNameOrContent)){ addItem?.Template(item.TemplateNameOrContent); }
					else if (!string.IsNullOrWhiteSpace(item.OptionKey) && item.OptionValue != null)
					{
						addItem?.Option(item.OptionKey, item.OptionValue);
					}
                    else if (!string.IsNullOrWhiteSpace(item.Text))
                    {
                        addItem?.Text(item.Text);
                    }
                    else
					{
                        addItem
						.Template(item.TemplateNameOrContent)
						.Option(item.OptionKey, item.OptionValue);
					}
                }

            });

			return tabPanel;
        }

		/// <summary>
		/// Each Tab panel item add specific property. (we can used in direct html or tab panel items configuration)
		/// </summary>
		/// <param name="items"></param>
		/// <param name="title"></param>
		/// <param name="text"></param>
		/// <param name="tabTemplateName"></param>
		/// <param name="optionKey"></param>
		/// <param name="optionValue"></param>
		/// <returns></returns>
        internal static TabPanelItemBuilder IPSTabPanelActionItems(this TabPanelItemBuilder items,string title = "", string text="",string tabTemplateName="", string optionKey="optionKey",object optionValue = null )
        {

			if (!string.IsNullOrWhiteSpace(title)) { items.Title(title); }
			if (!string.IsNullOrWhiteSpace(text)) { items.Text(""); }
			if (!string.IsNullOrWhiteSpace(tabTemplateName)) { items.TabTemplate(new JS(tabTemplateName)); }
			if (!string.IsNullOrWhiteSpace(optionKey) && optionValue != null) { items.Option(optionKey, optionValue); }

			return items;
        }


        #endregion

        #endregion


    }
}
