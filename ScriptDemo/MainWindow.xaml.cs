using ScriptDemo.ViewModel;
using ScriptPlugin.Common.Extensions;

namespace ScriptDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            if (!this.IsDesignMode())
            {
                DataContext = MainViewModel.Instance;
            }
        }
    }
}
