using System.Windows;
using System.Windows.Controls;

namespace ScriptPlugin.Common.Controls
{
    public static class DesignModeExtensions
    {
        public static bool IsDesignMode(this Control control)
        {
            return System.ComponentModel.DesignerProperties.GetIsInDesignMode(control);
        }

        public static bool IsDesignMode(this Window control)
        {
            return System.ComponentModel.DesignerProperties.GetIsInDesignMode(control);
        }
    }
}
