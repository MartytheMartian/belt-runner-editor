using System;
using System.Text;
using System.Windows;
using System.Xml;

using BeltRunnerEditor.Enums;

namespace BeltRunnerEditor.Entities
{
    /// <summary>
    /// Graphic that is a static image
    /// </summary>
    public class StaticGraphic : BaseGraphic
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
        /// Constructs a new instance of the <see cref="StaticGraphic" /> class
        /// with all the properties set to their default values
        /// </summary>
        public StaticGraphic()
            : base(GraphicType.Static)
        {
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="StaticGraphic" /> class
        /// with properties loaded in from XML
        /// </summary>
        /// <param name="xml">XML to read the properties from</param>
        public StaticGraphic(XmlNode xml)
            : base(GraphicType.Static, xml)
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

        /// <summary>
        /// Gets the size of the graphic
        /// </summary>
        /// <returns>Size of the graphic</returns>
        public override Size GetSize()
        {
            return new Size(Width, Height);
        }

        /// <summary>
        /// Converts the graphic to XML
        /// </summary>
        /// <param name="indention">Indention to apply to the XML</param>
        /// <returns>Graphic XML</returns>
        public override string ToXml(int indention)
        {
            StringBuilder xmlBuilder = new StringBuilder();

            // Append the start of the graphic line
            xmlBuilder.AppendFormat("{0}<graphic id=\"{1}\" ", new string(' ', indention), ID);

            // Append additional properties
            xmlBuilder.AppendFormat("type=\"static\" path=\"{0}\" width=\"{1}\" height=\"{2}\" />",
                Path, Width, Height);

            return xmlBuilder.ToString();
        }
    }
}
