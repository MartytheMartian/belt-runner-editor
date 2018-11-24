using System;
using System.Windows;
using System.Xml;

using BeltRunnerEditor.Enums;

namespace BeltRunnerEditor.Entities
{
    /// <summary>
    /// A single grpahic
    /// </summary>
    public abstract class BaseGraphic
    {
        /// <summary>
        /// Unique identifier of the graphic
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Type of the graphic
        /// </summary>
        public GraphicType Type { get; private set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="BaseGraphic" /> class
        /// </summary>
        /// <param name="type">Type of the graphic</param>
        public BaseGraphic(GraphicType type)
        {
            Type = type;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BaseGraphic" /> class
        /// with properties filed from XML
        /// </summary>
        /// <param name="type">Type of the graphic</param>
        /// <param name="xml">XML to read properties from</param>
        public BaseGraphic(GraphicType type, XmlNode xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            Type = type;

            ID = xml.Attributes["id"].Value;
        }

        /// <summary>
        /// Gets the size of the graphic
        /// </summary>
        /// <returns>Size of the graphic</returns>
        public abstract Size GetSize();

        /// <summary>
        /// Converts the graphic to XML
        /// </summary>
        /// <param name="indention">Indention to apply to the XML</param>
        /// <returns>Graphic XML</returns>
        public abstract string ToXml(int indention);
    }
}
