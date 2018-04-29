using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Minesweeper.MyViewModel
{
    /// <summary>
    /// RelayCommand helper class
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Action execute
        /// </summary>
        private Action<object> execute;

        /// <summary>
        /// Predicate canExecute
        /// </summary>
        private Predicate<object> canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand" /> class
        /// </summary>
        /// <param name="execute">Action execute</param>
        public RelayCommand(Action<object> execute) : this(execute, DefaultCanExecute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand" /> class
        /// </summary>
        /// <param name="execute">Action execute</param>
        /// <param name="canExecute">Predicate canExecute</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute ?? throw new ArgumentNullException("canExecute");
        }

        /// <summary>
        /// public event handler
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        /// <summary>
        /// Internal event handler
        /// </summary>
        private event EventHandler CanExecuteChangedInternal;

        /// <summary>
        /// Gets if can be executed
        /// </summary>
        /// <param name="parameter">Object to execute</param>
        /// <returns>Boolean value</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        /// <summary>
        /// Execute given param
        /// </summary>
        /// <param name="parameter">Param to execute</param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        /// <summary>
        /// On Execute changed
        /// </summary>
        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Destroy RelayCommand
        /// </summary>
        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }

        /// <summary>
        /// Return default value for CanExecute
        /// </summary>
        /// <param name="parameter">Object to execute</param>
        /// <returns>Boolean value</returns>
        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
