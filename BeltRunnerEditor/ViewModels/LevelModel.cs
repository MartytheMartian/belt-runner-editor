using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using BeltRunnerEditor.Models;
using BeltRunnerEditor.Repositories;

namespace BeltRunnerEditor.ViewModels
{
    /// <summary>
    /// View model for a level
    /// </summary>
    public class LevelModel : ObservableObject
    {
        // Get the level repository instance
        private LevelRepository _lr = LevelRepository.Instance;

        private int _index = -1;
        private Entity _entity = null;

        // Track the current applied scaling
        double _scaleX = 1;
        double _scaleY = 1;

        /// <summary>
        /// Is the level loaded
        /// </summary>
        public bool Loaded
        {
            get
            {
                return LevelRepository.Instance.Get() != null;
            }
        }

        /// <summary>
        /// Visibility of the canvas
        /// </summary>
        public Visibility CanvasVisibility
        {
            get
            {
                return Loaded ? Visibility.Visible : Visibility.Hidden;
            }
        }

        /// <summary>
        /// First point of the label
        /// </summary>
        public Visibility FirstPointVisibility
        {
            get
            {
                return PointOne.HasValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Second point of the label
        /// </summary>
        public Visibility SecondPointVisibility
        {
            get
            {
                return PointTwo.HasValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Is an entity currently selected
        /// </summary>
        public bool EntityUnselected
        {
            get
            {
                return _entity == null;
            }
        }

        /// <summary>
        /// List of entities in the current level
        /// </summary>
        public ObservableCollection<Entity> Entities
        {
            get
            {
                return new ObservableCollection<Entity>(LevelRepository.Instance.Get().Entities);
            }
        }

        /// <summary>
        /// List of available graphics
        /// </summary>
        public List<string> Graphics
        {
            get
            {
                return _lr.Get().Graphics.Select(g => g.ID).ToList();
            }
        }

        /// <summary>
        /// Selected entity
        /// </summary>
        public Entity SelectedEntity
        {
            get
            {
                return _entity;
            }
        }

        /// <summary>
        /// Position of point one
        /// </summary>
        public Point? PointOne
        {
            get
            {
                // Get the entity
                if (_entity == null)
                {
                    return null;
                }

                // Get the original point
                Point point = _entity.GetStartingPoint();

                return new Point
                (
                    (point.X * _scaleX),
                    (point.Y * _scaleY)
                );
            }
        }

        /// <summary>
        /// Position of point two
        /// </summary>
        public Point? PointTwo
        {
            get
            {
                // Get the entity
                if (_entity == null)
                {
                    return null;
                }

                // Get the original point
                Point? point = _entity.GetEndingPoint();
                if (!point.HasValue)
                {
                    return null;
                }

                return new Point(point.Value.X * _scaleX, point.Value.Y * _scaleY);
            }
        }

        /// <summary>
        /// New command
        /// </summary>
        public ICommand NewCommand
        {
            get
            {
                return new DelegateCommand(NewLevel);
            }
        }

        /// <summary>
        /// Open command
        /// </summary>
        public ICommand OpenCommand
        {
            get
            {
                return new DelegateCommand(OpenLevel);
            }
        }

        /// <summary>
        /// Save command
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                return new DelegateCommand(SaveLevel);
            }
        }

        /// <summary>
        /// Add point command
        /// </summary>
        public ICommand AddPointCommand
        {
            get
            {
                return new DelegateCommand(AddPoint);
            }
        }

        /// <summary>
        /// Command for editing an existing entity
        /// </summary>
        public ICommand EditEntityCommand
        {
            get
            {
                return new DelegateCommand(EditEntity);
            }
        }

        /// <summary>
        /// Command for canceling an entity's changes
        /// </summary>
        public ICommand CancelEntityCommand
        {
            get
            {
                return new DelegateCommand(CancelEntity);
            }
        }

        /// <summary>
        /// Command for saving an entity
        /// </summary>
        public ICommand SaveEntityCommand
        {
            get
            {
                return new DelegateCommand(SaveEntity);
            }
        }

        /// <summary>
        /// Called when the main window is resized
        /// </summary>
        /// <param name="size">Size of the window</param>
        public void Resize(Size size)
        {
            // Calculate scales
            _scaleX = 1334 / size.Width;
            _scaleY = 750 / size.Height;

            RaisePropertyChangedEvent();
        }

        /// <summary>
        /// Adds a new entity
        /// </summary>
        public void AddEntity()
        {
            // Create a new entity
            _entity = new Entity();

            // Set the index to "new"
            _index = -1;

            // Notify the UI
            RaisePropertyChangedEvent();
        }

        /// <summary>
        /// Updates the entity
        /// </summary>
        public void UpdateEntity()
        {
            RaisePropertyChangedEvent();
        }

        /// <summary>
        /// Adds a point to the canvas
        /// </summary>
        /// <param name="canvas">Canvas element</param>
        private void AddPoint(object canvas)
        {
            // Ignore if not laoded
            if (!Loaded)
            {
                return;
            }

            if (_entity == null)
            {
                return;
            }

            // Get the element
            IInputElement element = (IInputElement)canvas;

            // Get the mouse position
            Point clickPoint = Mouse.GetPosition(element);

            // Adjust point based on scaling
            clickPoint.X /= _scaleX;
            clickPoint.Y /= _scaleY;

            // Set point one
            if (PointOne == null)
            {
                _entity.SetStartingPoint(clickPoint);
            }
            else if (PointTwo == null)
            {
                _entity.SetEndingPoint(clickPoint);
            }
            else
            {
                // Clear point two if points are about to be reset
                _entity.SetEndingPoint(null);
                _entity.SetStartingPoint(clickPoint);
            }

            RaisePropertyChangedEvent();
        }

        /// <summary>
        /// Creates a new level
        /// </summary>
        private void NewLevel()
        {
            // Reset values and create a new level
            LevelRepository.Instance.New();
            _entity = null;
            _index = -1;

            RaisePropertyChangedEvent();
        }

        /// <summary>
        /// Opens a level
        /// </summary>
        private void OpenLevel()
        {
            // Only set the level if one was selected
            if (LevelRepository.Instance.Open())
            {
                RaisePropertyChangedEvent();
            }
        }

        /// <summary>
        /// Saves the current level
        /// </summary>
        private void SaveLevel()
        {
            // Save if one is loaded currently
            if (Loaded)
            {
                LevelRepository.Instance.Save();
            }
        }

        /// <summary>
        /// Edits an existing entity
        /// </summary>
        private void EditEntity(object index)
        {
            // Should be an integer
            int intIndex = (int)index;

            // Set the entity
            _index = intIndex;
            _entity = Entities[intIndex];
            RaisePropertyChangedEvent();
        }
        
        /// <summary>
        /// Cancels editing an entity
        /// </summary>
        private void CancelEntity()
        {
            // Clear out the entity 
            _entity = null;

            // Clear out the index
            _index = -1;

            // Notify the UI
            RaisePropertyChangedEvent();
        }

        /// <summary>
        /// Called to save an entity
        /// </summary>
        private void SaveEntity()
        {
            // Is new or existing
            if (_index == -1)
            {
                // Insert the new entity
                _lr.Get().Entities.Add(SelectedEntity);

                // Capture the index
                _index = _lr.Get().Entities.Count - 1;
            }
            else
            {
                // Swap the entity
                _lr.Get().Entities[_index] = SelectedEntity; 
            }

            // Update the UI
            RaisePropertyChangedEvent();
        }
    }
}
