using System.Windows;

using BeltRunnerEditor.Entities;

namespace BeltRunnerEditor.ViewModels
{
    /// <summary>
    /// Current loaded model
    /// </summary>
    public class EntityModel : ObservableObject
    {
        private Entity _entity = null;

        /// <summary>
        /// Current loaded index
        /// </summary>
        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// Current loaded entity
        /// </summary>
        public Entity Entity
        {
            get
            {
                return _entity;
            }
            set
            {
                _entity = value;
                RaisePropertyChangedEvent();
            }
        }

        /// <summary>
        /// Is an entity currently loaded
        /// </summary>
        public bool Loaded
        {
            get
            {
                return Entity != null;
            }
        }

        /// <summary>
        /// Is the entity currently unloaded
        /// </summary>
        public bool Unloaded
        {
            get
            {
                return Entity == null;
            }
        }

        /// <summary>
        /// Is the position set
        /// </summary>
        public bool DestinationSet
        {
            get
            {
                return Destination.HasValue;
            }
        }

        /// <summary>
        /// Is the position set
        /// </summary>
        public bool PositionSet
        {
            get
            {
                return Position.HasValue;
            }
        }

        /// <summary>
        /// Is the line set
        /// </summary>
        public bool LineSet
        {
            get
            {
                return Position.HasValue && Destination.HasValue;
            }
        }

        /// <summary>
        /// Position
        /// </summary>
        public Point? Position
        {
            get
            {
                if (Entity == null)
                {
                    return null;
                }

                // Get the position
                Point position = Entity.GetStartingPoint();

                // X out of bounds adjustemnt
                if (position.X < 0)
                {
                    position.X = 166.75;
                }
                else if (position.X > 1334)
                {
                    position.X = 1500.75;
                }
                else
                {
                    position.X += 166.75;
                }

                // Y out of bounds adjustment
                if (position.Y < 0)
                {
                    position.Y = 93.75;
                }
                else if (position.Y > 750)
                {
                    position.Y = 843.75;
                }
                else
                {
                    position.Y += 93.75;
                }

                return position;
            }
        }

        /// <summary>
        /// Destination
        /// </summary>
        public Point? Destination
        {
            get
            {
                if (Entity == null)
                {
                    return null;
                }

                Point? p = Entity.GetEndingPoint();
                if (!p.HasValue)
                {
                    return p;
                }

                Point position = p.Value;

                // X out of bounds adjustemnt
                if (position.X < 0)
                {
                    position.X = 166.75;
                }
                else if (position.X > 1334)
                {
                    position.X = 1500.75;
                }
                else
                {
                    position.X += 166.75;
                }

                // Y out of bounds adjustment
                if (position.Y < 0)
                {
                    position.Y = 93.75;
                }
                else if (position.Y > 750)
                {
                    position.Y = 843.75;
                }
                else
                {
                    position.Y += 93.75;
                }

                return position;
            }
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="EntityModel" /> class
        /// </summary>
        public EntityModel()
        {
            _entity = null;
        }

        /// <summary>
        /// Sets the next point
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public void SetPoint(double x, double y)
        {
            if (Destination == null)
            {
                Entity.SetEndingPoint(new Point(x, y));
                RaisePropertyChangedEvent();
                return;
            }

            Entity.SetEndingPoint(null);
            Entity.SetStartingPoint(new Point(x, y));
            RaisePropertyChangedEvent();
        }

        /// <summary>
        /// Force update
        /// </summary>
        public void Update()
        {
            RaisePropertyChangedEvent();
        }
    }
}
