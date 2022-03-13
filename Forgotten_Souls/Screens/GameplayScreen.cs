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
    public class GameplayScreen : GameScreen, IParticleEmitter
    {
        private ContentManager content;
        private SpriteFont gameFont;

        private List<Bullet> bullets;

        private Texture2D playerTexture;
        private Texture2D farmerTexture;
        private Texture2D gameplayTexture;

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        private SoundEffect bang;
        private Song gameMusic;
        private Song menuMusic;

        private Player player;
        private Game game;

        private float bulletTimer;
        public bool BulletRemoved = false;
        public Vector2 BulletPosition;
        public float BulletLinearVelocity = 20f;
        public Vector2 BulletOrigin;

        public float PlayerRotation;
        public Vector2 PlayerOrigin;

        public Vector2 Direction;
        public PlayerIndex PlayerIndex;

        private Vector2 playerPosition1 = new Vector2(300, 300);
        private Vector2 farmerPosition = new Vector2(100, 100);
        private BoundingCircle farmerBound;
        private BoundingCircle playerBound;
        public bool shaking;
        private float shakeTime;

        public FireworkParticleSystem Firework;

        private string farmerMessage = "Hello there";
        private bool farmerDisplay;



        public BoundingCircle FarmerBound => farmerBound;
        public BoundingCircle PlayerBound => playerBound;

        private readonly Random random = new Random();

        private float pauseAlpha;
        private readonly InputAction pauseAction;

        public GameplayScreen(Game g)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

            game = g;

            
            farmerBound = new BoundingCircle(farmerPosition - new Vector2(-32, -32), 32);



        }

        

        public override void Activate()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("menufont");
            farmerTexture = content.Load<Texture2D>("Farmer");
            gameplayTexture = content.Load<Texture2D>("GameplayBackground");

            
            
            Bullet b = new Bullet();
            bullets = new List<Bullet>();
            bullets.Add(b);
            player = new Player(game, ScreenManager.GraphicsDevice.Viewport, playerPosition1, bullets);
            player.LoadContent(content);
            
            gameMusic = content.Load<Song>("ambience");
            menuMusic = content.Load<Song>("Phantom");
            MediaPlayer.Stop();
            MediaPlayer.Play(gameMusic);


            game.Components.Add(player.Initialize(game));


            Thread.Sleep(1000);

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



            player.Update(gameTime, bullets);
            if (bullets.Equals(null) && bullets.Count > 0)
            {
                foreach (Bullet b in bullets)
                {
                    if (b.IsRemoved) bullets.Remove(b);
                }
            }
            if (CoveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

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
            
            PlayerIndex playIn = ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex Player;
            if (pauseAction.Occurred(input, ControllingPlayer, out Player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(game), ControllingPlayer);

            }
            else
            {
                player.HandleInput(gameTime, input, keyboardState, gamePadState,Player, playIn, ref shaking);
                
            }

                

            
        }

       
        

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            var spriteBatch = ScreenManager.SpriteBatch;
            Matrix shakeTransform = Matrix.Identity;
            if (shaking)
            {
                shakeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                shakeTransform = Matrix.CreateTranslation(2 *
                    MathF.Sin(shakeTime), 2 * MathF.Cos(shakeTime), 0);
                if (shakeTime > 100)
                {
                    shakeTime = 0;
                    shaking = false;
                }
            }

            spriteBatch.Begin();
            spriteBatch.Draw(gameplayTexture, Vector2.Zero , Color.White);
            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: shakeTransform);
            player.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(farmerTexture, farmerPosition, Color.White);
            if (farmerDisplay) spriteBatch.DrawString(gameFont, farmerMessage, farmerPosition, Color.White);
            spriteBatch.End();

            if(TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                


                ScreenManager.FadeBackBufferToBlack(alpha);
            } 
        }
    }
}
