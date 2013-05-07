using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.Model.Interfaces;
using Imagio.Helpdesk.ViewModel.Helper;
using Imagio.Helpdesk.ViewModel.Workspace;

namespace Imagio.Helpdesk.ViewModel
{
    public class ShellViewModel: WindowViewModel
    {
        public ShellViewModel()
        {
            TabCollection = new ObservableCollection<TabViewModel>();

            _generateDirectoryMenu();
        }

        public IList<MenuItem> DirectoryMenuCollection { get; private set; }
        private void _generateDirectoryMenu()
        {
            DirectoryMenuCollection = new List<MenuItem>
                {
                    new MenuItem("Производители", () => _addEntityTab<Firm>("Производители")),
                    new MenuItem("Типы картриджей", () => _addEntityTab<CartridgeType>("Типы картриджей")),
                    new MenuItem("Типы расходных материалов",
                                 () => _addEntityTab<ConsumableType>("Типы расходных материалов")),
                    new MenuItem("Типы аппаратного обеспечения",
                                 () => _addEntityTab<HardwareType>("Типы аппаратного обеспечения"))
                };
        }

        public void AddTab(TabViewModel tabViewModel)
        {
            TabCollection.Insert(0, tabViewModel);
            tabViewModel.CloseTabClick += sender => TabCollection.Remove(sender);
            CurrentTab = tabViewModel;
        }

        private void _addEntityTab<TE>(string title) where TE : class, IEntity
        {
            if (!TabCollection.OfType<Tab<Workspace<TE>>>().Any())
            {
                Waiter.WaitHandler.Run(() =>
                    {
                        var workspace = new Workspace<TE>();
                        var tab = new Tab<Workspace<TE>>(workspace, title);
                        AddTab(tab);
                    });
            }
            else
            {
                CurrentTab = TabCollection.OfType<Tab<Workspace<TE>>>().Single();
            }
        }

        public ObservableCollection<TabViewModel> TabCollection { get; private set; }

        private TabViewModel _currentTab;
        public TabViewModel CurrentTab
        {
            get { return _currentTab; }
            set
            {
                _currentTab = value;
                OnPropertyChanged(() => CurrentTab);
            }
        }

        private ICommand _softwareMenuCommand;
        public ICommand SoftwareMenuCommand 
        {
            get
            {
                _softwareMenuCommand = _softwareMenuCommand ?? new RelayCommand(() => _addEntityTab<Software>("Прогрммное обеспечение"));
                return _softwareMenuCommand;
            }
        }

        private ICommand _hardwareMenuCommand;
        public ICommand HardwareMenuCommand
        {
            get
            {
                _hardwareMenuCommand = _hardwareMenuCommand ?? new RelayCommand(() => _addEntityTab<Hardware>("Аппаратное обеспечение"));
                return _hardwareMenuCommand;
            }
        }

        private ICommand _consumableMenuCommand;
        public ICommand ConsumableMenuCommand
        {
            get
            {
                _consumableMenuCommand = _consumableMenuCommand ?? new RelayCommand(() => _addEntityTab<Consumable>("Расходные материалы"));
                return _consumableMenuCommand;
            }
        }

        private ICommand _cartridgeMenuCommand;
        public ICommand CartridgeMenuCommand
        {
            get
            {
                _cartridgeMenuCommand = _cartridgeMenuCommand ?? new RelayCommand(() => _addEntityTab<Cartridge>("Картриджи"));
                return _cartridgeMenuCommand;
            }
        }

        private ICommand _accountMenuCommand;
        public ICommand AccountMenuCommand
        {
            get
            {
                _accountMenuCommand = _accountMenuCommand ?? new RelayCommand(() => _addEntityTab<Account>("Учетные записи"));
                return _accountMenuCommand;
            }
        }

        private ICommand _employeeMenuCommand;
        public ICommand EmployeeMenuCommand
        {
            get
            {
                _employeeMenuCommand = _employeeMenuCommand ?? new RelayCommand(() => _addEntityTab<Employee>("Сотрудники"));
                return _employeeMenuCommand;
            }
        }

        private ICommand _backupMenuCommand;
        public ICommand BackupMenuCommand
        {
            get
            {
                _backupMenuCommand = _backupMenuCommand ?? new RelayCommand(() => AddTab(new Tab<BackupViewModel>(new BackupViewModel(), "Резервное копирование")));
                return _backupMenuCommand;
            }
        }

        private ICommand _serviceConsumableCommand;
        public ICommand ServiceConsumableCommand
        {
            get
            {
                _serviceConsumableCommand = _serviceConsumableCommand ?? new RelayCommand(() => _addEntityTab<ConsumableAccounting>("Учет расходных материалов"));
                return _serviceConsumableCommand;
            }
        }

        private ICommand _serviceCartridgeCommand;
        public ICommand ServiceCartridgeCommand
        {
            get { _serviceCartridgeCommand = _serviceCartridgeCommand ?? new RelayCommand(() => _addEntityTab<CartridgeAccounting>("Учет заправок картриджей"));
                return _serviceCartridgeCommand;
            }
        }
    }
}
