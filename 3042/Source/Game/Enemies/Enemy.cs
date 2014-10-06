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
            Warp_CurvUpRight,
            Warp_JumpLeftRightDown,
            Warp_JumpRightLeftDown,
        };

        public enum EWeaponType
        {
            Basic,
            DoubleBarrel,
            BossWep
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
        private Player player;
        private float Rotation = 0;

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
                case EEnemyType.Asteroid: Asteroid_Type(); break;
                case EEnemyType.Warp_CurvLeft: Warp_CurvLeft_Type(); break;
                case EEnemyType.Warp_CurvRight: Warp_CurvRight_Type(); break;
                case EEnemyType.Warp_CurvUpLeft: Warp_CurvUpLeft_Type(); break;
                case EEnemyType.Warp_CurvUpRight: Warp_CurvUpRight_Type(); break;
                case EEnemyType.Warp_JumpLeftRightDown: Warp_JumpLeftRightDown_Type(); break;
                case EEnemyType.Warp_JumpRightLeftDown: Warp_JumpRightLeftDown_Type(); break;
            }

            if (EnemyType == EEnemyType.Asteroid)
            {
                Position += Direction * Speed;
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
                else if (WeaponType == EWeaponType.BossWep)
                {

                }
            }
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


            // Rotate to player code
            //Rotation = (float)Math.Atan2((double)player.Position.X - (double)Position.X, (double)player.Position.Y - (double)Position.Y);
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
                            Sprite.Draw(sB, Position, -Rotation);
                            CollisionBox = new Rectangle((int)Position.X - Sprite.Width / 2, (int)Position.Y - Sprite.Height / 2, Sprite.Width, Sprite.Height);
                        }; break;

                    case ESpriteType.ANIM:
                        {
                            SpriteAnim.Draw(sB, Position, -Rotation);
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
                        DisplayScore.Draw(sB, Score.ToString(), Position, .5f, Color.White);
                    }
                }

            }
        }

    }
}
