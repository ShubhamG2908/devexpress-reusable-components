using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DevExtreme.AspNet.Mvc;
[assembly: InternalsVisibleTo("MVCDemoApp")]
[assembly: InternalsVisibleTo("UtilitiesProject")]
namespace ModelsProject
{
	internal class GridColumnSettings
	{
		public GridColumnDataType DataType { get; set; }
		public string Field { get; set; }
		public Format Format { get; set; }
		public string Caption { get; set; }
	}
}
