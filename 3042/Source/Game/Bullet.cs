using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace _3042
{
    class Bullet
    {
        public BasicSprite sprite;
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;
        public Rectangle CollisionBox;
        public bool isAlive = true;
        public bool DebugMode = false;

        private Texture2D CollisionBoxTexture;

        public void Update(ContentManager getContent, string getTexture, int getWidth, int getHeight)
        {
            sprite = new BasicSprite(getContent, getTexture, getWidth, getHeight);

            CollisionBoxTexture = getContent.Load<Texture2D>("graphics/collisionbox");

            sprite.Position = Position;

            Position += Direction * Speed;

        }

        public void Draw(SpriteBatch sB)
        {
            if (isAlive)
            {
                sprite.Draw(sB);
                CollisionBox = new Rectangle((int)Position.X - sprite.Width / 2, (int)Position.Y - sprite.Height / 2, sprite.Width, sprite.Height);

                if (DebugMode)
                    sB.Draw(CollisionBoxTexture, CollisionBox, Color.White);

            }
            else
                CollisionBox = Rectangle.Empty;
        }

    }
}
