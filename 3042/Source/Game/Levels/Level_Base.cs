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
    class Level_Base
    {
        //Misc
        ContentManager Content;
        Background _BackGround;

        //Varibles
        public Player player;
        public List<Enemy> EnemyList = new List<Enemy>();
        public List<BasicItem> WepUpList = new List<BasicItem>();
        public List<BasicItem> OneUpList = new List<BasicItem>();
        public GUI gui;

        private int[] SpawnTimer = new int[8];
        private int RandItemDropNum;
        private Random RandItemDrop;
        private int ItemNextDropTimer;
        private Rectangle ScreenSize;

        public Level_Base(ContentManager getContent, Rectangle getScreenSize)
        {
            //Misc
            Content = getContent;
            RandItemDrop = new Random();

            gui = new GUI(getContent, getScreenSize);

            player = new Player(getContent, getScreenSize);

            _BackGround = new Background(getContent);
        }

        public void Update(GameTime getGameTime, Rectangle getScreenSize)
        {
            if (Input.KeyboardPressed(Keys.Escape))
                GameMode.Mode = GameMode.EGameMode.MENU;

            ScreenSize = getScreenSize;

            player.Update(getGameTime, gui);

            gui.Update(player);

            _BackGround.Update();

            foreach (Enemy enemy in EnemyList)
            {
                enemy.Update(gui, getScreenSize);
            }

            CollisionDetection();

            ItemNextDropTimer++;
        }

        public void WeaponUpgradeItem(Vector2 getPosition)
        {
            BasicItem WepUp = new BasicItem(Content, "graphics/wepupss", 48, 32, 1, 6);
            WepUp.Position = getPosition;
            WepUpList.Add(WepUp);
        }
        public void LifeItem(Vector2 getPosition)
        {
            BasicItem OneUp = new BasicItem(Content, "graphics/oneupss", 48, 32, 1, 6);
            OneUp.Position = getPosition;
            OneUpList.Add(OneUp);
        }

        public void RandAsteroidWave(int getDelay)
        {
            Random RandSize = new Random();
            Random RandStartXPos = new Random();
            Random RandEndXPos = new Random();
            Random RandSpeed = new Random();

            SpawnTimer[0]++;
            if (SpawnTimer[0] >= getDelay)
            {
                int RandSizeNum = RandSize.Next(35, 90);
                int RandStartXPosNum = RandSize.Next(20, 680);
                int RandEndXPosNum = RandSize.Next(20, 680);
                int RandSpeedNum = RandSize.Next(5, 10);

                Asteroid(RandSizeNum / 2, RandSizeNum, RandSizeNum,
                    new Vector2(RandStartXPosNum, -100), new Vector2(RandEndXPosNum, 800),
                    RandSpeedNum / 2, RandSizeNum);

                SpawnTimer[0] = 0;
            }
        }
        private void Asteroid(float getHealth, int getWidth, int getHeight, Vector2 getPos, Vector2 getDir, float getSpeed, int getScore)
        {
            Enemy enemy = new Enemy(Content, "graphics/asteroidss", getWidth, getHeight, 30, 1);
            enemy.Delay = 0.3f;
            enemy._spriteType = Enemy.ESpriteType.ANIM;
            enemy.Position = getPos;
            enemy.Direction = getDir - enemy.Position;
            enemy.Direction.Normalize();
            enemy.Speed = getSpeed;
            enemy.MaxHealth = getHealth;
            enemy.Health = enemy.MaxHealth;
            enemy.Score = getScore;
            enemy.EnemyType = Enemy.EEnemyType.Asteroid;
            EnemyList.Add(enemy);
        }

        public void SmallEnemyCurvLeftWave(int getDelay)
        {
            SpawnTimer[1]++;
            if (SpawnTimer[1] >= getDelay)
            {
                SpawnEnemyCurvLeft("graphics/enemysmall", 25, 48, 48, 100);
                SpawnTimer[1] = 0;
            }
        }
        private void SpawnEnemyCurvLeft(string getTexture, float getHealth, int getWidth, int getHeight, int getScore)
        {
            Enemy enemy = new Enemy(Content, getTexture, getWidth, getHeight);
            enemy._spriteType = Enemy.ESpriteType.BASIC;
            enemy.EnemyType = Enemy.EEnemyType.Warp_CurvLeft;
            enemy.Position = new Vector2(ScreenSize.X - 50, 50);
            enemy.GotoPos = enemy.Position;
            enemy.MaxHealth = getHealth;
            enemy.Health = enemy.MaxHealth;
            enemy.Score = getScore;
            enemy.WeaponDamage = 10;
            enemy.BulletDirection = new Vector2(enemy.Position.X, 800) - enemy.Position;

            Random RandWep = new Random();
            int RandWepNum = RandWep.Next(3);
            if (RandWepNum == 0) 
                enemy.HasWeapon = true;

            EnemyList.Add(enemy);
        }

        public void SmallEnemyCurvRightWave(int getDelay)
        {
            SpawnTimer[2]++;
            if (SpawnTimer[2] >= getDelay)
            {
                SpawnEnemyCurvRight("graphics/enemysmall", 25, 48, 48, 100);
                SpawnTimer[2] = 0;
            }
        }
        private void SpawnEnemyCurvRight(string getTexture, float getHealth, int getWidth, int getHeight, int getScore)
        {
            Enemy enemy = new Enemy(Content, getTexture, getWidth, getHeight);
            enemy._spriteType = Enemy.ESpriteType.BASIC;
            enemy.EnemyType = Enemy.EEnemyType.Warp_CurvRight;
            enemy.Position = new Vector2(50, 50);
            enemy.GotoPos = enemy.Position;
            enemy.MaxHealth = getHealth;
            enemy.Health = enemy.MaxHealth;
            enemy.Score = getScore;
            enemy.WeaponDamage = 10;
            enemy.BulletDirection = new Vector2(enemy.Position.X, 800) - enemy.Position;

            Random RandWep = new Random();
            int RandWepNum = RandWep.Next(3);
            if (RandWepNum == 0)
                enemy.HasWeapon = true;

            EnemyList.Add(enemy);
        }

        public void SmallEnemyCurvUpLeftWave(int getDelay)
        {
            SpawnTimer[3]++;
            if (SpawnTimer[3] >= getDelay)
            {
                SpawnEnemyCurvUpLeftWave("graphics/enemysmall", 25, 48, 48, 100);
                SpawnTimer[3] = 0;
            }
        }
        private void SpawnEnemyCurvUpLeftWave(string getTexture, float getHealth, int getWidth, int getHeight, int getScore)
        {
            Enemy enemy = new Enemy(Content, getTexture, getWidth, getHeight);
            enemy._spriteType = Enemy.ESpriteType.BASIC;
            enemy.EnemyType = Enemy.EEnemyType.Warp_CurvUpLeft;
            enemy.Position = new Vector2(500, 400);
            enemy.GotoPos = enemy.Position;
            enemy.MaxHealth = getHealth;
            enemy.Health = enemy.MaxHealth;
            enemy.Score = getScore;
            enemy.WeaponDamage = 10;
            enemy.BulletDirection = new Vector2(enemy.Position.X, 800) - enemy.Position;

            Random RandWep = new Random();
            int RandWepNum = RandWep.Next(3);
            if (RandWepNum == 0)
                enemy.HasWeapon = true;

            EnemyList.Add(enemy);
        }

        private void CollisionDetection()
        {
            foreach (Enemy enemy in EnemyList)
            {
                if (CheckCollision.Collision(enemy.CollisionBox, player.CollisionBox))
                {
                    enemy.Health = 0;

                    if (!player.isImmune)
                    {
                        player.isAlive = false;
                        gui.PlayerHealth = 0;
                    }
                }

                if (CheckCollision.Collision(player.SecondaryFireRect, enemy.CollisionBox))
                {
                    enemy.Health = 0;
                }
                foreach (Bullet bullet in enemy.BulletList)
                {
                    if (CheckCollision.Collision(player.CollisionBox, bullet.CollisionBox))
                    {
                        if (!player.isImmune)
                            gui.PlayerHealth -= bullet.Damage;

                        bullet.isAlive = false;
                    }
                }

                foreach (Bullet bullet in player.BulletList)
                {
                    if (CheckCollision.Collision(bullet.CollisionBox, enemy.CollisionBox))
                    {
                        enemy.Health -= bullet.Damage;
                        bullet.isAlive = false;

                        if (enemy.Health <= 0 && ItemNextDropTimer >= 500)
                        {
                            RandItemDropNum = RandItemDrop.Next(25);

                            if (RandItemDropNum == 1)
                            {
                                ItemNextDropTimer = 0;
                                WeaponUpgradeItem(enemy.Position);
                            }
                            if (RandItemDropNum == 3)
                            {
                                ItemNextDropTimer = 0;
                                LifeItem(enemy.Position);
                            }
                        }
                    }
                }
            }
            foreach (BasicItem OneUp in OneUpList)
            {
                if (CheckCollision.Collision(player.CollisionBox, OneUp.CollisionBox))
                {
                    OneUp.isAlive = false;
                    if (gui.PlayerLives != 3)
                    {
                        gui.PlayerLives++;
                    }
                }
            }

            foreach (BasicItem WepUp in WepUpList)
            {
                if (CheckCollision.Collision(player.CollisionBox, WepUp.CollisionBox))
                {
                    WepUp.isAlive = false;

                    if (player._weaponType == Player.EWeaponType.BASIC)
                        player._weaponType = Player.EWeaponType.ADVANCED;
                    else if (player._weaponType == Player.EWeaponType.ADVANCED)
                        player._weaponType = Player.EWeaponType.MAX;
                }
            }
        }

        public void Draw(SpriteBatch sB)
        {
            _BackGround.Draw(sB);
            //Draw Background first

            foreach (Enemy enemy in EnemyList)
            {
                enemy.Draw(sB);
            }

            player.Draw(sB);

            foreach (BasicItem WepUp in WepUpList)
            {
                WepUp.Draw(sB);
            }
            foreach (BasicItem OneUp in OneUpList)
            {
                OneUp.Draw(sB);
            }
            //Draw GUI last
            gui.Draw(sB);
        }


    }
}
