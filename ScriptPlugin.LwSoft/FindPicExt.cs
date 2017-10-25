using System;
using System.Threading.Tasks;
using System.Windows;

namespace ScriptPlugin.LwSoft
{
    /// <summary>
    /// 找图封装
    /// </summary>
    public class FindPicExt
    {
        #region 字段

        private readonly Lwsoft3 _lw;
        private bool _isTimeout;
        private int _millisecond;
        private int _interval;
        private Rect _rect;
        private string _bmp;
        private string _deltaColor;
        private double _sim;
        private bool _isClick;
        private int _clickCount;
        private int _clickDelay;
        private int _offsetX;
        private int _offsetY;
        private Action _timeoutAction;

        #endregion

        #region 构造函数

        public FindPicExt(Lwsoft3 lwsoft3)
        {
            Reset();
            _lw = lwsoft3;
        }

        public FindPicExt(Lwsoft3 lwsoft3, Rect rect) : this(lwsoft3)
        {
            _rect = rect;
        }

        public FindPicExt(Lwsoft3 lwsoft3, Rect rect, string bmp) : this(lwsoft3)
        {
            _rect = rect;
            _bmp = bmp;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 重置参数，方便复用对象
        /// </summary>
        /// <returns></returns>
        public FindPicExt Reset()
        {
            _rect = Rect.Empty;
            ResetIgnoreRect();
            return this;
        }


        /// <summary>
        /// 重置参数,忽略区域，方便复用对象
        /// </summary>
        /// <returns></returns>
        public FindPicExt ResetIgnoreRect()
        {
            _isTimeout = false;
            _millisecond = 0;
            _interval = 0;
            _bmp = null;
            _deltaColor = "000000";
            _sim = 0.8;
            _isClick = false;
            _clickCount = 0;
            _clickDelay = 0;
            _offsetX = 0;
            _offsetY = 0;
            _timeoutAction = null;
            return this;
        }

        /// <summary>
        /// 设置识别区域
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public FindPicExt SetRect(Rect rect)
        {
            _rect = rect;
            return this;
        }

        /// <summary>
        /// 设置图片路径
        /// </summary>
        /// <param name="bmp">多张图片用|隔开</param>
        /// <returns></returns>
        public FindPicExt SetImgPath(string bmp)
        {
            _bmp = bmp;
            return this;
        }

        /// <summary>
        /// 设置偏色
        /// </summary>
        /// <param name="deltaColor">默认000000</param>
        /// <returns></returns>
        public FindPicExt SetDeltaColor(string deltaColor)
        {
            _deltaColor = deltaColor;
            return this;
        }

        /// <summary>
        /// 设置相识度
        /// </summary>
        /// <param name="sim">0-1(默认0.8)</param>
        /// <returns></returns>
        public FindPicExt SetSim(double sim)
        {
            _sim = sim;
            return this;
        }

        /// <summary>
        /// 设置超时，在这个时间段会不断的识别
        /// </summary>
        /// <param name="millisecond">超时时间(毫秒)</param>
        /// <param name="interval">找图间隔时间</param>
        /// <param name="timeoutAction"></param>
        /// <returns></returns>
        public FindPicExt SetTimeout(int millisecond, int interval = 100, Action timeoutAction = null)
        {
            _millisecond = millisecond;
            _interval = interval;
            _isTimeout = true;
            _timeoutAction = timeoutAction;
            return this;
        }

        /// <summary>
        /// 设置找到图之后鼠标点击操作
        /// </summary>
        /// <param name="clickCount">点击次数</param>
        /// <param name="clickDelay">点击延时(多次点击时有效)</param>
        /// <param name="offsetX">X偏移</param>
        /// <param name="offsetY">Y偏移</param>
        /// <returns></returns>
        public FindPicExt SetAfterClick(int clickCount = 1, int clickDelay = 100, int offsetX = 0, int offsetY = 0)
        {
            _isClick = true;
            _clickCount = clickCount;
            _clickDelay = clickDelay;
            _offsetX = offsetX;
            _offsetY = offsetY;
            return this;
        }

        /// <summary>
        /// 开始识图
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            if (_rect.IsEmpty) throw new Exception("请设置识图区域 SetRect()");
            if (String.IsNullOrEmpty(_bmp)) throw new Exception("请设置识别的图片路径 SetImgPath()");
            if (_isTimeout)
            {
                var date = DateTime.Now;
                while ((DateTime.Now - date).TotalSeconds < _millisecond)
                {
                    if (_lw.FindPic(_rect, _bmp, _deltaColor, _sim))
                    {
                        AfterClick();
                        return true;
                    }
                    Task.Delay(_interval).Wait();
                }
                Console.WriteLine("FindPic超时");
                _timeoutAction?.Invoke();
            }
            else
            {
                if (_lw.FindPic(_rect, _bmp, _deltaColor, _sim))
                {
                    AfterClick();
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 私有方法

        private void AfterClick()
        {
            if (_isClick)
            {
                _lw.MoveTo(_lw.X() + _offsetX, _lw.Y() + _offsetY);
                for (int i = 0; i < _clickCount; i++)
                {
                    _lw.LeftClick().Delay(_clickDelay);
                }
            }
        }

        #endregion
    }
}
