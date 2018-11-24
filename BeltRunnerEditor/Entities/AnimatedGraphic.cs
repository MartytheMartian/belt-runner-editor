using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Xml;

using BeltRunnerEditor.Enums;

namespace BeltRunnerEditor.Entities
{
    /// <summary>
    /// Graphic that is an animated spritesheet
    /// </summary>
    public class AnimatedGraphic : BaseGraphic
    {
        /// <summary>
        /// Path of the image
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Width of a single frame
        /// </summary>
        public short Width { get; set; }

        /// <summary>
        /// Height of a single frame
        /// </summary>
        public short Height { get; set; }

        /// <summary>
        /// Number of frames in the animation
        /// </summary>
        public short NumFrames { get; set; }

        /// <summary>
        /// Width of the entire spritesheet
        /// </summary>
        public short SheetContentWidth { get; set; }

        /// <summary>
        /// Height of the entire spritesheet
        /// </summary>
        public short SheetContentHeight { get; set; }

        /// <summary>
        /// Animation sequences
        /// </summary>
        public List<Sequence> Sequences { get; set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="AnimatedGraphic" /> class
        /// with all the properties set to their default values
        /// </summary>
        public AnimatedGraphic()
            : base(GraphicType.Animated)
        {
            Sequences = new List<Sequence>();
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="AnimatedGraphic" /> class
        /// with properties loaded in from XML
        /// </summary>
        /// <param name="xml">XML to read the properties from</param>
        public AnimatedGraphic(XmlNode xml)
            : base(GraphicType.Animated, xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            Sequences = new List<Sequence>();

            // Read the attributes
            Path = xml.Attributes["path"].Value;
            Width = Int16.Parse(xml.Attributes["width"].Value);
            Height = Int16.Parse(xml.Attributes["height"].Value);
            NumFrames = Int16.Parse(xml.Attributes["numFrames"].Value);
            SheetContentWidth = Int16.Parse(xml.Attributes["sheetContentWidth"].Value);
            SheetContentHeight = Int16.Parse(xml.Attributes["sheetContentHeight"].Value);

            // Read the sequences
            foreach (XmlNode sequenceXML in xml.ChildNodes)
            {
                // Create the sequence and read the attributes
                Sequence sequence = new Sequence();
                sequence.Name = sequenceXML.Attributes["name"].Value;
                sequence.Start = Int16.Parse(sequenceXML.Attributes["start"].Value);
                sequence.Count = Int16.Parse(sequenceXML.Attributes["count"].Value);
                sequence.Rotation = Int16.Parse(sequenceXML.Attributes["rotation"].Value);
                sequence.Time = Int32.Parse(sequenceXML.Attributes["time"].Value);
                sequence.LoopCount = Byte.Parse(sequenceXML.Attributes["loopCount"].Value);

                // Map the loop direction
                string loopDirection = sequenceXML.Attributes["loopDirection"].Value;
                switch (loopDirection)
                {
                    case "forward":
                        sequence.Direction = LoopDirection.Forward;
                        break;
                    case "bounce":
                        sequence.Direction = LoopDirection.Bounce;
                        break;
                    default:
                        sequence.Direction = LoopDirection.Unknown;
                        break;
                }

                // Add the sequence
                Sequences.Add(sequence);
            }
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

            // Append standard properties
            xmlBuilder.AppendFormat("type=\"animated\" path=\"{0}\" width=\"{1}\" height=\"{2}\" numFrames=\"{3}\" sheetContentWidth=\"{4}\" sheetContentHeight=\"{5}\">",
                Path, Width, Height, NumFrames,
                SheetContentWidth, SheetContentHeight);

            // Append sequences
            foreach (Sequence sequence in Sequences)
            {
                xmlBuilder.AppendLine();
                string direction = sequence.Direction == LoopDirection.Forward ? "forward" : "bounce";
                xmlBuilder.AppendFormat("{0}<sequence name=\"{1}\" start=\"{2}\" count=\"{3}\" rotation=\"{4}\" time=\"{5}\" loopCount=\"{6}\" loopDirection=\"{7}\" />",
                    new string(' ', indention + 2), sequence.Name, sequence.Start, sequence.Count,
                    sequence.Rotation, sequence.Time, sequence.LoopCount, direction);
            }

            // Start a new line
            xmlBuilder.AppendLine();
            xmlBuilder.AppendFormat("{0}</graphic>", new string(' ', indention));

            return xmlBuilder.ToString();
        }
    }
}