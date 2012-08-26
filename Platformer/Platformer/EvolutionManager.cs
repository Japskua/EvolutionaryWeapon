using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class EvolutionManager
    {
        private static volatile EvolutionManager instance;
        private static object syncRoot = new Object();

        /// <summary>
        /// The Private Constructor
        /// </summary>
        private EvolutionManager() 
        {
            HasGun = false;
            HasLegs = false;
        }

        public static EvolutionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new EvolutionManager();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Sets the player to correct situation at level loadup
        /// </summary>
        /// <param name="player">The player which to fit correctly</param>
        public void SetupPlayer(Player player)
        {
            player.Legs = HasLegs;
            player.Laser = HasGun;
        }

        /// <summary>
        /// Wheteher the player has a gun or not
        /// </summary>
        public bool HasGun
        {
            get { return hasGun; }
            set { hasGun = value; }
        }
        private bool hasGun;

        /// <summary>
        /// Whether the player has legs or not
        /// </summary>
        public bool HasLegs
        {
            get { return hasLegs; }
            set { hasLegs = value; }
        }

        private bool hasLegs;

    }
}