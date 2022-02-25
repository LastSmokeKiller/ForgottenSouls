using System;
using System.Collections.Generic;
using System.Text;
using Forgotten_Souls.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Forgotten_Souls.Screens
{
    public class SplashScreen : GameScreen
    {
        ContentManager content;
        Texture2D background;
        TimeSpan displayTime;
        public override void Activate()
        {
            base.Activate();

            if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");
            background = content.Load<Texture2D>("Splash");
            displayTime = TimeSpan.FromSeconds(2);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            displayTime -= gameTime.ElapsedGameTime;
            if (displayTime <= TimeSpan.Zero) ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            const string message = "LSK GAMES";
            var font = ScreenManager.Font;

            var viewport = ScreenManager.GraphicsDevice.Viewport;
            var viewportSize = new Vector2(viewport.Width, viewport.Height);
            var textSize = font.MeasureString(message);
            var higher = new Vector2(0, 225);
            var textPostition = ((viewportSize - textSize) / 2) + higher;

            ScreenManager.SpriteBatch.Begin();
     
            ScreenManager.SpriteBatch.Draw(background, Vector2.Zero, Color.White);
            ScreenManager.SpriteBatch.DrawString(font, message, textPostition, Color.White);

            ScreenManager.SpriteBatch.End();
        }
    }
}
