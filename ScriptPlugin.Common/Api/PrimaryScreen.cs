using System;
using System.Windows;

namespace ScriptPlugin.Common.Api
{
    public class PrimaryScreen
    {
       
        #region DeviceCaps常量
        const int HORZRES = 8;
        const int VERTRES = 10;
        const int LOGPIXELSX = 88;
        const int LOGPIXELSY = 90;
        const int DESKTOPVERTRES = 117;
        const int DESKTOPHORZRES = 118;
        #endregion

        #region 属性
        /// <summary>
        /// 获取屏幕分辨率当前物理大小
        /// </summary>
        public static Size WorkingArea
        {
            get
            {
                IntPtr hdc = Win32Api.GetDC(IntPtr.Zero);
                Size size = new Size();
                size.Width = Win32Api.GetDeviceCaps(hdc, HORZRES);
                size.Height = Win32Api.GetDeviceCaps(hdc, VERTRES);
                Win32Api.ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
        }
        /// <summary>
        /// 当前系统DPI_X 大小 一般为96
        /// </summary>
        public static int DpiX
        {
            get
            {
                IntPtr hdc = Win32Api.GetDC(IntPtr.Zero);
                int DpiX = Win32Api.GetDeviceCaps(hdc, LOGPIXELSX);
                Win32Api.ReleaseDC(IntPtr.Zero, hdc);
                return DpiX;
            }
        }
        /// <summary>
        /// 当前系统DPI_Y 大小 一般为96
        /// </summary>
        public static int DpiY
        {
            get
            {
                IntPtr hdc = Win32Api.GetDC(IntPtr.Zero);
                int DpiX = Win32Api.GetDeviceCaps(hdc, LOGPIXELSY);
                Win32Api.ReleaseDC(IntPtr.Zero, hdc);
                return DpiX;
            }
        }
        /// <summary>
        /// 获取真实设置的桌面分辨率大小
        /// </summary>
        public static Size DESKTOP
        {
            get
            {
                IntPtr hdc = Win32Api.GetDC(IntPtr.Zero);
                Size size = new Size();
                size.Width = Win32Api.GetDeviceCaps(hdc, DESKTOPHORZRES);
                size.Height = Win32Api.GetDeviceCaps(hdc, DESKTOPVERTRES);
                Win32Api.ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
        }

        /// <summary>
        /// 获取宽度缩放百分比
        /// </summary>
        public static float ScaleX
        {
            get
            {
                IntPtr hdc = Win32Api.GetDC(IntPtr.Zero);
                int t = Win32Api.GetDeviceCaps(hdc, DESKTOPHORZRES);
                int d = Win32Api.GetDeviceCaps(hdc, HORZRES);
                float ScaleX = (float) Win32Api.GetDeviceCaps(hdc, DESKTOPHORZRES) / (float) Win32Api.GetDeviceCaps(hdc, HORZRES);
                Win32Api.ReleaseDC(IntPtr.Zero, hdc);
                return ScaleX;
            }
        }
        /// <summary>
        /// 获取高度缩放百分比
        /// </summary>
        public static float ScaleY
        {
            get
            {
                IntPtr hdc = Win32Api.GetDC(IntPtr.Zero);
                float ScaleY = (float)(float) Win32Api.GetDeviceCaps(hdc, DESKTOPVERTRES) / (float) Win32Api.GetDeviceCaps(hdc, VERTRES);
                Win32Api.ReleaseDC(IntPtr.Zero, hdc);
                return ScaleY;
            }
        }
        #endregion
    }
}
