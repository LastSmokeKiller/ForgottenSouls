using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Forgotten_Souls.StateManagement;

namespace Forgotten_Souls.Screens
{
    public class LoadingScreen : GameScreen
    {
        private readonly bool loadingIsSlow;
        private bool otherScreensAreGone;
        private readonly GameScreen[] screensToLoad;

        private LoadingScreen(ScreenManager screenManager, bool LoadingIsSlow, GameScreen[] ScreensToLoad)
        {
            
            loadingIsSlow = LoadingIsSlow;
            screensToLoad = ScreensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }

        public static void Load(ScreenManager screenManager, bool LoadingIsSlow,
                                PlayerIndex? ControllingPlayer, params GameScreen[] ScreensToLoad)
        {
            foreach (var screen in screenManager.GetScreens())
                screen.ExitScreen();

            var loadingScreen = new LoadingScreen(screenManager, LoadingIsSlow, ScreensToLoad);

            screenManager.AddScreen(loadingScreen, ControllingPlayer);
        }

        public override void Update(GameTime gameTime, bool OtherScreenHasFocus, bool CoveredByOtherScreen)
        {
            base.Update(gameTime, OtherScreenHasFocus, CoveredByOtherScreen);

            if (otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (var screen in screensToLoad)
                {
                    if (screen != null)
                        ScreenManager.AddScreen(screen, ControllingPlayer);
                }

                ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (ScreenState == ScreenState.Active && ScreenManager.GetScreens().Length == 1)
                otherScreensAreGone = true;

            if (loadingIsSlow)
            {
                var spriteBatch = ScreenManager.SpriteBatch;
                var font = ScreenManager.Font;

                const string message = "Loading...";

                var viewport = ScreenManager.GraphicsDevice.Viewport;
                var viewportSize = new Vector2(viewport.Width, viewport.Height);
                var textSize = font.MeasureString(message);
                var textPosition = (viewportSize - textSize) / 2;

                var color = Color.White * TransitionAlpha;

                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, textPosition, color);
                spriteBatch.End();
            }
        }
    }
}
