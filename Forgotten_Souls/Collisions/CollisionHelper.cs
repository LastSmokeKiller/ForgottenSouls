using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Forgotten_Souls.Collisions
{
    public static class CollisionHelper
    {
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >=
                Math.Pow(a.Center.X - b.Center.X, 2) +
                Math.Pow(a.Center.Y - b.Center.Y, 2);
        }


    }
}
