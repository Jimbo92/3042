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

        public enum EEnemyType
        {
            Asteroid,
            Warp_CurvLeft,
            Warp_CurvRight,
            Warp_CurvUpLeft,
        };

        public BasicSprite Sprite;
        public AnimSprite SpriteAnim;
        public Vector2 Position;
        public Vector2 Direction;
        public Vector2 GotoPos;
        public float Speed;
        public float Delay;
        public bool isAlive = true;
        public bool DebugMode = false;
        public Rectangle CollisionBox;
        public float Health;
        public float MaxHealth;
        public int Score;
        public bool isWarpIn = false;
        public List<Bullet> BulletList = new List<Bullet>();
        public bool HasWeapon = false;
        public int WeaponDamage;
        public Vector2 BulletDirection;

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
        private Rectangle ScreenSize;
        private int RandShootNum;

        //Timers
        private int[] MoveTimer = new int[3];

        public ESpriteType _spriteType = ESpriteType.BASIC;
        public EEnemyType EnemyType = EEnemyType.Asteroid;

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

            WarpInEffect = new AnimSprite(Content, "graphics/warpinss", SpriteAnim.Width * 2, SpriteAnim.Height * 2, 1, 8);

            ExplosionSFX = Content.Load<SoundEffect>("sound/explosion");
            ExplosionSFXIns = ExplosionSFX.CreateInstance();
            ExplosionSFXIns.Volume = 0.1f;

            WarpInSFX = Content.Load<SoundEffect>("sound/warpin");
            WarpInSFXIns = WarpInSFX.CreateInstance();
            WarpInSFXIns.Volume = 0.05f;
            WarpInSFXIns.Pitch = 0.3f;
        }

        public void Update(GUI getGUI, Rectangle getScreenSize)
        {
            ScreenSize = getScreenSize;
            CollisionBoxTexture = Content.Load<Texture2D>("graphics/collisionbox");

            if (_spriteType == ESpriteType.ANIM)
                SpriteAnim.UpdateAnimation(Delay);

            switch (EnemyType)
            {
                case EEnemyType.Asteroid: Asteroid_Type(); break;
                case EEnemyType.Warp_CurvLeft: Warp_CurvLeft_Type(); break;
                case EEnemyType.Warp_CurvRight: Warp_CurvRight_Type(); break;
                case EEnemyType.Warp_CurvUpLeft: Warp_CurvUpLeft_Type(); break;
            }

            if (HasWeapon)
                Weapon();

            if (Health <= 0)
            {
                isAlive = false;

                ScoreTimer++;
                if (ScoreTimer <= 1)
                    getGUI.Score += Score;
                else
                    ScoreTimer = 2;
            }

            if (Position.Y >= 800 || Position.Y <= -100 || Position.X >= 800 || Position.X <= -100 ||
                GameMode.Mode == GameMode.EGameMode.GAMEOVER || GameMode.Mode == GameMode.EGameMode.MENU)
            {
                isRemoved = true;
                isAlive = false;
            }
        }

        private void Weapon()
        {
            foreach (Bullet bullet in BulletList)
                bullet.Update();
            if (isAlive)
            {
                Random RandShoot = new Random();
                RandShootNum = RandShoot.Next(1, 50);

                if (RandShootNum == 15)
                {
                    Bullet bullet = new Bullet(Content, "graphics/ebullet", 5, 16);
                    bullet._spriteType = Bullet.ESpriteType.BASIC;
                    bullet.FirePosition = new Vector2(Position.X, Position.Y + 25);
                    bullet.Position = new Vector2(Position.X, Position.Y + 25);
                    bullet.Direction = BulletDirection;
                    bullet.Direction.Normalize();
                    bullet.Speed = 8f;
                    bullet.Damage = WeaponDamage;
                    BulletList.Add(bullet);
                    RandShootNum = 0;
                }
            }
        }

        private void Asteroid_Type()
        {
            Position += Direction * Speed;
            isWarpIn = false;
        }
        private void Warp_CurvLeft_Type()
        {
            isWarpIn = true;

            MoveTimer[0]++;
            if (MoveTimer[0] <= 100)
            {
                GotoPos.X -= 8;
                GotoPos.Y += 3;
            }
            else if (MoveTimer[0] >= 100 && MoveTimer[0] <= 300)
            {
                GotoPos.X += 8;
                GotoPos.Y += 3;
            }
            else
                MoveTimer[0] = 301;

            if (MoveTimer[0] == 301)
            {
                GotoPos.X = ScreenSize.X * 2;
            }

            Direction = GotoPos - Position;
            Speed = Direction.Length() * 0.01f;
            Direction.Normalize();
            Position += Direction * Speed;
        }
        private void Warp_CurvRight_Type()
        {
            isWarpIn = true;

            MoveTimer[0]++;
            if (MoveTimer[0] <= 100)
            {
                GotoPos.X += 8;
                GotoPos.Y += 3;
            }
            else if (MoveTimer[0] >= 100 && MoveTimer[0] <= 300)
            {
                GotoPos.X -= 8;
                GotoPos.Y += 3;
            }
            else
                MoveTimer[0] = 301;

            if (MoveTimer[0] == 301)
            {
                GotoPos.X = -ScreenSize.X;
            }

            Direction = GotoPos - Position;
            Speed = Direction.Length() * 0.01f;
            Direction.Normalize();
            Position += Direction * Speed;
        }
        private void Warp_CurvUpLeft_Type()
        {
            isWarpIn = true;

            MoveTimer[0]++;
            if (MoveTimer[0] <= 100)
            {
                GotoPos.X += 1;
                GotoPos.Y -= 5;
            }
            else if (MoveTimer[0] >= 100 && MoveTimer[0] <= 300)
            {
                GotoPos.X -= 8;
                GotoPos.Y += 1;
            }
            else
                MoveTimer[0] = 301;

            if (MoveTimer[0] == 301)
            {
                GotoPos.X = -ScreenSize.X;
            }

            Direction = GotoPos - Position;
            Speed = Direction.Length() * 0.01f;
            Direction.Normalize();
            Position += Direction * Speed;
        }
        private void Warp_Bot_Mid_Left_Top_Mid_Type()
        {
            isWarpIn = true;
            Position += Direction * Speed;
        }
        private void Warp_Top_Left_Bot_Right_Type()
        {
            isWarpIn = true;
            Position += Direction * Speed;
        }
        private void Warp_Top_Right_Bot_Left_Type()
        {
            isWarpIn = true;
            Position += Direction * Speed;
        }

        public void Draw(SpriteBatch sB)
        {
            foreach (Bullet bullet in BulletList)
                if (bullet.Position.Y <= 800)
                bullet.Draw(sB);

            if (isWarpIn)
            {
                if (!WarpInEffect.AnimationFinnished)
                {
                    WarpInSFXIns.Play();
                    WarpInEffect.UpdateAnimation(0.4f);
                    WarpInEffect.Draw(sB, new Vector2(Position.X + 15, Position.Y), MathHelper.ToRadians(270));
                }
            }

            if (isAlive)
            {
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
                        Speed = 1;
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
