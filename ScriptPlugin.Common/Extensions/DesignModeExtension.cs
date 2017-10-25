using System.Windows;
using System.Windows.Controls;

namespace ScriptPlugin.Common.Extensions
{
    public static class DesignModeExtension
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
