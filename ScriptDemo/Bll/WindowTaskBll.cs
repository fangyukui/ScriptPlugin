using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptDemo.Bll
{
    public class WindowTaskBll : IDisposable
    {
        private readonly ObservableCollection<OperaBll> _operas;
        private bool _isExit;
        public WindowTaskBll(ObservableCollection<OperaBll> operas)
        {
            _operas = operas;
        }

        public WindowTaskBll(OperaBll opera)
        {
            _operas = new ObservableCollection<OperaBll> {opera};
        }

        public void StartCheckByOneThread()
        {
            Task.Run(() =>
            {
                while (!_isExit)
                {
                    var hwnd = WindowHwndBll.GetTargetWindow();
                    if (hwnd > 0)
                    {
                        OperaBll opear = _operas.First();
                        opear.Hwnd = hwnd;
                        opear.Load();
                        opear.StartTask();

                        while (!_isExit)
                        {
                            if (!WindowHwndBll.ExistsWindow(hwnd))
                            {
                                _operas.First().ThreadRun = false;
                                Task.Delay(1000).Wait();
                                _operas.First().UnBindWindow();
                                StartCheckByOneThread();
                                return;
                            }
                            Task.Delay(1000).Wait();
                        }
                    }
                    Task.Delay(1000).Wait();
                }
            });
        }

        public void StartCheckByMultiThread()
        {
            Task.Run(() =>
            {
                while (!_isExit)
                {
                    var hwnds = WindowHwndBll.EnumTargetWindow();
                    //todo
                    Task.Delay(1000).Wait();
                }
            });
        }

        public void Dispose()
        {
            _isExit = true;
        }
    }
}
