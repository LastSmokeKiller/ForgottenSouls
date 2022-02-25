using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Forgotten_Souls.Sprites
{
    public class Player
    {

        private Texture2D texture;


        public Vector2 Position { get; set; }

        private Color color;

        private Game game;

        public Player(Game game, Color color)
        {
            this.game = game;
            this.color = color;

        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Farmer");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (texture is null) throw new InvalidOperationException("Texture must be loaded to render");
            

        }
    }
}
