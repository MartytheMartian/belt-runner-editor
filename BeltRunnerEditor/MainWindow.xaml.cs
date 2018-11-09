using System.Threading;
using System.Windows;
using System.Windows.Input;

using BeltRunnerEditor.ViewModels;

namespace BeltRunnerEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EntitiesWindow _entitiesWindow;
        private Timer _resizeTimer;

        /// <summary>
        /// Constructs a new instance of the <see cref="MainWindow" /> class
        /// </summary>
        public MainWindow()
        {
            /// Setup the task
            _resizeTimer = new Timer(Resize, null, -1, Timeout.Infinite);

            InitializeComponent();
        }
        
        /// <summary>
        /// Called when the exit menu item has been clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void ExitClicked(object sender, RoutedEventArgs e)
        {
            // Exit
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Called when the entities menu item has been clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void EntityClicked(object sender, RoutedEventArgs e)
        {
            OpenEntitiesWindow();
        }

        /// <summary>
        /// Called when the label is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void LabelClicked(object sender, MouseButtonEventArgs e)
        {
            OpenEntitiesWindow();
        }

        /// <summary>
        /// Called when the size of the window has changed
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments for the event</param>
        private void Resized(object sender, SizeChangedEventArgs e)
        {
            // Set the timer
            _resizeTimer.Change(500, Timeout.Infinite);
        }

        /// <summary>
        /// Processes resize events
        /// </summary>
        /// <param name="state">Useless parameter :D</param>
        private void Resize(object state)
        {
            // Invoke on the UI thread
            Dispatcher.Invoke(() =>
            {
                ((LevelModel)DataContext).Resize(canvas.RenderSize);
            });

            // Reset the timer
            _resizeTimer.Change(-1, Timeout.Infinite);
        }
        
        /// <summary>
        /// Opens the entities window
        /// </summary>
        private void OpenEntitiesWindow()
        {
            if (_entitiesWindow == null)
            {
                // Create the window
                _entitiesWindow = new EntitiesWindow();

                // Null out the window when it is unloaded
                _entitiesWindow.Unloaded += (s, ea) =>
                {
                    _entitiesWindow = null;
                };

                // Show it
                _entitiesWindow.Show();
            }
        }
    }
}
