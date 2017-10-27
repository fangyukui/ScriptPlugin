using System.Windows;
using System.Windows.Controls;

namespace ScriptPlugin.Common.Controls
{
    /// <summary>
    /// 等待控件
    /// </summary>
    [TemplateVisualState(GroupName = GroupActiveStates, Name = StateInactive)]
    [TemplateVisualState(GroupName = GroupActiveStates, Name = StateActive)]
    public class ModernProgressRing : Control
    {
        #region 常亮字段

        private const string GroupActiveStates = "ActiveStates";
        private const string StateInactive = "Inactive";
        private const string StateActive = "Active";

        #endregion 常亮字段

        #region 依赖项属性

        #region 是否活动

        /// <summary>
        /// Identifies the IsActive property.
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ModernProgressRing), new PropertyMetadata(false, OnIsActiveChanged));

        /// <summary>
        /// Gets or sets a value that indicates whether the <see cref="ModernProgressRing"/> is showing progress.
        /// </summary>
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        private static void OnIsActiveChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((ModernProgressRing)o).GotoCurrentState(true);
        }

        #endregion 是否活动

        #endregion 依赖项属性

        #region 构造函数

        static ModernProgressRing()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernProgressRing), new FrameworkPropertyMetadata(typeof(ModernProgressRing)));
        }

        #endregion 构造函数

        #region 私有方法

        /// <summary>
        /// 在派生类中重写后，每当应用程序代码或内部进程调用 System.Windows.FrameworkElement.ApplyTemplate，都将调用此方法。
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            GotoCurrentState(false);
        }

        private void GotoCurrentState(bool animate)
        {
            var state = this.IsActive ? StateActive : StateInactive;

            VisualStateManager.GoToState(this, state, animate);
        }

        #endregion 私有方法
    }
}
