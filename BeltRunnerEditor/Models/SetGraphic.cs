using System;
using System.Collections.Generic;
using System.Xml;

using BeltRunnerEditor.Enums;

namespace BeltRunnerEditor.Models
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
    }
}
