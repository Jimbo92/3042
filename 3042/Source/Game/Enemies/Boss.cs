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
    class Boss
    {
        public BasicSprite Sprite;
        public Vector2 Position = new Vector2(350, -200);
        public bool isAlive = false;
        public bool isEnabled = false;
        public Enemy[] Turret = new Enemy[3];
        public AnimSprite MainCannon;
        public AnimSprite MainCannonChargeUp;

        private AnimSprite[] BackBurners = new AnimSprite[4];
        private int MoveTimer;
        private int MainCannonTimer;
        private Rectangle ScreenSize;
        private Rectangle MainCannonCollisionBox;
        private Player player;
        private GUI gui;

        public Boss(ContentManager getContent)
        {
            Sprite = new BasicSprite(getContent, "graphics/boss", 700, 128);

            for (int i = 0; i < BackBurners.Length; i++)
                BackBurners[i] = new AnimSprite(getContent, "graphics/pbullet2", 128, 64, 2, 1);

            for (int i = 0; i < 2; i++)
            {
                Turret[i] = new Enemy(getContent, "graphics/bossturret2", 64, 96);
                Turret[i].MaxHealth = 1550;
                Turret[i].Health = Turret[i].MaxHealth;
                Turret[i].HasWeapon = true;
                Turret[i].WeaponType = Enemy.EWeaponType.BossWepSide;
            }

            Turret[2] = new Enemy(getContent, "graphics/bossturret1", 128, 128);
            Turret[2].MaxHealth = 3550;
            Turret[2].Health = Turret[2].MaxHealth;

            Turret[0]._spriteType = Enemy.ESpriteType.BASIC;
            Turret[1]._spriteType = Enemy.ESpriteType.BASIC;
            Turret[2]._spriteType = Enemy.ESpriteType.BASIC;

            MainCannon = new AnimSprite(getContent, "graphics/bossmaincannonss", 125, 650, 1, 3);
            MainCannonChargeUp = new AnimSprite(getContent, "graphics/plasmaball2SS", 128, 128, 4, 5);
        }

        public void Update(GUI getGui, Player getPlayer, Rectangle getScreenSize)
        {
            ScreenSize = getScreenSize;
            player = getPlayer;
            gui = getGui;

            if (GameMode.Mode == GameMode.EGameMode.GAMEOVER || GameMode.Mode == GameMode.EGameMode.MENU)
                Reset();

            if (isAlive)
            {
                Position.Y += 1f;
                if (Position.Y >= 60)
                {
                    Position.Y = 60;
                    isEnabled = true;

                    MoveTimer++;
                    if (MoveTimer <= 100)
                        Position.X += 0.1f;
                    else if (MoveTimer >= 100 && MoveTimer <= 200)
                        Position.X -= 0.1f;
                    else if (MoveTimer >= 200 && MoveTimer <= 300)
                        Position.X -= 0.1f;
                    else if (MoveTimer >= 300 && MoveTimer <= 400)
                        Position.X += 0.1f;
                    if (MoveTimer >= 400)
                        MoveTimer = 0;
                }

                Turret[0].Position = new Vector2(Position.X - 300, Position.Y);
                Turret[1].Position = new Vector2(Position.X + 300, Position.Y);
                Turret[2].Position = new Vector2(Position.X, Position.Y + 25);

                if (isEnabled)
                {
                    foreach (Enemy turret in Turret)
                        turret.Update(getGui, getScreenSize, getPlayer);


                    for (int i = 0; i < 2; i++)
                    {
                        Turret[i].BulletDirection = getPlayer.Position - Turret[i].Position;

                        if (Turret[2].Health <= Turret[2].MaxHealth / 2 || Turret[i].Health <= Turret[i].MaxHealth / 2)
                            Turret[i].WeaponType = Enemy.EWeaponType.BossWepAlt;
                    }

                    Turret[0].Rotation = -(float)Math.Atan2((double)getPlayer.Position.X - (double)Turret[0].Position.X, (double)getPlayer.Position.Y - (double)Turret[0].Position.Y);
                    Turret[1].Rotation = -(float)Math.Atan2((double)getPlayer.Position.X - (double)Turret[1].Position.X, (double)getPlayer.Position.Y - (double)Turret[1].Position.Y);
                }

                Collisions();
            }

        }

        public void Reset()
        {
            Position = new Vector2(350, -200);
            isEnabled = false;
            isAlive = false;
            foreach (Enemy turret in Turret)
            {
                turret.Health = turret.MaxHealth;
                turret.isAlive = true;
                turret.WeaponType = Enemy.EWeaponType.BossWepSide;

                foreach (Bullet bullet in turret.BulletList)
                {
                    bullet.isRemoved = true;
                    bullet.isAlive = false;
                }
            }
            MainCannonCollisionBox = Rectangle.Empty;
            MainCannonChargeUp.Width = 128;
            MainCannonChargeUp.Height = 128;
            MainCannonTimer = 0;
        }

        private void Collisions()
        {
            foreach (Bullet bullet in player.BulletList)
            {
                foreach (Enemy turret in Turret)
                {
                    if (CheckCollision.Collision(bullet.CollisionBox, turret.CollisionBox))
                    {
                        turret.Health -= bullet.Damage;
                        bullet.isAlive = false;
                    }
                }
            }
            foreach (Enemy turret in Turret)
            {
                foreach (Bullet bullet in turret.BulletList)
                {
                    if (CheckCollision.Collision(player.CollisionBox, bullet.CollisionBox))
                    {
                        if (!player.isImmune)
                            gui.PlayerHealth -= bullet.Damage;

                        bullet.isAlive = false;
                    }
                }
            }

            if (CheckCollision.Collision(MainCannonCollisionBox, player.CollisionBox))
            {
                if (!player.isImmune)
                    gui.PlayerHealth -= 50;
            }


        }

        public void Draw(SpriteBatch sB)
        {
            if (isAlive)
            {
                foreach (AnimSprite Burner in BackBurners)
                    Burner.UpdateAnimation(0.3f);
                BackBurners[0].Draw(sB, new Vector2(Position.X - 150, Position.Y + 65));
                BackBurners[1].Draw(sB, new Vector2(Position.X - 190, Position.Y + 55));
                BackBurners[2].Draw(sB, new Vector2(Position.X + 135, Position.Y + 65));
                BackBurners[3].Draw(sB, new Vector2(Position.X + 175, Position.Y + 55));

                Sprite.Draw(sB, Position);

                if (isEnabled)
                {
                    MainCannonTimer++;
                    if (MainCannonTimer <= 300)
                    {
                        MainCannonChargeUp.Width += 1;
                        MainCannonChargeUp.Height += 1;
                    }
                    else if (MainCannonTimer >= 600)
                    {
                        MainCannonChargeUp.Width = 128;
                        MainCannonChargeUp.Height = 128;
                    }

                    if (MainCannonTimer >= 300 && MainCannonTimer <= 600)
                    {
                        MainCannon.UpdateAnimation(0.4f);
                        MainCannon.Draw(sB, new Vector2(Turret[2].Position.X, Turret[2].Position.Y * 5));
                        MainCannonCollisionBox = new Rectangle((int)Turret[2].Position.X, (int)Turret[2].Position.Y * 2, 100, 650);
                    }
                    else if (MainCannonTimer >= 600)
                    {
                        MainCannonCollisionBox = Rectangle.Empty;
                        MainCannonTimer = 0;
                    }
                }

                MainCannonChargeUp.UpdateAnimation(0.4f);
                MainCannonChargeUp.Draw(sB, new Vector2(Turret[2].Position.X, Turret[2].Position.Y + 70));

                for (int i = 0; i < 2; i++)
                    Turret[i].Draw(sB);
                Turret[2].Draw(sB);


            }
        }
    }
}
