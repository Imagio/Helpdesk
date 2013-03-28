using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagio.Helpdesk.Model;

namespace Imagio.Helpdesk.Events
{
    internal class SignedEventArgs : EventArgs
    {
        public SignedEventArgs(Account account)
        {
            Account = account;
        }

        public Account Account { get; private set; }
    }
}
