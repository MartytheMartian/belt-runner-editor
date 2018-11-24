using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using BeltRunnerEditor.DependencyResolution;

using ent = BeltRunnerEditor.Entities.Entity;

namespace BeltRunnerEditor.Views
{
    /// <summary>
    /// Interaction logic for Entity.xaml
    /// </summary>
    public partial class Entity : Window
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="Entity" /> class
        /// </summary>
        public Entity()
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
                ServiceLocator.Instance.Entity.Update();
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

        /// <summary>
        /// Called when the save button is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            // Get the button
            Button button = (Button)sender;

            // Get the index
            int index = (int)button.CommandParameter;

            // Copy the entity
            ent existingEntity = ServiceLocator.Instance.Entity.Entity;
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

            // Save the entity
            if (index == -1)
            {
                ServiceLocator.Instance.Level.Level.Entities.Add(entity);
                ServiceLocator.Instance.Entity.Index = ServiceLocator.Instance.Level.Entities.Count - 1;
                ServiceLocator.Instance.Level.Update();
            }
            else
            {
                ServiceLocator.Instance.Level.Entities[index].ID = existingEntity.ID;
                ServiceLocator.Instance.Level.Entities[index].Type = existingEntity.Type;
                ServiceLocator.Instance.Level.Entities[index].Graphic = existingEntity.Graphic;
                ServiceLocator.Instance.Level.Entities[index].X = existingEntity.X;
                ServiceLocator.Instance.Level.Entities[index].Y = existingEntity.Y;
                ServiceLocator.Instance.Level.Entities[index].DestinationX = existingEntity.DestinationX;
                ServiceLocator.Instance.Level.Entities[index].DestinationY = existingEntity.DestinationY;
                ServiceLocator.Instance.Level.Entities[index].Speed = existingEntity.Speed;
                ServiceLocator.Instance.Level.Entities[index].Delay = existingEntity.Delay;
                ServiceLocator.Instance.Level.Update();
            }
        }
    }
}
