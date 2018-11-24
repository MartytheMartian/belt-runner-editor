using BeltRunnerEditor.Interfaces;
using BeltRunnerEditor.Repositories;
using BeltRunnerEditor.ViewModels;

namespace BeltRunnerEditor.DependencyResolution
{
    /// <summary>
    /// Contains dependencies for the application
    /// </summary>
    public class ServiceLocator
    {
        #region Singleton

        private static ServiceLocator _instance;

        /// <summary>
        /// Instance of the service locator
        /// </summary>
        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceLocator();
                }

                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// Level view model
        /// </summary>
        public LevelModel Level { get; private set; }

        /// <summary>
        /// Entity view model
        /// </summary>
        public EntityModel Entity { get; private set; }

        /// <summary>
        /// Level repository for reading levels
        /// </summary>
        public ILevelRepository LevelRepository { get; private set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="ServiceLocator" /> class
        /// </summary>
        public ServiceLocator()
        {
            Level = new LevelModel();
            Entity = new EntityModel();
            LevelRepository = new LevelRepository();
        }
    }
}
