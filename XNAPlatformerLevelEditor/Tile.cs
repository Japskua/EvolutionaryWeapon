using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAPlatformerLevelEditor
{
    struct Tile
    {
        public Texture2D Texture;
        public char TileType;

        public const int Width = 40;
        public const int Height = 32;

        public static readonly Vector2 Size = new Vector2(Width, Height);

        public Tile(Texture2D texture, char tileType)
        {
            Texture = texture;
            TileType = tileType;
        }
    }
}
