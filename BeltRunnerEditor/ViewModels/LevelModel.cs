using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using BeltRunnerEditor.Entities;

namespace BeltRunnerEditor.ViewModels
{
    /// <summary>
    /// Currently loaded level
    /// </summary>
    public class LevelModel : ObservableObject
    {
        private Level _level;

        /// <summary>
        /// Current loaded level
        /// </summary>
        public Level Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                RaisePropertyChangedEvent();
            }
        }

        /// <summary>
        /// Current loaded entities
        /// </summary>
        public ObservableCollection<Entity> Entities
        {
            get
            {
                return new ObservableCollection<Entity>(Level.Entities);
            }
        }

        /// <summary>
        /// List of available graphics
        /// </summary>
        public List<string> Graphics
        {
            get
            {
                return _level.Graphics.Select(g => g.ID).ToList();
            }
        }

        /// <summary>
        /// List of available entity types
        /// </summary>
        public List<string> Types
        {
            get
            {
                return new List<string>
                {
                    "alien",
                    "asteroid",
                    "crate",
                    "debris",
                    "lurcher",
                    "moonBottom",
                    "moonTop",
                    "nebula",
                    "pirate",
                    "player",
                    "tentacle",
                    "turret"
                };
            }
        }

        /// <summary>
        /// List of available power-ups
        /// </summary>
        public List<string> PowerUps
        {
            get
            {
                return new List<string>
                {
                    string.Empty,
                    "bonusShield",
                    "fastEnemies",
                    "fastRecharge",
                    "killAll",
                    "lurcher",
                    "slowRecharge"
                };
            }
        }

        /// <summary>
        /// Is level currently loaded
        /// </summary>
        public bool Loaded
        {
            get
            {
                return _level != null;
            }
        }

        /// <summary>
        /// Force update
        /// </summary>
        public void Update()
        {
            RaisePropertyChangedEvent();
        }
    }
}
