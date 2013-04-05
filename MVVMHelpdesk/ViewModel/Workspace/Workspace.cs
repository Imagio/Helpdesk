using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.ViewModel.Helper;

namespace Imagio.Helpdesk.ViewModel.Workspace
{
    public abstract class Workspace: ViewModelBase
    {
    }

    public class Workspace<TE> : Workspace where TE : class
    {
        public Workspace()
        {
            ItemColletion = new ObservableCollection<TE>();
            _refreshCollection();
        }

        private void _refreshCollection()
        {
            ItemColletion.Clear();
            using (var context = new HelpdeskContext())
            {
                DbSet<TE> dbSet = EntityStore.Workspace<TE>(context) as DbSet<TE>;
                if (dbSet != null)
                {
                    foreach (var item in dbSet)
                    {
                        ItemColletion.Add(item);
                    }
                }
            }
        }

        public ObservableCollection<TE> ItemColletion { get; private set; }

        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                _addCommand = _addCommand ?? new RelayCommand(_addItem);
                return _addCommand;
            }
        }
        private void _addItem()
        {
        }
    }
}
