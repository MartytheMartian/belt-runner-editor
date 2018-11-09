using System;
using System.Windows.Input;

#pragma warning disable CS0067

namespace BeltRunnerEditor
{
    /// <summary>
    /// Delegate used by WPF views
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// Called when the executable status of the delegate changes
        /// </summary>
        public event EventHandler CanExecuteChanged;

        private Action<object> _action;

        /// <summary>
        /// Constructs a new instance of the <see cref="DelegateCommand" /> class
        /// </summary>
        /// <param name="action">Action to be called on invocation</param>
        public DelegateCommand(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // Bypass parameters
            _action = (parameters) => { action(); };
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="DelegateCommand" /> class
        /// with an action that accepts parameters
        /// </summary>
        /// <param name="action">Action to be called on invocation</param>
        public DelegateCommand(Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _action = action;
        }

        /// <summary>
        /// Invokes the command
        /// </summary>
        /// <param name="parameter">Parameters to use</param>
        public void Execute(object parameter)
        {
            _action(parameter);
        }

        /// <summary>
        /// Determines if the delegate can be invoked
        /// </summary>
        /// <param name="parameter">Parameter that would be invoked</param>
        /// <returns>Can the delegate be invoked</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}