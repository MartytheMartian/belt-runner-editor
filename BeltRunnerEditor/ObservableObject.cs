using System;
using System.ComponentModel;

namespace BeltRunnerEditor
{
    /// <summary>
    /// An observable object
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Called when a property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event for a given property
        /// </summary>
        protected void RaisePropertyChangedEvent()
        {
            // Invoke if the event has been setup
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }
    }
}
