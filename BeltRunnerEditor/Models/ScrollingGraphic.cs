using System;
using System.Xml;

using BeltRunnerEditor.Enums;

namespace BeltRunnerEditor.Models
{
    /// <summary>
    /// Graphic that is a single image that scrolls itself
    /// </summary>
    public class ScrollingGraphic : BaseGraphic
    {
        /// <summary>
        /// Path of the image
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Width of the graphic
        /// </summary>
        public short Width { get; set; }

        /// <summary>
        /// Height of the graphic
        /// </summary>
        public short Height { get; set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="ScrollingGraphic" /> class
        /// with all the properties set to their default values
        /// </summary>
        public ScrollingGraphic()
            : base(GraphicType.Scrolling)
        {
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="ScrollingGraphic" /> class
        /// with properties loaded in from XML
        /// </summary>
        /// <param name="xml">XML to read the properties from</param>
        public ScrollingGraphic(XmlNode xml)
            : base(GraphicType.Scrolling, xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            // Read the attributes
            Path = xml.Attributes["path"].Value;
            Width = Int16.Parse(xml.Attributes["width"].Value);
            Height = Int16.Parse(xml.Attributes["height"].Value);
        }
    }
}
