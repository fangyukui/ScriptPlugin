using System.Windows.Controls;
using ScriptPlugin.Theme.Control;

namespace ScriptPlugin.Common.Extensions
{
    public static class WpfExtension
    {
        public static bool IsInDesignMode(this Control control)
        {
            return System.ComponentModel.DesignerProperties.GetIsInDesignMode(control);
        }

        public static bool IsInDesignMode(this WindowBase control)
        {
            return System.ComponentModel.DesignerProperties.GetIsInDesignMode(control);
        }
    }
}
