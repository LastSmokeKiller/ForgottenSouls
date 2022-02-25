using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Forgotten_Souls.StateManagement;
using Forgotten_Souls.Collisions;
using Forgotten_Souls.Sprites;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Forgotten_Souls.Screens
{
    public class GameplayScreen : GameScreen
    {
        private ContentManager content;
        private SpriteFont gameFont;

        private Texture2D playerTexture;
        private Texture2D farmerTexture;

        private SoundEffect bang;
        private Song gameMusic;
        private Song menuMusic;

        private float bulletTimer;
        public bool BulletRemoved = false;
        public Vector2 BulletPosition;
        public float BulletLinearVelocity = 4f;
        public Vector2 BulletOrigin;

        public float PlayerRotation;
        public Vector2 PlayerOrigin;

        public Vector2 Direction;

        private Vector2 playerPosition = new Vector2(300, 300);
        private Vector2 farmerPosition = new Vector2(100, 100);
        private BoundingCircle farmerBound;
        private BoundingCircle playerBound;

        private string farmerMessage = "Hello there";
        private bool farmerDisplay;
        public const float pi = 3.14159274f;



        public BoundingCircle FarmerBound => farmerBound;
        public BoundingCircle PlayerBound => playerBound;

        private readonly Random random = new Random();

        private float pauseAlpha;
        private readonly InputAction pauseAction;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

            

            farmerBound = new BoundingCircle(farmerPosition - new Vector2(-32, -32), 32);

            playerBound = new BoundingCircle(playerPosition - new Vector2(-32, -32), 32);

        }

        public override void Activate()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("menufont");
            playerTexture = content.Load<Texture2D>("Chicken");
            farmerTexture = content.Load<Texture2D>("Farmer");

            bang = content.Load<SoundEffect>("Laser_Shoot3");
            gameMusic = content.Load<Song>("ambience");
            menuMusic = content.Load<Song>("Phantom");
            MediaPlayer.Stop();
            MediaPlayer.Play(gameMusic);
            Thread.Sleep(1000);

            PlayerOrigin = new Vector2((float)playerTexture.Width/2, (float)playerTexture.Height/2);
            BulletOrigin = new Vector2((float)playerTexture.Width / 2, (float)playerTexture.Height / 2);
            ScreenManager.Game.ResetElapsedTime();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool OtherScreenHasFocus, bool CoveredByOtherScreen)
        {
            base.Update(gameTime, OtherScreenHasFocus, false);
            bulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(bulletTimer > 2f)
            {
                BulletRemoved = true;
            }
                

            if (CoveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha + 1f / 32, 0);

            if (IsActive)
            {
                //This is where we insert information for enemies
                

                //enemyPosition = new Vector2(50, 50);
            }

            if (farmerBound.CollidesWith(playerBound))
            {
                farmerDisplay = true;
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));


            int playerIndex = (int)ControllingPlayer.Value;

            var playIn = ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            

            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);

            }
            else
            {
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                {
                    PlayerRotation = 3 * pi / 2;
                    movement.X--;
                }
                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                    movement.X++;
                    PlayerRotation = pi / 2;
                    
                }
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                {
                    movement.Y--;
                    PlayerRotation = 2 * pi;
                    
                }
                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                {
                    movement.Y++;
                    PlayerRotation = pi;
                }
                Direction = new Vector2((float)Math.Cos(PlayerRotation), (float)Math.Sin(PlayerRotation));
                if (input.IsNewKeyPress(Keys.Space, player, out playIn) || input.IsNewButtonPress(Buttons.A, player, out playIn))
                {
                    bang.Play();
                    BulletPosition += Direction * BulletLinearVelocity;
                }
                    

                var thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();

                playerPosition += movement * 8f;

                CheckBounds();
            }
        }

        public void CheckBounds()
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            if(playerPosition.Y < 32)
            {
                playerPosition.Y = 32;
            }
            if(playerPosition.Y > viewport.Height - 32)
            {
                playerPosition.Y = viewport.Height - 32;
            }
            if(playerPosition.X < 32)
            {
                playerPosition.X = 32;
            }
            if(playerPosition.X > viewport.Width - 32)
            {
                playerPosition.X = viewport.Width - 32;
            }
        }
        

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.Draw(playerTexture, playerPosition,null, Color.White, PlayerRotation, PlayerOrigin, 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(playerTexture, BulletPosition, null, Color.Red, PlayerRotation, BulletOrigin, .25f, SpriteEffects.None, 0);
            spriteBatch.Draw(farmerTexture, farmerPosition, Color.White);
            if (farmerDisplay) spriteBatch.DrawString(gameFont, farmerMessage, farmerPosition, Color.White);
            spriteBatch.End();

            if(TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);


            } 
        }
    }
}
