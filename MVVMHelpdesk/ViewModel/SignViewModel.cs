using System;
using System.Collections.Generic;
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

        private string _databasePath;
        public string DatabasePath
        {
            get { return _databasePath; }
            set { OnPropertyChanged(() => DatabasePath, ref _databasePath, value); }
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
                        if (String.IsNullOrEmpty(_login) || String.IsNullOrEmpty(_password))
                        {
                            if (Signed != null)
                                Signed(this, new SignedEventArgs(null));
                            return;
                        }

                        using (var context = new FakeContext())
                        {
                            Account account = null;
                            try
                            {
                                account = context.Accounts.Local.Single(w => w.Login == _login);
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
    }
}
