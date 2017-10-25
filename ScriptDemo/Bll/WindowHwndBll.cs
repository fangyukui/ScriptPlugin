using System;
using ScriptPlugin.LwSoft;

namespace ScriptDemo.Bll
{
    public class WindowHwndBll
    {
        public static int[] EnumTargetWindow()
        {
            var hwndstr = LwFactory.Default.EnumWindow(null, "WeChatMainWndForPC", null);
            var hwnds = hwndstr?.Split(',');
            if (hwnds?.Length > 0)
            {
                var ints = new int[hwnds.Length];
                for (var i = 0; i < hwnds.Length; i++)
                    ints[i] = int.Parse(hwnds[i]);
                return ints;
            }
            return new int[0];
        }

        public static int GetTargetWindow()
        {
            var hwnd = LwFactory.Default.FindWindow(null, "360se6_Frame", null);
            return hwnd;
        }

        public static bool ExistsWindow(int hwnd)
        {
            return !String.IsNullOrEmpty(LwFactory.Default.GetWindowClass(hwnd));
        }
    }
}
