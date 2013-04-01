using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.ViewModel.Helper;

namespace Imagio.Helpdesk.ViewModel
{
    public class ShellViewModel: WindowViewModel
    {
        public ShellViewModel()
        {
            WorkspaceCollection = new ObservableCollection<Workspace>();
            WorkspaceCollection.Add(new Workspace { Label = "Test" });
        }

        public ObservableCollection<Workspace> WorkspaceCollection { get; private set; }
    }
}
