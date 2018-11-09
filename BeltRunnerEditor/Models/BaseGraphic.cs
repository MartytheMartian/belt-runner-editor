using System;
using System.Xml;

using BeltRunnerEditor.Enums;

namespace BeltRunnerEditor.Models
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
    }
}
