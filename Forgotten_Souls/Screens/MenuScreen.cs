using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Forgotten_Souls.StateManagement;

namespace Forgotten_Souls.Screens
{
    public abstract class MenuScreen : GameScreen
    {
        private readonly List<MenuEntry> menuEntries = new List<MenuEntry>();
        private int selectedEntry;
        private readonly string menuTitle;

        private readonly InputAction menuUp;
        private readonly InputAction menuDown;
        private readonly InputAction menuSelect;
        private readonly InputAction menuCancel;

        protected IList<MenuEntry> MenuEntries => menuEntries;

        protected MenuScreen(string MenuTitle)
        {
            menuTitle = MenuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            menuUp = new InputAction(
                new[] { Buttons.DPadUp, Buttons.LeftThumbstickUp },
                new[] { Keys.Up, Keys.W }, true);
            menuDown = new InputAction(
                new[] { Buttons.DPadDown, Buttons.LeftThumbstickDown },
                new[] { Keys.Down, Keys.S }, true);
            menuSelect = new InputAction(
                new[] { Buttons.A, Buttons.Start },
                new[] { Keys.Enter, Keys.Space }, true);
            menuCancel = new InputAction(
                new[] { Buttons.B, Buttons.Back},
                new[] { Keys.Back, Keys.Escape }, true);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex;

            if(menuUp.Occurred(input, ControllingPlayer, out playerIndex))
            {
                selectedEntry--;

                if (selectedEntry < 0) 
                    selectedEntry = menuEntries.Count - 1;
            }

            if(menuDown.Occurred(input, ControllingPlayer, out playerIndex))
            {
                selectedEntry++;

                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
            }

            if (menuSelect.Occurred(input, ControllingPlayer, out playerIndex))
                OnSelectEntry(selectedEntry, playerIndex);
            else if (menuCancel.Occurred(input, ControllingPlayer, out playerIndex))
                OnCancel(playerIndex);
        }

        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }

        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }

        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }

        protected virtual void UpdateMenuEntryLocations()
        {
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            var position = new Vector2(0f, 175f);

            foreach(var menuEntry in menuEntries)
            {
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                menuEntry.Position = position;

                position.Y += menuEntry.GetHeight(this);
            }
        }

        public override void Update(GameTime gameTime, bool OtherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, OtherScreenHasFocus, coveredByOtherScreen);

            for(int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && i == selectedEntry;
                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            UpdateMenuEntryLocations();

            var graphics = ScreenManager.GraphicsDevice;
            var spriteBatch = ScreenManager.SpriteBatch;
            var font = ScreenManager.Font;

            spriteBatch.Begin();

            for(int i = 0; i < menuEntries.Count; i++)
            {
                var menuEntry = menuEntries[i];
                bool isSelected = IsActive && i == selectedEntry;
                menuEntry.Draw(this, isSelected, gameTime);
            }

            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            var titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            var titleOrigin = font.MeasureString(menuTitle) / 2;
            var titleColor = new Color(192, 192, 192) * TransitionAlpha;
            const float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor,
                0, titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
            
        }
    }
}
