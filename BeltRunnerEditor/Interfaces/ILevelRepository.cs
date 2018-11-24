using BeltRunnerEditor.Entities;

namespace BeltRunnerEditor.Interfaces
{
    /// <summary>
    /// Reads and writes levels
    /// </summary>
    public interface ILevelRepository
    {
        /// <summary>
        /// Reads in a new level
        /// </summary>
        /// <returns>New level</returns>
        Level Read();

        /// <summary>
        /// Saves a level
        /// </summary>
        /// <param name="level">Level to save</param>
        void Save(Level level);
    }
}
