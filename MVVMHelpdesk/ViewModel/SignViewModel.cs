using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Imagio.Helpdesk.Events;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.ViewModel.Helper;

namespace Imagio.Helpdesk.ViewModel
{
    internal class SignViewModel: WindowViewModel
    {
        public delegate void OnSignedEventHandler(object sender, SignedEventArgs e);
        public event OnSignedEventHandler Signed;

        private string _login;
        public string Login
        {
            get { return _login; }
            set
            {
                OnPropertyChanged(() => Login, ref _login, value);
            }
        }

        private string _password;
        public string Password
        {
            set
            {
                _password = value;
            }
        }

        public string DatabasePath
        {
            get { return Properties.Settings.Default.DatabasePath; }
            set 
            {
                Properties.Settings.Default.DatabasePath = value;
                OnPropertyChanged(() => DatabasePath); 
            }
        }
        
        private ICommand _changeDatabasePathCommand;
        public ICommand ChangeDatabasePathCommand
        {
            get 
            {
                _changeDatabasePathCommand = _changeDatabasePathCommand ?? new RelayCommand(
                    () =>
                        {
                            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                            openFileDialog.Filter = "Файл БД MS SQL Compact*.sdf|*.sdf";
                            openFileDialog.Multiselect = false;
                            if (openFileDialog.ShowDialog() == true)
                            {
                                DatabasePath = openFileDialog.FileName;
                            }
                        });
                return _changeDatabasePathCommand; 
            }
        }

        private ICommand _checkSignCommand;
        public ICommand CheckSignCommand
        {
            get
            {
                _checkSignCommand = _checkSignCommand ?? new RelayCommand(
                    () =>
                    {
                        _generateDataBase();
                        using (var context = new HelpdeskContext())
                        {
                            Account account = null;
                            try
                            {
                                account = context.Accounts.Single(w => w.Login == _login && w.IsActive);
                                var pwd = Hash.ToString(account.Password);
                                var pwd_check = Hash.ToString(_password);

                                account = pwd == pwd_check ? account : null;
                            }
                            catch
                            {

                            }
                            if (Signed != null)
                                Signed(this, new SignedEventArgs(account));
                        }
                    });
                return _checkSignCommand;
            }
        }

        private void _generateDataBase()
        {
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
            var connection = Database.DefaultConnectionFactory.CreateConnection(String.Format("Data Source={0}", Properties.Settings.Default.DatabasePath));
            App.DatabaseConnection = connection;
            if (!Database.Exists(connection))
            {
                Waiter.WaitHandler.Run(() =>
                    {
                        using (var context = new HelpdeskContext())
                        {
                            context.Database.CreateIfNotExists();
                            Account account = new Account();
                            account.Login = "root";
                            account.Password = Hash.Calc("root");
                            account.IsActive = true;
                            context.Set<Account>().Add(account);
                            context.SaveChanges();
                        }
                    });
            }
        }
    }
}
