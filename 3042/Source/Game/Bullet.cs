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
        public enum ESpriteType
        {
            ANIM,
            BASIC
        };

        public BasicSprite sprite;
        public AnimSprite SpriteAnim;
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;
        public Rectangle CollisionBox;
        public bool isAlive = true;
        public bool isRemoved = false;
        public bool DebugMode = false;
        public float Delay;
        public Vector2 FirePosition;
        public float Damage;

        private AnimSprite ExplosionAnim;
        private bool isExplosion = false;
        private AnimSprite ShootAnim;
        private bool isShooting = true;
        private ContentManager Content;
        private Texture2D CollisionBoxTexture;

        public ESpriteType _spriteType = ESpriteType.BASIC;


        public Bullet(ContentManager getContent, string getTexture, int getWidth, int getHeight)
        {
            Content = getContent;
            sprite = new BasicSprite(getContent, getTexture, getWidth, getHeight);
            ExplosionAnim = new AnimSprite(Content, "graphics/BulletExplosionSS", getWidth, getHeight, 1, 8);
            ShootAnim = new AnimSprite(Content, "graphics/ShootEffect1SS", getWidth * 2, getHeight, 2, 1);

        }
        public Bullet(ContentManager getContent, string getTexture, int getWidth, int getHeight, int getRows, int getColumns)
        {
            Content = getContent;
            SpriteAnim = new AnimSprite(Content, getTexture, getWidth, getHeight, getRows, getColumns);
            ExplosionAnim = new AnimSprite(Content, "graphics/BulletExplosionSS", getWidth, getHeight, 1, 8);
            ShootAnim = new AnimSprite(Content, "graphics/ShootEffect1SS", getWidth * 2, getHeight, 2, 1);

        }

        public void Update()
        {
            CollisionBoxTexture = Content.Load<Texture2D>("graphics/collisionbox");

            if (_spriteType == ESpriteType.ANIM)
                SpriteAnim.UpdateAnimation(Delay);

            Position += Direction * Speed;

            if (Position.Y >= 800 || Position.Y <= -100 || GameMode.Mode == GameMode.EGameMode.GAMEOVER || GameMode.Mode == GameMode.EGameMode.MENU)
            {
                isRemoved = true;
                isAlive = false;
            }
        }

        public void Draw(SpriteBatch sB)
        {
            if (isAlive)
            {
                switch (_spriteType)
                {
                    case ESpriteType.BASIC:
                        {
                            sprite.Draw(sB, Position);
                            CollisionBox = new Rectangle((int)Position.X - sprite.Width / 2, (int)Position.Y - sprite.Height / 2, sprite.Width, sprite.Height);
                        }; break;

                    case ESpriteType.ANIM:
                        {
                            SpriteAnim.Draw(sB, Position);
                            CollisionBox = new Rectangle((int)Position.X - SpriteAnim.Width / 2, (int)Position.Y - SpriteAnim.Height / 2, SpriteAnim.Width, SpriteAnim.Height);
                        }; break;
                }
                if (DebugMode)
                    sB.Draw(CollisionBoxTexture, CollisionBox, Color.White);

                if (!ShootAnim.AnimationFinnished)
                    isShooting = true;
                else
                    isShooting = false;

                if (isShooting)
                {
                    ShootAnim.UpdateAnimation(1f);
                    ShootAnim.Draw(sB, FirePosition);
                }

            }           
            else
            {
                CollisionBox.X = -100;
                CollisionBox.Y = -100;
                CollisionBox.Width = 0;
                CollisionBox.Height = 0;

                if (!isRemoved)
                {
                    if (!ExplosionAnim.AnimationFinnished)
                        isExplosion = true;
                    else
                        isExplosion = false;

                    if (isExplosion)
                    {
                        Speed = 1f;
                        ExplosionAnim.UpdateAnimation(0.6f);
                        ExplosionAnim.Draw(sB, Position);
                    }
                }
            }
        }

    }
}
