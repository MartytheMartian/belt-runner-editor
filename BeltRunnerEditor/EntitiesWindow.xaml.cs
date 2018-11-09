using BeltRunnerEditor.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace BeltRunnerEditor
{
    /// <summary>
    /// Interaction logic for EntitiesWindow.xaml
    /// </summary>
    public partial class EntitiesWindow : Window
    {
        private NewEntityWindow _newEntityWindow;

        /// <summary>
        /// Constructs a new instance of the <see cref="EntitiesWindow" /> class
        /// </summary>
        public EntitiesWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the add label on the entity list is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void AddEntityClicked(object sender, MouseButtonEventArgs e)
        {
            // Force the data model to create a new entity
            ((LevelModel)DataContext).AddEntity();

            Open();
        }

        /// <summary>
        /// Called when the edit button on the entity list is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void EditClicked(object sender, RoutedEventArgs e)
        {
            Open();
        }

        /// <summary>
        /// Opens the entity window
        /// </summary>
        private void Open()
        {
            if (_newEntityWindow == null)
            {
                _newEntityWindow = new NewEntityWindow();
                _newEntityWindow.Unloaded += (s, args) =>
                {
                    _newEntityWindow = null;
                };

                _newEntityWindow.Show();
            }
        }
    }
}
