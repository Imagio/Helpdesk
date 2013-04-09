using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.ViewModel.Helper
{
    public class ColumnData
    {
        public ColumnData(string data, string header)
        {
            Header = header;
            Data = data;
        }

        public string Header { get; private set; }

        public string Data { get; private set; }
    }
}
