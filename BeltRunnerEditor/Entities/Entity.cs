using System;
using System.Text;
using System.Windows;
using System.Xml;
using BeltRunnerEditor.Views;

namespace BeltRunnerEditor.Entities
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
        /// Power-up applied to the entity
        /// </summary>
        public string PowerUp
        {
            get;
            set;
        }

        /// <summary>
        /// ID of the attached lurcher. Only applies to crates.
        /// </summary>
        public string LurcherID
        {
            get;
            set;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Entity" /> class
        /// with the properties defaulted
        /// </summary>
        public Entity()
        {
            ID = null;
            Type = null;
            Graphic = null;
            X = 0;
            Y = 0;
            DestinationX = null;
            DestinationY = null;
            Speed = null;
            Delay = null;
            PowerUp = null;
            LurcherID = null;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Entity" /> class
        /// with the properties populated from XML
        /// </summary>
        /// <param name="xml">XML node to read the properties from</param>
        /// <param name="graphicSize">Size of the graphic the entity has</param>
        public Entity(XmlNode xml, Size graphicSize)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            // Read required properties
            ID = xml.Attributes["id"] == null ? null : xml.Attributes["id"].Value;
            Type = xml.Attributes["type"].Value;
            Graphic = xml.Attributes["graphic"].Value;
            X = Double.Parse(xml.Attributes["x"].Value);
            Y = Double.Parse(xml.Attributes["y"].Value);
            Delay = xml.Attributes["delay"] == null ? null : (int?)int.Parse(xml.Attributes["delay"].Value);

            // Read extra props
            PowerUp = xml.Attributes["powerUp"] == null ? null : xml.Attributes["powerUp"].Value;
            LurcherID = xml.Attributes["lurcherId"] == null ? null : xml.Attributes["lurcherId"].Value;

            // Default optional types
            DestinationX = null;
            DestinationY = null;
            Speed = null;

            // Read properties based on type
            switch (Type)
            {
                case "alien":
                case "asteroid":
                case "background":
                case "debris":
                case "nebula":
                case "pirate":
                case "crate":
                case "lurcher":
                    // Read the properties
                    double? vX = xml.Attributes["vX"] == null ? null : (double?)Double.Parse(xml.Attributes["vX"].Value);
                    double? vY = xml.Attributes["vY"] == null ? null : (double?)Double.Parse(xml.Attributes["vY"].Value);

                    // There are no properties to convert
                    if (!vX.HasValue || !vY.HasValue)
                    {
                        return;
                    }

                    // Build vectors
                    Vector position = new Vector(X, Y);
                    Vector velocity = new Vector(vX.Value, vY.Value);

                    // Calculate destination
                    Vector destination = BRMath.CalculateDestination(X, Y, graphicSize.Width, graphicSize.Height, velocity.X, velocity.Y);
                    DestinationX = destination.X;
                    DestinationY = destination.Y;

                    // Calculate speed
                    Speed = BRMath.CalculateSpeed(position, destination, velocity);
                    break;
            }
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

            if (!DestinationX.HasValue || !DestinationY.HasValue)
            {
                return null;
            }

            return new Point(DestinationX.Value, DestinationY.Value);
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

            if (!point.HasValue)
            {
                DestinationX = null;
                DestinationY = null;
                return;
            }

            DestinationX = point.Value.X;
            DestinationY = point.Value.Y;
        }

        /// <summary>
        /// Converts the entity to XML
        /// </summary>
        /// <param name="indention">Indention to apply</param>
        /// <returns>Entity XML</returns>
        public string ToXml(int indention)
        {
            // Start building XML
            StringBuilder xmlBuilder = new StringBuilder();

            // Open the entity
            xmlBuilder.AppendFormat("{0}<entity ", new string(' ', indention));

            // Append ID if it exists
            if (!String.IsNullOrWhiteSpace(ID))
            {
                xmlBuilder.AppendFormat("id=\"{0}\" ", ID);
            }

            // Append properties that always exist
            xmlBuilder.AppendFormat("type=\"{0}\" ", Type);
            xmlBuilder.AppendFormat("graphic=\"{0}\" ", Graphic);
            xmlBuilder.AppendFormat("x=\"{0}\" ", X);
            xmlBuilder.AppendFormat("y=\"{0}\" ", Y);

            // Determine which velocity props to write
            if (Type == "police")
            {
                xmlBuilder.AppendFormat("dX=\"{0}\" ", DestinationX);
                xmlBuilder.AppendFormat("dY=\"{0}\" ", DestinationY);
                xmlBuilder.AppendFormat("s=\"{0}\" ", Speed);
            }
            else if (DestinationX.HasValue && DestinationY.HasValue && Speed.HasValue)
            {
                // Calculate velocity
                Vector velocity = BRMath.CalculateVelocity(X, Y, DestinationX.Value, DestinationY.Value, Speed.Value);

                xmlBuilder.AppendFormat("vX=\"{0}\" ", velocity.X);
                xmlBuilder.AppendFormat("vY=\"{0}\" ", velocity.Y);
            }

            if (Delay.HasValue)
            {
                xmlBuilder.AppendFormat("delay=\"{0}\" ", Delay);
            }

            if (!String.IsNullOrWhiteSpace(PowerUp))
            {
                xmlBuilder.AppendFormat("powerUp=\"{0}\" ", PowerUp);
            }

            if (!String.IsNullOrWhiteSpace(LurcherID))
            {
                xmlBuilder.AppendFormat("lurcherId=\"{0}\" ", LurcherID);
            }

            // Close the entity
            xmlBuilder.Append("/>");

            return xmlBuilder.ToString();
        }
    }
}
