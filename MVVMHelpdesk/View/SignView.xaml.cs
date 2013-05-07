using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Imagio.Helpdesk.ViewModel;

namespace Imagio.Helpdesk.View
{
    /// <summary>
    /// Логика взаимодействия для SignView.xaml
    /// </summary>
    public partial class SignView
    {
        public SignView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var signViewModel = DataContext as SignViewModel;
            if (signViewModel != null)
            {
                var passwordBox = sender as PasswordBox;
                if (passwordBox != null) signViewModel.Password = passwordBox.Password;
            }
        }
    }
}
