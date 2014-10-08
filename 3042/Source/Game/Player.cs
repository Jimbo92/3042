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
    class Player
    {
        //Enums
        enum EMoveAnim
        {
            STOP,
            LEFT,
            RIGHT
        };
        public enum EWeaponType
        {
            BASIC,
            ADVANCED,
            MAX
        };
        EMoveAnim _moveAnimation = EMoveAnim.STOP;
        public EWeaponType _weaponType = EWeaponType.BASIC;

        Texture2D CollisionBoxTexture;

        //Varibles
        public AnimSprite Sprite;
        public AnimSprite BackBurnerEffect;
        public Vector2 Position;
        public Vector2 GotoPos;
        public Rectangle CollisionBox;
        public List<Bullet> BulletList = new List<Bullet>();
        public bool isAlive = true;
        public bool isImmune = true;
        public bool ControlsEnabled = true;
        public bool isReset = true;
        public int ImmuneTimer;
        public AnimSprite SecondaryFireAnim;
        public AnimSprite SecondaryFireChargeUpAnim;
        public Rectangle SecondaryFireRect;
        public bool isAltFire;

        private Random RandShootSFX = new Random();
        private SoundEffect[] PlayerShootSFX = new SoundEffect[2];
        private SoundEffectInstance[] PlayerShootSFXIns = new SoundEffectInstance[2];
        private SoundEffect[] PlayerAltSFX = new SoundEffect[2];
        private SoundEffectInstance[] PlayerAltSFXIns = new SoundEffectInstance[2];
        private AnimSprite ShieldEffect;
        private Rectangle ScreenSize;
        private Vector2 Direction;
        private float Speed = 0;
        private float DeltaTime;
        private ContentManager Content;
        private int ShootTimer;
        private AnimSprite Cursor;
        private bool SecondaryFireShot;
        private GUI gui;

        public Player(ContentManager getContent, Rectangle getScreenSize)
        {
            ScreenSize = getScreenSize;
            Content = getContent;

            GotoPos = new Vector2(ScreenSize.X / 2 + 1, ScreenSize.Y - 50);

            Sprite = new AnimSprite(getContent, "graphics/playerss", 48, 48, 1, 7);
            Sprite.currentFrame = 3;
            Position.X = getScreenSize.X / 2;
            Position.Y = ScreenSize.Y - 50;

            SecondaryFireAnim = new AnimSprite(getContent, "graphics/Secondfiress", ScreenSize.X / 2, ScreenSize.Y, 1, 8);
            SecondaryFireChargeUpAnim = new AnimSprite(getContent, "graphics/plasmaball2ss", 164, 164, 4, 5);

            CollisionBoxTexture = Content.Load<Texture2D>("graphics/collisionbox");

            BackBurnerEffect = new AnimSprite(getContent, "graphics/BackBurner2SS", 32, 32, 4, 1);
            ShieldEffect = new AnimSprite(getContent, "graphics/shieldss", Sprite.Width * 2, Sprite.Height * 2, 1, 8);
            Cursor = new AnimSprite(getContent, "graphics/cursor", 50, 35, 1, 3);

            PlayerShootSFX[0] = Content.Load<SoundEffect>("sound/playershoot4");
            PlayerShootSFX[1] = Content.Load<SoundEffect>("sound/playershoot5");
            PlayerAltSFX[0] = Content.Load<SoundEffect>("sound/AltChargeUp");
            PlayerAltSFX[1] = Content.Load<SoundEffect>("sound/AltFire");
            for (int i = 0; i < 2; i++)
            {
                PlayerAltSFXIns[i] = PlayerAltSFX[i].CreateInstance();
                PlayerAltSFXIns[i].Volume = 0.1f;
            }
            PlayerAltSFXIns[1].Pitch = 0.5f;
            
        }

        public void Update(GameTime getGameTime, GUI getGUI)
        {
            //Misc
            DeltaTime = (float)getGameTime.ElapsedGameTime.TotalMilliseconds / 12;
            gui = getGUI;

            if (isReset)
            {
                Reset();
            }

            if (BulletList.Count >= 100)
                BulletList.RemoveAt(0);

            if (isAlive)
            {
                SetupAnimations();

                if (ControlsEnabled)
                {
                    Weapon();
                    switch (GameMode.Controls)
                    {
                        case GameMode.EControls.Keyboard: KeyboardControls(); break;
                        case GameMode.EControls.Mouse: MouseControls(); break;
                    }
                }

                //Weapon Level Text
                switch (_weaponType)
                {
                    case EWeaponType.BASIC:     gui.WepLvl = "Basic"; break;
                    case EWeaponType.ADVANCED:  gui.WepLvl = "Adv"; break;
                    case EWeaponType.MAX:       gui.WepLvl = "Max"; break;
                }

                //Player Movement
                Direction = GotoPos - Position;
                Speed = Direction.Length() * 0.13f;
                Direction.Normalize();
                Position += Direction * Speed;

                //Backburner effect
                BackBurnerEffect.UpdateAnimation(0.5f);

                //Shield effect
                ShieldEffect.UpdateAnimation(0.5f);

                //Cursor
                Cursor.currentFrame = 0;
                
            }

        }

        private void Weapon()
        {
            if (Input.KeyboardPressed(Keys.RightControl) && GameMode.Controls == GameMode.EControls.Keyboard ||
                Input.ClickPressed(Input.EClicks.LEFT) && GameMode.Controls == GameMode.EControls.Mouse)
                Shoot();

            if (Input.KeyboardPress(Keys.RightControl) && GameMode.Controls == GameMode.EControls.Keyboard ||
                Input.ClickPress(Input.EClicks.LEFT) && GameMode.Controls == GameMode.EControls.Mouse)
            {
                ShootTimer++;
                if (ShootTimer >= 10)
                {
                    Shoot();
                    ShootTimer = 0;
                }
            }
            else
                ShootTimer = 0;

            if (Input.KeyboardPressed(Keys.RightAlt) && GameMode.Controls == GameMode.EControls.Keyboard ||
                Input.ClickPressed(Input.EClicks.RIGHT) && GameMode.Controls == GameMode.EControls.Mouse)
                if (isAltFire)
                SecondaryFireShot = true;

            for (int i = 0; i < BulletList.Count; i++)
            {
                BulletList[i].Update();
            }

        }
        private void Shoot()
        {
            for (int i = 0; i < 2; i++)
            {
                PlayerShootSFXIns[i] = PlayerShootSFX[i].CreateInstance();
                PlayerShootSFXIns[i].Volume = 1f;
            }


            int RandShootSFXNum = RandShootSFX.Next(2);

            switch (RandShootSFXNum)
            {
                case 0: PlayerShootSFXIns[0].Play(); break;
                case 1: PlayerShootSFXIns[1].Play(); break;
            }

            switch (_weaponType)
            {
                case EWeaponType.BASIC:
                    {
                        ShootBasic();
                    }; break;
                case EWeaponType.ADVANCED:
                    {
                        ShootBasic();
                        ShootAdvanced();
                    }; break;
                case EWeaponType.MAX:
                    {
                        ShootBasic();
                        ShootAdvanced();
                        ShootMax();
                    }; break;
            }
        }
        private void ShootBasic()
        {
            Bullet bullet = new Bullet(Content, Bullet.EBulletType.Player, "graphics/PBullet2", 42, 58, 2, 1);
            bullet._spriteType = Bullet.ESpriteType.ANIM;
            bullet.Delay = 0.3f;
            bullet.FirePosition = new Vector2(Position.X, Position.Y - 25);
            bullet.Position = new Vector2(Position.X, Position.Y - 50);
            bullet.Direction = new Vector2(Position.X, -100) - bullet.Position;
            bullet.Direction.Normalize();
            bullet.Speed = 15f;
            bullet.Damage = 30;
            BulletList.Add(bullet);
        }
        private void ShootAdvanced()
        {
            Bullet bulletLeft = new Bullet(Content, Bullet.EBulletType.Player, "graphics/PBullet2", 32, 48, 2, 1);
            bulletLeft._spriteType = Bullet.ESpriteType.ANIM;
            bulletLeft.Delay = 0.3f;
            bulletLeft.FirePosition = new Vector2(Position.X - 17, Position.Y - 10);
            bulletLeft.Position = new Vector2(Position.X - 20, Position.Y - 25);
            bulletLeft.Direction = new Vector2(Position.X - 20, -100) - bulletLeft.Position;
            bulletLeft.Direction.Normalize();
            bulletLeft.Speed = 15f;
            bulletLeft.Damage = 5;
            BulletList.Add(bulletLeft);

            Bullet bulletRight = new Bullet(Content, Bullet.EBulletType.Player, "graphics/PBullet2", 32, 48, 2, 1);
            bulletRight._spriteType = Bullet.ESpriteType.ANIM;
            bulletRight.Delay = 0.3f;
            bulletRight.FirePosition = new Vector2(Position.X + 17, Position.Y - 10);
            bulletRight.Position = new Vector2(Position.X + 20, Position.Y - 25);
            bulletRight.Direction = new Vector2(Position.X + 20, -100) - bulletRight.Position;
            bulletRight.Direction.Normalize();
            bulletRight.Speed = 15f;
            bulletRight.Damage = 5;
            BulletList.Add(bulletRight);
        }
        private void ShootMax()
        {
            Bullet bulletLeft = new Bullet(Content, Bullet.EBulletType.Player, "graphics/PBullet2", 22, 38, 2, 1);
            bulletLeft._spriteType = Bullet.ESpriteType.ANIM;
            bulletLeft.Delay = 0.3f;
            bulletLeft.FirePosition = new Vector2(Position.X - 5, Position.Y - 25);
            bulletLeft.Position = new Vector2(Position.X, Position.Y - 25);
            bulletLeft.Direction = new Vector2(Position.X - 100, -100) - bulletLeft.Position;
            bulletLeft.Direction.Normalize();
            bulletLeft.Speed = 18f;
            bulletLeft.Damage = 2;
            BulletList.Add(bulletLeft);

            Bullet bulletRight = new Bullet(Content, Bullet.EBulletType.Player, "graphics/PBullet2", 22, 38, 2, 1);
            bulletRight._spriteType = Bullet.ESpriteType.ANIM;
            bulletRight.Delay = 0.3f;
            bulletRight.FirePosition = new Vector2(Position.X + 5, Position.Y - 25);
            bulletRight.Position = new Vector2(Position.X, Position.Y - 25);
            bulletRight.Direction = new Vector2(Position.X + 100, -100) - bulletRight.Position;
            bulletRight.Direction.Normalize();
            bulletRight.Speed = 18f;
            bulletRight.Damage = 2;
            BulletList.Add(bulletRight);
        }

        private void KeyboardControls()
        {

            if (Input.KeyboardPress(Keys.Left))
                GotoPos.X -= 10;
            else if (Input.KeyboardPress(Keys.Right))
                GotoPos.X += 10;
            if (Input.KeyboardPress(Keys.Up))
                GotoPos.Y -= 10;
            else if (Input.KeyboardPress(Keys.Down))
                GotoPos.Y += 10;

            //Check if outside viewport
             if (GotoPos.X <= 0)
                 GotoPos.X += 10;
             if (GotoPos.X >= ScreenSize.X)
                 GotoPos.X -= 10;
             if (GotoPos.Y <= 0)
                 GotoPos.Y += 10;
             if (GotoPos.Y >= ScreenSize.Y)
                 GotoPos.Y -= 10;

             if (Input.KeyboardPress(Keys.Left))
                 _moveAnimation = EMoveAnim.LEFT;

             else if (Input.KeyboardPress(Keys.Right))
                 _moveAnimation = EMoveAnim.RIGHT;

             else
                 _moveAnimation = EMoveAnim.STOP;
        }
        private void MouseControls()
        {
            GotoPos.X = Mouse.GetState().X;
            GotoPos.Y = Mouse.GetState().Y;

            if (GotoPos.X < Position.X - 1)
                _moveAnimation = EMoveAnim.LEFT;

            else if (GotoPos.X > Position.X + 1)
                _moveAnimation = EMoveAnim.RIGHT;

            else
                _moveAnimation = EMoveAnim.STOP;
        }

        private void SetupAnimations()
        {
            switch (_moveAnimation)
            {
                case EMoveAnim.STOP:
                    {
                        if (Sprite.currentFrame <= 6 && Sprite.currentFrame >= 3)
                        {
                            Sprite.currentFrame--;
                            if (Sprite.currentFrame <= 3)
                                Sprite.currentFrame = 3;
                        }
                        else if (Sprite.currentFrame >= 0 && Sprite.currentFrame <= 3)
                        {
                            Sprite.currentFrame++;
                            if (Sprite.currentFrame >= 3)
                                Sprite.currentFrame = 3;
                        }

                    }; break;

                case EMoveAnim.LEFT: 
                    {
                        Sprite.currentFrame--;
                        if (Sprite.currentFrame <= 0)
                            Sprite.currentFrame = 0;
                    
                    }; break;

                case EMoveAnim.RIGHT:
                    {
                        Sprite.currentFrame++;
                        if (Sprite.currentFrame >= 6)
                            Sprite.currentFrame = 6;

                    }; break;
            }

        }

        private void Reset()
        {
            Position = new Vector2(ScreenSize.X / 2, ScreenSize.Y - 100);
            GotoPos = new Vector2(ScreenSize.X / 2, ScreenSize.Y - 101);
            isAlive = true;
            isImmune = true;
            ControlsEnabled = true;
            isReset = false;
        }

        public void Draw(SpriteBatch sB)
        {
            if (isAlive)
            {
                for (int i = 0; i < BulletList.Count; i++)
                {
                    BulletList[i].Draw(sB);
                }

                if (SecondaryFireShot)
                {
                    gui.AltBarAmount = 0;

                    if (!SecondaryFireChargeUpAnim.AnimationFinnished)
                    {
                        PlayerAltSFXIns[0].Play();
                        SecondaryFireChargeUpAnim.Height += 3;
                        SecondaryFireChargeUpAnim.Width += 3;
                        SecondaryFireChargeUpAnim.UpdateAnimation(0.5f);
                        SecondaryFireChargeUpAnim.Draw(sB, new Vector2(Position.X, Position.Y - 20));
                    }
                    else
                    {
                        if (!SecondaryFireAnim.AnimationFinnished)
                        {
                            PlayerAltSFXIns[1].Play();
                            SecondaryFireRect = new Rectangle((int)Position.X - SecondaryFireAnim.Width / 6, (int)Position.Y - SecondaryFireAnim.Height, ScreenSize.Width / 6, ScreenSize.Height);
                            SecondaryFireAnim.UpdateAnimation(0.5f);
                            SecondaryFireAnim.Draw(sB, new Vector2((int)Position.X, (int)Position.Y - SecondaryFireAnim.Height / 2));
                            //sB.Draw(CollisionBoxTexture, SecondaryFireRect, Color.White);
                        }
                        else
                        {
                            SecondaryFireRect = new Rectangle(-100, -100, 0, 0);
                            SecondaryFireShot = false;
                            SecondaryFireAnim.AnimationFinnished = false;
                            SecondaryFireChargeUpAnim.AnimationFinnished = false;
                            SecondaryFireChargeUpAnim.Height = 164;
                            SecondaryFireChargeUpAnim.Width = 164;
                        }
                    }
                }

                BackBurnerEffect.Draw(sB, new Vector2(Position.X, Position.Y + 22));
                Sprite.Draw(sB, Position);

                if (isImmune)
                {
                    ImmuneTimer++;
                    if (ImmuneTimer <= 300)
                    {
                        ShieldEffect.Draw(sB, Position, MathHelper.ToRadians(180));
                    }
                    else
                    {
                        ImmuneTimer = 301;
                        isImmune = false;
                    }
                }

                CollisionBox = new Rectangle((int)Position.X - Sprite.Width / 4, (int)Position.Y - Sprite.Height / 2, Sprite.Width / 2, Sprite.Height);
            }
            else
            {
                CollisionBox = new Rectangle(-100, -100, 0, 0);
                for (int i = 0; i < BulletList.Count; i++)
                {
                    BulletList[i].CollisionBox = new Rectangle(-100, -100, 0, 0);
                }
            }

            if (GameMode.Controls == GameMode.EControls.Mouse)
                Cursor.Draw(sB, new Vector2(GotoPos.X - 5, GotoPos.Y + 5));
        }
    }
}
