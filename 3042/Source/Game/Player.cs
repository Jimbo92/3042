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
        public enum EControls
        {
            MOUSE,
            KEYBOARD
        };
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
        public EControls _controls = EControls.MOUSE;
        public EWeaponType _weaponType = EWeaponType.BASIC;

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

        private AnimSprite ShieldEffect;
        private Rectangle ScreenSize;
        private Vector2 Direction;
        private float Speed = 0;
        private float DeltaTime;
        private ContentManager Content;
        private int ShootTimer;

        public Player(ContentManager getContent, Rectangle getScreenSize)
        {
            ScreenSize = getScreenSize;
            Content = getContent;

            GotoPos = new Vector2(ScreenSize.X / 2 + 1, ScreenSize.Y - 50);

            Sprite = new AnimSprite(getContent, "graphics/playerss", 48, 48, 1, 7);
            Sprite.currentFrame = 3;
            Position.X = getScreenSize.X / 2;
            Position.Y = ScreenSize.Y - 50;

            BackBurnerEffect = new AnimSprite(getContent, "graphics/BackBurner2SS", 32, 32, 4, 1);
            ShieldEffect = new AnimSprite(getContent, "graphics/shieldss", Sprite.Width * 2, Sprite.Height * 2, 1, 8);

        }

        public void Update(GameTime getGameTime)
        {
            //Misc
            DeltaTime = (float)getGameTime.ElapsedGameTime.TotalMilliseconds / 12;

            if (isReset)
            {
                Reset();
            }

            if (isAlive)
            {
                SetupAnimations();

                if (ControlsEnabled)
                {
                    Weapon();
                    switch (_controls)
                    {
                        case EControls.KEYBOARD: KeyboardControls(); break;
                        case EControls.MOUSE: MouseControls(); break;
                    }
                }

                //Player Movement
                Direction = GotoPos - Position;
                Speed = Direction.Length() * 0.1f;
                Direction.Normalize();
                Position += Direction * Speed;

                //Backburner effect
                BackBurnerEffect.UpdateAnimation(0.5f);

                //Shield effect
                ShieldEffect.UpdateAnimation(0.5f);
                
            }

        }

        private void Weapon()
        {
            if (Input.KeyboardPressed(Keys.RightControl) && _controls == EControls.KEYBOARD ||
                Input.ClickPressed(Input.EClicks.LEFT) && _controls == EControls.MOUSE)
                Shoot();

            if (Input.KeyboardPress(Keys.RightControl) && _controls == EControls.KEYBOARD ||
                Input.ClickPress(Input.EClicks.LEFT) && _controls == EControls.MOUSE)
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

            for (int i = 0; i < BulletList.Count; i++)
            {
                BulletList[i].Update();
            }

        }
        private void Shoot()
        {
            switch (_weaponType)
            {
                case EWeaponType.BASIC: ShootBasic(); break;
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
            Bullet bullet = new Bullet(Content, "graphics/PBullet2", 32, 48, 2, 1);
            bullet._spriteType = Bullet.ESpriteType.ANIM;
            bullet.Delay = 0.3f;
            bullet.FirePosition = new Vector2(Position.X, Position.Y - 25);
            bullet.Position = new Vector2(Position.X, Position.Y - 50);
            bullet.Direction = new Vector2(Position.X, -100) - bullet.Position;
            bullet.Direction.Normalize();
            bullet.Speed = 15f;
            bullet.Damage = 20;
            BulletList.Add(bullet);
        }
        private void ShootAdvanced()
        {
            Bullet bulletLeft = new Bullet(Content, "graphics/PBullet2", 32, 48, 2, 1);
            bulletLeft._spriteType = Bullet.ESpriteType.ANIM;
            bulletLeft.Delay = 0.3f;
            bulletLeft.FirePosition = new Vector2(Position.X - 17, Position.Y - 10);
            bulletLeft.Position = new Vector2(Position.X - 20, Position.Y - 25);
            bulletLeft.Direction = new Vector2(Position.X - 20, -100) - bulletLeft.Position;
            bulletLeft.Direction.Normalize();
            bulletLeft.Speed = 15f;
            bulletLeft.Damage = 10;
            BulletList.Add(bulletLeft);

            Bullet bulletRight = new Bullet(Content, "graphics/PBullet2", 32, 48, 2, 1);
            bulletRight._spriteType = Bullet.ESpriteType.ANIM;
            bulletRight.Delay = 0.3f;
            bulletRight.FirePosition = new Vector2(Position.X + 17, Position.Y - 10);
            bulletRight.Position = new Vector2(Position.X + 20, Position.Y - 25);
            bulletRight.Direction = new Vector2(Position.X + 20, -100) - bulletRight.Position;
            bulletRight.Direction.Normalize();
            bulletRight.Speed = 15f;
            bulletRight.Damage = 10;
            BulletList.Add(bulletRight);
        }
        private void ShootMax()
        {
            Bullet bulletLeft = new Bullet(Content, "graphics/PBullet2", 32, 48, 2, 1);
            bulletLeft._spriteType = Bullet.ESpriteType.ANIM;
            bulletLeft.Delay = 0.3f;
            bulletLeft.FirePosition = new Vector2(Position.X - 5, Position.Y - 25);
            bulletLeft.Position = new Vector2(Position.X, Position.Y - 25);
            bulletLeft.Direction = new Vector2(Position.X - 100, -100) - bulletLeft.Position;
            bulletLeft.Direction.Normalize();
            bulletLeft.Speed = 18f;
            bulletLeft.Damage = 5;
            BulletList.Add(bulletLeft);

            Bullet bulletRight = new Bullet(Content, "graphics/PBullet2", 32, 48, 2, 1);
            bulletRight._spriteType = Bullet.ESpriteType.ANIM;
            bulletRight.Delay = 0.3f;
            bulletRight.FirePosition = new Vector2(Position.X + 5, Position.Y - 25);
            bulletRight.Position = new Vector2(Position.X, Position.Y - 25);
            bulletRight.Direction = new Vector2(Position.X + 100, -100) - bulletRight.Position;
            bulletRight.Direction.Normalize();
            bulletRight.Speed = 18f;
            bulletRight.Damage = 5;
            BulletList.Add(bulletRight);
        }

        private void KeyboardControls()
        {

            if (Input.KeyboardPress(Keys.Left))
            {
                GotoPos.X -= 10;
            }
             if (Input.KeyboardPress(Keys.Right))
            {
                GotoPos.X += 10;
            }
             if (Input.KeyboardPress(Keys.Up))
            {
                GotoPos.Y -= 10;
            }
             if (Input.KeyboardPress(Keys.Down))
            {
                GotoPos.Y += 5;
            }

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
                CollisionBox = Rectangle.Empty;
                for (int i = 0; i < BulletList.Count; i++)
                {
                    BulletList[i].CollisionBox = Rectangle.Empty;
                }
            }
        }
    }
}
