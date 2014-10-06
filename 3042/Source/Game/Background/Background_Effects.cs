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
    class Background_effects
    {
        enum ESpriteType
        {
            BASIC,
            ANIM
        }
        ESpriteType SpriteType = ESpriteType.BASIC;

        public AnimSprite SpriteAnim;
        public BasicSprite Sprite;
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;
        public bool isAlive = true;

        public Background_effects(ContentManager getContent, string getTexture, int getWidth, int getHeight, int getRows, int getColumns)
        {
            SpriteType = ESpriteType.ANIM;
            SpriteAnim = new AnimSprite(getContent, getTexture, getWidth, getHeight, getRows, getColumns);
        }
        public Background_effects(ContentManager getContent, string getTexture, int getWidth, int getHeight)
        {
            SpriteType = ESpriteType.BASIC;
            Sprite = new BasicSprite(getContent, getTexture, getWidth, getHeight);
        }

        public void Update()
        {
            if (isAlive)
                Position += Direction * Speed;
        }
        public void Update(float getDelay)
        {
            if (isAlive)
            {
                SpriteAnim.UpdateAnimation(getDelay);
                Position += Direction * Speed;
            }
        }

        public void Draw(SpriteBatch sB)
        {
            if (isAlive)
                if (SpriteType == ESpriteType.ANIM)
                    SpriteAnim.Draw(sB, Position);
                else
                    Sprite.Draw(sB, Position);
            else
                Position = Vector2.Zero;

        }

    }
}
