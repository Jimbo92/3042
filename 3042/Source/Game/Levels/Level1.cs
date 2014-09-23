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

        private int GameTicks;

        public Level1(ContentManager getContent, Rectangle getScreenSize)
        {
            BaseCode = new Level_Base(getContent, getScreenSize);
            //Code Bellow this


        }

        public void Update(GameTime getGameTime)
        {
            BaseCode.Update(getGameTime);
            GameTicks = getGameTime.TotalGameTime.Seconds;
            //Code bellow this


            if (GameTicks >= 5 && GameTicks <= 10)
            {
                BaseCode.RandAsteroidWave(200);
                BaseCode.SmallEnemyWave(50, new Vector2(50, 100), new Vector2(800, 200), 2f);
            }
            if (GameTicks >= 8 && GameTicks <= 15)
            {
                BaseCode.RandAsteroidWave(200);
                BaseCode.SmallEnemyWave(100, new Vector2(350, 100), new Vector2(-100, 200), 2f);
            }

        }

        public void Draw(SpriteBatch sB)
        {
            BaseCode.Draw(sB);
            //Code bellow this



        }


    }
}
