using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using ScriptDemo.Bll;
using ScriptPlugin.Common.Enums;
using ScriptPlugin.Common.Helper;
using ScriptPlugin.LwSoft;
using ScriptPlugin.Theme;
using ScriptPlugin.Theme.Annotations;

namespace ScriptDemo.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region 字段

        public static MainViewModel Instance { get; } = new MainViewModel();
        private readonly ILog _log = LogManager.GetLogger(typeof(MainViewModel));
        private bool _isExit;
        private ObservableCollection<OperaBll> _operas;
        private int _maxThreadCount;
        private RunState _taskState;

        #endregion

        #region 属性

        public ObservableCollection<OperaBll> Operas
        {
            get => _operas;
            set
            {
                if (Equals(value, _operas)) return;
                _operas = value;
                OnPropertyChanged();
            }
        }

        //最大线程数
        public int MaxThreadCount
        {
            get => _maxThreadCount;
            set
            {
                if (value == _maxThreadCount) return;
                _maxThreadCount = value;
                OnPropertyChanged();
            }
        }

        //任务状态
        public RunState TaskState
        {
            get => _taskState;
            set
            {
                if (value == _taskState) return;
                _taskState = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region 构造函数

        public MainViewModel()
        {
            //数据加载
            if (!InitData()) return;
            //环境监测
            if (!CheckEnvironment()) return;
            //todo...
        }

        #endregion

        #region 命令

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        public RelayCommand ClosedCommand => new RelayCommand(() =>
        {
            _isExit = true;
           
            LwFactory.Clear();
        });

        #endregion

        #region 方法

        //配置文件加载
        private bool InitData()
        {
            try
            {
                MaxThreadCount = AppConfig.GetValue("MaxThreadCount", 20);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e);
                MessageBox.Show("配置文件出错，请检查", "错误");
            }
            return false;
        }

        //监测环境
        private bool CheckEnvironment()
        {
            try
            {
                var lw = LwFactory.Default;
                var width = lw.GetScreenWidth();
                var height = lw.GetScreenHeight();
                if (width != 1920 && height != 1080)
                {
                    MessageBox.Show($"只支持1920*1080分辨率，当前分辨率({width}*{height})不正确", "错误");
                }
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e);
                MessageBox.Show(e.Message, "错误");
                return false;
            }
        }

        private int[] EnumTargetWindow()
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
        private int GetTargetWindow()
        {
            var hwnd = LwFactory.Default.FindWindow(null, "360se6_Frame", null);
            return hwnd;
        }

        #region 线程

        //实时监测窗体线程
        private void StartCheckTargetThread()
        {
            Task.Run(() =>
            {
                while (!_isExit)
                {
                    var hwnd = GetTargetWindow();
                    if (hwnd > 0)
                    {
                        OperaBll opear = Operas.First();
                        opear.Hwnd = hwnd;
                        opear.Load();
                        //opear.StartTaskThread();

                        while (!_isExit)
                        {
                            //窗体不存在
                            if (String.IsNullOrEmpty(LwFactory.Default.GetWindowClass(hwnd)))
                            {
                                Operas.First().ThreadRun = false;
                                Task.Delay(1000).Wait();
                                Operas.First().UnBindWindow();
                                StartCheckTargetThread();
                                return;
                            }
                            Task.Delay(1000).Wait();
                        }
                    }
                    Task.Delay(1000).Wait();
                }
            });
        }

        #endregion

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
