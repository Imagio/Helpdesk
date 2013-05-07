using System;
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
