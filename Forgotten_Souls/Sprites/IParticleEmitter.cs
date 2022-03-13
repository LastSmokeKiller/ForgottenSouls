using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Forgotten_Souls.Sprites
{
    public interface IParticleEmitter
    {
        public Vector2 Position { get; }

        public Vector2 Velocity { get; }
    }
}
