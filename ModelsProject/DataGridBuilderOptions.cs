using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsProject
{
	internal class DataGridBuilderOptions<T>
	{
		internal List<T> DataSourceCollection { get; set; }
		internal List<GridColumnSettings> ListColumns { get; set; }
		internal string ControllerName { get; set; }
		internal string Key { get; set; }
		internal string Action { get; set; }
		internal object Parameters { get; set; }
		internal string ControlId { get; set; }
		internal string InsertAction { get; set; }
		internal string UpdateAction { get; set; }
		internal string DeleteAction { get; set; }
		internal string OnBeforeSend { get; set; }
		internal bool AllowFilter { get; set; }
		internal bool AllowSearchable{ get; set; }
		internal DataGridBuilderOptions()
		{
			// Set default values for properties if applicable
			Action = "Get";
			ControlId = "dataGridId";
			AllowFilter = false;
			AllowSearchable = false;
		}
	}
}
