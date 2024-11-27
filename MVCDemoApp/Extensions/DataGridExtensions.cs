using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Mvc.Builders;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCDemoApp.Models;

namespace MVCDemoApp.Extensions
{
    public static class DevExpressExtensions
    {
        public static DataGridBuilder<T> DevExpressDataGrid<T>(
             this IHtmlHelper htmlHelper,
        DataGridConfigModel<T> config) where T : class
        {
            var dataGrid = htmlHelper.DevExtreme().DataGrid<T>()
                .ID(config.GridId)
                .ShowBorders(config.ShowBorders)
                .RemoteOperations(true)
                .Paging(paging => paging.PageIndex(0).PageSize(config.PageSize))
                .Sorting(sort => sort.Mode(GridSortingMode.Multiple))
                .AllowColumnReordering(config.AllowColumnReordering)
                .AllowColumnResizing(config.AllowColumnResizing)
                .FilterRow(filterRow =>
                {
                    filterRow.Visible(config.ShowFilterRow);
                    filterRow.ShowOperationChooser(false);
                })
                .StateStoring(s => s
                        .Enabled(true)
                        .Type(StateStoringType.Custom)
                        .StorageKey("storage"))
                .Pager(pager =>
                {
                    pager.Visible(config.EnablePaging);
                    pager.DisplayMode(GridPagerDisplayMode.Compact);
                    pager.ShowPageSizeSelector(true);
                    pager.AllowedPageSizes(new JS("[10, 25, 50, 100]"));
                    pager.ShowInfo(true);
                    pager.ShowNavigationButtons(true);
                })
                .Scrolling(scrolling => scrolling.Mode(GridScrollingMode.Standard))
                .Height("auto")
                .NoDataText(config.NoDataText)
                .CacheEnabled(false)
                .Editing(e => e.Mode(GridEditMode.Popup)
                .AllowUpdating(config.EnableCRUDOperations)
                .AllowAdding(config.EnableCRUDOperations)
                .AllowDeleting(config.EnableCRUDOperations)
                .UseIcons(true)
                .Popup(p => p
                .Title("Add/Edit record")
                .ShowTitle(true)
                .Width(700)
                .Height(600)
                ))
                .DataSource(d => d.Mvc()
                .Controller(config.ControllerName)
                .LoadAction(config.DataAction)
                .UpdateAction(config.UpdateAction)
                .InsertAction(config.InsertAction)
                .DeleteAction(config.DeleteAction)
                .Key("Id"));

            config.Columns?.Invoke(dataGrid);
            return dataGrid;
        }
    }
}
