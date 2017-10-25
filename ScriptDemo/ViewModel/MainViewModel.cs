using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using ScriptDemo.Bll;
using ScriptPlugin.Common.Api;
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
        private WindowTaskBll _windowTask;
        private ObservableCollection<OperaBll> _operas;
        private int _maxThreadCount;
        private RunState _taskState;

        #endregion

        #region 属性

        //控制业务
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
            //开启监测线程
            //_windowTask.StartCheckByOneThread();
        }

        #endregion

        #region 命令

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        public RelayCommand ClosedCommand => new RelayCommand(() =>
        {
            _windowTask?.Dispose();
            LwFactory.Clear();
        });

        #endregion

        #region 方法

        //配置文件加载
        private bool InitData()
        {
            //初始化参数
            Operas = new ObservableCollection<OperaBll> {new OperaBll(LwFactory.Default)};
            _windowTask = new WindowTaskBll(Operas[0]);

            //初始化配置文件
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
                var area = PrimaryScreen.WorkingArea;
                var width = (int)area.Width;
                var height = (int)area.Height;
                var scaleX = PrimaryScreen.ScaleX;
                var scaleY = PrimaryScreen.ScaleY;
                if (Math.Abs(scaleX - 1) > 0 || Math.Abs(scaleY - 1) > 0)
                {
                    MessageBox.Show("当前缩放设置不正确，请设置为100%", "环境错误");
                    return false;
                }
                if (width != 1920 && height != 1080)
                {
                    MessageBox.Show($"只支持1920*1080分辨率，当前分辨率({width}*{height})不正确", "环境错误");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e);
                MessageBox.Show(e.Message, "环境错误");
                return false;
            }
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
