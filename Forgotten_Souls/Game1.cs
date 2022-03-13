using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Forgotten_Souls.StateManagement;
using Forgotten_Souls.Screens;
using Microsoft.Xna.Framework.Media;
using Forgotten_Souls.Sprites;

namespace Forgotten_Souls
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private readonly ScreenManager screenManager;
        public Viewport viewport;

        public FireworkParticleSystem fireworks;

        public Song menuMusic;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            Window.Title = "FORGOTTEN SOULS";

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(this), null);
            screenManager.AddScreen(new SplashScreen(), null);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            menuMusic = Content.Load<Song>("Phantom");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
