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
    class Lives
    {
        public BasicSprite Sprite;
        public Vector2 Position;
        public bool isAlive = true;


        public void LoadContent(ContentManager getContent)
        {
            Sprite = new BasicSprite(getContent, "graphics/life", 28, 39);
        }

        public void Draw(SpriteBatch sB)
        {
            if (isAlive)
            Sprite.Draw(sB, Position);
        }
    }
}
