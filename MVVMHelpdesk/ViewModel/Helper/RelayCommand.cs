﻿using System;
using System.Windows.Input;

namespace Imagio.Helpdesk.ViewModel.Helper
{
    public class RelayCommand: ICommand
    {
        readonly Action<object> _execute;
        readonly Func<bool> _canExecute;

        /// <summary>
        /// Initializes a new instance of the RelayCommand class that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <exception cref="System.ArgumentNullException">If the execute argument is null.</exception>
        public RelayCommand(Action execute)
            : this(o => { execute(); })
        {

        }

        public RelayCommand(Action<object> execute)
            : this(execute, () => true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <exception cref="System.ArgumentNullException">If the execute or canExecute argument is null.</exception>
        public RelayCommand(Action<object> execute, Func<bool> canExecute)
        {
            if (execute == null) throw new ArgumentNullException("execute");
            if (canExecute == null) throw new ArgumentNullException("canExecute");

            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        public void Execute(object parameter)
        {
            if (CanExecute(null))
                _execute(parameter);
        }

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
