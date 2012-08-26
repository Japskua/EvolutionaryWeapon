using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    interface IItem
    {

        /// <summary>
        /// Loads the gem texture and collected sound.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Gets a circle which bounds this item in world space.
        /// </summary>
        Circle BoundingCircle { get; }

        /// <summary>
        /// Gets the current position of this item in world space.
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Gets the current level
        /// </summary>
        Level Level { get; }


        /// <summary>
        /// Bounces up and down in the air to entice players to collect them.
        /// </summary>
        void Update(GameTime gameTime);

        /// <summary>
        /// Called when this gem has been collected by a player and removed from the level.
        /// </summary>
        /// <param name="collectedBy">
        /// The player who collected this gem. Although currently not used, this parameter would be
        /// useful for creating special powerup gems. For example, a gem could make the player invincible.
        /// </param>
        void OnCollected(Player collectedBy);

        /// <summary>
        /// Draws a gem in the appropriate color.
        /// </summary>
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
