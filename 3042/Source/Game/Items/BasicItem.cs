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
    class BasicItem
    {
        public BasicSprite Sprite;
        public AnimSprite SpriteAnim;
        public Rectangle CollisionBox;
        public Vector2 Position;
        public bool isAlive = true;

        private Random RandXDir = new Random();
        private int RandXDirNum;
        private int LifeTimer;

        private bool IsUp;
        private bool IsLeft;

        private enum ESpriteType
        {
            BASIC,
            ANIM
        }
        ESpriteType SpriteType = ESpriteType.BASIC;

        public BasicItem(ContentManager getContent, string getTexture, int getWidth, int getHeight)
        {
            Sprite = new BasicSprite(getContent, getTexture, getWidth, getHeight);
            RandXDirNum = RandXDir.Next(2);
            if (RandXDirNum == 0)
            {
                IsLeft = true;
                IsUp = false;
            }
            else
            {
                IsLeft = false;
                IsUp = false;
            }
        }
        public BasicItem(ContentManager getContent, string getTexture, int getWidth, int getHeight, int getRows, int getColumns)
        {
            SpriteAnim = new AnimSprite(getContent, getTexture, getWidth, getHeight, getRows, getColumns);
            SpriteType = ESpriteType.ANIM;
            RandXDirNum = RandXDir.Next(2);
            if (RandXDirNum == 0)
            {
                IsLeft = true;
                IsUp = false;
            }
            else
            {
                IsLeft = false;
                IsUp = false;
            }
        }

        public void Draw(SpriteBatch sB)
        {
            LifeTimer++;
            if (LifeTimer < 500 && isAlive)
            {
                isAlive = true;
            }
            else
            {
                LifeTimer = 501;
                isAlive = false;
            }

            if (IsUp)
                Position.Y += 3;
            else
                Position.Y -= 3;
            if (IsLeft)
                Position.X += 3;
            else
                Position.X -= 3;

            if (Position.X >= 700)
                IsLeft = false;
            if (Position.X <= 0)
                IsLeft = true;
            if (Position.Y >= 700)
                IsUp = false;
            if (Position.Y <= 0)
                IsUp = true;

            if (SpriteType == ESpriteType.BASIC && isAlive)
            {
                Sprite.Draw(sB, Position);
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
            }
            else
                CollisionBox = Rectangle.Empty;

            if (SpriteType == ESpriteType.ANIM && isAlive)
            {
                CollisionBox = new Rectangle((int)Position.X - SpriteAnim.Width / 2, (int)Position.Y - SpriteAnim.Height / 2, SpriteAnim.Width, SpriteAnim.Height);
                SpriteAnim.UpdateAnimation(0.5f);
                SpriteAnim.Draw(sB, Position);
            }
            else
                CollisionBox = Rectangle.Empty;
        }
    }
}
