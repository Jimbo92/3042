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

        //Varibles
        public Player player;
        public List<Enemy> EnemyList = new List<Enemy>();

        int timertest;


        public Level_Base(ContentManager getContent, Rectangle getScreenSize)
        {
            //Misc
            Content = getContent;

            player = new Player(getContent, getScreenSize);
        }

        public void Update(GameTime getGameTime)
        {
            player.Update(getGameTime);

            timertest++;
            if (timertest >= 50)
            {
                SpawnEnemy("graphics/asteroidss", 54, 54, 30, 1,
                    new Vector2(350, -100), new Vector2 (350, 800), 3f, 0.5f);

                timertest = 0;
            }

            foreach (Enemy enemy in EnemyList)
            {
                enemy.Update();
            }

            CollisionDetection();

        }

        public void SpawnEnemy(string getTexture, int getWidth, int getHeight, int geRows, int getColumns, Vector2 getPos, Vector2 getDir, float getSpeed, float getDelay)
        {
            Enemy enemy = new Enemy(Content, getTexture, getWidth, getHeight, geRows, getColumns);
            enemy.Delay = getDelay;
            enemy._spriteType = Enemy.ESpriteType.ANIM;
            enemy.Position = getPos;
            enemy.Direction = getDir - enemy.Position;
            enemy.Direction.Normalize();
            enemy.Speed = getSpeed;

            EnemyList.Add(enemy);
        }
        public void SpawnEnemy(string getTexture, int getWidth, int getHeight, Vector2 getPos, Vector2 getDir, float getSpeed)
        {
            Enemy enemy = new Enemy(Content, getTexture, getWidth, getHeight);
            enemy._spriteType = Enemy.ESpriteType.BASIC;
            enemy.Position = getPos;
            enemy.Direction = getDir - enemy.Position;
            enemy.Direction.Normalize();
            enemy.Speed = getSpeed;

            EnemyList.Add(enemy);
        }

        private void CollisionDetection()
        {
            foreach (Enemy enemy in EnemyList)
            {
                foreach (Bullet bullet in player.BulletList)
                {
                    if (CheckCollision.Collision(bullet.CollisionBox, enemy.CollisionBox))
                    {
                        enemy.isAlive = false;
                        bullet.isAlive = false;
                    }
                }
            }
        }

        public void Draw(SpriteBatch sB)
        {
            foreach (Enemy enemy in EnemyList)
            {
                enemy.Draw(sB);
            }

            player.Draw(sB);
        }


    }
}
