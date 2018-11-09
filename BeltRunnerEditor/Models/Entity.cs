using System;
using System.Windows;

namespace BeltRunnerEditor.Models
{
    /// <summary>
    /// An entity
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// ID of the entity
        /// </summary>
        public string ID
        {
            get;
            set;
        }

        /// <summary>
        /// Type of the entity
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the entity's graphic
        /// </summary>
        public string Graphic
        {
            get;
            set;
        }

        /// <summary>
        /// X position of the entity
        /// </summary>
        public double X
        {
            get;
            set;
        }

        /// <summary>
        /// Y position of the entity
        /// </summary>
        public double Y
        {
            get;
            set;
        }

        /// <summary>
        /// X velocity of the entity
        /// </summary>
        public double? VelocityX
        {
            get;
            set;
        }

        /// <summary>
        /// Y velocity of the entity
        /// </summary>
        public double? VelocityY
        {
            get;
            set;
        }

        /// <summary>
        /// X position of the entity's destination
        /// </summary>
        public double? DestinationX
        {
            get;
            set;
        }

        /// <summary>
        /// Y position of the entity's destination
        /// </summary>
        public double? DestinationY
        {
            get;
            set;
        }

        /// <summary>
        /// Speed of the entity
        /// </summary>
        public double? Speed
        {
            get;
            set;
        }

        /// <summary>
        /// Delay to apply before the entity is initialized
        /// </summary>
        public int? Delay
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the starting point of the entity
        /// </summary>
        /// <returns>Starting point</returns>
        public Point GetStartingPoint()
        {
            return new Point(X, Y);
        }

        /// <summary>
        /// Sets the starting point of the entity
        /// </summary>
        /// <param name="point">Point to apply</param>
        public void SetStartingPoint(Point? point)
        {
            if (!point.HasValue)
            {
                point = new Point(0, 0);
            }

            X = point.Value.X;
            Y = point.Value.Y;
        }

        /// <summary>
        /// Gets the ending point of the entity
        /// </summary>
        /// <returns>Ending point</returns>
        public Point? GetEndingPoint()
        {
            // There is no ending point if there is no type
            if (String.IsNullOrWhiteSpace(Type))
            {
                return null; 
            }

            switch (Type)
            {
                case "alien":
                case "asteroid":
                case "debris":
                case "nebula":
                    double velocityX = VelocityX.HasValue ? VelocityX.Value : 0;
                    double velocityY = VelocityY.HasValue ? VelocityY.Value : 0;
                    return !VelocityX.HasValue && !VelocityY.HasValue ? (Point?)null : new Point(velocityX, velocityY);
                case "pirate":
                    double destinationX = DestinationX.HasValue ? DestinationX.Value : 0;
                    double destinationY = DestinationY.HasValue ? DestinationY.Value : 0;
                    return !DestinationX.HasValue && !DestinationY.HasValue ? (Point?)null : new Point(destinationX, destinationY);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Sets the ending point for the entity
        /// </summary>
        /// <param name="point">Point to set</param>
        public void SetEndingPoint(Point? point)
        {
            // There is no ending point if there is no type
            if (String.IsNullOrWhiteSpace(Type))
            {
                return;
            }

            switch (Type)
            {
                case "alien":
                case "asteroid":
                case "debris":
                case "nebula":
                    VelocityX = point.HasValue ? point.Value.X : (double?)null;
                    VelocityY = point.HasValue ? point.Value.Y : (double?)null;
                    break;
                case "pirate":
                    DestinationX = point.HasValue ? point.Value.X : (double?)null;
                    DestinationY = point.HasValue ? point.Value.Y : (double?)null;
                    break;
                default:
                    return;
            }
        }
    }
}
