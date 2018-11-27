using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using BeltRunnerEditor.DependencyResolution;
using BeltRunnerEditor.Entities;

using ent = BeltRunnerEditor.Entities.Entity;

namespace BeltRunnerEditor.Views
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {
        private Entities _entitiesWindow;
        private Timer _resizeTimer;

        /// <summary>
        /// Constructs a new instance of the <see cref="Editor" /> class
        /// </summary>
        public Editor()
        {
            /// Setup the task
            _resizeTimer = new Timer(Resize, null, -1, Timeout.Infinite);

            InitializeComponent();
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

            // Modifiers
            bool shift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            // Handle specific inputs
            switch (e.Key)
            {
                case Key.S:
                    if (shift)
                    {
                        SaveAsClicked(null, null);
                    }
                    else
                    {
                        SaveClicked(null, null);
                    }
                    break;
                case Key.O:
                    OpenClicked(null, null);
                    break;
            }

            // Pass down to the base class
            base.OnKeyUp(e);
        }

        #region UI Events

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
                // Resize for model
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
                _entitiesWindow = new Entities();

                // Null out the window when it is unloaded
                _entitiesWindow.Unloaded += (s, ea) =>
                {
                    _entitiesWindow = null;
                };

                // Show it
                _entitiesWindow.Show();
            }
        }

        /// <summary>
        /// Called when the "New" menu item is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments for the event</param>
        private void NewClicked(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Instance.Entity.Entity = null;
            ServiceLocator.Instance.Level.Level = new Level();
        }

        /// <summary>
        /// Called when the "New" menu item is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments for the event</param>
        private void OpenClicked(object sender, RoutedEventArgs e)
        {
            // Read a level
            Level level = ServiceLocator.Instance.LevelRepository.Read();

            // Do nothing if a level wasn't loaded
            if (level == null)
            {
                return;
            }

            // Clear the current entity
            ServiceLocator.Instance.Entity.Entity = null;

            // Load the level
            ServiceLocator.Instance.Level.Level = level;
        }

        /// <summary>
        /// Called when the "Save" menu item is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments for the event</param>
        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            // Do nothing if a level isn't loaded
            if (!ServiceLocator.Instance.Level.Loaded)
            {
                return;
            }

            // Get the level
            Level level = ServiceLocator.Instance.Level.Level;

            // Set the entity
            ServiceLocator.Instance.LevelRepository.Save(level);
        }

        /// <summary>
        /// Called when the "Save As" menu item is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments for the event</param>
        private void SaveAsClicked(object sender, RoutedEventArgs e)
        {
            // Do nothing if a level isn't loaded
            if (!ServiceLocator.Instance.Level.Loaded)
            {
                return;
            }

            // Get the level
            Level level = ServiceLocator.Instance.Level.Level;

            // Set the entity
            ServiceLocator.Instance.LevelRepository.SaveAs(level);
        }

        #endregion

        #region Grid events

        /// <summary>
        /// Called when the top left cell is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments for the event</param>
        private void TopLeftClicked(object sender, RoutedEventArgs e)
        {
            // Get the current entity
            ent current = ServiceLocator.Instance.Entity.Entity;
            if (String.IsNullOrWhiteSpace(current.Graphic))
            {
                return;
            }

            // Get the current graphic
            BaseGraphic graphic = ServiceLocator.Instance.Level.Level.Graphics.Where(g => g.ID == current.Graphic).First();
            if (graphic == null)
            {
                return;
            }

            // Capture the size
            Size size = graphic.GetSize();

            // Create a point for the entity that represents the top left
            double x = 0 - (size.Width / 2);
            double y = 0 - (size.Height / 2);

            // Set the point
            ServiceLocator.Instance.Entity.SetPoint(x, y);
        }

        /// <summary>
        /// Called when the top center cell is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments for the event</param>
        private void TopCenterClicked(object sender, RoutedEventArgs e)
        {
            // Get the button
            Button button = (Button)sender;

            // Get the canvas
            Canvas grid = (Canvas)((Grid)(button.Parent)).Parent;

            // Get the mouse position on the canvas
            Point position = Mouse.GetPosition(canvas);

            // Get the current entity
            ent current = ServiceLocator.Instance.Entity.Entity;
            if (String.IsNullOrWhiteSpace(current.Graphic))
            {
                return;
            }

            // Get the current graphic
            BaseGraphic graphic = ServiceLocator.Instance.Level.Level.Graphics.Where(g => g.ID == current.Graphic).First();
            if (graphic == null)
            {
                return;
            }

            // Capture the size
            Size size = graphic.GetSize();

            // Create a point for the entity that represents the top center
            double x = position.X - 166.75;
            double y = 0 - (size.Height / 2);

            // Set the point
            ServiceLocator.Instance.Entity.SetPoint(x, y);
        }

        /// <summary>
        /// Called when the top right cell is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments for the event</param>
        private void TopRightClicked(object sender, RoutedEventArgs e)
        {
            // Get the current entity
            ent current = ServiceLocator.Instance.Entity.Entity;
            if (String.IsNullOrWhiteSpace(current.Graphic))
            {
                return;
            }

            // Get the current graphic
            BaseGraphic graphic = ServiceLocator.Instance.Level.Level.Graphics.Where(g => g.ID == current.Graphic).First();
            if (graphic == null)
            {
                return;
            }

            // Capture the size
            Size size = graphic.GetSize();

            // Create a point for the entity that represents the top right
            double x = 1334 + (size.Width / 2);
            double y = 0 - (size.Height / 2);

            // Set the point
            ServiceLocator.Instance.Entity.SetPoint(x, y);
        }

        /// <summary>
        /// Called when the middle left cell is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments for the event</param>
        private void MiddleLeftClicked(object sender, RoutedEventArgs e)
        {
            // Get the button
            Button button = (Button)sender;

            // Get the canvas
            Canvas grid = (Canvas)((Grid)(button.Parent)).Parent;

            // Get the mouse position on the canvas
            Point position = Mouse.GetPosition(canvas);

            // Get the current entity
            ent current = ServiceLocator.Instance.Entity.Entity;
            if (String.IsNullOrWhiteSpace(current.Graphic))
            {
                return;
            }

            // Get the current graphic
            BaseGraphic graphic = ServiceLocator.Instance.Level.Level.Graphics.Where(g => g.ID == current.Graphic).First();
            if (graphic == null)
            {
                return;
            }

            // Capture the size
            Size size = graphic.GetSize();

            // Create a point for the entity that represents the top center
            double x = 0 - (size.Width / 2);
            double y = position.Y - 93.75;

            // Set the point
            ServiceLocator.Instance.Entity.SetPoint(x, y);
        }

        /// <summary>
        /// Called when the middle center center is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments for the event</param>
        private void MiddleCenterClicked(object sender, RoutedEventArgs e)
        {
            // Get the button
            Button button = (Button)sender;

            // Get the canvas
            Canvas grid = (Canvas)((Grid)(button.Parent)).Parent;

            // Get the mouse position on the canvas
            Point position = Mouse.GetPosition(canvas);

            // Get the current entity
            ent current = ServiceLocator.Instance.Entity.Entity;
            if (String.IsNullOrWhiteSpace(current.Graphic))
            {
                return;
            }

            // Get the current graphic
            BaseGraphic graphic = ServiceLocator.Instance.Level.Level.Graphics.Where(g => g.ID == current.Graphic).First();
            if (graphic == null)
            {
                return;
            }

            // Capture the size
            Size size = graphic.GetSize();

            // Create a point for the entity that represents the top center
            double x = position.X - 166.75;
            double y = position.Y - 93.75;

            // Set the point
            ServiceLocator.Instance.Entity.SetPoint(x, y);
        }

        /// <summary>
        /// Called when the middle right cell is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments for the event</param>
        private void MiddleRightClicked(object sender, RoutedEventArgs e)
        {
            // Get the button
            Button button = (Button)sender;

            // Get the canvas
            Canvas grid = (Canvas)((Grid)(button.Parent)).Parent;

            // Get the mouse position on the canvas
            Point position = Mouse.GetPosition(canvas);

            // Get the current entity
            ent current = ServiceLocator.Instance.Entity.Entity;
            if (String.IsNullOrWhiteSpace(current.Graphic))
            {
                return;
            }

            // Get the current graphic
            BaseGraphic graphic = ServiceLocator.Instance.Level.Level.Graphics.Where(g => g.ID == current.Graphic).First();
            if (graphic == null)
            {
                return;
            }

            // Capture the size
            Size size = graphic.GetSize();

            // Create a point for the entity that represents the top center
            double x = 1334 + (size.Width / 2);
            double y = position.Y - 93.75;

            // Set the point
            ServiceLocator.Instance.Entity.SetPoint(x, y);
        }

        /// <summary>
        /// Called when the bottom left cell is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments for the event</param>
        private void BottomLeftClicked(object sender, RoutedEventArgs e)
        {
            // Get the current entity
            ent current = ServiceLocator.Instance.Entity.Entity;
            if (String.IsNullOrWhiteSpace(current.Graphic))
            {
                return;
            }

            // Get the current graphic
            BaseGraphic graphic = ServiceLocator.Instance.Level.Level.Graphics.Where(g => g.ID == current.Graphic).First();
            if (graphic == null)
            {
                return;
            }

            // Capture the size
            Size size = graphic.GetSize();

            // Create a point for the entity that represents the top right
            double x = 0 - (size.Width / 2);
            double y = 750 + (size.Height / 2);

            // Set the point
            ServiceLocator.Instance.Entity.SetPoint(x, y);
        }

        /// <summary>
        /// Called when the bottom middle center is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments for the event</param>
        private void BottomCenterClicked(object sender, RoutedEventArgs e)
        {
            // Get the button
            Button button = (Button)sender;

            // Get the canvas
            Canvas grid = (Canvas)((Grid)(button.Parent)).Parent;

            // Get the mouse position on the canvas
            Point position = Mouse.GetPosition(canvas);

            // Get the current entity
            ent current = ServiceLocator.Instance.Entity.Entity;
            if (String.IsNullOrWhiteSpace(current.Graphic))
            {
                return;
            }

            // Get the current graphic
            BaseGraphic graphic = ServiceLocator.Instance.Level.Level.Graphics.Where(g => g.ID == current.Graphic).First();
            if (graphic == null)
            {
                return;
            }

            // Capture the size
            Size size = graphic.GetSize();

            // Create a point for the entity that represents the top center
            double x = position.X - 166.75;
            double y = 750 + (size.Height / 2);

            // Set the point
            ServiceLocator.Instance.Entity.SetPoint(x, y);
        }

        /// <summary>
        /// Called when the bottom right cell is clicked
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments for the event</param>
        private void BottomRightClicked(object sender, RoutedEventArgs e)
        {
            // Get the current entity
            ent current = ServiceLocator.Instance.Entity.Entity;
            if (String.IsNullOrWhiteSpace(current.Graphic))
            {
                return;
            }

            // Get the current graphic
            BaseGraphic graphic = ServiceLocator.Instance.Level.Level.Graphics.Where(g => g.ID == current.Graphic).First();
            if (graphic == null)
            {
                return;
            }

            // Capture the size
            Size size = graphic.GetSize();

            // Create a point for the entity that represents the top right
            double x = 1334 + (size.Width / 2);
            double y = 750 + (size.Height / 2);

            // Set the point
            ServiceLocator.Instance.Entity.SetPoint(x, y);
        }

        #endregion
    }
}
