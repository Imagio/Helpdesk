using System.Windows;

namespace Waiter
{
    /// <summary>
    /// Логика взаимодействия для WaitIndicator.xaml
    /// </summary>
    public partial class WaitIndicator
    {
        public WaitIndicator()
        {
            InitializeComponent();
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                LayoutRoot.Visibility = Visibility.Collapsed;
        }

        #region Public Functions
        public void Start()
        {
            LayoutRoot.Visibility = Visibility.Visible;
            IndicatorStoryboard.Begin();
        }

        public void Stop()
        {
            LayoutRoot.Visibility = Visibility.Collapsed;
            IndicatorStoryboard.Stop();
        }
        #endregion
    }
}
