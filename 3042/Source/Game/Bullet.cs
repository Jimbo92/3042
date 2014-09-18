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

        public void Update(ContentManager getContent, string getTexture, int getWidth, int getHeight)
        {
            sprite = new BasicSprite(getContent, getTexture, getWidth, getHeight);

            sprite.Position = Position;

            Position += Direction * Speed;

        }

        public void Draw(SpriteBatch sB)
        {
            sprite.Draw(sB);
        }

    }
}
