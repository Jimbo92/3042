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
    class Level1
    {
        public Level_Base BaseCode;
        public Font AsteroidWarning;

        private int GameTicks;
        private Rectangle ScreenSize;
        private int AsteroidWarningTimer;

        public Level1(ContentManager getContent, Rectangle getScreenSize)
        {
            ScreenSize = getScreenSize;

            BaseCode = new Level_Base(getContent, getScreenSize);
            //Code Bellow this

            AsteroidWarning = new Font(getContent);

        }

        public void Update(GameTime getGameTime)
        {
            BaseCode.Update(getGameTime);
            GameTicks = getGameTime.TotalGameTime.Seconds;
            //Code bellow this

            if (GameTicks >= 1 && GameTicks <= 20)
            {
                BaseCode.RandAsteroidWave(200);
            }
            if (GameTicks >= 23 && GameTicks <= 35)
            {
                BaseCode.RandAsteroidWave(10);
            }

            if (GameTicks >= 5 && GameTicks <= 10)
            {             
                BaseCode.SmallEnemyWave(50, new Vector2(50, 100), new Vector2(800, 200), 2f);
            }
            if (GameTicks >= 11 && GameTicks <= 15)
            {               
                BaseCode.SmallEnemyWave(100, new Vector2(600, 100), new Vector2(-100, 200), 2f);
            }

        }

        public void Draw(SpriteBatch sB)
        {
            BaseCode.Draw(sB);
            //Code Bellow this

            if (GameTicks >= 20 && GameTicks <= 20.5)
            {
                AsteroidWarningTimer++;
                if (AsteroidWarningTimer >= 3)
                {
                    AsteroidWarning.Draw(sB, "Incoming Asteroid Field!",
                        new Vector2(ScreenSize.X / 2, ScreenSize.Y / 3),
                        .5f,
                        Color.White);

                    AsteroidWarningTimer = 0;
                }
            }
        }


    }
}
