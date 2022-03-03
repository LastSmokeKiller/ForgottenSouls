using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Forgotten_Souls.Sprites
{
    public class Bullet
    {

        private float timer;
        public bool IsRemoved = false;
        public Vector2 Postion;
        public Vector2 Direction;
        public float LinearVelocity;
        public Texture2D bulletText;
        public Player Parent;
        public float LifeSpan = 2f;
        protected float rotation;
        public Vector2 Origin;

        public Bullet() { }

        public Bullet(Vector2 position, Vector2 direction, float linerV, Player p, float rot, float lifeSpan)
        {
            Postion = position;
            Direction = direction;
            LinearVelocity = linerV;
            Parent = p;
            rotation = rot;
            LifeSpan = lifeSpan;
        }

        public void LoadContent(ContentManager content)
        {
            bulletText = content.Load<Texture2D>("Chicken");
            Origin = new Vector2(bulletText.Width / 2, bulletText.Height / 2);
        }

        public void Update(GameTime gameTime)
        {

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= LifeSpan)
            {
                IsRemoved = true;
                timer = 0;
            }
                


            Postion += Direction * LinearVelocity;

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, List<Bullet> bullets, Texture2D texture)
        {
            spriteBatch.Draw(texture, Postion, null, Color.Red, rotation, Origin, .25f, SpriteEffects.None, 0);
        }

   








    }
}
