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
        enum EControls
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
        enum EWeaponType
        {
            BASIC,
            ADVANCED,
            MAX
        };
        EMoveAnim _moveAnimation = EMoveAnim.STOP;
        EControls _controls = EControls.MOUSE;
        EWeaponType _weaponType = EWeaponType.MAX;

        //Varibles
        public AnimSprite Sprite;
        public AnimSprite BackBurnerEffect;
        public Vector2 BackBurnerEffectPos;
        public Vector2 Position;

        public List<Bullet> BulletList = new List<Bullet>();

        private Rectangle ScreenSize;
        private Vector2 Direction;
        private float Speed = 0;
        private float DeltaTime;
        private Vector2 GotoPos;
        private ContentManager Content;

        public Player(ContentManager getContent, Rectangle getScreenSize)
        {
            ScreenSize = getScreenSize;
            Content = getContent;

            GotoPos = new Vector2(ScreenSize.X / 2 + 1, ScreenSize.Y / 2);

            Sprite = new AnimSprite(getContent, "graphics/playerss", 48, 48, 1, 7);
            Sprite.currentFrame = 3;
            Position.X = getScreenSize.X / 2;
            Position.Y = getScreenSize.Y / 2;

            BackBurnerEffect = new AnimSprite(getContent, "graphics/backburnerss", 40, 32, 1, 5);

        }

        public void Update(GameTime getGameTime)
        {
            //Misc
            DeltaTime = (float)getGameTime.ElapsedGameTime.TotalMilliseconds / 12;

            BackBurnerEffect.UpdateAnimation();

            SetupAnimations();

            Weapon();

            switch (_controls)
            {
                case EControls.KEYBOARD: KeyboardControls(); break;
                case EControls.MOUSE:    MouseControls();    break;
            }

            //Player Movement
            Direction = GotoPos - Position;
            Speed = Direction.Length() * 0.1f;
            Direction.Normalize();
            Position += Direction * Speed;

            BackBurnerEffectPos.X = Position.X;
            BackBurnerEffectPos.Y = Position.Y + 30;

        }

        private void Weapon()
        {
            if (Input.KeyboardPressed(Keys.RightControl) && _controls == EControls.KEYBOARD ||
                Input.ClickPressed(Input.EClicks.LEFT) && _controls == EControls.MOUSE)
            {
                Shoot();
            }

            for (int i = 0; i < BulletList.Count; i++)
            {
                BulletList[i].Update(Content, "graphics/pbullet", 5, 32);
            }

        }

        public void Shoot()
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
            Bullet bullet = new Bullet();
            bullet.Position = new Vector2(Position.X, Position.Y - 50);
            bullet.Direction = new Vector2(Position.X, -100) - bullet.Position;
            bullet.Direction.Normalize();
            bullet.Speed = 20f;
            BulletList.Add(bullet);
        }
        private void ShootAdvanced()
        {
            Bullet bulletLeft = new Bullet();
            bulletLeft.Position = new Vector2(Position.X - 20, Position.Y - 25);
            bulletLeft.Direction = new Vector2(Position.X - 20, -100) - bulletLeft.Position;
            bulletLeft.Direction.Normalize();
            bulletLeft.Speed = 20f;
            BulletList.Add(bulletLeft);

            Bullet bulletRight = new Bullet();
            bulletRight.Position = new Vector2(Position.X + 20, Position.Y - 25);
            bulletRight.Direction = new Vector2(Position.X + 20, -100) - bulletRight.Position;
            bulletRight.Direction.Normalize();
            bulletRight.Speed = 20f;
            BulletList.Add(bulletRight);
        }
        private void ShootMax()
        {
            Bullet bulletLeft = new Bullet();
            bulletLeft.Position = new Vector2(Position.X, Position.Y - 25);
            bulletLeft.Direction = new Vector2(Position.X - 100, -100) - bulletLeft.Position;
            bulletLeft.Direction.Normalize();
            bulletLeft.Speed = 20f;
            BulletList.Add(bulletLeft);

            Bullet bulletRight = new Bullet();
            bulletRight.Position = new Vector2(Position.X, Position.Y - 25);
            bulletRight.Direction = new Vector2(Position.X + 100, -100) - bulletRight.Position;
            bulletRight.Direction.Normalize();
            bulletRight.Speed = 20f;
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

        public void Draw(SpriteBatch sB)
        {
            for (int i = 0; i < BulletList.Count; i++)
            {
                BulletList[i].Draw(sB);
            }

            BackBurnerEffect.Draw(sB, BackBurnerEffectPos);
            Sprite.Draw(sB, Position);
        }
    }
}
