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
    class Enemy
    {
        public enum ESpriteType
        {
            ANIM,
            BASIC
        };

        public BasicSprite Sprite;
        public AnimSprite SpriteAnim;
        public Vector2 Position;
        public Vector2 Direction;
        public Vector2 GotoPos;
        public Vector2 GotoPos2;
        public float Speed;
        public float Delay;
        public bool isAlive = true;
        public bool DebugMode = false;
        public Rectangle CollisionBox;
        public float Health;
        public float MaxHealth;
        public int Score;
        public bool isWarpIn = false;

        private int ScoreTimer;
        private ContentManager Content;
        private AnimSprite ExplosionAnim;
        private bool isExplosion = false;
        private Texture2D CollisionBoxTexture;
        private AnimSprite WarpInEffect;
        private int TimerTest;

        public ESpriteType _spriteType = ESpriteType.BASIC;

        public Enemy(ContentManager getContent, string getTexture, int getWidth, int getHeight)
        {
            Content = getContent;

            Sprite = new BasicSprite(getContent, getTexture, getWidth, getHeight);
            ExplosionAnim = new AnimSprite(Content, "graphics/Explosion1SS", getWidth * 2, getHeight * 2, 1, 8);
            WarpInEffect = new AnimSprite(Content, "graphics/warpinss", Sprite.Width * 4, Sprite.Height * 3, 1, 9);
        }
        public Enemy(ContentManager getContent, string getTexture, int getWidth, int getHeight, int getRows, int getColumns)
        {
            Content = getContent;

            SpriteAnim = new AnimSprite(getContent, getTexture, getWidth, getHeight, getRows, getColumns);
            ExplosionAnim = new AnimSprite(Content, "graphics/Explosion1SS", getWidth * 2, getHeight * 2, 1, 8);
            //WarpInEffect = new AnimSprite(Content, "graphics/warpinss", Sprite.Width * 2, Sprite.Height * 2, 1, 8);
        }

        public void Update(GUI getGUI)
        {
            CollisionBoxTexture = Content.Load<Texture2D>("graphics/collisionbox");

            if (_spriteType == ESpriteType.ANIM)
                SpriteAnim.UpdateAnimation(Delay);

            Position += Direction * Speed;

            if (Health <= 0)
            {
                isAlive = false;

                ScoreTimer++;
                if (ScoreTimer <= 1)
                    getGUI.Score += Score;
                else
                    ScoreTimer = 2;
            }

            if (Position.Y >= 800 || Position.Y <= -100)
                isAlive = false;

        }

        public void Draw(SpriteBatch sB)
        {
            if (isAlive)
            {
                if (isWarpIn)
                {
                    if (!WarpInEffect.AnimationFinnished)
                    {
                        WarpInEffect.UpdateAnimation(0.4f);
                        WarpInEffect.Draw(sB, new Vector2(Position.X + 15, Position.Y), MathHelper.ToRadians(270));
                    }
                }

                switch (_spriteType)
                {
                    case ESpriteType.BASIC:
                        {
                            Sprite.Draw(sB, Position);
                            CollisionBox = new Rectangle((int)Position.X - Sprite.Width / 2, (int)Position.Y - Sprite.Height / 2, Sprite.Width, Sprite.Height);
                        }; break;

                    case ESpriteType.ANIM:
                        {
                            SpriteAnim.Draw(sB, Position);
                            CollisionBox = new Rectangle((int)Position.X - SpriteAnim.Width / 2, (int)Position.Y - SpriteAnim.Height / 2, SpriteAnim.Width, SpriteAnim.Height);
                        }; break;
                }

                if (DebugMode)
                    sB.Draw(CollisionBoxTexture, CollisionBox, Color.White);
            }
            else
            {
                CollisionBox = Rectangle.Empty;

                if (!ExplosionAnim.AnimationFinnished)
                    isExplosion = true;
                else
                    isExplosion = false;

                if (isExplosion)
                {
                    ExplosionAnim.UpdateAnimation(0.3f);
                    ExplosionAnim.Draw(sB, Position);
                }
            }
        }

    }
}
