using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("MVCDemoApp")]
[assembly: InternalsVisibleTo("UtilitiesProject")]
namespace ModelsProject
{
    internal class Company
    {
        internal int ID { get; set; }
        internal string Name { get; set; }
        internal string Address { get; set; }
        internal string City { get; set; }
        internal string State { get; set; }

        internal int ZipCode { get; set; }
        internal string Phone { get; set; }
        internal string Fax { get; set; }
        internal string Website { get; set; } = "http://www.nowebsitesupermart.dx";
    }
}
