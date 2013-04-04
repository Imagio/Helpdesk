using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Imagio.Helpdesk.ViewModel.Helper;

namespace Imagio.Helpdesk.ViewModel
{
    public class Workspace: ViewModelBase
    {
        public delegate void CloseTabClickHandler(object sender);
        public event CloseTabClickHandler CloseTabClick;

        public String Label { get; set; }

        private ICommand _closeTabCommand;
        public ICommand CloseTabCommand
        {
            get
            {
                _closeTabCommand = _closeTabCommand ?? new RelayCommand(() =>
                {
                    if (CloseTabClick != null)
                        CloseTabClick(this);
                });
                return _closeTabCommand;
            }
        }
    }
}
