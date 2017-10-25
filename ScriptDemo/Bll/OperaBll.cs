using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using ScriptPlugin.Common;
using ScriptPlugin.Common.Enums;
using ScriptPlugin.LwSoft;
using ScriptPlugin.LwSoft.Enums;
using ScriptPlugin.Theme.Annotations;

namespace ScriptDemo.Bll
{
    public class OperaBll : INotifyPropertyChanged, IDisposable
    {
        #region 字段

        public readonly Lwsoft3 Lw;
        //主窗口句柄
        public int Hwnd;
        private string _logs;
        private string _name;
        private readonly ILog _log = LogManager.GetLogger(typeof(OperaBll));

        #endregion

        #region 属性

        //绑定状态
        public bool Bindinged { get; set; }
        //线程启动
        public bool ThreadRun { get; set; }

        //日志
        public string Logs
        {
            get => _logs;
            set
            {
                if (value == _logs) return;
                _logs = value;
                OnPropertyChanged();
            }
        }

        //模拟器名字
        public string Name
        {
            get => _name + $"({Hwnd})";
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public RunState RunState { get; private set; } = RunState.Idle;

        #endregion

        #region 构造函数

        public OperaBll(Lwsoft3 lw)
        {
            Lw = lw;
        }

        #endregion

        #region 初始化

        public void Load()
        {
            LoadWindowsHandle(Hwnd);
            Bindinged = true;
        }

        /// <summary>
        /// 初始化窗体信息
        /// </summary>
        private void LoadWindowsHandle(int hwnd)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "imgs");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Lw.SetPath(path);
            SetWindowsHandle(hwnd);
            _log.Info("初始化窗体成功");
            Log("初始化窗体成功");
            if (Lw.IsBind(hwnd) == 0)
            {
                //绑定
                Lw.BindWindow(this.Hwnd, LwDisplayBind.Gdi, LwMouseBind.Windows, LwKeypadBind.Windows, 32);
                Lw.SetWindowState(this.Hwnd, 1);
                Log("绑定成功");
            }
#if DEBUG
            //关闭错误消息
            Lw.SetShowErrorMsg(0);
#endif
        }

        public void SetWindowsHandle(int parent)
        {
            Hwnd = parent;
            Name = Lw.GetWindowTitle(Hwnd);
        }

        #endregion

        #region 方法

        #region 移动方法
        /// <summary>
        /// 向上移动一行
        /// </summary>
        public void MoveUpOneRow()
        {
            Lw.MoveTo(OpearPoints.FirstRow);
            for (int i = 0; i < 2; i++)
                Lw.WheelUp();
        }

        /// <summary>
        /// 快速向上移动
        /// </summary>
        public void MoveUpQuick()
        {
            Lw.MoveTo(OpearPoints.FirstRow);
            for (int i = 0; i < 200; i++)
                Lw.Delay(5).WheelUp();
        }

        /// <summary>
        /// 向下移动一行
        /// </summary>
        public void MoveDownOneRow()
        {
            Lw.MoveTo(OpearPoints.FirstRow);
            for (int i = 0; i < 2; i++)
                Lw.WheelDown();
        }

        /// <summary>
        /// 向下快速移动
        /// </summary>
        public void MoveDownQuick()
        {
            Lw.MoveTo(OpearPoints.FirstRow);
            for (int i = 0; i < 200; i++)
                Lw.Delay(5).WheelDown();
        }

        #endregion

        //解除绑定
        public void UnBindWindow()
        {
            Lw.UnBindWindow();
            _log.Info("解除绑定");
            Log("解除绑定");
        }

        private async Task WaitBusy()
        {
            while (this.RunState == RunState.Busy)
            {
                //超过窗口数，等待处理
                await Task.Delay(200);
            }
        }

        private void Log(string log)
        {
            _log.Info(log);

            Logs += $"{DateTime.Now:HH:mm:ss}: {log}\r\n";
            var lines = Logs.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 10)
            {
                StringBuilder strb = new StringBuilder();
                for (int i = lines.Length - 10; i < lines.Length; i++)
                {
                    strb.Append(lines[i]);
                }
                Logs = strb.ToString();
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            ThreadRun = false;
            UnBindWindow();
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
