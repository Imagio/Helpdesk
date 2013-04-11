using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.ViewModel.Entity;
using Imagio.Helpdesk.ViewModel.Helper;
using Imagio.Helpdesk.ViewModel.Workspace;

namespace Imagio.Helpdesk.ViewModel
{
    public class ShellViewModel: WindowViewModel
    {
        public ShellViewModel()
        {
            TabCollection = new ObservableCollection<TabViewModel>();
        }

        private bool _isWait;
        public bool IsWait
        {
            get { return _isWait; }
            set
            {
                _isWait = value;
                OnPropertyChanged(() => IsWait);
            }
        }

        public void AddTab(TabViewModel tabViewModel)
        {
            TabCollection.Insert(0, tabViewModel);
            tabViewModel.CloseTabClick += (sender) =>
                {
                    TabCollection.Remove(sender);
                };
            CurrentTab = tabViewModel;
        }

        private void _addEntityTab<TE>(string title) where TE : class, IEntity
        {
            Waiter.WaitHandler.Run(() =>
                {
                    var workspace = new Workspace<TE>();
                    var tab = new Tab<Workspace<TE>>(workspace, title);
                    AddTab(tab);
                });
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

        private ICommand _firmMenuCommand;
        public ICommand FirmMenuCommand
        {
            get
            {
                _firmMenuCommand = _firmMenuCommand ?? new RelayCommand(() => _addEntityTab<Firm>("Производители"));
                return _firmMenuCommand;
            }
        }
    }
}
