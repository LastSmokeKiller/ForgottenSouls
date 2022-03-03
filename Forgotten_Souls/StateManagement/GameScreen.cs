using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Forgotten_Souls.StateManagement
{
    public abstract class GameScreen
    {
        public bool IsPopup { get; protected set; }

        protected TimeSpan TransitionOnTime { get; set; } = TimeSpan.Zero;

        protected TimeSpan TransitionOffTime { get; set; } = TimeSpan.Zero;


        protected float TransitionPosition { get; set; } = 1;

        public float TransitionAlpha => 1f - TransitionPosition;

        public ScreenState ScreenState { get; set; } = ScreenState.TransitionOn;

        public bool IsExiting { get; protected internal set; }

        public Game Game { get; set; }

        public bool IsActive => !otherScreenHasFocus && (
            ScreenState == ScreenState.TransitionOn ||
            ScreenState == ScreenState.Active);

        private bool otherScreenHasFocus;

        public ScreenManager ScreenManager { get; internal set; }

        public PlayerIndex? ControllingPlayer { protected get; set; }

        public virtual void Activate() { }

        public virtual void Deactivate() { }

        public virtual void Unload() { }

        public virtual void Update(GameTime gameTime, bool OtherScreenHasFocus, bool coveredByOtherScreen)
        {
            otherScreenHasFocus = OtherScreenHasFocus;

            if (IsExiting)
            {
                ScreenState = ScreenState.TransitionOff;

                if (!UpdateTransitionPosition(gameTime, TransitionOffTime, 1))
                    ScreenManager.RemoveScreen(this);
            }
            else if (coveredByOtherScreen)
            {
                ScreenState = UpdateTransitionPosition(gameTime, TransitionOffTime, 1)
                    ? ScreenState.TransitionOff
                    : ScreenState.Hidden;
            }
            else
            {
                ScreenState = UpdateTransitionPosition(gameTime, TransitionOnTime, -1)
                    ? ScreenState.TransitionOn
                    : ScreenState.Active;
            }
        }

        private bool UpdateTransitionPosition(GameTime gameTime, TimeSpan time, int direction)
        {
            float transitionDelta = (time == TimeSpan.Zero)
                ? 1
                : (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);

            TransitionPosition += transitionDelta * direction;

            if(direction < 0 && TransitionPosition <= 0 || direction > 0 && TransitionPosition >= 0)
            {
                TransitionPosition = MathHelper.Clamp(TransitionPosition, 0, 1);
                return false;
            }

            return true;
        }

        public virtual void HandleInput(GameTime gameTime, InputState input) { }

        public virtual void Draw(GameTime gameTime) { }

        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
                ScreenManager.RemoveScreen(this);
            else
                IsExiting = true;
        }
    }
}
