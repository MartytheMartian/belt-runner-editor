using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace BeltRunnerEditor.Entities
{
    /// <summary>
    /// A single level
    /// </summary>
    public class Level
    {
        /// <summary>
        /// Full file path
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Name of the level
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Graphics in the level
        /// </summary>
        public List<BaseGraphic> Graphics { get; set; }

        /// <summary>
        /// XML for any graphics read that cannot be edited
        /// </summary>
        public List<string> Ignorables { get; set; }

        /// <summary>
        /// Entities in the level
        /// </summary>
        public List<Entity> Entities { get; set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="Level" /> class
        /// with default properties set
        /// </summary>
        public Level()
        {
            FileName = null;
            Name = "New Level";
            Graphics = new List<BaseGraphic>();
            Ignorables = new List<string>();
            Entities = new List<Entity>();
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Level" /> class
        /// with properties populated from XML
        /// </summary>
        /// <param name="file">Full file path</param>
        /// <param name="xml">XML to read</param>
        public Level(string file, XmlDocument xml)
            : this()
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            // Load the file name
            FileName = file;

            // Read the info section
            Name = xml.ChildNodes[0].ChildNodes[0].Attributes["name"].Value;

            // Read each graphic as a graphic element
            foreach (XmlNode graphicXML in xml.ChildNodes[0].ChildNodes[1].ChildNodes)
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
                    case "set":
                        graphic = new SetGraphic(graphicXML);
                        break;
                }

                // Add the graphic
                Graphics.Add(graphic);
            }

            // Read entity as an entity element
            foreach (XmlNode entityXML in xml.ChildNodes[0].ChildNodes[2].ChildNodes)
            {
                // Read backgrounds directly and move on
                if (entityXML.Attributes["type"].Value == "background" ||
                    entityXML.Attributes["type"].Value == "player" ||
                    entityXML.Attributes["type"].Value == "turret")
                {
                    Ignorables.Add(entityXML.OuterXml);
                    continue;
                }

                // Must have a graphic
                string graphicID = entityXML.Attributes["graphic"].Value;
                if (String.IsNullOrWhiteSpace(graphicID))
                {
                    continue;
                }

                // Must have a graphic that exists
                BaseGraphic graphic = Graphics.FirstOrDefault(g => g.ID == graphicID);
                if (graphic == null)
                {
                    continue;
                }

                // Create the entity
                Entity entity = new Entity(entityXML, graphic.GetSize());

                // Add the entity
                Entities.Add(entity);
            }

            // Order the entities in the desired order
            Entities = Entities.OrderBy(e => e.Delay.HasValue ? e.Delay.Value : 0)
                .ThenBy(e => e.Type).ToList();
        }

        /// <summary>
        /// Converts the level to XML
        /// </summary>
        /// <returns>Level XML</returns>
        public string ToXml()
        {
            // Start the XML builder
            StringBuilder xmlBuilder = new StringBuilder();

            // Create the opening tag
            xmlBuilder.AppendLine("<map>");

            // Create the info section
            xmlBuilder.AppendFormat("  <info name=\"{0}\" />{1}", Name, Environment.NewLine);

            // Open the graphics section
            xmlBuilder.AppendLine("  <graphics>");

            // Create each graphic
            foreach (BaseGraphic graphic in Graphics)
            {
                xmlBuilder.AppendLine(graphic.ToXml(4));
            }

            // Close the graphics section
            xmlBuilder.AppendLine("  </graphics>");

            // Open the entities section
            xmlBuilder.AppendLine("  <entities>");

            // Append each background XML
            foreach (string ignorable in Ignorables)
            {
                xmlBuilder.AppendLine("    " + ignorable);
            }

            // Create each entity
            foreach (Entity entity in Entities)
            {
                xmlBuilder.AppendLine(entity.ToXml(4));
            }

            // Close the entities section
            xmlBuilder.AppendLine("  </entities>");

            // Create the closing map tag
            xmlBuilder.AppendLine("</map>");

            return xmlBuilder.ToString();
        }
    }
}
