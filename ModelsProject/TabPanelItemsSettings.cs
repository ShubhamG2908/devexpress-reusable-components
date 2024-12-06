using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("MVCDemoApp")]
[assembly: InternalsVisibleTo("UtilitiesProject")]
namespace ModelsProject
{
    internal class TabPanelItemsSettings
    {
        internal string Title { get; set; }
        internal string Text { get; set; }
        internal string? TemplateNameOrContent { get; set; }
        internal string? OptionKey { get; set; }
        internal object? OptionValue { get; set; }
        internal string? ItemTemplateNameOrContent { get;set; }

    }

    public class TabPanelItem
    {
        public string icon { get; set; }
        public string title { get; set; }
        public IEnumerable<TaskItem> tasks { get; set; }
    }

    public class TaskItem
    {
        public string status { get; set; }
        public string priority { get; set; }
        public string text { get; set; }
        public string date { get; set; }
        public string assignedBy { get; set; }
    }
}
