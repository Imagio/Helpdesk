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
            for (int i = 0; i < 45; i++)
            {
                var w = new Workspace { Label = "Test" + i.ToString() };
                WorkspaceCollection.Add(w);
                w.CloseTabClick += w_CloseTabClick;
            }
        }

        void w_CloseTabClick(object sender)
        {
            WorkspaceCollection.Remove(sender as Workspace);
        }

        public ObservableCollection<Workspace> WorkspaceCollection { get; private set; }
    }
}
