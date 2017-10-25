using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ScriptDemo.Models;
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
        public int Hwnd;
        private string _logs;
        private string _name;
        private readonly ILog _log = LogManager.GetLogger(typeof(OperaBll));
        private bool _threadRun;
        private bool _bindinged;

        #endregion

        #region 属性

        //绑定状态
        public bool Bindinged
        {
            get => _bindinged;
            set
            {
                if (value.Equals(_bindinged)) return;
                _bindinged = value;
                OnPropertyChanged();
            }
        }

        //线程启动
        public bool ThreadRun
        {
            get => _threadRun;
            set
            {
                if (value == _threadRun) return;
                _threadRun = value;
                OnPropertyChanged();
            }
        }

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

        //窗体名字
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

        //运行状态
        public RunState RunState { get; private set; } = RunState.Idle;

        #endregion

        #region 构造函数

        public OperaBll(Lwsoft3 lw)
        {
            Lw = lw;
        }

        public OperaBll(Lwsoft3 lw, int hwnd) : this(lw)
        {
            Hwnd = hwnd;
        }

        #endregion

        #region 方法

        #region 初始化

        public void Load()
        {
            LoadWindowsHandle(Hwnd);
        }

        /// <summary>
        /// 初始化窗体信息
        /// </summary>
        private void LoadWindowsHandle(int hwnd)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "imgs");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            //设置全局目录
            Lw.SetPath(path);

            SetWindowsHandle(hwnd);
            Log("初始化窗体成功");

            if (Lw.IsBind(hwnd) == 0)
            {
                //绑定窗体
                if (Lw.BindWindow(hwnd, LwDisplayBind.Normal, LwMouseBind.Windows, LwKeypadBind.Windows, 0, 0) == 1)
                {
                    Bindinged = true;
                    //激活窗体
                    Lw.SetWindowState(hwnd, 1);
                    Log("绑定成功");
                }
            }
#if DEBUG
            //关闭错误消息
            Lw.SetShowErrorMsg(0);
#endif
        }

        private void SetWindowsHandle(int parent)
        {
            Hwnd = parent;
            Name = Lw.GetWindowTitle(Hwnd);
        }

        #endregion

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
            Log("解除绑定");
        }

        //忙碌状态时等待
        private async Task WaitBusy()
        {
            while (RunState == RunState.Busy)
            {
                //超过窗口数，等待处理
                await Task.Delay(200);
            }
        }

        //日志
        private void Log(string log)
        {
            _log.Info(log);

            Logs += $"{DateTime.Now:HH:mm:ss}: {log}\r\n";
            var lines = Logs.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            const int showLine = 10;
            if (lines.Length > showLine)
            {
                StringBuilder strb = new StringBuilder();
                for (int i = lines.Length - showLine; i < lines.Length; i++)
                {
                    strb.Append(lines[i]);
                }
                Logs = strb.ToString();
            }
        }

        #region 线程

        //开始任务
        public void StartTask()
        {
            ThreadRun = true;
            Task.Run(() =>
            {
                while (ThreadRun)
                {
                    //todo...
                    Task.Delay(100).Wait();
                }
            });
        }

        #endregion

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
