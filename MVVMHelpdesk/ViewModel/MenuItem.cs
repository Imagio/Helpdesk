using System;
using System.Windows.Input;
using Imagio.Helpdesk.ViewModel.Helper;

namespace Imagio.Helpdesk.ViewModel
{
    public class MenuItem: ViewModelBase
    {
        public MenuItem(string name, Action action)
        {
            Name = name;
            Command = new RelayCommand(action);
        }

        public string Name { get; private set; }

        public ICommand Command { get; private set; }
    }
}
