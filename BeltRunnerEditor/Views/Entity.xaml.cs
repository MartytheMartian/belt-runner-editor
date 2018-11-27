using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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
        /// Called when key press occurs
        /// </summary>
        /// <param name="e">Key event arguments</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Only care about shift commands
            bool ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            if (!ctrl)
            {
                return;
            }

            // Handle specific inputs
            switch (e.Key)
            {
                case Key.S:
                    SaveClicked(null, null);
                    break;
            }

            // Pass down to the base class
            base.OnKeyUp(e);
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
            // Get the index
            int index = ServiceLocator.Instance.Entity.Index;

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
                ServiceLocator.Instance.Entity.Index = ServiceLocator.Instance.Level.Level.Entities.Count - 1;
            }
            else
            {
                ServiceLocator.Instance.Level.Level.Entities[index].ID = existingEntity.ID;
                ServiceLocator.Instance.Level.Level.Entities[index].Type = existingEntity.Type;
                ServiceLocator.Instance.Level.Level.Entities[index].Graphic = existingEntity.Graphic;
                ServiceLocator.Instance.Level.Level.Entities[index].X = existingEntity.X;
                ServiceLocator.Instance.Level.Level.Entities[index].Y = existingEntity.Y;
                ServiceLocator.Instance.Level.Level.Entities[index].DestinationX = existingEntity.DestinationX;
                ServiceLocator.Instance.Level.Level.Entities[index].DestinationY = existingEntity.DestinationY;
                ServiceLocator.Instance.Level.Level.Entities[index].Speed = existingEntity.Speed;
                ServiceLocator.Instance.Level.Level.Entities[index].Delay = existingEntity.Delay;
            }

            // Ensure the entities are kept in the right order
            ServiceLocator.Instance.Level.Level.Entities = ServiceLocator.Instance.Level.Level.Entities
                .OrderBy(ent => ent.Delay.HasValue ? ent.Delay.Value : 0)
                .ThenBy(ent => ent.Type).ToList();
            ServiceLocator.Instance.Level.Update();
            ServiceLocator.Instance.Entity.Update();
        }
    }
}
