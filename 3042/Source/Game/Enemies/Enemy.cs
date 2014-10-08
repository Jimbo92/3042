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
            Turret,
            Asteroid,
            Warp_CurvLeft,
            Warp_CurvRight,
            Warp_CurvUpLeft,
            Warp_CurvUpRight,
            Warp_JumpLeftRightDown,
            Warp_JumpRightLeftDown,
        };

        public enum EWeaponType
        {
            Basic,
            DoubleBarrel,
            BossWepSide,
            BossWepAlt,
            BossWepMain,
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
        public float Rotation = 0;

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
        private Player player;


        //Timers
        private int[] MoveTimer = new int[3];
        private int[] WeaponTimer = new int[5];

        public ESpriteType _spriteType = ESpriteType.BASIC;
        public EEnemyType EnemyType = EEnemyType.Asteroid;
        public EWeaponType WeaponType = EWeaponType.Basic;

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

            WarpInEffect = new AnimSprite(Content, "graphics/warpinss", SpriteAnim.Width * 4, SpriteAnim.Height * 3, 1, 9);

            ExplosionSFX = Content.Load<SoundEffect>("sound/explosion");
            ExplosionSFXIns = ExplosionSFX.CreateInstance();
            ExplosionSFXIns.Volume = 0.1f;

            WarpInSFX = Content.Load<SoundEffect>("sound/warpin");
            WarpInSFXIns = WarpInSFX.CreateInstance();
            WarpInSFXIns.Volume = 0.05f;
            WarpInSFXIns.Pitch = 0.3f;
        }

        public void Update(GUI getGUI, Rectangle getScreenSize, Player getPlayer)
        {
            ScreenSize = getScreenSize;
            player = getPlayer;
            CollisionBoxTexture = Content.Load<Texture2D>("graphics/collisionbox");

            if (_spriteType == ESpriteType.ANIM)
                SpriteAnim.UpdateAnimation(Delay);

            switch (EnemyType)
            {
                case EEnemyType.Turret: Turret_Type(); break;
                case EEnemyType.Asteroid: Asteroid_Type(); break;
                case EEnemyType.Warp_CurvLeft: Warp_CurvLeft_Type(); break;
                case EEnemyType.Warp_CurvRight: Warp_CurvRight_Type(); break;
                case EEnemyType.Warp_CurvUpLeft: Warp_CurvUpLeft_Type(); break;
                case EEnemyType.Warp_CurvUpRight: Warp_CurvUpRight_Type(); break;
                case EEnemyType.Warp_JumpLeftRightDown: Warp_JumpLeftRightDown_Type(); break;
                case EEnemyType.Warp_JumpRightLeftDown: Warp_JumpRightLeftDown_Type(); break;
            }

            if (EnemyType == EEnemyType.Asteroid || EnemyType == EEnemyType.Turret)
            {
                if (EnemyType == EEnemyType.Asteroid)
                    Position += Direction * Speed;
                if (EnemyType == EEnemyType.Turret)
                    Rotation = (float)Math.Atan2((double)player.Position.X - (double)Position.X, (double)player.Position.Y - (double)Position.Y);
            }
            else
            {
                Direction = GotoPos - Position;
                Speed = Direction.Length() * 0.01f;
                Direction.Normalize();
                Position += Direction * Speed;
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

            if (Position.Y >= 800 || Position.Y <= -300 || Position.X >= 800 || Position.X <= -100 ||
                GameMode.Mode == GameMode.EGameMode.GAMEOVER || GameMode.Mode == GameMode.EGameMode.MENU)
            {
                isRemoved = true;
                isAlive = false;
            }
        }

        private void Weapon()
        {
            foreach (Bullet bullet in BulletList)
            {
                bullet.Update();

                if (bullet.Position.Y >= 800 || bullet.Position.Y <= -100)
                {
                    bullet.isRemoved = true;
                    bullet.isAlive = false;
                }
            }

            if (isAlive)
            {
                if (WeaponType == EWeaponType.Basic)
                {
                    WeaponTimer[0]++;
                    if (WeaponTimer[0] <= 100)
                    {
                        WeaponTimer[1]++;
                        if (WeaponTimer[1] >= 25)
                        {
                            Bullet bullet = new Bullet(Content, Bullet.EBulletType.Enemy, "graphics/ebullet", 8, 16);
                            bullet._spriteType = Bullet.ESpriteType.BASIC;
                            bullet.FirePosition = new Vector2(Position.X, Position.Y + 25);
                            bullet.Position = bullet.FirePosition;
                            bullet.Direction = BulletDirection;
                            bullet.Direction.Normalize();
                            bullet.Speed = 8f;
                            bullet.Damage = WeaponDamage;
                            BulletList.Add(bullet);

                            WeaponTimer[1] = 0;
                        }
                    }
                    else if (WeaponTimer[0] >= 500)
                        WeaponTimer[0] = 0;
                }
                else if (WeaponType == EWeaponType.DoubleBarrel)
                {
                    WeaponTimer[0]++;
                    if (WeaponTimer[0] <= 100)
                    {
                        WeaponTimer[1]++;
                        WeaponTimer[2]++;
                        if (WeaponTimer[1] >= 15)
                        {
                            Bullet bullet = new Bullet(Content, Bullet.EBulletType.Enemy, "graphics/ebullet3", 10, 32, 2, 1);
                            bullet._spriteType = Bullet.ESpriteType.ANIM;
                            bullet.Delay = 0.2f;
                            bullet.FirePosition = new Vector2(Position.X + 16, Position.Y + 35);
                            bullet.Position = bullet.FirePosition;
                            bullet.Direction = BulletDirection;
                            bullet.Direction.Normalize();
                            bullet.Speed = 10f;
                            bullet.Damage = WeaponDamage;
                            BulletList.Add(bullet);

                            WeaponTimer[1] = 0;
                        }
                        if (WeaponTimer[2] >= 25)
                        {
                            Bullet bullet = new Bullet(Content, Bullet.EBulletType.Enemy, "graphics/ebullet3", 10, 32, 2, 1);
                            bullet._spriteType = Bullet.ESpriteType.ANIM;
                            bullet.Delay = 0.2f;
                            bullet.FirePosition = new Vector2(Position.X - 16, Position.Y + 35);
                            bullet.Position = bullet.FirePosition;
                            bullet.Direction = BulletDirection;
                            bullet.Direction.Normalize();
                            bullet.Speed = 10f;
                            bullet.Damage = WeaponDamage;
                            BulletList.Add(bullet);

                            WeaponTimer[2] = 0;
                        }
                    }
                    else if (WeaponTimer[0] >= 150)
                        WeaponTimer[0] = 0;
                }
                else if (WeaponType == EWeaponType.BossWepSide)
                {
                    WeaponTimer[0]++;
                    if (WeaponTimer[0] <= 100)
                    {
                        WeaponTimer[1]++;
                        if (WeaponTimer[1] >= 25)
                        {
                            Bullet bullet = new Bullet(Content, Bullet.EBulletType.Enemy, "graphics/ebullet", 10, 20);
                            bullet._spriteType = Bullet.ESpriteType.BASIC;
                            bullet.FirePosition = new Vector2(Position.X, Position.Y + 10);
                            bullet.Position = bullet.FirePosition;
                            bullet.Direction = BulletDirection;
                            bullet.Direction.Normalize();
                            bullet.Speed = 8f;
                            bullet.Damage = 10;
                            bullet.Rotation = -(float)Math.Atan2((double)player.Position.X - (double)bullet.Position.X, (double)player.Position.Y - (double)bullet.Position.Y);
                            BulletList.Add(bullet);

                            WeaponTimer[1] = 0;
                        }
                    }
                    else if (WeaponTimer[0] >= 300)
                        WeaponTimer[0] = 0;
                }
                else if (WeaponType == EWeaponType.BossWepAlt)
                {
                    WeaponTimer[0]++;
                    if (WeaponTimer[0] <= 100)
                    {
                        WeaponTimer[1]++;
                        if (WeaponTimer[1] >= 10)
                        {
                            Bullet bullet = new Bullet(Content, Bullet.EBulletType.Enemy, "graphics/ebullet2", 48, 48, 1, 4);
                            bullet._spriteType = Bullet.ESpriteType.ANIM;
                            bullet.Delay = 0.3f;
                            bullet.FirePosition = new Vector2(Position.X, Position.Y);
                            bullet.Position = bullet.FirePosition;
                            bullet.Direction = new Vector2(BulletDirection.X, BulletDirection.Y);
                            bullet.Direction.Normalize();
                            bullet.Speed = 4f;
                            bullet.Damage = 20;
                            BulletList.Add(bullet);

                            Bullet bullet2 = new Bullet(Content, Bullet.EBulletType.Enemy, "graphics/ebullet2", 32, 32, 1, 4);
                            bullet2._spriteType = Bullet.ESpriteType.ANIM;
                            bullet2.Delay = 0.3f;
                            bullet2.FirePosition = new Vector2(Position.X, Position.Y);
                            bullet2.Position = bullet2.FirePosition;
                            bullet2.Direction = new Vector2(BulletDirection.X - 50, BulletDirection.Y);
                            bullet2.Direction.Normalize();
                            bullet2.Speed = 4f;
                            bullet2.Damage = 15;
                            BulletList.Add(bullet2);

                            Bullet bullet3 = new Bullet(Content, Bullet.EBulletType.Enemy, "graphics/ebullet2", 32, 32, 1, 4);
                            bullet3._spriteType = Bullet.ESpriteType.ANIM;
                            bullet3.Delay = 0.3f;
                            bullet3.FirePosition = new Vector2(Position.X, Position.Y);
                            bullet3.Position = bullet3.FirePosition;
                            bullet3.Direction = new Vector2(BulletDirection.X + 50, BulletDirection.Y);
                            bullet3.Direction.Normalize();
                            bullet3.Speed = 4f;
                            bullet3.Damage = 15;
                            BulletList.Add(bullet3);

                            WeaponTimer[1] = 0;
                        }
                    }
                    else if (WeaponTimer[0] >= 150 && WeaponTimer[0] <= 250)
                    {
                        WeaponTimer[2]++;
                        if (WeaponTimer[2] >= 25)
                        {
                            Bullet bullet = new Bullet(Content, Bullet.EBulletType.Enemy, "graphics/ebullet", 10, 20);
                            bullet._spriteType = Bullet.ESpriteType.BASIC;
                            bullet.FirePosition = new Vector2(Position.X, Position.Y + 10);
                            bullet.Position = bullet.FirePosition;
                            bullet.Direction = BulletDirection;
                            bullet.Direction.Normalize();
                            bullet.Speed = 8f;
                            bullet.Damage = 10;
                            bullet.Rotation = -(float)Math.Atan2((double)player.Position.X - (double)bullet.Position.X, (double)player.Position.Y - (double)bullet.Position.Y);
                            BulletList.Add(bullet);

                            WeaponTimer[2] = 0;
                        }
                    }
                    else if (WeaponTimer[0] >= 300)
                        WeaponTimer[0] = 0;
                }
            }
        }

        private void Turret_Type()
        {
            // Rotate to player code
            Rotation = (float)Math.Atan2((double)player.Position.X - (double)Position.X, (double)player.Position.Y - (double)Position.Y);
        }
        private void Asteroid_Type()
        {
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
            {
                MoveTimer[0] = 301;
                GotoPos.X = ScreenSize.X * 2;
            }
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
            {
                MoveTimer[0] = 301;
                GotoPos.X = -ScreenSize.X;
            }

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
            {
                MoveTimer[0] = 301;
                GotoPos.X = -ScreenSize.X;
            }

        }
        private void Warp_CurvUpRight_Type()
        {
            isWarpIn = true;
            MoveTimer[0]++;
            if (MoveTimer[0] <= 100)
            {
                GotoPos.X -= 1;
                GotoPos.Y -= 5;
            }
            else if (MoveTimer[0] >= 100 && MoveTimer[0] <= 300)
            {
                GotoPos.X += 8;
                GotoPos.Y += 1;
            }
            else
            {
                MoveTimer[0] = 301;
                GotoPos.X = ScreenSize.X * 2;
            }

        }
        private void Warp_JumpLeftRightDown_Type()
        {
            isWarpIn = true;
            MoveTimer[0]++;
            if (MoveTimer[0] <= 150)
            {
                GotoPos = new Vector2(100, 100);
            }
            else if (MoveTimer[0] >= 150 && MoveTimer[0] <= 250)
            {
                GotoPos = new Vector2(600, 300);
            }
            else if (MoveTimer[0] >= 250 && MoveTimer[0] <= 350)
            {
                GotoPos = new Vector2(100, 500);
            }
            else if (MoveTimer[0] >= 350 && MoveTimer[0] <= 450)
            {
                GotoPos = new Vector2(600, 700);
            }
            else
            {
                MoveTimer[0] = 451;
                GotoPos.Y = ScreenSize.Y * 2;
            }
        }
        private void Warp_JumpRightLeftDown_Type()
        {
            isWarpIn = true;
            MoveTimer[0]++;
            if (MoveTimer[0] <= 150)
            {
                GotoPos = new Vector2(600, 100);
            }
            else if (MoveTimer[0] >= 150 && MoveTimer[0] <= 250)
            {
                GotoPos = new Vector2(100, 300);
            }
            else if (MoveTimer[0] >= 250 && MoveTimer[0] <= 350)
            {
                GotoPos = new Vector2(600, 500);
            }
            else if (MoveTimer[0] >= 350 && MoveTimer[0] <= 450)
            {
                GotoPos = new Vector2(100, 700);
            }
            else
            {
                MoveTimer[0] = 451;
                GotoPos.Y = ScreenSize.Y * 2;
            }
        }

        public void Draw(SpriteBatch sB)
        {
            foreach (Bullet bullet in BulletList)
                bullet.Draw(sB);

            if (isWarpIn)
            {
                if (!WarpInEffect.AnimationFinnished)
                {
                    WarpInSFXIns.Play();
                    WarpInEffect.UpdateAnimation(0.4f);
                    WarpInEffect.Draw(sB, new Vector2(Position.X, Position.Y), MathHelper.ToRadians(270));
                }
            }

            if (isAlive)
            {
                switch (_spriteType)
                {
                    case ESpriteType.BASIC:
                        {
                            Sprite.Draw(sB, Position, Rotation);
                            CollisionBox = new Rectangle((int)Position.X - Sprite.Width / 2, (int)Position.Y - Sprite.Height / 2, Sprite.Width, Sprite.Height);
                        }; break;

                    case ESpriteType.ANIM:
                        {
                            SpriteAnim.Draw(sB, Position, Rotation);
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
                        Speed = 0;
                        ExplosionAnim.UpdateAnimation(0.5f);
                        ExplosionAnim.Draw(sB, Position);
                        ShockWaveAnim.UpdateAnimation(0.5f);
                        ShockWaveAnim.Draw(sB, Position);
                        if (Score != 0)
                            DisplayScore.Draw(sB, Score.ToString(), Position, .5f, Color.White);
                    }
                }

            }
        }

    }
}
