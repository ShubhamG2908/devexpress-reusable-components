using DevExtreme.AspNet.Mvc.Builders;

namespace MVCDemoApp.Models
{
    public class DataGridConfigModel<T> where T : class
    {
        public string? GridId { get; set; }
        public string? ControllerName { get; set; }
        public string? DataAction { get; set; }
        public string? UpdateAction { get; set; }
        public string? InsertAction { get; set; }
        public string? DeleteAction { get; set; }
        public int PageSize { get; set; }
        public bool ShowBorders { get; set; } = true;
        public bool AllowColumnReordering { get; set; } = true;
        public bool AllowColumnResizing { get; set; } = true;
        public bool ShowFilterRow { get; set; } = true;
        public bool EnablePaging { get; set; } = true;
        public bool EnableSorting { get; set; } = true;
        public bool EnableCRUDOperations { get; set; } = true;
        public string NoDataText { get; set; } = "No data available.";
        public Action<DataGridBuilder<T>>? Columns { get; set; }
        public Action<DataGridBuilder<T>>? Fields { get; set; }
    }
}
