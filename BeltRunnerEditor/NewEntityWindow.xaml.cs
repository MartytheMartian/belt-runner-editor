using System.Windows;
using System.Windows.Data;

using BeltRunnerEditor.ViewModels;

namespace BeltRunnerEditor
{
    /// <summary>
    /// Interaction logic for NewEntityWindow.xaml
    /// </summary>
    public partial class NewEntityWindow : Window
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="NewEntityWindow" /> class
        /// </summary>
        public NewEntityWindow()
        {
            InitializeComponent();

            // Bind to events
            SourceUpdated += EntityUpdated;
        }

        /// <summary>
        /// Called when the entity is updated
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void EntityUpdated(object sender, DataTransferEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ((LevelModel)DataContext).UpdateEntity();
            });
        }

        /// <summary>
        /// Called when the cancel button is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
