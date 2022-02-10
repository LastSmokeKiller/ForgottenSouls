using System;
using System.Collections.Generic;
using System.Text;
using Forgotten_Souls.StateManagement;

namespace Forgotten_Souls
{
    public class ScreenFactory : IScreenFactory
    {
        public GameScreen CreateScreen(Type screenType)
        {
            return Activator.CreateInstance(screenType) as GameScreen;
        }
    }
}
