using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.View;
using Imagio.Helpdesk.ViewModel;

namespace Imagio.Helpdesk
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SignView _signView;
        private ShellView _shellView;

        internal static Account Account { get; private set; }
        internal static DbConnection DatabaseConnection;

        protected override void OnStartup(StartupEventArgs e)
        {
            /*var context = new Imagio.Helpdesk.Model.HelpdeskContext();
            context.Database.CreateIfNotExists();
            context.SaveChanges();*/

            base.OnStartup(e);

            this.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;

            var signViewModel = new SignViewModel();
            signViewModel.Signed += signViewModel_Signed;
            signViewModel.OnShutdown += signViewModel_OnShutdown;

            _signView = new SignView();
            _signView.DataContext = signViewModel;
            _signView.Show();
        }

        void signViewModel_OnShutdown(object sender)
        {
            if (Account == null)
                this.Shutdown();
        }

        void signViewModel_Signed(object sender, Events.SignedEventArgs e)
        {
            if (e.Account == null)
            {
                MessageBox.Show(
                    "Неправильный логин или пароль",
                    "Ошибка входа",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.OK);
            }
            else
            {
                Account = e.Account;
                _signView.Close();

                var shellViewModel = new ShellViewModel();
                shellViewModel.OnShutdown += OnShutdown;

                _shellView = new ShellView();
                _shellView.DataContext = shellViewModel;
                _shellView.Show();
            }
        }

        void OnShutdown(object sender)
        {
            Shutdown();
        }
    }
}
