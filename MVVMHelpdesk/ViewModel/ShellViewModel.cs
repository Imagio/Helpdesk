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

        public void AddTab(TabViewModel tabViewModel)
        {
            TabCollection.Add(tabViewModel);
            tabViewModel.CloseTabClick += (sender) =>
                {
                    TabCollection.Remove(sender);
                };
            CurrentTab = tabViewModel;
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
                _softwareMenuCommand = _softwareMenuCommand ?? new RelayCommand(
                    () =>
                    {
                        var workspace = new Workspace<Software>();
                        var tab = new Tab<Workspace<Software>>(workspace, "Прогрммное обеспечение");
                        AddTab(tab);
                    });
                return _softwareMenuCommand;
            }
        }
    }
}
