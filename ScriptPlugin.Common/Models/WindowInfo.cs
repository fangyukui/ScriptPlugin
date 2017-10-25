using System.Windows;

namespace ScriptPlugin.Common.Models
{
    public class WindowInfo
    {
        public WindowInfo()
        {
        }

        public WindowInfo(string className, string title)
        {
            ClassName = className;
            Title = title;
        }

        public string ClassName { get; set; }
        public string Title { get; set; }
        public Rect WindowRect { get; set; }
    }
}
