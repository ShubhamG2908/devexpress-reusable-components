using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsProject
{
    internal class MenuItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Disabled { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public List<MenuItem> Items { get; set; }
    }
}
