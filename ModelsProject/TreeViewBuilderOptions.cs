using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsProject
{
	internal class TreeViewBuilderOptions<T>
	{
        internal string DataStructure { get; set; }
        internal string KeyExpr { get; set; }
        internal string DisplayExpr { get; set; }
        internal string ExpandedExpr { get; set; }
        internal string ParentIdExpr { get; set; } = "";
        internal string ItemsExpr { get; set; } = "";
        internal List<T> DataSourceCollection { get; set; }
        internal string ControllerName { get; set; } = "";
        internal string Key { get; set; } = "";
        internal string Action { get; set; } = "Get";
        internal object ControllerParameters { get; set; }
        internal TreeViewBuilderOptions()
		{
			// Set default values for properties if applicable
			Action = "Get";
		}
	}
}
