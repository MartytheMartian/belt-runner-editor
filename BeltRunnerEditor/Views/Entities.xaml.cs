using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using BeltRunnerEditor.DependencyResolution;

using ent = BeltRunnerEditor.Entities.Entity;

namespace BeltRunnerEditor.Views
{
    /// <summary>
    /// Interaction logic for Entities.xaml
    /// </summary>
    public partial class Entities : Window
    {
        private Entity _entityWindow;

        /// <summary>
        /// Constructs a new instance of the <see cref="Entities" /> class
        /// </summary>
        public Entities()
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
            // Create a new entity
            ServiceLocator.Instance.Level.Entities.Add(new ent());
            ServiceLocator.Instance.Entity.Entity = new ent();
            ServiceLocator.Instance.Entity.Index = -1;

            Open();
        }

        /// <summary>
        /// Called when the edit button on the entity list is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void EditClicked(object sender, RoutedEventArgs e)
        {
            // Get the button
            Button button = (Button)sender;

            // Grab the index
            int index = (int)button.CommandParameter;

            // Copy the entity
            ent existingEntity = ServiceLocator.Instance.Level.Entities[index];
            ent entity = new ent
            {
                ID = existingEntity.ID,
                Type = existingEntity.Type,
                Graphic = existingEntity.Graphic,
                X = existingEntity.X,
                Y = existingEntity.Y,
                DestinationX = existingEntity.DestinationX,
                DestinationY = existingEntity.DestinationY,
                Speed = existingEntity.Speed,
                Delay = existingEntity.Delay
            };

            // Load the entity
            ServiceLocator.Instance.Entity.Index = index;
            ServiceLocator.Instance.Entity.Entity = entity;

            Open();
        }

        /// <summary>
        /// Called when the delete button on the entity list is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void DeleteClicked(object sender, RoutedEventArgs e)
        {
            // Get the button
            Button button = (Button)sender;

            // Grab the index
            int index = (int)button.CommandParameter;

            // Delete the entity
            ServiceLocator.Instance.Level.Level.Entities.RemoveAt(index);

            // Unload the entity if necessary
            if (ServiceLocator.Instance.Entity.Index == index)
            {
                ServiceLocator.Instance.Entity.Entity = null;
                ServiceLocator.Instance.Entity.Index = -1;
                ServiceLocator.Instance.Entity.Update();
            }

            ServiceLocator.Instance.Level.Update();
        }

        /// <summary>
        /// Opens the entity window
        /// </summary>
        private void Open()
        {
            if (_entityWindow == null)
            {
                _entityWindow = new Entity();
                _entityWindow.Unloaded += (s, args) =>
                {
                    ServiceLocator.Instance.Entity.Entity = null;
                    ServiceLocator.Instance.Entity.Index = -1;
                    _entityWindow = null;
                };

                _entityWindow.Show();
            }
            else
            {
                ServiceLocator.Instance.Level.Update();
                ServiceLocator.Instance.Entity.Update();
            }
        }
    }
}
