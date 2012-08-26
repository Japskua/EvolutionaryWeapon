using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Platformer.Items
{
    class GunItem : AbstractItem
    {
        public GunItem(Level level, Vector2 position, string name)
        {
            
            this.level = level;
            this.basePosition = position;

            this.name = name;

            LoadContent();
        }
    }
}
