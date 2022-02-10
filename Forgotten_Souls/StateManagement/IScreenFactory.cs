using System;
using System.Collections.Generic;
using System.Text;

namespace Forgotten_Souls.StateManagement
{
    public interface IScreenFactory
    {
        GameScreen CreateScreen(Type screenType);
    }
}
