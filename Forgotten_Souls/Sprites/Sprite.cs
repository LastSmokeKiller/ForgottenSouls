using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Forgotten_Souls.StateManagement;
using Forgotten_Souls.Collisions;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace Forgotten_Souls.Sprites
{
    public class Sprite : ICloneable
    {
        protected Texture2D texture;

        protected float rotation;

        public Vector2 Position;

        public Vector2 Origin;

        public Vector2 Direction;

        public float RotationVelocity = 3f;

        public float LinearVelocity = 4f;

        public Sprite Parent;

        public float LifeSpan = 0f;

        public bool IsRemoved = false;

        public Sprite(Texture2D Texture)
        {
            texture = Texture;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, rotation, Origin, 1, SpriteEffects.None, 0);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        
    }
}
