using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Imagio.Helpdesk.ViewModel.Helper
{
    public abstract class WindowViewModel: ViewModelBase
    {
        public delegate void ShutdownEventHandler(object sender);
        public event ShutdownEventHandler OnShutdown;

        private ICommand _shutdownCommand;
        public ICommand ShutdownCommand
        {
            get
            {
                _shutdownCommand = _shutdownCommand ?? new RelayCommand(
                    () =>
                    {
                        if (OnShutdown != null)
                        {
                            OnShutdown(this);
                        }
                    });
                return _shutdownCommand;
            }
        }
    }
}
