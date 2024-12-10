using ModelsProject;
using static System.Net.WebRequestMethods;

namespace MVCDemoApp.Data
{
    internal static class MenuData
    {
        public static List<MenuItem> CustomMenus = new List<MenuItem>() {
            new MenuItem {
                Id = 1,
                Text = "Home",
                Icon = "fa-solid fa-home",
                Disabled = false,
                Url = "http://localhost:5117"
            },
            new MenuItem {
                Id = 2,
                Text = "Privacy",
                Icon = "fa-solid fa-lock",
                Disabled = false,
                Url = "http://localhost:5117/Home/Privacy"
            },
            new MenuItem {
                Id = 3,
                Text = "Components",
                Icon = "fa fa-book fa-fw",
                Disabled = false,
                Url = "",
                //Url = "http://localhost:5117/Components",
                Items = new List<MenuItem>() {
                    new MenuItem {
                        Id = 4,
                        Text = "Buttons",
                        Icon = "fa-solid fa-square",
                        Disabled = false,
                        Url = "http://localhost:5117/Components/Buttons"
                    },
                    new MenuItem {
                        Id = 5,
                        Text = "Data Grid",
                        Icon = "fa-solid fa-grip",
                        Disabled = false,
                        Url = "http://localhost:5117/Components/DataGrid"
                    },
                    new MenuItem {
                        Id = 6,
                        Text = "Tab Panel",
                        Icon = "fa-solid fa-table-columns",
                        Disabled = false,
                        Url = "http://localhost:5117/Components/NavMenu"
                    }

                }
            },
        };
    }
}
