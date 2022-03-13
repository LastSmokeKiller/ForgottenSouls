using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Forgotten_Souls.Collisions;
using Forgotten_Souls.StateManagement;
using Forgotten_Souls.Screens;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace Forgotten_Souls.Sprites
{
    public class Player 
    {

        private Texture2D texture;
        private SoundEffect bang;
        private SpriteFont font;

        public Vector2 Position;
        public float LinearVelocity = 4f;
        public float Rotation;
        public Vector2 Origin;


        private Game game;

        public Bullet Bullet;

        public int Index;
        public bool Shaking;

        public bool lor = false;
        public bool uod = false;


        private float tutorialTimer;
        public PlayerIndex playerIndex;

        public Vector2 Direction;

        private List<Bullet> bullets;

        private BoundingCircle bound;
        public BoundingCircle PlayerBound => bound;

        private readonly InputAction pauseAction;
        private GameplayScreen gameScreen;
        private ScreenManager screenManager;
        private Viewport viewport;
        public FireworkParticleSystem Firework;

        public Player(Game Game, Viewport Viewport, Vector2 StartPosition, List<Bullet> b)
        {
            game = Game;
            viewport = Viewport;
            Position = StartPosition;


            pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

            bound = new BoundingCircle(Position - new Vector2(-32, -32), 32);
            bullets = b;

        }

        public FireworkParticleSystem Initialize(Game game)
        {
            Firework = new FireworkParticleSystem(game, 15);
            return Firework;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Chicken");
            bang = content.Load<SoundEffect>("Laser_Shoot3");
            font = content.Load<SpriteFont>("menufont");
            Bullet = new Bullet();
            Bullet.LoadContent(content);
            Origin = new Vector2((float)texture.Width / 2, (float)texture.Height / 2);
            
        }


        public void Update(GameTime gameTime, List<Bullet> Bullets)
        {
            bullets = Bullets;
            foreach(Bullet b in bullets)
            {
                b.Update(gameTime);
            }
        }

        public void HandleInput(GameTime gameTime, InputState input, KeyboardState keyboard, GamePadState gamePad, PlayerIndex player, PlayerIndex playIn, ref bool shaking)
        {

            
                var movement = Vector2.Zero;
                if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
                {
                    Rotation = (float)Math.PI;
                    movement.X--;
                    lor = false;
                    
                }
                if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
                {
                    movement.X++;
                    Rotation = 2 * (float)Math.PI;
                    lor = true;

                }
                if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
                {
                    movement.Y--;
                    Rotation = 3 * (float)Math.PI /2;
                uod = false;
                }
                if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
                {
                    movement.Y++;
                    Rotation = (float)Math.PI / 2;
                uod = true;
                }
                Direction = new Vector2(MathF.Cos(Rotation), MathF.Sin(Rotation));
                





                var thumbstick = gamePad.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();

                Position += movement * 8f;
            if (input.IsNewKeyPress(Keys.Space, player, out playIn) || input.IsNewButtonPress(Buttons.A, player, out playIn))
            {
                float x = -25;
                float y = -25;
                bang.Play();
                AddBullet(bullets);
                if (lor)
                {
                    x = 25;
                }
                if (uod)
                {
                    y = 25;
                }
                Vector2 bomb = new Vector2(x, y);
                Firework.PlaceFirework(Position + bomb);
                shaking = true;
            }
            CheckBounds(viewport);
            
        }

        public void CheckBounds(Viewport viewport)
        {
            
            if (Position.Y < 32)
            {
                Position.Y = 32;
            }
            if (Position.Y > viewport.Height - 32)
            {
                Position.Y = viewport.Height - 32;
            }
            if (Position.X < 32)
            {
                Position.X = 32;
            }
            if (Position.X > viewport.Width - 32)
            {
                Position.X = viewport.Width - 32;
            }
        }

        private void AddBullet(List<Bullet> bullets)
        {
            Bullet b = new Bullet(this.Position, this.Direction, this.LinearVelocity * 2, this, this.Rotation, 2f, Firework);


            bullets.Add(b);
        }
    

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (texture is null) throw new InvalidOperationException("Texture must be loaded to render");

            tutorialTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;


            spriteBatch.Draw(texture, Position, null, Color.White, Rotation, Origin, 1f, SpriteEffects.None, 0);
            foreach(Bullet b in bullets) b.Draw(spriteBatch, gameTime, bullets, texture);
            if (tutorialTimer < 3f) spriteBatch.DrawString(font, "Press Space or A to fire weapon", Position + new Vector2(32, -64), Color.Black);

        }

        //public object Clone()
       // {
         //   return this.MemberwiseClone();
       // }
    }
}
