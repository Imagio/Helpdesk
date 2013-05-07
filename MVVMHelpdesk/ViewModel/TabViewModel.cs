using System;
using System.Windows.Input;
using Imagio.Helpdesk.ViewModel.Helper;

namespace Imagio.Helpdesk.ViewModel
{
    public class TabViewModel: ViewModelBase
    {
        public delegate void CloseTabClickHandler(TabViewModel sender);
        public event CloseTabClickHandler CloseTabClick;

        public String Label { get; protected set; }
        public ViewModelBase ViewModel { get; protected set; }

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

    public class Tab<TE> : TabViewModel where TE : ViewModelBase
    {
        public Tab(TE content, string title)
        {
            Label = title;
            ViewModel = content;
        }
    }
}
