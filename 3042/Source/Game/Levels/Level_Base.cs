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

        private int AsteroidTimer;
        private int SmallEnemyTimer;
        private int RandItemDropNum;
        private Random RandItemDrop;
        private int ItemNextDropTimer;

        public Level_Base(ContentManager getContent, Rectangle getScreenSize)
        {
            //Misc
            Content = getContent;
            RandItemDrop = new Random();

            gui = new GUI(getContent, getScreenSize);

            player = new Player(getContent, getScreenSize);

            _BackGround = new Background(getContent);
        }

        public void Update(GameTime getGameTime)
        {
            if (Input.KeyboardPressed(Keys.Escape))
                GameMode.Mode = GameMode.EGameMode.MENU;

            player.Update(getGameTime, gui);

            gui.Update(player);

            _BackGround.Update();

            foreach (Enemy enemy in EnemyList)
            {
                enemy.Update(gui);
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

        public void SmallEnemyWave(int getDelay, Vector2 getPos, Vector2 getDir1, float getSpeed)
        {

            SmallEnemyTimer++;
            if (SmallEnemyTimer >= getDelay)
            {
                SpawnEnemy("graphics/enemysmall", 50, 48, 48,
                    getPos, getDir1, getSpeed, 100, true);

                SmallEnemyTimer = 0;
            }
        }

        public void RandAsteroidWave(int getDelay)
        {
            Random RandSize = new Random();
            Random RandStartXPos = new Random();
            Random RandEndXPos = new Random();
            Random RandSpeed = new Random();

            AsteroidTimer++;
            if (AsteroidTimer >= getDelay)
            {
                int RandSizeNum = RandSize.Next(35, 90);
                int RandStartXPosNum = RandSize.Next(20, 680);
                int RandEndXPosNum = RandSize.Next(20, 680);
                int RandSpeedNum = RandSize.Next(5, 10);

                SpawnEnemy("graphics/asteroidss", RandSizeNum, RandSizeNum, RandSizeNum, 30, 1,
                    new Vector2(RandStartXPosNum, -100), new Vector2(RandEndXPosNum, 800),
                    RandSpeedNum / 2, 0.3f, RandSizeNum, false);

                AsteroidTimer = 0;
            }
        }

        private void SpawnEnemy(string getTexture, float getHealth, int getWidth, int getHeight, int geRows, int getColumns, Vector2 getPos, Vector2 getDir, float getSpeed, float getDelay, int getScore, bool getWarpIn)
        {
            Enemy enemy = new Enemy(Content, getTexture, getWidth, getHeight, geRows, getColumns);
            enemy.Delay = getDelay;
            enemy._spriteType = Enemy.ESpriteType.ANIM;
            enemy.Position = getPos;
            enemy.Direction = getDir - enemy.Position;
            enemy.Direction.Normalize();
            enemy.Speed = getSpeed;
            enemy.MaxHealth = getHealth;
            enemy.Health = enemy.MaxHealth;
            enemy.Score = getScore;
            enemy.isWarpIn = getWarpIn;
            EnemyList.Add(enemy);
        }
        private void SpawnEnemy(string getTexture, float getHealth, int getWidth, int getHeight, Vector2 getPos, Vector2 getDir, float getSpeed, int getScore, bool getWarpIn)
        {
            Enemy enemy = new Enemy(Content, getTexture, getWidth, getHeight);
            enemy.isWarpIn = getWarpIn;
            enemy._spriteType = Enemy.ESpriteType.BASIC;
            enemy.Position = getPos;
            enemy.Direction = getDir - enemy.Position;
            enemy.Direction.Normalize();
            enemy.Speed = getSpeed;
            enemy.MaxHealth = getHealth;
            enemy.Health = enemy.MaxHealth;
            enemy.Score = getScore;
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
