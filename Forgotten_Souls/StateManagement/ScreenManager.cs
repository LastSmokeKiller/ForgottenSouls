using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;


namespace Forgotten_Souls.StateManagement
{
    public class ScreenManager : DrawableGameComponent
    {
        private readonly List<GameScreen> screens = new List<GameScreen>();
        private readonly List<GameScreen> tmpScreensList = new List<GameScreen>();

        private readonly ContentManager content;
        private readonly InputState input = new InputState();

        public Song menuMusic;

        private bool isInitialized;

        public SpriteBatch SpriteBatch { get; private set; }

        public SpriteFont Font { get; private set; }

        public Texture2D BlankTexture { get; private set; }


        public ScreenManager(Game game) : base(game)
        {
            content = new ContentManager(game.Services, "Content");

        }

        public override void Initialize()
        {
            base.Initialize();
            isInitialized = true;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Font = content.Load<SpriteFont>("menufont");
            BlankTexture = content.Load<Texture2D>("blank");

            menuMusic = content.Load<Song>("Phantom");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(menuMusic);

            foreach(var screen in screens)
            {
                screen.Activate();
            }
        }

        protected override void UnloadContent()
        {
            foreach (var screen in screens)
            {
                screen.Unload();
            }
        }


        public override void Update(GameTime gameTime)
        {
            input.Update();

            tmpScreensList.Clear();
            tmpScreensList.AddRange(screens);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            while (tmpScreensList.Count > 0)
            {
                var screen = tmpScreensList[tmpScreensList.Count - 1];
                tmpScreensList.RemoveAt(tmpScreensList.Count - 1);

                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                {
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(gameTime, input);
                        otherScreenHasFocus = true;
                    }

                    if (!screen.IsPopup) coveredByOtherScreen = true;
                }
            }
        }


        public override void Draw(GameTime gameTime)
        {
            foreach( var screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden) continue;

                screen.Draw(gameTime);
            }
        }

        public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer)
        {
            screen.ControllingPlayer = controllingPlayer;
            screen.ScreenManager = this;
            screen.IsExiting = false;

            if (isInitialized) screen.Activate();

            screens.Add(screen);
        }

        public void RemoveScreen(GameScreen screen)
        {
            if (isInitialized) screen.Unload();

            screens.Remove(screen);
            tmpScreensList.Remove(screen);
        }

        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }

        public void FadeBackBufferToBlack(float alpha)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(BlankTexture, GraphicsDevice.Viewport.Bounds, Color.Black * alpha);
            SpriteBatch.End();
        }

        public void Deactivate()
        {
        }

        public bool Activate()
        {
            return false;
        }
    }
}
