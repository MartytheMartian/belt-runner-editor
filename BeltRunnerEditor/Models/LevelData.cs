using System.Collections.Generic;

namespace BeltRunnerEditor.Models
{
    /// <summary>
    /// A single level
    /// </summary>
    public class LevelData
    {
        /// <summary>
        /// Name of the level
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Graphics in the level
        /// </summary>
        public List<BaseGraphic> Graphics { get; set; }

        /// <summary>
        /// XML for any graphics read
        /// </summary>
        public List<string> BackgroundXML { get; set; }

        /// <summary>
        /// Entities in the level
        /// </summary>
        public List<Entity> Entities { get; set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="LevelData" /> class
        /// with default properties set
        /// </summary>
        public LevelData()
        {
            Name = "New Level";
            Graphics = new List<BaseGraphic>();
            BackgroundXML = new List<string>();
            Entities = new List<Entity>();
        }
    }
}
