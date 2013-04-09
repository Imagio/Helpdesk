using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Input;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.ViewModel.Helper;

namespace Imagio.Helpdesk.ViewModel.Workspace
{
    public abstract class Workspace: ViewModelBase
    {
    }

    public class Workspace<TE> : Workspace where TE : IEntity
    {
        public Workspace()
        {
            ItemColletion = new ObservableCollection<IEntity>();
            _refreshCollection();
        }

        private void _refreshCollection()
        {
            ItemColletion.Clear();
            using (var context = new HelpdeskContext())
            {
                DbSet dbSet = EntityStore.Workspace<TE>(context) as DbSet;
                if (dbSet != null)
                {
                    foreach (var item in dbSet)
                    {
                        ItemColletion.Add(item as IEntity);
                    }
                }
            }
        }

        public ObservableCollection<IEntity> ItemColletion { get; private set; }

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
                try
                {
                    var dbSet = EntityStore.Workspace<TE>(context);
                    var item = dbSet.Create() as IEntity;
                    item.Id = Guid.NewGuid();
                    dbSet.Add(item);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException validationException)
                {
                    var sb = new StringBuilder();
                    foreach (var item in validationException.EntityValidationErrors)
                        foreach (var itm in item.ValidationErrors)
                            sb.AppendFormat("[{0}] {1}\n", itm.PropertyName, itm.ErrorMessage);
                    System.Windows.MessageBox.Show(sb.ToString());
                }
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
        }

        private ObservableCollection<ColumnData> _columns = new ObservableCollection<ColumnData>();
        public ObservableCollection<ColumnData> Columns { get {return _columns;} }
        protected void AddColumn<T>(Expression<Func<T>> property, string header)
        {
            var columnData = new ColumnData((property.Body as MemberExpression).Member.Name, header);
            Columns.Add(columnData);
        }
    }
}
