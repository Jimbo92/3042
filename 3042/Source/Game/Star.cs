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
    class Star
    {
        public AnimSprite Sprite;
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;
        public bool isAlive = true;

        public Star(ContentManager getContent)
        {
            Sprite = new AnimSprite(getContent, "graphics/starss", 6, 6, 1, 2);
        }

        public void Update()
        {
            if (isAlive)
            {
                Sprite.UpdateAnimation(0.1f);

                Position += Direction * Speed;
            }
        }

        public void Draw(SpriteBatch sB)
        {
            if (isAlive)
                Sprite.Draw(sB, Position);
        }

    }
}
