using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Xml;

using BeltRunnerEditor.Enums;
using BeltRunnerEditor.Models;

namespace BeltRunnerEditor.Repositories
{
    /// <summary>
    /// Reads and writes levels from the file system
    /// </summary>
    public class LevelRepository
    {
        private static LevelRepository _instance = null;

        /// <summary>
        /// Single instance of the repository
        /// </summary>
        public static LevelRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LevelRepository();
                }

                return _instance;
            }
        }

        private LevelData _levelData;

        /// <summary>
        /// Gets the current loaded level
        /// </summary>
        public LevelData Get()
        {
            return _levelData;
        }

        /// <summary>
        /// Creates a new level
        /// </summary>
        public void New()
        {
            _levelData = new LevelData();
        }

        /// <summary>
        /// Opens a level
        /// </summary>
        /// <returns>Did the open succeed</returns>
        public bool Open()
        {
            // Unload the current level
            _levelData = new LevelData();

            // Setup a dialog for a level
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".xml";
            dialog.Filter = "XML Document (.xml)|*.xml";

            // Open the dialog
            bool? success = dialog.ShowDialog();

            // Default file name if none was selected
            string file = success.HasValue && success.Value ? dialog.FileName : null;

            // Quick out for no file selected
            if (String.IsNullOrWhiteSpace(file))
            {
                _levelData = new LevelData();
                return false;
            }

            // Parse the file as XML
            XmlDocument document = new XmlDocument();
            document.Load(file);

            // Read the info section
            _levelData.Name = document.ChildNodes[0].ChildNodes[0].Attributes["name"].Value;

            // Read each graphic as a graphic element
            foreach (XmlNode graphicXML in document.ChildNodes[0].ChildNodes[1].ChildNodes)
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
                _levelData.Graphics.Add(graphic);
            }

            // Read entity as an entity element
            foreach (XmlNode entityXML in document.ChildNodes[0].ChildNodes[2].ChildNodes)
            {
                // Read backgrounds directly and move on
                if (entityXML.Attributes["type"].Value == "background")
                {
                    _levelData.BackgroundXML.Add(entityXML.OuterXml);
                    continue;
                }

                // Create the entity
                Entity entity = new Entity();

                // Read each property
                entity.ID = entityXML.Attributes["id"] == null ? null : entityXML.Attributes["id"].Value;
                entity.Type = entityXML.Attributes["type"].Value;
                entity.Graphic = entityXML.Attributes["graphic"].Value;
                entity.X = Int32.Parse(entityXML.Attributes["x"].Value);
                entity.Y = Int32.Parse(entityXML.Attributes["y"].Value);
                entity.VelocityX = entityXML.Attributes["vX"] == null ? null : (int?)Int32.Parse(entityXML.Attributes["vX"].Value);
                entity.VelocityY = entityXML.Attributes["vY"] == null ? null : (int?)Int32.Parse(entityXML.Attributes["vY"].Value);
                entity.DestinationX = entityXML.Attributes["dX"] == null ? null : (int?)Int32.Parse(entityXML.Attributes["dX"].Value);
                entity.DestinationY = entityXML.Attributes["dY"] == null ? null : (int?)Int32.Parse(entityXML.Attributes["dY"].Value);
                entity.Speed = entityXML.Attributes["s"] == null ? null : (int?)Int32.Parse(entityXML.Attributes["s"].Value);
                entity.Delay = entityXML.Attributes["delay"] == null ? null : (int?)Int32.Parse(entityXML.Attributes["delay"].Value);

                // Add the entity
                _levelData.Entities.Add(entity);
            }

            return true;
        }

        /// <summary>
        /// Saves a level
        /// </summary>
        /// <param name="level">Level to save</param>
        public void Save()
        {
            // Start the XML builder
            StringBuilder xmlBuilder = new StringBuilder();

            // Create the opening tag
            xmlBuilder.AppendLine("<map>");

            // Create the info section
            xmlBuilder.AppendFormat("  <info name=\"{0}\" />{1}", _levelData.Name, Environment.NewLine);

            // Open the graphics section
            xmlBuilder.AppendLine("  <graphics>");

            // Create each graphic
            foreach (BaseGraphic graphic in _levelData.Graphics)
            {
                AppendGraphic(xmlBuilder, graphic, 4);
            }

            // Close the graphics section
            xmlBuilder.AppendLine("  </graphics>");

            // Open the entities section
            xmlBuilder.AppendLine("  <entities>");

            // Append each background XML
            foreach (string background in _levelData.BackgroundXML)
            {
                xmlBuilder.AppendLine(background);
            }

            // Create each entity
            foreach (Entity entity in _levelData.Entities)
            {
                // Open the entity
                xmlBuilder.Append("    <entity ");

                // Append ID if it exists
                if (!String.IsNullOrWhiteSpace(entity.ID))
                {
                    xmlBuilder.AppendFormat("id=\"{0}\"", entity.ID);
                }

                // Append properties that always exist
                xmlBuilder.AppendFormat("type=\"{0}\" ", entity.Type);
                xmlBuilder.AppendFormat("graphic=\"{0}\" ", entity.Graphic);
                xmlBuilder.AppendFormat("x=\"{0}\" ", entity.X);
                xmlBuilder.AppendFormat("y=\"{0}\" ", entity.Y);

                // Append velocity X if it exists
                if (entity.VelocityX.HasValue)
                {
                    xmlBuilder.AppendFormat("vX=\"{0}\" ", entity.VelocityX);
                }

                // Append velocity Y if it exists
                if (entity.VelocityY.HasValue)
                {
                    xmlBuilder.AppendFormat("vY=\"{0}\" ", entity.VelocityY);
                }

                // Append destination X if it exists
                if (entity.DestinationX.HasValue)
                {
                    xmlBuilder.AppendFormat("dX=\"{0}\" ", entity.DestinationX);
                }

                // Append destination Y if it exists
                if (entity.DestinationY.HasValue)
                {
                    xmlBuilder.AppendFormat("dY=\"{0}\" ", entity.DestinationY);
                }

                // Append speed if it exists
                if (entity.Speed.HasValue)
                {
                    xmlBuilder.AppendFormat("s=\"{0}\" ", entity.Speed);
                }

                // Append delay if it exists
                if (entity.Delay.HasValue)
                {
                    xmlBuilder.AppendFormat("delay=\"{0}\" ", entity.Delay.Value);
                }

                // Close the entity
                xmlBuilder.Append("/>");

                // New line
                xmlBuilder.AppendLine();
            }

            // Close the entities section
            xmlBuilder.AppendLine("  </entities>");

            // Determine the file to save
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".xml";
            dialog.Filter = "XML Document (.xml)|*.xml";

            // Backout if necessary
            bool? confirm = dialog.ShowDialog();
            if (!confirm.HasValue || !confirm.Value)
            {
                return;
            }

            // Delete the file if it previously existed
            if (File.Exists(dialog.FileName))
            {
                File.Delete(dialog.FileName);
            }

            // Write the file
            File.WriteAllText(dialog.FileName, xmlBuilder.ToString());
        }

        /// <summary>
        /// Appends graphic XML to a string builder
        /// </summary>
        /// <param name="xmlBuilder">XML builder to use</param>
        /// <param name="graphic">Graphic to append</param>
        /// <param name="indention">Indention to apply</param>
        private void AppendGraphic(StringBuilder xmlBuilder, BaseGraphic graphic, int indention)
        {
            // Append the start of the graphic line
            xmlBuilder.AppendFormat("{0}<graphic id=\"{1}\" ", new string(' ', indention), graphic.ID);

            // Append additional properties based on type
            switch (graphic.Type)
            {
                case GraphicType.Static:
                    StaticGraphic staticGraphic = graphic as StaticGraphic;
                    xmlBuilder.AppendFormat("type=\"static\" path=\"{0}\" width=\"{1}\" height=\"{2}\" />", staticGraphic.Path, 
                        staticGraphic.Width, staticGraphic.Height);
                    break;
                case GraphicType.Scrolling:
                    ScrollingGraphic scrollingGraphic = graphic as ScrollingGraphic;
                    xmlBuilder.AppendFormat("type=\"scrolling\" path=\"{0}\" width=\"{1}\" height=\"{2}\" />", scrollingGraphic.Path,
                        scrollingGraphic.Width, scrollingGraphic.Height);
                    break;
                case GraphicType.Animated:
                    AnimatedGraphic animatedGraphic = graphic as AnimatedGraphic;
                    xmlBuilder.AppendFormat("type=\"animated\" path=\"{0}\" width=\"{1}\" height=\"{2}\" numFrames=\"{3}\" sheetContentWidth=\"{4}\" sheetContentHeight=\"{5}\" />",
                        animatedGraphic.Path, animatedGraphic.Width, animatedGraphic.Height, animatedGraphic.NumFrames,
                        animatedGraphic.SheetContentWidth, animatedGraphic.SheetContentHeight);
                    foreach (Sequence sequence in animatedGraphic.Sequences)
                    {
                        xmlBuilder.AppendLine();
                        string direction = sequence.Direction == LoopDirection.Forward ? "forward" : "bounce";
                        xmlBuilder.AppendFormat("{0}<sequence name=\"{1}\" start=\"{2}\" count=\"{3}\" rotation=\"{4}\" time=\"{5}\" loopCount=\"{6}\" loopDirection=\"{7}\" />",
                            new string(' ', indention + 2), sequence.Name, sequence.Start, sequence.Count,
                            sequence.Rotation, sequence.Time, sequence.LoopCount, direction);
                    }
                    break;
                case GraphicType.Set:
                    SetGraphic setGraphic = graphic as SetGraphic;
                    xmlBuilder.AppendFormat("type=\"set\">{0}", Environment.NewLine);
                    foreach (BaseGraphic child in setGraphic.Graphics)
                    {
                        AppendGraphic(xmlBuilder, child, indention + 2);
                    }
                    xmlBuilder.AppendFormat("{0}</graphic>", new string(' ', indention));
                    break;
                default:
                    throw new InvalidOperationException("Attempting to write unsupported graphic");
            }

            // Start a new line
            xmlBuilder.Append(Environment.NewLine);
        }
    }
}
