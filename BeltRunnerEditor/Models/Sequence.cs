using BeltRunnerEditor.Enums;

namespace BeltRunnerEditor.Models
{
    /// <summary>
    /// Animation sequence
    /// </summary>
    public class Sequence
    {
        /// <summary>
        /// Name of the sequence
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Index of the starting frame
        /// </summary>
        public short Start { get; set; }

        /// <summary>
        /// Number of frames in the sequence
        /// </summary>
        public short Count { get; set; }

        /// <summary>
        /// Rotation of the graphic during the sequence
        /// </summary>
        public short Rotation { get; set; }

        /// <summary>
        /// Amount of time it should take to get through the sequence
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// Number of times the sequence should loop
        /// </summary>
        public byte LoopCount { get; set; }

        /// <summary>
        /// Direction the sequence should loop in
        /// </summary>
        public LoopDirection Direction { get; set; }
    }
}
