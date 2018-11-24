using BeltRunnerEditor.ViewModels;

namespace BeltRunnerEditor.DependencyResolution
{
    /// <summary>
    /// Dependency root for the application
    /// </summary>
    public class DependencyRoot
    {
        /// <summary>
        /// Current loaded entity
        /// </summary>
        public LevelModel Level
        {
            get
            {
                return ServiceLocator.Instance.Level;
            }
        }

        /// <summary>
        /// Current loaded entity
        /// </summary>
        public EntityModel Entity
        {
            get
            {
                return ServiceLocator.Instance.Entity;
            }
        }
    }
}
