using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.Model.Interfaces;
using Imagio.Helpdesk.View;
using Imagio.Helpdesk.ViewModel.Helper;

namespace Imagio.Helpdesk.ViewModel.Workspace
{
    public abstract class Workspace : ViewModelBase
    {
    }

    public class Workspace<TE> : Workspace where TE : class, IEntity
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
                var dbSet = EntityStore.GetEntityQuery<TE>(context);
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
        private IEntity _selectedItem;
        public IEntity SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }

        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                _addCommand = _addCommand ?? new RelayCommand(_addAction);
                return _addCommand;
            }
        }
        private void _addAction()
        {
            using (var context = new HelpdeskContext())
            {
                var dbSet = context.Set<TE>();
                var item = dbSet.Create();
                item.Id = Guid.NewGuid();
                dbSet.Add(item);
                var window = new DialogView();
                var viewModel = EntityStore.ViewModel(item, context);

                viewModel.OnCancel += sender =>
                    {
                        if (window.IsVisible)
                            window.DialogResult = false;
                        _refreshCollection();
                    };

                viewModel.OnSave += sender =>
                    {
                        if (window.IsVisible)
                            window.DialogResult = false;
                        context.SaveChanges();
                        _refreshCollection();
                    };

                window.DataContext = viewModel;
                window.ShowDialog();
            }
        }

        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                _editCommand = _editCommand ?? new RelayCommand(_editAction);
                return _editCommand;
            }
        }
        private void _editAction()
        {
            if (_selectedItem == null)
                return;
            using (var context = new HelpdeskContext())
            {
                var query = EntityStore.GetEntityQuery<TE>(context);
                var item = query.Single(w => w.Id == _selectedItem.Id);

                var window = new DialogView();
                var viewModel = EntityStore.ViewModel(item, context);

                viewModel.OnCancel += sender =>
                {
                    if (window.IsVisible)
                        window.DialogResult = false;
                    _refreshCollection();
                };

                viewModel.OnSave += sender =>
                    {
                        if (window.IsVisible)
                            window.DialogResult = false;
                        context.SaveChanges();
                        _refreshCollection();
                    };
                window.DataContext = viewModel;
                window.ShowDialog();
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                _deleteCommand = _deleteCommand ?? new RelayCommand(_deleteAction);
                return _deleteCommand;
            }
        }
        private void _deleteAction()
        {
            if (_selectedItem == null)
                return;
            using (var context = new HelpdeskContext())
            {
                var item = context.Set<TE>().Single(w => w.Id == _selectedItem.Id);
                context.Set<TE>().Remove(item);
                context.SaveChanges();
                _refreshCollection();
            }
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                _refreshCommand = _refreshCommand ?? new RelayCommand(_refreshCollection);
                return _refreshCommand;
            }
        }
    }
}
