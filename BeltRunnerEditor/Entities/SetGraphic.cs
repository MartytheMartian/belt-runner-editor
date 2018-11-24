using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Xml;

using BeltRunnerEditor.Enums;

namespace BeltRunnerEditor.Entities
{
    /// <summary>
    /// Graphic that is a set of other graphics
    /// </summary>
    public class SetGraphic : BaseGraphic
    {
        /// <summary>
        /// List of graphics
        /// </summary>
        public List<BaseGraphic> Graphics { get; set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="SetGraphic" /> class
        /// with all the properties set to their default values
        /// </summary>
        public SetGraphic()
            : base(GraphicType.Set)
        {
            Graphics = new List<BaseGraphic>();
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="SetGraphic" /> class
        /// with properties loaded in from XML
        /// </summary>
        /// <param name="xml">XML to read the properties from</param>
        public SetGraphic(XmlNode xml)
            : base(GraphicType.Set, xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            // Create graphics
            Graphics = new List<BaseGraphic>();
            foreach (XmlNode graphicXML in xml.ChildNodes)
            {
                // Create the graphic
                BaseGraphic graphic = null;

                switch (graphicXML.Attributes["type"].Value)
                {
                    case "animated":
                        graphic = new AnimatedGraphic(graphicXML);
                        break;
                    case "static":
                        graphic = new StaticGraphic(graphicXML);
                        break;
                    case "scrolling":
                        graphic = new ScrollingGraphic(graphicXML);
                        break;
                }

                // Add the graphic
                Graphics.Add(graphic);
            }
        }

        /// <summary>
        /// Gets the size of the graphic
        /// </summary>
        /// <returns>Size of the graphic</returns>
        public override Size GetSize()
        {
            return Graphics[0].GetSize();
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
            xmlBuilder.AppendFormat("type=\"set\">{0}", Environment.NewLine);
            foreach (BaseGraphic child in Graphics)
            {
                xmlBuilder.AppendLine(child.ToXml(indention + 2));
            }
            xmlBuilder.AppendFormat("{0}</graphic>", new string(' ', indention));

            return xmlBuilder.ToString();
        }
    }
}
