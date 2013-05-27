using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Imagio.Helpdesk.Events;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.ViewModel.Helper;

namespace Imagio.Helpdesk.ViewModel
{
    internal class SignViewModel : WindowViewModel
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
                            var account = new Account { Login = "root", Password = Hash.Calc("root"), IsActive = true };
                            context.Set<Account>().Add(account);
                            _generateData(context);
                            context.SaveChanges();
                        }
                    });
            }
        }

        private void _addCartridge(HelpdeskContext context, CartridgeType cartridgeType, int name)
        {
            var cartridge = new Cartridge { Id = Guid.NewGuid(), CartridgeType = cartridgeType, Name = name.ToString() };
            context.Cartridges.Add(cartridge);
            context.SaveChanges();
        }

        private void _addAccounting(HelpdeskContext context, int num, DateTime date)
        {
            string name = num.ToString();
            var cartridge = context.Cartridges.First(w => w.Name == name);
            var cartridgeAccounting = new CartridgeAccounting { Id = Guid.NewGuid(), Cartridge = cartridge, StartDate = date, EndDate = date, IsRefill = true };
            context.CartridgeAccountings.Add(cartridgeAccounting);
            context.SaveChanges();
        }

        private void _generateData(HelpdeskContext context)
        {
            context.CartridgeTypes.Add(new CartridgeType { Id = Guid.NewGuid(), Name = "12A" });
            context.SaveChanges();
            var cartridgeType = context.CartridgeTypes.First();

            #region

            var carts = new Dictionary<int, DateTime[]>
                {
                    {
                        64, new[]
                            {
                                new DateTime(2010, 07, 12),
                                new DateTime(2010, 11, 08)
                            }
                    },
                    {
                        100, new[]
                            {
                                new DateTime(2010, 02, 18),
                                new DateTime(2010, 06, 29),
                                new DateTime(2010, 11, 10),
                                new DateTime(2011, 03, 29),
                                new DateTime(2011, 09, 08),
                                new DateTime(2011, 12, 13),
                            }
                    },
                    {
                        102, new[]
                            {
                                new DateTime(2010, 01, 12),
                                new DateTime(2010, 08, 17),
                                new DateTime(2010, 08, 30),
                                new DateTime(2010, 09, 08),
                                new DateTime(2010, 09, 23),
                                new DateTime(2010, 11, 03),
                                new DateTime(2010, 12, 10)
                            }
                    },
                    {
                        103, new[]
                            {
                                new DateTime(2010, 01, 18),
                                new DateTime(2010, 03, 31),
                                new DateTime(2010, 04, 08),
                                new DateTime(2010, 06, 24),
                                new DateTime(2010, 09, 30),
                                new DateTime(2010, 11, 22),
                                new DateTime(2011, 03, 23),
                                new DateTime(2011, 06, 07),
                                new DateTime(2011, 08, 16),
                                new DateTime(2011, 10, 19),
                            }
                    },
                    {
                        105, new[]
                            {
                                new DateTime(2010, 04, 29),
                                new DateTime(2010, 08, 12),
                                new DateTime(2010, 10, 14),
                                new DateTime(2011, 03, 16),
                                new DateTime(2011, 08, 31),
                                new DateTime(2011, 11, 30),
                            }
                    },
                    {
                        107, new[]
                            {
                                new DateTime(2010, 01, 23),
                                new DateTime(2010, 06, 04),
                                new DateTime(2010, 07, 21),
                                new DateTime(2010, 09, 06),
                                new DateTime(2011, 01, 18),
                                new DateTime(2011, 04, 05),
                                new DateTime(2011, 03, 23),
                                new DateTime(2011, 05, 19),
                                new DateTime(2011, 09, 28),
                                new DateTime(2011, 11, 21),
                            }
                    },
                    {
                        108, new[]
                            {
                                new DateTime(2010, 03, 11),
                                new DateTime(2010, 07, 05),
                                new DateTime(2010, 09, 14),
                                new DateTime(2010, 12, 07),
                                new DateTime(2011, 03, 01),
                                new DateTime(2011, 5, 19),
                                new DateTime(2011, 06, 22),
                                new DateTime(2011, 07, 25),
                                new DateTime(2011, 09, 01),
                                new DateTime(2011, 10, 10),
                                new DateTime(2011, 11, 09),
                            }
                    },
                    {
                        115, new[]
                            {
                                new DateTime(2010, 04, 01),
                                new DateTime(2010, 04, 10),
                                new DateTime(2010, 04, 29),
                                new DateTime(2010, 06, 29),
                                new DateTime(2010, 09, 24),
                                new DateTime(2010, 11, 09),
                                new DateTime(2011, 01, 12),
                                new DateTime(2011, 03, 18),
                                new DateTime(2011, 03, 22),
                                new DateTime(2011, 06, 28),
                                new DateTime(2011, 10, 27),
                            }
                    },
                    {
                        116, new[]
                            {
                                new DateTime(2010, 01, 21),
                                new DateTime(2010, 04, 10),
                                new DateTime(2010, 04, 30),
                                new DateTime(2010, 05, 26),
                                new DateTime(2010, 07, 02),
                                new DateTime(2010, 07, 20),
                                new DateTime(2010, 08, 11),
                                new DateTime(2010, 08, 23),
                                new DateTime(2010, 09, 24),
                                new DateTime(2010, 09, 07),
                                new DateTime(2010, 11, 10),
                                new DateTime(2010, 12, 10),
                                new DateTime(2011, 01, 28),
                                new DateTime(2011, 03, 10),
                                new DateTime(2011, 04, 07),
                                new DateTime(2011, 06, 02),
                                new DateTime(2011, 09, 08),
                                new DateTime(2011, 12, 13),
                            }
                    },
                    {
                        117, new[]
                            {
                                new DateTime(2010, 01, 19)
                            }
                    },
                    {
                        118, new[]
                            {
                                new DateTime(2010, 02, 02),
                                new DateTime(2010, 04, 12),
                                new DateTime(2010, 06, 08),
                                new DateTime(2010, 07, 22),
                                new DateTime(2010, 10, 26),
                            }
                    },
                    {
                        123, new[]
                            {
                                new DateTime(2010, 01, 12),
                                new DateTime(2010, 01, 19),
                                new DateTime(2011, 11, 01),
                                new DateTime(2011, 12, 23),
                            }
                    },
                    {
                        124, new[]
                            {
                                new DateTime(2010, 08, 02),
                                new DateTime(2011, 05, 03),
                                new DateTime(2011, 09, 21),
                                new DateTime(2011, 11, 23),
                            }
                    },
                    {
                        126, new[]
                            {
                                new DateTime(2010, 02, 08),
                                new DateTime(2010, 04, 29),
                                new DateTime(2010, 06, 10),
                                new DateTime(2010, 07, 22),
                                new DateTime(2010, 09, 16),
                                new DateTime(2010, 11, 19),
                                new DateTime(2011, 01, 28),
                                new DateTime(2011, 04, 16),
                                new DateTime(2011, 06, 22),
                                new DateTime(2011, 09, 12),
                                new DateTime(2011, 12, 13),
                            }
                    },
                    {
                        128, new[]
                            {
                                new DateTime(2010, 12, 16)
                            }
                    },
                    {
                        131, new[]
                            {
                                new DateTime(2011, 01, 12),
                                new DateTime(2011, 06, 22),
                                new DateTime(2011, 09, 08),
                                new DateTime(2011, 11, 09),
                            }
                    },
                    {
                        134, new[]
                            {
                                new DateTime(2010, 01, 20),
                                new DateTime(2010, 03, 25),
                                new DateTime(2010, 05, 04),
                                new DateTime(2010, 07, 15),
                                new DateTime(2010, 11, 03),
                                new DateTime(2011, 01, 28),
                                new DateTime(2011, 09, 09),
                                new DateTime(2011, 10, 19),
                                new DateTime(2011, 12, 13),
                            }
                    },
                    {
                        135, new[]
                            {
                                new DateTime(2010, 02, 10),
                                new DateTime(2010, 04, 01),
                                new DateTime(2010, 06, 02),
                                new DateTime(2010, 09, 01),
                                new DateTime(2010, 10, 28),
                                new DateTime(2010, 12, 23),
                                new DateTime(2011, 03, 11),
                                new DateTime(2011, 05, 10),
                                new DateTime(2011, 05, 19),
                                new DateTime(2011, 10, 3),
                            }
                    },
                    {
                        137, new[]
                            {
                                new DateTime(2010, 05, 04),
                                new DateTime(2010, 07, 08),
                                new DateTime(2010, 08, 24),
                                new DateTime(2010, 10, 14),
                                new DateTime(2011, 01, 12),
                                new DateTime(2011, 03, 23),
                                new DateTime(2011, 06, 07),
                                new DateTime(2011, 09, 22),
                                new DateTime(2011, 10, 10),
                                new DateTime(2011, 11, 30),
                            }
                    },
                    {
                        138, new[]
                            {
                                new DateTime(2010, 03, 01),
                                new DateTime(2010, 04, 30),
                                new DateTime(2010, 06, 10),
                                new DateTime(2010, 07, 22),
                                new DateTime(2010, 08, 23),
                                new DateTime(2010, 12, 28),
                                new DateTime(2011, 01, 12),
                                new DateTime(2011, 03, 18),
                                new DateTime(2011, 05, 19),
                                new DateTime(2011, 07, 12),
                                new DateTime(2011, 09, 01),
                                new DateTime(2011, 10, 19),
                            }
                    },
                    {
                        140, new[]
                            {
                                new DateTime(2010, 01, 26),
                                new DateTime(2010, 11, 09),
                                new DateTime(2011, 10, 31),
                            }
                    },
                    {
                        145, new[]
                            {
                                new DateTime(2010, 01, 29),
                                new DateTime(2011, 01, 14),
                                new DateTime(2011, 09, 08),
                                new DateTime(2011, 12, 23),
                            }
                    },
                    {
                        149, new[]
                            {
                                new DateTime(2011, 01, 31),
                                new DateTime(2011, 04, 19),
                                new DateTime(2011, 09, 21),
                                new DateTime(2011, 10, 31),
                            }
                    },
                    {
                        152, new[]
                            {
                                new DateTime(2010, 03, 19),
                                new DateTime(2010, 08, 09),
                                new DateTime(2011, 03, 18),
                                new DateTime(2011, 09, 28),
                                new DateTime(2011, 12, 12),
                            }
                    },
                    {
                        154, new[]
                            {
                                new DateTime(2010, 06, 02),
                                new DateTime(2010, 10, 18),
                                new DateTime(2011, 04, 27),
                                new DateTime(2011, 05, 19),
                                new DateTime(2011, 06, 22),
                                new DateTime(2011, 09, 15),
                            }
                    },
                    {
                        155, new[]
                            {
                                new DateTime(2010, 03, 03),
                                new DateTime(2010, 04, 13),
                                new DateTime(2010, 05, 12),
                                new DateTime(2010, 06, 08),
                                new DateTime(2010, 06, 21),
                                new DateTime(2010, 07, 02),
                                new DateTime(2010, 07, 08),
                                new DateTime(2010, 07, 29),
                                new DateTime(2010, 11, 10),
                                new DateTime(2010, 12, 06),
                                new DateTime(2011, 01, 18),
                                new DateTime(2011, 03, 10),
                                new DateTime(2011, 04, 27),
                                new DateTime(2011, 06, 03),
                                new DateTime(2011, 07, 22),
                                new DateTime(2011, 10, 3),
                                new DateTime(2011, 11, 9),
                            }
                    },
                    {
                        156, new[]
                            {
                                new DateTime(2010, 03, 11),
                                new DateTime(2010, 08, 23),
                                new DateTime(2011, 04, 26),
                                new DateTime(2011, 10, 11),
                                new DateTime(2011, 12, 14),
                            }
                    },
                    {
                        158, new[]
                            {
                                new DateTime(2010, 07, 01),
                                new DateTime(2010, 11, 12),
                                new DateTime(2011, 03, 11),
                                new DateTime(2011, 03, 16),
                                new DateTime(2011, 05, 30),
                                new DateTime(2011, 09, 12),
                            }
                    },
                    {
                        159, new[]
                            {
                                new DateTime(2010, 02, 24),
                                new DateTime(2010, 04, 10),
                                new DateTime(2010, 05, 14),
                                new DateTime(2010, 07, 02),
                                new DateTime(2010, 09, 24),
                                new DateTime(2010, 11, 09),
                                new DateTime(2010, 12, 10),
                                new DateTime(2010, 12, 24),
                                new DateTime(2011, 01, 28),
                                new DateTime(2011, 02, 11),
                                new DateTime(2011, 03, 10),
                                new DateTime(2011, 03, 18),
                                new DateTime(2011, 04, 07),
                                new DateTime(2011, 04, 25),
                                new DateTime(2011, 07, 06),
                                new DateTime(2011, 10, 10),
                            }
                    },
                    {
                        161, new[]
                            {
                                new DateTime(2010, 01, 11),
                                new DateTime(2010, 03, 22),
                                new DateTime(2010, 04, 02),
                                new DateTime(2010, 04, 13),
                                new DateTime(2010, 04, 13),
                                new DateTime(2010, 05, 25),
                                new DateTime(2010, 08, 12),
                                new DateTime(2010, 10, 20),
                                new DateTime(2010, 12, 24),
                                new DateTime(2011, 03, 01),
                                new DateTime(2011, 05, 16),
                                new DateTime(2011, 07, 12),
                                new DateTime(2011, 09, 22),
                            }
                    },
                    {
                        162, new[]
                            {
                                new DateTime(2010, 01, 12),
                                new DateTime(2010, 02, 19),
                                new DateTime(2011, 01, 27),
                                new DateTime(2011, 06, 14),
                                new DateTime(2011, 12, 01),
                            }
                    },
                    {
                        163, new[]
                            {
                                new DateTime(2010, 01, 18),
                                new DateTime(2010, 03, 11),
                                new DateTime(2010, 04, 30),
                                new DateTime(2010, 08, 18),
                                new DateTime(2010, 10, 16),
                                new DateTime(2010, 12, 10),
                                new DateTime(2011, 04, 04),
                                new DateTime(2011, 07, 14),
                                new DateTime(2011, 10, 11),
                            }
                    },
                    {
                        166, new[]
                            {
                                new DateTime(2010, 01, 22)
                            }
                    },
                    {
                        168, new[]
                            {
                                new DateTime(2010, 01, 26)
                            }
                    },
                    {
                        171, new[]
                            {
                                new DateTime(2010, 01, 29),
                                new DateTime(2010, 11, 03),
                                new DateTime(2011, 03, 03),
                                new DateTime(2011, 05, 16),
                                new DateTime(2011, 10, 11),
                            }
                    },
                    {
                        172, new[]
                            {
                                new DateTime(2011, 10, 7),
                                new DateTime(2011, 11, 9),
                                new DateTime(2011, 12, 23),
                            }
                    },
                    {
                        173, new[]
                            {
                                new DateTime(2010, 02, 01)
                            }
                    },
                    {
                        177, new[]
                            {
                                new DateTime(2010, 03, 31)
                            }
                    },
                    {
                        179, new[]
                            {
                                new DateTime(2010, 02, 24),
                                new DateTime(2010, 08, 12),
                                new DateTime(2010, 09, 30),
                                new DateTime(2010, 12, 22),
                                new DateTime(2011, 03, 18),
                                new DateTime(2011, 07, 15),
                                new DateTime(2011, 11, 30),
                            }
                    },
                    {
                        183, new[]
                            {
                                new DateTime(2010, 03, 17),
                                new DateTime(2011, 06, 10),
                                new DateTime(2011, 10, 20),
                                new DateTime(2011, 11, 25),
                            }
                    },
                    {
                        190, new[]
                            {
                                new DateTime(2010, 07, 08)
                            }
                    },
                    {
                        191, new[]
                            {
                                new DateTime(2010, 05, 05),
                                new DateTime(2010, 07, 16),
                                new DateTime(2010, 11, 29),
                                new DateTime(2011, 05, 05),
                                new DateTime(2011, 09, 26),
                            }
                    },
                    {
                        195, new[]
                            {
                                new DateTime(2011, 10, 3),
                                new DateTime(2011, 11, 9),
                                new DateTime(2011, 12, 23),
                            }
                    },
                    {
                        198, new[]
                            {
                                new DateTime(2011, 05, 03),
                                new DateTime(2011, 09, 14),
                                new DateTime(2011, 11, 09),
                            }
                    },
                    {
                        199, new[]
                            {
                                new DateTime(2010, 06, 02)
                            }
                    },
                    {
                        202, new[]
                            {
                                new DateTime(2010, 06, 28),
                                new DateTime(2010, 08, 11),
                                new DateTime(2010, 09, 11),
                                new DateTime(2010, 12, 24),
                                new DateTime(2011, 09, 08),
                                new DateTime(2011, 10, 10),
                                new DateTime(2011, 11, 09),
                                new DateTime(2011, 12, 14),
                            }
                    },
                    {
                        207, new[]
                            {
                                new DateTime(2011, 01, 18),
                                new DateTime(2011, 07, 26),
                                new DateTime(2011, 12, 13),
                            }
                    },
                    {
                        213, new[]
                            {
                                new DateTime(2010, 09, 24)
                            }
                    },
                    {
                        214, new[]
                            {
                                new DateTime(2010, 10, 15),
                                new DateTime(2010, 11, 25)
                            }
                    },
                    {
                        215, new[]
                            {
                                new DateTime(2011, 04, 01),
                                new DateTime(2011, 12, 20),
                            }
                    },
                    {
                        222, new[]
                            {
                                new DateTime(2011, 05, 10),
                                new DateTime(2011, 07, 23),
                                new DateTime(2011, 11, 30),
                            }
                    },
                    {
                        226, new[]
                            {
                                new DateTime(2011, 08, 22),
                                new DateTime(2011, 12, 13),
                            }
                    },
                    {
                        228, new[]
                            {
                                new DateTime(2011, 05, 10),
                                new DateTime(2011, 07, 07),
                                new DateTime(2011, 09, 27),
                            }
                    }
                };

            foreach (var num in carts)
            {
                _addCartridge(context, cartridgeType, num.Key);
                foreach (var dat in num.Value)
                {
                    _addAccounting(context, num.Key, dat);
                }
            }

            #endregion

            #region Сотрудники

            context.Employees.Add(new Employee { Id = Guid.NewGuid(), LastName = "Иванов", FirstName = "Петр", SecondName = "Федорович" });
            context.Employees.Add(new Employee { Id = Guid.NewGuid(), LastName = "Петров", FirstName = "Иван", SecondName = "Сидорович" });
            context.Employees.Add(new Employee { Id = Guid.NewGuid(), LastName = "Карлов", FirstName = "Полиграф", SecondName = "Ефремович" });
            context.Employees.Add(new Employee { Id = Guid.NewGuid(), LastName = "Сидорова", FirstName = "Ольга", SecondName = "Николаевна" });
            context.Employees.Add(new Employee { Id = Guid.NewGuid(), LastName = "Якушонок", FirstName = "Галина", SecondName = "Георгиевна" });
            context.SaveChanges();

            #endregion

            #region Производители

            context.Firms.Add(new Firm { Id = Guid.NewGuid(), Name = "Microsoft" });
            context.Firms.Add(new Firm { Id = Guid.NewGuid(), Name = "Oracle" });
            context.Firms.Add(new Firm { Id = Guid.NewGuid(), Name = "HP" });
            context.Firms.Add(new Firm { Id = Guid.NewGuid(), Name = "Canon" });
            context.Firms.Add(new Firm { Id = Guid.NewGuid(), Name = "LG" });
            context.Firms.Add(new Firm { Id = Guid.NewGuid(), Name = "Samsung" });
            context.Firms.Add(new Firm { Id = Guid.NewGuid(), Name = "Asus" });
            context.Firms.Add(new Firm { Id = Guid.NewGuid(), Name = "Lenovo" });
            context.Firms.Add(new Firm { Id = Guid.NewGuid(), Name = "ABBYY" });
            context.Firms.Add(new Firm { Id = Guid.NewGuid(), Name = "Autodesk" });
            context.Firms.Add(new Firm { Id = Guid.NewGuid(), Name = "Adobe" });
            context.Firms.Add(new Firm { Id = Guid.NewGuid(), Name = "Kaspersky" });
            context.SaveChanges();

            #endregion

            #region Софт

            var microsoft = context.Firms.Single(w => w.Name.Contains("Microsoft"));

            context.Softwares.Add(new Software { Id = Guid.NewGuid(), Maker = microsoft, Name = "Windows XP", InventoryNumber = Guid.NewGuid().ToString().Substring(1, 7) });
            context.Softwares.Add(new Software { Id = Guid.NewGuid(), Maker = microsoft, Name = "Windows Vista", InventoryNumber = Guid.NewGuid().ToString().Substring(1, 7) });
            context.Softwares.Add(new Software { Id = Guid.NewGuid(), Maker = microsoft, Name = "Windows 7", InventoryNumber = Guid.NewGuid().ToString().Substring(1, 7) });
            context.Softwares.Add(new Software { Id = Guid.NewGuid(), Maker = microsoft, Name = "Windows 8", InventoryNumber = Guid.NewGuid().ToString().Substring(1, 7) });
            context.Softwares.Add(new Software { Id = Guid.NewGuid(), Maker = microsoft, Name = "Windows Server 2008 R2", InventoryNumber = Guid.NewGuid().ToString().Substring(1, 7) });

            var oracle = context.Firms.Single(w => w.Name.Contains("Oracle"));

            context.Softwares.Add(new Software { Id = Guid.NewGuid(), Maker = oracle, Name = "Java EE", InventoryNumber = Guid.NewGuid().ToString().Substring(1, 7) });
            context.Softwares.Add(new Software { Id = Guid.NewGuid(), Maker = oracle, Name = "MySQL Server Enterprise", InventoryNumber = Guid.NewGuid().ToString().Substring(1, 7) });

            var abbyy = context.Firms.Single(w => w.Name.Contains("ABBYY"));
            context.Softwares.Add(new Software { Id = Guid.NewGuid(), Maker = abbyy, Name = "FineReader 10.0", InventoryNumber = Guid.NewGuid().ToString().Substring(1, 7) });

            context.SaveChanges();

            #endregion

            #region Тип аппаратного обеспечения

            context.HardwareTypes.Add(new HardwareType { Id = Guid.NewGuid(), Name = "Принтер" });
            context.HardwareTypes.Add(new HardwareType { Id = Guid.NewGuid(), Name = "Сканер" });
            context.HardwareTypes.Add(new HardwareType { Id = Guid.NewGuid(), Name = "Копир" });
            context.HardwareTypes.Add(new HardwareType { Id = Guid.NewGuid(), Name = "Монитор" });
            context.HardwareTypes.Add(new HardwareType { Id = Guid.NewGuid(), Name = "Внешний жесткий диск" });
            context.HardwareTypes.Add(new HardwareType { Id = Guid.NewGuid(), Name = "Внешний привод" });
            context.HardwareTypes.Add(new HardwareType { Id = Guid.NewGuid(), Name = "МФУ" });

            context.SaveChanges();

            #endregion

            #region Устройства

            var scaner = context.HardwareTypes.Single(w => w.Name.Contains("Сканер"));
            var samsung = context.Firms.Single(w => w.Name.Contains("Samsung"));

            context.Hardwares.Add(new Hardware { Id = Guid.NewGuid(), Name = "ScanJet 300 S2", HardwareType = scaner, Maker = samsung, InventoryNumber = Guid.NewGuid().ToString().Substring(1, 7), StartDate = new DateTime(2012, 04, 1) });

            context.SaveChanges();

            #endregion

            #region Расходники

            context.ConsumableTypes.Add(new ConsumableType { Id = Guid.NewGuid(), Name = "Тонер" });
            context.ConsumableTypes.Add(new ConsumableType { Id = Guid.NewGuid(), Name = "Фоторецептор" });

            context.SaveChanges();

            #endregion
        }
    }
}
