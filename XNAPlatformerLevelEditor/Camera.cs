using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNAPlatformerLevelEditor
{
    public class Camera
    {
        float speed = 5;
        int screenWidth, screenHeight;
        public int ScreenWidth
        {
            get { return screenWidth; }
        }
        public int ScreenHeight
        {
            get { return screenHeight; }
        }

        public Vector2 Position = Vector2.Zero;

        /// <summary>
        /// Gets or Sets the current speed that the camera scrolls.
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = (float)MathHelper.Max(value, 1f); }
        }

        /// <summary>
        /// Gets a Rectangle of the camera's current view
        /// </summary>
        public Rectangle ScreenRect
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    screenWidth,
                    screenHeight);
            }
        }

        public Camera()
        {
        }

        public Camera(int width, int height)
        {
            screenWidth = width;
            screenHeight = height;
        }

        public void Update()
        {
            //KeyboardState keyboardState = Keyboard.GetState();
            //Vector2 motion = Vector2.Zero;

            //if (keyboardState.IsKeyDown(Keys.Up))
            //    motion.Y--;
            //if (keyboardState.IsKeyDown(Keys.Down))
            //    motion.Y++;
            //if (keyboardState.IsKeyDown(Keys.Left))
            //    motion.X--;
            //if (keyboardState.IsKeyDown(Keys.Right))
            //    motion.X++;

            //if (motion != Vector2.Zero)
            //{
            //    motion.Normalize();
            //    Position += motion * Speed;
            //}
        }

        public void LockToTarget(Vector2 pos)
        {
            Position.X = pos.X - (screenWidth / 2);
            Position.Y = pos.Y - (screenHeight / 2);
        }

        public void ClampToArea(int width, int height)
        {
            if (Position.X > width)
                Position.X = width;
            if (Position.Y > height)
                Position.Y = height;

            if (Position.X < 0)
                Position.X = 0;
            if (Position.Y < 0)
                Position.Y = 0;
        }
    }
}