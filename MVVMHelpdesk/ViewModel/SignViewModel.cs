using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
                            var openFileDialog = new Microsoft.Win32.OpenFileDialog
                                {
                                    Filter = "Файл БД MS SQL Compact*.sdf|*.sdf",
                                    FileName = DatabasePath,
                                    Multiselect = false
                                };
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
                                account = context.Accounts.FirstOrDefault(w => w.Login == _login && w.IsActive);
                                if (account != null)
                                {
                                    var pwd = Hash.ToString(account.Password);
                                    var pwdCheck = Hash.ToString(_password);

                                    account = pwd == pwdCheck ? account : null;
                                }
                            }
                            catch (NullReferenceException)
                            {
                                account = null;
                            }
                            catch (Exception ex)
                            {
                                System.Windows.MessageBox.Show(ex.Message);
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
                            var account = new Account {Login = "root", Password = Hash.Calc("root"), IsActive = true};
                            context.Set<Account>().Add(account);
                            context.SaveChanges();
                        }
                    });
            }
        }
    }
}
