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
        public float Speed;
        public float Delay;
        public bool isAlive = true;
        public bool DebugMode = false;
        public Rectangle CollisionBox;
        public float Health;
        public float MaxHealth;
        public int Score;
        public bool isWarpIn = false;

        private bool isRemoved = false;
        private int ScoreTimer;
        private ContentManager Content;
        private AnimSprite ExplosionAnim;
        private AnimSprite ShockWaveAnim;
        private bool isExplosion = false;
        private Texture2D CollisionBoxTexture;
        private AnimSprite WarpInEffect;
        private Font DisplayScore;
        private SoundEffect ExplosionSFX;
        private SoundEffectInstance ExplosionSFXIns;
        private SoundEffect WarpInSFX;
        private SoundEffectInstance WarpInSFXIns;

        public ESpriteType _spriteType = ESpriteType.BASIC;

        public Enemy(ContentManager getContent, string getTexture, int getWidth, int getHeight)
        {
            Content = getContent;
            DisplayScore = new Font(getContent);
            Sprite = new BasicSprite(getContent, getTexture, getWidth, getHeight);
            ExplosionAnim = new AnimSprite(Content, "graphics/Explosion2SS", getWidth * 2, getHeight * 2, 1, 13);
            ShockWaveAnim = new AnimSprite(Content, "graphics/shockwaveSS", getWidth * 3, getHeight * 3, 1, 13);

            WarpInEffect = new AnimSprite(Content, "graphics/warpinss", Sprite.Width * 4, Sprite.Height * 3, 1, 9);

            ExplosionSFX = Content.Load<SoundEffect>("sound/explosion");
            ExplosionSFXIns = ExplosionSFX.CreateInstance();
            ExplosionSFXIns.Volume = 0.1f;

            WarpInSFX = Content.Load<SoundEffect>("sound/warpin");
            WarpInSFXIns = WarpInSFX.CreateInstance();
            WarpInSFXIns.Volume = 0.05f;
            WarpInSFXIns.Pitch = 0.3f;
        }
        public Enemy(ContentManager getContent, string getTexture, int getWidth, int getHeight, int getRows, int getColumns)
        {
            Content = getContent;
            DisplayScore = new Font(getContent);
            SpriteAnim = new AnimSprite(getContent, getTexture, getWidth, getHeight, getRows, getColumns);
            ExplosionAnim = new AnimSprite(Content, "graphics/Explosion2SS", getWidth * 2, getHeight * 2, 1, 13);
            ShockWaveAnim = new AnimSprite(Content, "graphics/shockwaveSS", getWidth * 3, getHeight * 3, 1, 13);

            //WarpInEffect = new AnimSprite(Content, "graphics/warpinss", Sprite.Width * 2, Sprite.Height * 2, 1, 8);

            ExplosionSFX = Content.Load<SoundEffect>("sound/explosion");
            ExplosionSFXIns = ExplosionSFX.CreateInstance();
            ExplosionSFXIns.Volume = 0.1f;

            WarpInSFX = Content.Load<SoundEffect>("sound/warpin");
            WarpInSFXIns = WarpInSFX.CreateInstance();
            WarpInSFXIns.Volume = 0.05f;
            WarpInSFXIns.Pitch = 0.3f;
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

            if (Position.Y >= 800 || Position.Y <= -100 || Position.X >= 800 || Position.X <= -100)
            {
                isRemoved = true;
                isAlive = false;
            }
        }

        public void Draw(SpriteBatch sB)
        {
            if (isAlive)
            {
                if (isWarpIn)
                {
                    if (!WarpInEffect.AnimationFinnished)
                    {
                        WarpInSFXIns.Play();
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
                        ExplosionSFXIns.Play();
                        Speed = 1f;
                        ExplosionAnim.UpdateAnimation(0.5f);
                        ExplosionAnim.Draw(sB, Position);
                        ShockWaveAnim.UpdateAnimation(0.5f);
                        ShockWaveAnim.Draw(sB, Position);
                        DisplayScore.Draw(sB, Score.ToString(), Position, .5f, Color.White);
                    }
                }

            }
        }

    }
}
