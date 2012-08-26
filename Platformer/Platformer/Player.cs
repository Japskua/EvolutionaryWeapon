#region File Description
//-----------------------------------------------------------------------------
// Player.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Platformer
{
    /// <summary>
    /// Our fearless adventurer!
    /// </summary>
    class Player
    {
        // Animations
        private Animation idleAnimation;
        private Animation runAnimation;
        private Animation jumpAnimation;
        private Animation celebrateAnimation;
        private Animation dieAnimation;
        private SpriteEffects flip = SpriteEffects.None;
        private AnimationPlayer sprite;

        // Sounds
        private SoundEffect killedSound;
        private SoundEffect jumpSound;
        private SoundEffect fallSound;
        
        // Previous game and mouse states
        GamePadState previousGamePadState;
        MouseState previousMouseState;

        // The Weapon game object
        GameObject gun;
        // The bullet
        GameObject[] bullets;

        public Level Level
        {
            get { return level; }
        }
        Level level;

        public bool IsAlive
        {
            get { return isAlive; }
        }
        bool isAlive;

        // Physics state
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        private float previousBottom;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

        public bool Legs;
        public bool Laser;
        public bool Hands;

        // Constants for controling horizontal movement
        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        // Constants for controlling vertical movement
        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.14f; 

        // Input configuration
        private const float MoveStickScale = 1.0f;
        private const float AccelerometerScale = 1.5f;
        private const Buttons JumpButton = Buttons.A;

        // Used for calculating the mouse distance
        Vector2 distance;

        /// <summary>
        /// Gets whether or not the player's feet are on the ground.
        /// </summary>
        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;

        /// <summary>
        /// Current user movement input.
        /// </summary>
        private float movement;

        // Jumping state
        private bool isJumping;
        private bool wasJumping;
        private float jumpTime;

        private Rectangle localBounds;
        /// <summary>
        /// Gets a rectangle which bounds this player in world space.
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + localBounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }

        /// <summary>
        /// Constructors a new player.
        /// </summary>
        public Player(Level level, Vector2 position)
        {
            this.level = level;

            Legs = true;
            Hands = false;
            Laser = false;

            LoadContent();

            Reset(position);
        }

        /// <summary>
        /// Loads the player sprite sheet and sounds.
        /// </summary>
        public void LoadContent()
        {
            // Load the gun here
            gun = new GameObject(Level.Content.Load<Texture2D>("Sprites/Player/Gun"));
            // Load 12 bullets
            bullets = new GameObject[12];
            for (int i = 0; i < 12; i++)
            {
                bullets[i] = new GameObject(Level.Content.Load<Texture2D>("Sprites/Player/Bullet"));
            }


            // Load correct animations 
            if (Legs == true)
            {
                // Load animated textures.
                idleAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/WithLegs/Idle_withlegs"), 0.1f, true);
                runAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/WithLegs/Run_withlegs"), 0.1f, true);
                jumpAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/WithLegs/Jump_withlegs"), 0.1f, false);
                celebrateAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/WithLegs/Celebrate_withlegs"), 0.1f, false);
                dieAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/WithLegs/Die_withlegs"), 0.1f, false);
            }
            else
            {
                // Load animated textures.
                idleAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Basic/Idle"), 0.1f, true);
                runAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Basic/Run"), 0.1f, true);
                jumpAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Basic/Jump"), 0.1f, false);
                celebrateAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Basic/Celebrate"), 0.1f, false);
                dieAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Basic/Die"), 0.1f, false);
            }

            // Calculate bounds within texture size.            
            int width = (int)(idleAnimation.FrameWidth * 0.4);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameWidth * 0.8);
            int top = idleAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);

            // Load sounds.            
            killedSound = Level.Content.Load<SoundEffect>("Sounds/PlayerKilled");
            jumpSound = Level.Content.Load<SoundEffect>("Sounds/PlayerJump");
            fallSound = Level.Content.Load<SoundEffect>("Sounds/PlayerFall");
        }

        /// <summary>
        /// Resets the player to life.
        /// </summary>
        /// <param name="position">The position to come to life at.</param>
        public void Reset(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
            isAlive = true;
            sprite.PlayAnimation(idleAnimation);
        }

        /// <summary>
        /// Handles input, performs physics, and animates the player sprite.
        /// </summary>
        /// <remarks>
        /// We pass in all of the input states so that our game is only polling the hardware
        /// once per frame. We also pass the game's orientation because when using the accelerometer,
        /// we need to reverse our motion when the orientation is in the LandscapeRight orientation.
        /// </remarks>
        public void Update(
            GameTime gameTime, 
            KeyboardState keyboardState, 
            GamePadState gamePadState, 
            TouchCollection touchState, 
            AccelerometerState accelState,
            DisplayOrientation orientation,
            MouseState mouseState)
        {
            GetInput(keyboardState, gamePadState, touchState, accelState, orientation, mouseState);

            ApplyPhysics(gameTime);

            if (IsAlive && IsOnGround)
            {
                if (Math.Abs(Velocity.X) - 0.02f > 0)
                {
                    sprite.PlayAnimation(runAnimation);
                }
                else
                {
                    sprite.PlayAnimation(idleAnimation);
                }
            }

            // Clear input.
            movement = 0.0f;
            isJumping = false;

            // Handling the gun position correctly
            if (flip == SpriteEffects.FlipHorizontally)
            {
                gun.position = new Vector2(position.X, position.Y - 45);
            }
            else
            {
                gun.position = new Vector2(position.X, position.Y - 45);
            }

            // Update the bullets
            UpdateBullets();
        }

        /// <summary>
        /// This function fires the bullet from the gun
        /// </summary>
        private void FireBullet()
        {
            foreach (GameObject bullet in bullets)
            {
                // Find a bullet that is not alive
                if(!bullet.alive)
                {
                    // Set it alive
                    bullet.alive = true;

                    //Facing right
                    if (flip == SpriteEffects.FlipHorizontally) 
                    {
                        float armCos = (float)Math.Cos(gun.rotation - MathHelper.PiOver2);
                        float armSin = (float)Math.Sin(gun.rotation - MathHelper.PiOver2);
                        //Set the initial position of our bullet at the end of our gun arm
                        //42 is obtained be taking the width of the Arm_Gun texture / 2
                        //and subtracting the width of the Bullet texture / 2. ((96/2)-(12/2))
                        bullet.position = new Vector2(
                            gun.position.X + 42 * armCos,
                            gun.position.Y + 42 * armSin);
                        //And give it a velocity of the direction we're aiming.
                        //Increase/decrease speed by changing 15.0f
                        bullet.velocity = new Vector2(
                            (float)Math.Cos(gun.rotation - MathHelper.PiOver2),
                            (float)Math.Sin(gun.rotation - MathHelper.PiOver2)) * 15.0f;
                    }
                    //Facing left
                    else 
                    {
                        float armCos = (float)Math.Cos(gun.rotation + MathHelper.PiOver2);
                        float armSin = (float)Math.Sin(gun.rotation + MathHelper.PiOver2);
                        //Set the initial position of our bullet at the end of our gun arm
                        //42 is obtained be taking the width of the Arm_Gun texture / 2
                        //and subtracting the width of the Bullet texture / 2. ((96/2)-(12/2))
                        bullet.position = new Vector2(
                            gun.position.X - 42 * armCos,
                            gun.position.Y - 42 * armSin);
                        //And give it a velocity of the direction we're aiming.
                        //Increase/decrease speed by changing 15.0f
                        bullet.velocity = new Vector2(
                           -armCos,
                           -armSin) * 15.0f;
                    }
                    return;
                }
            }


        }

        /// <summary>
        /// Used to update the situation of the bullets
        /// </summary>
        private void UpdateBullets()
        {
            //Check all of our bullets
            foreach (GameObject bullet in bullets)
            {
                //Only update them if they're alive
                if (bullet.alive)
                {
                    //Move our bullet based on it's velocity
                    bullet.position += bullet.velocity;
                    //Rectangle the size of the screen so bullets that
                    //fly off screen are deleted.
                    Rectangle screenRect = new Rectangle(0, 0, level.WidthInPixels, level.HeightInPixels);
                    if (!screenRect.Contains(new Point(
                        (int)bullet.position.X,
                        (int)bullet.position.Y)))
                    {
                        bullet.alive = false;
                        continue;
                    }
                    //Collision rectangle for each bullet -Will also be
                    //used for collisions with enemies.
                    Rectangle bulletRect = new Rectangle(
                        (int)bullet.position.X - bullet.sprite.Width * 2,
                        (int)bullet.position.Y - bullet.sprite.Height * 2,
                        bullet.sprite.Width * 4,
                        bullet.sprite.Height * 4);

                    // Check for collision with the enemies
                    foreach (Enemy enemy in level.enemies)
                    {
                        if (bulletRect.Intersects(enemy.BoundingRectangle))
                        {
                            enemy.Alive = false;
                        }
                    }

                    //Everything below here can be deleted if you want
                    //your bullets to shoot through all tiles.
                    //Look for adjacent tiles to the bullet
                    Rectangle bounds = new Rectangle(
                        bulletRect.Center.X - 6,
                        bulletRect.Center.Y - 6,
                        bulletRect.Width / 4,
                        bulletRect.Height / 4);
                    int leftTile = (int)Math.Floor((float)bounds.Left / Tile.Width);
                    int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.Width)) - 1;
                    int topTile = (int)Math.Floor((float)bounds.Top / Tile.Height);
                    int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.Height)) - 1;
                    // For each potentially colliding tile
                    for (int y = topTile; y <= bottomTile; ++y)
                    {
                        for (int x = leftTile; x <= rightTile; ++x)
                        {
                            TileCollision collision = Level.GetCollision(x, y);
                            //If we collide with an Impassable or Platform tile
                            //then delete our bullet.
                            if (collision == TileCollision.Impassable ||
                                collision == TileCollision.Platform)
                            {
                                if (bulletRect.Intersects(bounds))
                                    bullet.alive = false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets player horizontal movement and jump commands from input.
        /// </summary>
        private void GetInput(
            KeyboardState keyboardState, 
            GamePadState gamePadState, 
            TouchCollection touchState,
            AccelerometerState accelState, 
            DisplayOrientation orientation,
            MouseState mouseState)
        {
            // Get analog horizontal movement.
            movement = gamePadState.ThumbSticks.Left.X * MoveStickScale;

            // Ignore small movements to prevent running in place.
            if (Math.Abs(movement) < 0.5f)
                movement = 0.0f;

            // Move the player with accelerometer
            if (Math.Abs(accelState.Acceleration.Y) > 0.10f)
            {
                // set our movement speed
                movement = MathHelper.Clamp(-accelState.Acceleration.Y * AccelerometerScale, -1f, 1f);

                // if we're in the LandscapeLeft orientation, we must reverse our movement
                if (orientation == DisplayOrientation.LandscapeRight)
                    movement = -movement;
            }

            // If any digital horizontal movement input is found, override the analog movement.
            if (gamePadState.IsButtonDown(Buttons.DPadLeft) ||
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A))
            {
                movement = -1.0f;
            }
            else if (gamePadState.IsButtonDown(Buttons.DPadRight) ||
                     keyboardState.IsKeyDown(Keys.Right) ||
                     keyboardState.IsKeyDown(Keys.D))
            {
                movement = 1.0f;
            }

            // Check if the player wants to jump.
            isJumping =
                gamePadState.IsButtonDown(JumpButton) ||
                keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.W) ||
                touchState.AnyTouch();

            // Controlling the player gun arm, with the 
            // GAMEPAD
            gun.rotation = (float)Math.Atan2(gamePadState.ThumbSticks.Right.X, gamePadState.ThumbSticks.Right.Y);

            // Handle the mouse rotation here
            //distance.X = mouseState.X - gun.position.X;
            //distance.Y = mouseState.Y - gun.position.Y;

            distance.X = mouseState.X + level.cameraPosition - gun.position.X;
            distance.Y = mouseState.Y - gun.position.Y;



            //distance.Normalize();
            gun.rotation = (float)Math.Atan2(distance.Y, distance.X) + 1.5f;

            //Console.WriteLine("Distance=" + distance);
            //Console.WriteLine("Rotation=" + gun.rotation.ToString());
            //Console.WriteLine("CameraPosition=" + level.cameraPosition);

            // If facing right
            if (flip == SpriteEffects.FlipHorizontally)
            {
                // If we the player tries to aim behind the head, flip the character
                // to prevent breaking the arm!
                if (gun.rotation < 0)
                    flip = SpriteEffects.None;

                // If we are not rotating the gun, then set it
                // to default position, aiming in front of the player
                if (gun.rotation == 0 && Math.Abs(gamePadState.ThumbSticks.Right.Length()) < 0.5f)
                    gun.rotation = MathHelper.PiOver2;
                
            }

            // Otherwise, we are facing left
            else
            {
                // Once again, flip the character
                if (gun.rotation > 0)
                    flip = SpriteEffects.None;

                // Otherwise, aim
                if (gun.rotation == 0 && Math.Abs(gamePadState.ThumbSticks.Right.Length()) < 0.5f)
                    gun.rotation = -MathHelper.PiOver2;

            }

            // Shoot = Right Tricker
            if (previousGamePadState.Triggers.Right < 0.5 && gamePadState.Triggers.Right > 0.5)
                FireBullet();

            // Shoot = Left Mousebutton
            if (mouseState.LeftButton == ButtonState.Pressed &&
                previousMouseState.LeftButton == ButtonState.Released)
            {
                FireBullet();
            }



            // Get the gamepad state
            previousGamePadState = gamePadState;
            // The the mousestate
            previousMouseState = mouseState;
        }

        /// <summary>
        /// Updates the player's velocity and position based on input, gravity, etc.
        /// </summary>
        public void ApplyPhysics(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = Position;

            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            velocity.X += movement * MoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = DoJump(velocity.Y, gameTime);

            // Apply pseudo-drag horizontally.
            if (IsOnGround)
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;

            // Prevent the player from running faster than his top speed.            
            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            // Apply velocity.
            Position += velocity * elapsed;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            // If the player is now colliding with the level, separate them.
            HandleCollisions();

            // If the collision stopped us from moving, reset the velocity to zero.
            if (Position.X == previousPosition.X)
                velocity.X = 0;

            if (Position.Y == previousPosition.Y)
                velocity.Y = 0;
        }

        /// <summary>
        /// Calculates the Y velocity accounting for jumping and
        /// animates accordingly.
        /// </summary>
        /// <remarks>
        /// During the accent of a jump, the Y velocity is completely
        /// overridden by a power curve. During the decent, gravity takes
        /// over. The jump velocity is controlled by the jumpTime field
        /// which measures time into the accent of the current jump.
        /// </remarks>
        /// <param name="velocityY">
        /// The player's current velocity along the Y axis.
        /// </param>
        /// <returns>
        /// A new Y velocity if beginning or continuing a jump.
        /// Otherwise, the existing Y velocity.
        /// </returns>
        private float DoJump(float velocityY, GameTime gameTime)
        {
            // If the player wants to jump
            if (isJumping)
            {
                // Begin or continue a jump
                if ((!wasJumping && IsOnGround) || jumpTime > 0.0f)
                {
                    if (jumpTime == 0.0f)
                        jumpSound.Play();

                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    sprite.PlayAnimation(jumpAnimation);
                }

                // If we are in the ascent of the jump
                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    jumpTime = 0.0f;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                jumpTime = 0.0f;
            }
            wasJumping = isJumping;

            return velocityY;
        }

        /// <summary>
        /// Detects and resolves all collisions between the player and his neighboring
        /// tiles. When a collision is detected, the player is pushed away along one
        /// axis to prevent overlapping. There is some special logic for the Y axis to
        /// handle platforms which behave differently depending on direction of movement.
        /// </summary>
        private void HandleCollisions()
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle bounds = BoundingRectangle;
            int leftTile = (int)Math.Floor((float)bounds.Left / Tile.Width);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.Width)) - 1;
            int topTile = (int)Math.Floor((float)bounds.Top / Tile.Height);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.Height)) - 1;

            // Reset flag to search for ground collision.
            isOnGround = false;

            //For each potentially colliding movable tile.
            foreach (var movableTile in level.movableTiles)
            {
                // Reset flag to search for movable tile collision.
                movableTile.PlayerIsOn = false;
                //check to see if player is on tile.
                if ((BoundingRectangle.Bottom == movableTile.BoundingRectangle.Top + 1) &&
                    (BoundingRectangle.Left >= movableTile.BoundingRectangle.Left - (BoundingRectangle.Width / 2) &&
                    BoundingRectangle.Right <= movableTile.BoundingRectangle.Right + (BoundingRectangle.Width / 2)))
                {
                    movableTile.PlayerIsOn = true;
                }
                bounds = HandleCollision(bounds, movableTile.Collision, movableTile.BoundingRectangle);
            }

            // For each potentially colliding tile,
            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    // If this tile is collidable,
                    TileCollision collision = Level.GetCollision(x, y);
                    if (collision != TileCollision.Passable)
                    {
                        // Determine collision depth (with direction) and magnitude.
                        Rectangle tileBounds = Level.GetBounds(x, y);
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            // Resolve the collision along the shallow axis.
                            if (absDepthY < absDepthX || collision == TileCollision.Platform)
                            {
                                // If we crossed the top of a tile, we are on the ground.
                                if (previousBottom <= tileBounds.Top)
                                    isOnGround = true;

                                // Ignore platforms, unless we are on the ground.
                                if (collision == TileCollision.Impassable || IsOnGround)
                                {
                                    // Resolve the collision along the Y axis.
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);

                                    // Perform further collisions with the new bounds.
                                    bounds = BoundingRectangle;
                                }
                            }
                            else if (collision == TileCollision.Impassable) // Ignore platforms.
                            {
                                // Resolve the collision along the X axis.
                                Position = new Vector2(Position.X + depth.X, Position.Y);

                                // Perform further collisions with the new bounds.
                                bounds = BoundingRectangle;
                            }
                        }
                    }
                }
            }

            // Save the new bounds bottom.
            previousBottom = bounds.Bottom;
        }

        private Rectangle HandleCollision(Rectangle bounds, TileCollision collision, Rectangle tileBounds)
        {
            Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
            if (depth != Vector2.Zero)
            {
                float absDepthX = Math.Abs(depth.X);
                float absDepthY = Math.Abs(depth.Y);

                // Resolve the collision along the shallow axis.
                if (absDepthY < absDepthX || collision == TileCollision.Platform)
                {
                    // If we crossed the top of a tile, we are on the ground.
                    if (previousBottom <= tileBounds.Top)
                        isOnGround = true;
                    // Ignore platforms, unless we are on the ground.
                    if (collision == TileCollision.Impassable || IsOnGround)
                    {
                        // Resolve the collision along the Y axis.
                        Position = new Vector2(Position.X, Position.Y + depth.Y);
                        // Perform further collisions with the new bounds.
                        bounds = BoundingRectangle;
                    }
                }
                else if (collision == TileCollision.Impassable) // Ignore platforms.
                {
                    // Resolve the collision along the X axis.
                    Position = new Vector2(Position.X + depth.X, Position.Y);
                    // Perform further collisions with the new bounds.
                    bounds = BoundingRectangle;
                }
            }
            return bounds;

        }

    

        /// <summary>
        /// Called when the player has been killed.
        /// </summary>
        /// <param name="killedBy">
        /// The enemy who killed the player. This parameter is null if the player was
        /// not killed by an enemy (fell into a hole).
        /// </param>
        public void OnKilled(Enemy killedBy)
        {
            isAlive = false;

            if (killedBy != null)
                killedSound.Play();
            else
                fallSound.Play();

            sprite.PlayAnimation(dieAnimation);
        }

        /// <summary>
        /// Called when this player reaches the level's exit.
        /// </summary>
        public void OnReachedExit()
        {
            sprite.PlayAnimation(celebrateAnimation);
        }

        /// <summary>
        /// Draws the animated player.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Flip the sprite to face the way we are moving.
            if (Velocity.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            else if (Velocity.X < 0)
                flip = SpriteEffects.None;

            // Draw that sprite.
            sprite.Draw(gameTime, spriteBatch, Position, flip);

            //Draw the bullets
            foreach (GameObject bullet in bullets)
            {
                if (bullet.alive)
                {
                    spriteBatch.Draw(bullet.sprite,
                        bullet.position, Color.White);
                }
            }

            // Draw the arm on top of the player!
            if (IsAlive)
            {
                spriteBatch.Draw(
                    gun.sprite,
                    gun.position,
                    null,
                    Color.White,
                    gun.rotation,
                    gun.center,
                    1.0f,
                    flip,
                    0);
            }
        }
    }
}
