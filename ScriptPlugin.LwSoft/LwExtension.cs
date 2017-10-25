using System;
using System.Collections.Generic;
using System.Windows;
using ScriptPlugin.LwSoft.Enums;

namespace ScriptPlugin.LwSoft
{
    public static class LwExtension
    {
        public static IDisposable BindWindow(this Lwsoft3 lw, int hwnd,
            LwDisplayBind display, LwMouseBind mouse, LwKeypadBind keypad, int added)
        {
            lw.BindWindow(hwnd, display, mouse, keypad, added, 0);
            return new BindingDisposable(lw);
        }

        public static Lwsoft3 ClickOnce(this Lwsoft3 lw, int x, int y)
        {
            lw.MoveTo(x, y);
            lw.LeftClick();
            return lw;
        }

        public static Lwsoft3 ClickOnce(this Lwsoft3 lw, Point point)
        {
            return ClickOnce(lw, (int)point.X, (int)point.Y);
        }

        public static Lwsoft3 ClickDouble(this Lwsoft3 lw, int x, int y)
        {
            ClickOnce(lw, x, y);
            lw.LeftClick();
            return lw;
        }

        public static Lwsoft3 ClickDouble(this Lwsoft3 lw, Point point)
        {
            return ClickDouble(lw, (int)point.X, (int)point.Y);
        }

        public static Lwsoft3 MoveTo(this Lwsoft3 lw, Point point)
        {
            lw.MoveTo((int)point.X, (int)point.Y);
            return lw;
        }

        public static Lwsoft3 MoveR(this Lwsoft3 lw, Point point)
        {
            lw.MoveR((int)point.X, (int)point.Y);
            return lw;
        }

        public static string Ocr(this Lwsoft3 lw, Rect rect, string color_format, double sim)
        {
            return lw.Ocr((int)rect.Left, (int)rect.Top, (int)rect.Right, (int)rect.Bottom, color_format, sim);
        }

        public static void SetWindowSize(this Lwsoft3 lw, int hwnd, Size size)
        {
            lw.SetWindowSize(hwnd, (int)size.Width, (int)size.Height);
        }

        public static bool FindPic(this Lwsoft3 lw, Rect rect, string picName,
            string deltaColor = "000000", double sim = 0.95, int dir = 0, int timeout = 0, int ischick = 0,
            int chickdex = 0, int chickdey = 0, int chickdely = 0)
        {
            return lw.FindPic((int)rect.Left, (int)rect.Top, (int)rect.Right, (int)rect.Bottom,
                picName, deltaColor, sim, dir, timeout, ischick, chickdex, chickdey, chickdely);
        }
    }

    public class BindingDisposable : IDisposable
    {
        private readonly Lwsoft3 _lw;
        public BindingDisposable(Lwsoft3 lw)
        {
            _lw = lw;
        }
        public void Dispose()
        {
            _lw.UnBindWindow();
        }
    }
}
