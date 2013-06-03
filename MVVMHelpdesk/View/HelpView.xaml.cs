using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;

namespace Imagio.Helpdesk.View
{
    /// <summary>
    /// Логика взаимодействия для HelpView.xaml
    /// </summary>
    public partial class HelpView : Window
    {
        public HelpView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            XpsDocument doc = new XpsDocument("help.xps", System.IO.FileAccess.Read);
            dv.Document = doc.GetFixedDocumentSequence();
        }
    }
}
